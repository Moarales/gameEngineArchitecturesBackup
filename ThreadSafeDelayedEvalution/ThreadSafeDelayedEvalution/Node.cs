using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreadSafeDelayedEvalution
{
    public class Node
    {

        private bool _dirty = false;
        private int _param = 0;
        private Node _parent = null;
        private bool _enabled = true;
        private int _result = 0;
        private List<Node> _observerList = new();

        private uint _refCounter = 0;


        private void CustomDeStructor()
        {

            //delete my self from observer list of parent if i was destroyed
            if (_parent._observerList.Contains(this))
            {
                _observerList.Remove(this);
            }
        }


        private void checkIfValidNode()
        {
            if (_observerList.Count == 0 && _refCounter == 0)
            {
                throw new AccessViolationException("This node is not valid and should never be called :(");
            }
        }



        public void  addReference()
        {

            //node is always invalid with this example code=?
            //checkIfValidNode();
            
            _refCounter++;
        }

        public void releaseReference()
        {
            checkIfValidNode();

            _refCounter--;
            if (_refCounter == 0 && _observerList.Count == 0)
            {

                CustomDeStructor();
            }

        }

        public void setParent(Node parent)
        {
            checkIfValidNode();

            _parent = parent;

            _parent.registerNotification(this);
            setDirty();
        }



/*
        public void setEnabled(bool enabled)
        {
            _enabled = enabled;
            setDirty();
        }*/

        public void setParam(int param)
        {
            checkIfValidNode();
            _param = param;
            setDirty();
        }

        // Return the sum of the parameter and the result of the parent:
        public int getResult()
        {
            checkIfValidNode();

            if (_dirty)
            {
                _dirty = false;
                _result = 0;
                if (_enabled)
                {
                    _result = _param;
                    if (_parent != null)
                    {
                        _result += _parent.getResult();
                        _parent.registerNotification(this);
                    }
                }
            }
            return _result;
        }

        public void setDirty()
        {
            checkIfValidNode();

            _dirty = true;

            foreach (var observer in _observerList)
            {
                observer.setDirty();
            }

            //TODO:: Clear i echt die Observer wenn is dirty gsetzt hab=?

            _observerList.Clear();
        }


        void registerNotification(Node depNode)
        {
            checkIfValidNode();

            if (!_observerList.Contains(depNode))
            {
                _observerList.Add(depNode);
            }
        }
    }
}