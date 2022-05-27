namespace ThreadSafeDelayedEvalution;

public class Node
{

    private bool _dirty = false;
    private int _param = 0;
    private Node? _parent = null;
    private bool _enabled = true;
    private List<Node> _observerList = new List<Node>();
    ~Node()
    {
        throw new Exception("not implemented");

    }

    public void addReference()
    {
        throw new Exception("not implemented");

    }

    public void releaseReference()
    {
        throw new Exception("not implemented");

    }

    public void setParent(Node parent)
    {
        _parent = parent;
        setDirty();
    }

    public void setParam(int param)
    {
        throw new Exception("not implemented");

    }
    // Return the sum of the parameter and the result of the parent:
    public int getResult()
    {
        //maybe this is wrong?
        int result = 0;

        if (_dirty)
        {
            _dirty = false;
            result = 0;
            if (_enabled)
            {
                result = _param;
                if (_parent != null)
                {
                    result = result += _parent.getResult();
                    _parent.registerNotification(this);
                }
            }
        }
        return result;
    }

    public void setDirty()
    {
        _dirty = true;
        observerscopy =;
        oservers = emptylist;
        for (observer in observerscopy)
        observer->setDirty();
    }
};