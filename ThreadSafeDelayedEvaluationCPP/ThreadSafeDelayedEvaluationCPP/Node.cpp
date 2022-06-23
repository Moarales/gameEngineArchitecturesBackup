#include "Node.h"

Node::~Node()
{

	const std::lock_guard<std::mutex>locked(_mutex);
	if(_parent != nullptr)
	{
		_parent->unsubscribeNotification(this);
		_parent->releaseReference();
		_parent = nullptr;
	}


}

void Node::addReference()
{
	const std::lock_guard<std::mutex>locked(_mutexRef);
	_refCounter++;
}

void Node::releaseReference()
{

	//set scope
	{
		const std::lock_guard<std::mutex>locked(_mutexRef);
		_refCounter--;
	}

	if(_refCounter == 0 && _observerList.empty())
	{
		delete this;
	}
}

void Node::setParent(Node* parent)
{
	const std::lock_guard<std::mutex>locked(_mutex);

	if(_parent != nullptr)
	{
		parent->unsubscribeNotification(this);
	}
	_parent = parent;

	_parent->registerNotification(this);
	setDirty();
}

void Node::setParam(int param)
{

	{
		_param = param;

	}
	setDirty();
}

int Node::getResult()
{
	if(_dirty)
	{
		_dirty = false;
		_result = 0;
		if(_enabled)
		{
			_result = _param;
			if(_parent != nullptr)
			{
				_result += _parent->getResult();
				_parent->registerNotification(this);
			}
		}
	}

	return _result;
}

void Node::setDirty()
{
	_dirty = true;
	const auto observerCopy = _observerList;
	_observerList.clear();
	for (const auto observer : observerCopy)
	{
		observer->setDirty();
	}

	_parent->unsubscribeNotification(this);
}

void Node::registerNotification(Node* depNode)
{

	const std::lock_guard<std::mutex>locked(_mutex);

	if(std::find(_observerList.begin(), _observerList.end(), depNode) != _observerList.end())
	{
		_observerList.push_back(depNode);
	}
}

void Node::unsubscribeNotification(const Node* depNode)
{
	const std::lock_guard<std::mutex>locked(_mutex);

	const auto childNode = std::find(_observerList.begin(), _observerList.end(), depNode);
	if(childNode!= _observerList.end())
	{
		_observerList.erase(childNode);
	}
}
