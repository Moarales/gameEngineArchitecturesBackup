#pragma once
#include <list>
#include <mutex>
#include <vector>

class Node
{
public:

	~Node();

	void addReference();
	void releaseReference();
	void setParent(Node* parent);
	void setParam(int param);
	int getResult();
	void setDirty();
	void registerNotification(Node* depNode);
	void unsubscribeNotification(const Node* depNode);




private:
   bool _dirty = false;
   int _param = 0;
   Node* _parent = nullptr;
   bool _enabled = true;
   int _result = 0;
   std::vector<Node*> _observerList = {};
   int _refCounter = 0;
   std::mutex _mutex;
   std::mutex _mutexRef;
};
