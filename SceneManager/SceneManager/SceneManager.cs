using System;
using System.Diagnostics;

// ReSharper disable file InconsistentNaming


namespace SceneManager
{
    internal class SceneManager
    {
        public SceneManager(int size)
        {
            _size = size;
        }

        private readonly int _size;
        public Node _root { get; private set; }

        // Insert a axis aligned bounding box
        // cenX – extX <= x <= cenX + extX, cenY – extY <= y <= cenY + extY
        // with cenX > 0 and cenY > 0, and return a non-zero ID
        // for referring to that bounding box.
        public int insertBoundingBox(
            int cenX, int cenY, int extX, int extY)
        {

            var boundingBox = BoundingBoxManager.Instance.GetBoundingBox(cenX, cenY, extX, extY);
            Console.WriteLine("Created bb with id: " + boundingBox.Id);


            //TODO: Root is always created? what  if there is only one node we don't need a root node
            if (_root == null)
            {
                _root = new Node(_size / 2, _size / 2, _size, 0);
            }


            var currentNode = _root;

            Node prevNode = null;
            var diagonalSquared = (int)(Math.Pow(extX, 2) + Math.Pow(extY, 2));


            //check if root is already the place for new bb
            var bb_found = currentNode.DiagonalSquared <= diagonalSquared;

            bool createdEmptyNodeInPreviousIteration = false;



            while (!bb_found)
            {
                Debug.Assert(currentNode != null, "current node shouldn't be null");

                prevNode = currentNode;

                if (currentNode.CenterX >= cenX && currentNode.CenterY >= cenY)
                {
                    //if this is true just update the node and make it smaller
                    if (createdEmptyNodeInPreviousIteration)
                    {

                        Debug.Assert(currentNode != null, "Current node should never be null in here");
                        //if prev node is empty we can just reuse the current node and update center/ ext 
                        currentNode.UpdateNode(currentNode.CenterX - currentNode.CenterExt / 4,
                            currentNode.CenterY - currentNode.CenterExt / 4,
                            _size, currentNode.Level + 1);
                    }
                    else
                    {
                        currentNode = currentNode.UpperLeft;
                        if (currentNode == null)
                        {
                            createdEmptyNodeInPreviousIteration = true;

                            prevNode.UpperLeft = new Node(prevNode.CenterX - prevNode.CenterExt / 4,
                                prevNode.CenterY - prevNode.CenterExt / 4,
                                _size, prevNode.Level + 1);
                            currentNode = prevNode.UpperLeft;

                        }
                        else if (currentNode.Level != prevNode.Level + 1 && !currentNode.IsPointInside(cenX, cenY))
                        {
/*                            //create empty parent to distinguish both nodes
                            var parentNode = new Node(prevNode.CenterX - prevNode.CenterExt / 4,
                                prevNode.CenterY - prevNode.CenterExt / 4,
                                _size, prevNode.Level + 1);


                            while (DoesNodeContainBothChildNodes(currentNode, cenX, cenY, diagonalSquared,
                                parentNode.CenterX - parentNode.CenterExt / 4, parentNode.CenterY - parentNode.CenterExt / 4, parentNode.Level + 1))
                            {
                                parentNode.UpdateNode(parentNode.CenterX - parentNode.CenterExt / 4, parentNode.CenterY - parentNode.CenterExt / 4, _size, parentNode.Level + 1);

                            }*/

                            var parentNode = createEmptyParentForTwochildren(currentNode, cenX, cenY, diagonalSquared);
                            //currentNode is the wrong Prophet
                            prevNode.UpperLeft = parentNode;

                            currentNode = parentNode;

                        }


                    }

                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY >= cenY)
                {
                    //if this is true just update the node and make it smaller

                    if (createdEmptyNodeInPreviousIteration)
                    {

                        Debug.Assert(currentNode != null, "Current node should never be null in here");
                        //if prev node is empty we can just reuse the current node and update center/ ext 
                        currentNode.UpdateNode(currentNode.CenterX + currentNode.CenterExt / 4,
                            currentNode.CenterY - currentNode.CenterExt / 4,
                            _size, currentNode.Level + 1);

                    }
                    else
                    {
                        currentNode = currentNode.UpperRight;
                        if (currentNode == null)
                        {

                            createdEmptyNodeInPreviousIteration = true;

                            prevNode.UpperRight = new Node(prevNode.CenterX + prevNode.CenterExt / 4,
                                prevNode.CenterY - prevNode.CenterExt / 4,
                                _size, prevNode.Level + 1);
                            currentNode = prevNode.UpperRight;
                        }
                        else if (currentNode.Level != prevNode.Level + 1 && !currentNode.IsPointInside(cenX, cenY))
                        {
/*                            //create empty parent to distinguish both nodes
                            var parentNode = new Node(prevNode.CenterX + prevNode.CenterExt/4,
                                prevNode.CenterY - prevNode.CenterExt/4,
                                _size, prevNode.Level + 1);


                            while (DoesNodeContainBothChildNodes(currentNode, cenX, cenY, diagonalSquared,
                                parentNode.CenterX+ parentNode.CenterExt / 4 , parentNode.CenterY - parentNode.CenterExt / 4, parentNode.Level +1))
                            {
                                parentNode.UpdateNode(parentNode.CenterX + parentNode.CenterExt/4, parentNode.CenterY - parentNode.CenterExt/4, _size, parentNode.Level+1);

                            }
                            
                            //currentNode is the wrong Prophet
                            prevNode.UpperRight = parentNode;

                            parentNode.PlaceNodeAsChild(currentNode);
                            currentNode = parentNode;
*/


                            var parentNode = createEmptyParentForTwochildren(currentNode, cenX, cenY, diagonalSquared);
                            //currentNode is the wrong Prophet
                            prevNode.UpperRight = parentNode;

                            currentNode = parentNode;

                        }
                    }
                }
                else if (currentNode.CenterX >= cenX && currentNode.CenterY < cenY)
                {
                    //if this is true just update the node and make it smaller

                    if (createdEmptyNodeInPreviousIteration)
                    {
                        Debug.Assert(currentNode != null, "Current node should never be null in here");
                        //if prev node is empty we can just reuse the current node and update center/ ext 
                        currentNode.UpdateNode(currentNode.CenterX - currentNode.CenterExt / 4,
                            currentNode.CenterY + currentNode.CenterExt / 4,
                            _size, currentNode.Level + 1);
                    }
                    else
                    {
                        currentNode = currentNode.DownLeft;

                        if (currentNode == null)
                        {
                            createdEmptyNodeInPreviousIteration = true;

                            prevNode.DownLeft = new Node(prevNode.CenterX - prevNode.CenterExt / 4,
                                prevNode.CenterY + prevNode.CenterExt / 4,
                                _size, prevNode.Level + 1);
                            currentNode = prevNode.DownLeft;
                        }
                        else if (currentNode.Level != prevNode.Level + 1 && !currentNode.IsPointInside(cenX, cenY))
                        {
/*                            //create empty parent to distinguish both nodes
                            var parentNode = new Node(prevNode.CenterX - prevNode.CenterExt/4,
                                prevNode.CenterY + prevNode.CenterExt/4,
                                _size, prevNode.Level + 1);


                            while (DoesNodeContainBothChildNodes(currentNode, cenX, cenY, diagonalSquared,
                                parentNode.CenterX - parentNode.CenterExt / 4 , parentNode.CenterY + parentNode.CenterExt / 4, parentNode.Level +1))
                            {
                                parentNode.UpdateNode(parentNode.CenterX - parentNode.CenterExt/4, parentNode.CenterY + parentNode.CenterExt/4, _size, parentNode.Level+1);

                            }
                            
                            //currentNode is the wrong Prophet
                            prevNode.DownLeft = parentNode;

                            parentNode.PlaceNodeAsChild(currentNode);
                            currentNode = parentNode;*/


                            var parentNode = createEmptyParentForTwochildren(currentNode, cenX, cenY, diagonalSquared);
                            //currentNode is the wrong Prophet
                            prevNode.DownLeft = parentNode;

                            currentNode = parentNode;

                        }
                    }
                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY < cenY)
                {
                    //if this is true just update the node and make it smaller

                    if (createdEmptyNodeInPreviousIteration)
                    {
                        Debug.Assert(currentNode != null, "Current node should never be null in here");
                        //if prev node is empty we can just reuse the current node and update center/ ext 
                        currentNode.UpdateNode(currentNode.CenterX + currentNode.CenterExt / 4,
                            currentNode.CenterY + currentNode.CenterExt / 4,
                            _size, currentNode.Level + 1);
                    }
                    else
                    {
                        currentNode = currentNode.DownRight;

                        if (currentNode == null)
                        {
                            createdEmptyNodeInPreviousIteration = true;

                            prevNode.DownRight = new Node(prevNode.CenterX + prevNode.CenterExt / 4,
                                prevNode.CenterY + prevNode.CenterExt / 4,
                                _size, prevNode.Level + 1);
                            currentNode = prevNode.DownRight;
                        }
                        else if (currentNode.Level != prevNode.Level + 1 && !currentNode.IsPointInside(cenX, cenY))
                        {
/*                            //create empty parent to distinguish both nodes
                            var parentNode = new Node(prevNode.CenterX + prevNode.CenterExt/4,
                                prevNode.CenterY + prevNode.CenterExt/4,
                                _size, prevNode.Level + 1);


                            while (DoesNodeContainBothChildNodes(currentNode, cenX, cenY, diagonalSquared,
                                parentNode.CenterX + parentNode.CenterExt / 4 , parentNode.CenterY + parentNode.CenterExt / 4, parentNode.Level +1))
                            {
                                parentNode.UpdateNode(parentNode.CenterX + parentNode.CenterExt/4, parentNode.CenterY + parentNode.CenterExt/4, _size, parentNode.Level+1);

                            }
                            
                            //currentNode is the wrong Prophet
                            prevNode.DownRight = parentNode;

                            parentNode.PlaceNodeAsChild(currentNode);
                            currentNode = parentNode;*/

                            var parentNode = createEmptyParentForTwochildren(currentNode, cenX, cenY, diagonalSquared);
                            //currentNode is the wrong Prophet
                            prevNode.DownRight = parentNode;

                            currentNode = parentNode;


                        }

                    }


                }
                else
                {
                    throw new ArgumentException("Quartile not found which is weird and should never happen");
                }


                bb_found = CheckIfNodeFound(currentNode, diagonalSquared);
            }


            currentNode.BoundingBoxes.Add(boundingBox);

            Debug.Assert(currentNode.DiagonalSquared >= diagonalSquared, "Bounding Box to Big for Node :(");


            return boundingBox.Id;
        }

