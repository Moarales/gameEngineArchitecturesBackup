using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

// ReSharper disable file InconsistentNaming


namespace SceneManager
{

    class DictionaryPair
    {
        public DictionaryPair(Node node, BoundingBox box)
        {
            Node = node;
            Box = box;
        }
        public Node Node;
        public BoundingBox Box;
    }
    internal class SceneManager
    {
        public SceneManager(int size)
        {
            _size = size;
            _dictionary = new Dictionary<int, DictionaryPair>();
        }

        private readonly int _size;
        private Dictionary<int, DictionaryPair> _dictionary;

        public Node _root { get; private set; }

        // Insert a axis aligned bounding box
        // cenX – extX <= x <= cenX + extX, cenY – extY <= y <= cenY + extY
        // with cenX > 0 and cenY > 0, and return a non-zero ID
        // for referring to that bounding box.
        public int insertBoundingBox(
            int cenX, int cenY, int extX, int extY)
        {
            if (_root == null)
            {
                _root = new Node(_size / 2, _size / 2, _size, 0);
            }

            var currentNode = _root;

            var diagonalSquared = (int)(Math.Pow(extX, 2) + Math.Pow(extY, 2));

            if (diagonalSquared > _root.DiagonalSquared)
            {
                throw new ArgumentException("BB is to big for our SceneManager, SceneManger set to " + _size + "x" + _size);
            }
            if (cenX > _root.CenterExt || cenX < 0 ||
                      cenY > _root.CenterExt || cenY < 0)
            {
                throw new ArgumentException("BB center is not in our scene");
            }

            if (extX <= 0 || extY <= 0)
            {
                throw new ArgumentException("BB x or y ext is zero");

            }

            var boundingBox = BoundingBoxManager.Instance.GetBoundingBox(cenX, cenY, extX, extY);
            Console.WriteLine("Created bb with id: " + boundingBox.Id);


            //check if root is already the place for new bb
            var node_found = currentNode.DiagonalSquared <= boundingBox.DiagonalSquared;

            bool createdEmptyNodeInPreviousIteration = false;


            while (!node_found)
            {
                Debug.Assert(currentNode != null, "current node shouldn't be null");

                var prevNode = currentNode;

                Action<Node> setPrevNode;
                Func<Node> getCurrentNode;

                if (currentNode.CenterX >= cenX && currentNode.CenterY >= cenY)
                {
                    setPrevNode = (node) => prevNode.UpperLeft = node;
                    getCurrentNode = () => prevNode.UpperLeft;

                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY >= cenY)
                {
                    setPrevNode = (node) => prevNode.UpperRight = node;
                    getCurrentNode = () => prevNode.UpperRight;


                }
                else if (currentNode.CenterX >= cenX && currentNode.CenterY < cenY)
                {
                    setPrevNode = (node) => prevNode.DownLeft = node;
                    getCurrentNode = () => prevNode.DownLeft;


                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY < cenY)
                {
                    setPrevNode = (node) => prevNode.DownRight = node;
                    getCurrentNode = () => prevNode.DownRight;

                }
                else
                {
                    throw new ArgumentException("Quartile not found which is weird and should never happen");
                }

                //if this is true just update the node and make it smaller
                if (createdEmptyNodeInPreviousIteration)
                {
                    Debug.Assert(currentNode != null, "Current node should never be null in here");
                    //if prev node is empty we can just reuse the current node and update center/ ext  to make it smaller
                    DecreaseNodeSize(boundingBox, currentNode);
                }
                else
                {
                    currentNode = getCurrentNode();

                    handleChildCreation(ref currentNode, prevNode, setPrevNode, ref createdEmptyNodeInPreviousIteration, boundingBox);

                }

                //if the next node would be to small we current node should contain our bounding box
                node_found = currentNode.DiagonalSquared / 4 < boundingBox.DiagonalSquared;
            }


            currentNode.BoundingBoxes.Add(boundingBox);
            _dictionary.Add(boundingBox.Id, new DictionaryPair(currentNode, boundingBox));

            Debug.Assert(currentNode.DiagonalSquared >= diagonalSquared, "Bounding Box to Big for Node :(");


            return boundingBox.Id;
        }