        private bool CheckIfNodeFound(Node currentNode, int diagonalSquared)
        {
            //if the next node would be to small we found the right node in currentNode
            return currentNode.DiagonalSquared / 4 < diagonalSquared;
        }

        private bool DoesNodeContainBothChildNodes(Node firstNode, int ChildX, int ChildY, int diagonalSquardChild, int CenterX, int CenterY, int level)
        {
            int CenterExt = this._size / (int)Math.Pow(2, level);

            return (firstNode.CenterX >= CenterX - CenterExt && firstNode.CenterX <= CenterX + CenterExt) &&
                   (firstNode.CenterY >= CenterY - CenterExt && firstNode.CenterY <= CenterY + CenterExt) &&
                   (ChildX >= CenterX - CenterExt && ChildX <= CenterX + CenterExt) &&
                   (ChildY >= CenterY - CenterExt && ChildY <= CenterY + CenterExt) && (CenterExt*CenterExt) >= diagonalSquardChild;
        }

        private Node createEmptyParentForTwochildren(Node childNode, int cenX, int cenY, int diagonalSquared)

        {
            int x = calculateParentPosition(childNode.CenterX, childNode.CenterExt);
            int y = calculateParentPosition(childNode.CenterY, childNode.CenterExt);
            int level = childNode.Level - 1;


            var parentNode = new Node(x, y, this._size, level);

            while (!DoesNodeContainBothChildNodes(childNode, cenX, cenY, diagonalSquared, x, y, level))
            {
                x = calculateParentPosition(x, parentNode.CenterExt);
                y = calculateParentPosition(y, parentNode.CenterExt);
                level -= 1;
                parentNode.UpdateNode(x,y,this._size, level);
            }


            parentNode.PlaceNodeAsChild(childNode);

            return parentNode;
        }

        private int calculateParentPosition(int xOrY, int childSize)
        {

            int parentSize = childSize * 2;

            int value = xOrY / parentSize;
            return value* parentSize + parentSize / 2;
        }



        // Remove a bounding box which was inserted before.
        public void removeBoundingBox(int id)
        {
            throw new Exception("not implemented");
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
            throw new Exception("not implemented");
        }
    }
}