        private void handleChildCreation(ref Node currentNode, Node prevNode, Action<Node> setChildOfPrevNode, ref bool createdEmptyNodeInPreviousIteration, BoundingBox boundingBox)
        {
            //node hasn't been created before
            if (currentNode == null)
            {
                createdEmptyNodeInPreviousIteration = true;

                Node newNode = createEmptyChildNode(boundingBox.CenX, boundingBox.CenY, prevNode);
                newNode.ParentNode = prevNode;
                setChildOfPrevNode(newNode);

                //update parentNode
                newNode.ParentNode = prevNode;

                currentNode = newNode;
            }
            //node skipped 1 or more levels and can't contain new boundingBox
            else if (currentNode.Level != prevNode.Level + 1 && !currentNode.IsBoundingBoxCenterInsideNode(boundingBox))
            {
                //create empty parent that is big enough to contain existing childnode and new childNode where new bounding box is placed
                var parentNode = createEmptyParentForTwochildren(currentNode, boundingBox);
                //currentNode is the wrong Prophet
                setChildOfPrevNode(parentNode);

                //update parent nodes
                parentNode.ParentNode = prevNode;
                currentNode.ParentNode = parentNode;

                currentNode = parentNode;
            }

        }


        private bool DoesNodeContainChildNodeAndBoundingBox(Node firstNode, BoundingBox boundingBox, int ParentCenterX, int ParentCenterY, int ParentLevel)
        {
            int CenterExt = this._size / (int)Math.Pow(2, ParentLevel);

            return (firstNode.CenterX >= ParentCenterX - CenterExt / 2 && firstNode.CenterX <= ParentCenterX + CenterExt / 2) &&
                   (firstNode.CenterY >= ParentCenterY - CenterExt / 2 && firstNode.CenterY <= ParentCenterY + CenterExt / 2) &&
                   (boundingBox.CenX >= ParentCenterX - CenterExt / 2 && boundingBox.CenX <= ParentCenterX + CenterExt / 2) &&
                   (boundingBox.CenY >= ParentCenterY - CenterExt / 2 && boundingBox.CenY <= ParentCenterY + CenterExt / 2) && (Math.Pow(CenterExt, 2) * 2) >= boundingBox.DiagonalSquared;
        }

        private void DecreaseNodeSize(BoundingBox boundingBox, Node currentNode)
        {
            int smallerCenX = calculateNextChildPosition(boundingBox.CenX, currentNode.CenterExt);
            int smallerCenY = calculateNextChildPosition(boundingBox.CenY, currentNode.CenterExt);


            currentNode.UpdateNode(smallerCenX, smallerCenY, _size, currentNode.Level + 1);
        }



        private Node createEmptyParentForTwochildren(Node childNode, BoundingBox boundingBox)

        {
            int parentX = calculateParentPosition(childNode.CenterX, childNode.CenterExt);
            int parentY = calculateParentPosition(childNode.CenterY, childNode.CenterExt);
            int parentLevel = childNode.Level - 1;


            var parentNode = new Node(parentX, parentY, this._size, parentLevel);

            while (!DoesNodeContainChildNodeAndBoundingBox(childNode, boundingBox, parentX, parentY, parentLevel))
            {
                parentX = calculateParentPosition(parentX, parentNode.CenterExt);
                parentY = calculateParentPosition(parentY, parentNode.CenterExt);
                parentLevel -= 1;
                parentNode.UpdateNode(parentX, parentY, this._size, parentLevel);
            }
            parentNode.PlaceNodeAsChild(childNode);

            return parentNode;
        }

        private Node createEmptyChildNode(int childX, int childY, Node parentNode)
        {
            childX = calculateNextChildPosition(childX, parentNode.CenterExt);
            childY = calculateNextChildPosition(childY, parentNode.CenterExt);

            return new Node(childX, childY, _size, parentNode.Level + 1);
        }

        private int calculateParentPosition(int xOrY, int childSize)
        {

            int parentSize = childSize * 2;

            int value = xOrY / parentSize;
            return value * parentSize + parentSize / 2;
        }

        private int calculateNextChildPosition(int xOrY, int parentSize)
        {
            int childSize = parentSize / 2;

            int value = xOrY / childSize;
            return value * childSize + childSize / 2;
        }

        public void clearScene()
        {
            _root = null;
        }



        // Remove a bounding box which was inserted before.
        public void removeBoundingBox(int id)
        {
            if (_dictionary.TryGetValue(id, out var currentEntry))
            {
                var node = currentEntry.Node;
                var box = currentEntry.Box;

                Debug.Assert(node.BoundingBoxes.Count != 0, "Can't be null if inside dictionary");


                //remove bb from node
                node.BoundingBoxes.Remove(box);

                if (node.BoundingBoxes.Count == 0 && node != _root)
                {
                    if (node.NumberOfChildNodes() == 1)
                    {
                        //if we only have one node get our childNode and add it to our parentNode
                        var childNodes = node.GetChildNodes();

                        //overwrite parent ref to delete our node which is useless
                        node.ParentNode.PlaceNodeAsChild(childNodes[0]);


                    }else if (node.NumberOfChildNodes() == 0)
                    {
                        //remove currentNode
                        node.ParentNode.DeleteChildNode(node);

                        //edge case:: parent without bb and only one child is unecessary
                        if (node.ParentNode != _root && node.ParentNode.GetChildNodes().Count == 1&&node.ParentNode.BoundingBoxes.Count == 0&& node.ParentNode !=null)
                        {
                            var childNodes =node.ParentNode.GetChildNodes();
                            node.ParentNode.ParentNode.PlaceNodeAsChild(childNodes[0]);
                        }
                    }

                }

                //remove id from dictionary
                _dictionary.Remove(id);

            }
            else
            {
                throw new ArgumentException("Bounding Box Id not found in Tree");
            }

        }

        // Return the ID of one bounding box which touches, intersects
        // or is inside the cube
        // cenX – ext <= x <= cenX + ext,
        // cenY - ext <= y <= cenY + ext,
        // with ext > 0.
        // Return 0 if no such bounding box is registered.

        public int searchAnyBoundingBoxInCube(
            int centerX, int centerY, int ext)
        {
            var currentNode = _root;


            int id = recursiveSearchBoundingBox(centerX, centerY, ext, ref currentNode);

            Debug.Assert(id >= 0, "Id can't be -1"); 
            return recursiveSearchBoundingBox(centerX, centerY, ext, ref currentNode);
        }


        private int recursiveSearchBoundingBox(int centerX, int centerY, int ext, ref Node currentNode)
        {

            if (currentNode == null)
            {
                return 0;
            }

            //search in currentNode
            var id = currentNode.FindCollidingBoundingBoxes(centerX, centerY, ext);
            if (id > 0)
            {
                return id;
            }

            //skip all children if node can't contain colliding bb
            if (id == -1)
            {
                return 0;
            }

            //search all children and return on the first find :)
            id = recursiveSearchBoundingBox(centerX, centerY, ext,ref currentNode.UpperLeft);
            if (id > 0)
            {
                return id;
            }
            id = recursiveSearchBoundingBox(centerX, centerY, ext, ref currentNode.UpperRight);
            if (id > 0)
            {
                return id;
            }
            id = recursiveSearchBoundingBox(centerX, centerY, ext, ref currentNode.DownLeft);
            if (id > 0)
            {
                return id;
            }
            id = recursiveSearchBoundingBox(centerX, centerY, ext, ref currentNode.DownRight);
            return id;

        }
    }
}

