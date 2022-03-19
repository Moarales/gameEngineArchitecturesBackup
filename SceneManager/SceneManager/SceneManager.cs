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
        private Node _root;

        // Insert a axis aligned bounding box
        // cenX – extX <= x <= cenX + extX, cenY – extY <= y <= cenY + extY
        // with cenX > 0 and cenY > 0, and return a non-zero ID
        // for referring to that bounding box.
        public int insertBoundingBox(
            int cenX, int cenY, int extX, int extY)
        {
            var boundingBox = BoundingBoxManager.Instance.GetBoundingBox(cenX, cenY, extX, extY);
            Console.WriteLine("Created bb with id: " + boundingBox.Id);

            if (_root == null)
            {
                _root = new Node(_size / 2, _size / 2, _size);
            }


            var currentNode = _root;
            Node prevNode = null;
            var diagonal = (int) (Math.Pow(extX, 2) + Math.Pow(extY, 2));

            var bbPlaced = false;

            while (!bbPlaced)
            {
                //smallest node found that can fit bb

                Debug.Assert(currentNode != null, "current node shouldn't be null");
                if (2 * Math.Pow(currentNode.CenterExt, 2) <= diagonal)
                {
                    bbPlaced = true;
                }

                prevNode = currentNode;

                if (currentNode.CenterX > cenX && currentNode.CenterY > cenY)
                {
                    currentNode = currentNode.UpperLeft;

                    if (currentNode == null)
                    {
                        prevNode.UpperLeft = new Node(prevNode.CenterX - prevNode.CenterExt / 4,
                            prevNode.CenterY - prevNode.CenterExt / 4,
                            prevNode.CenterExt / 2);
                    }

                    currentNode = prevNode.UpperLeft;
                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY > cenY)
                {
                    currentNode = currentNode.UpperRight;
                    if (currentNode == null)
                    {
                        prevNode.UpperRight = new Node(prevNode.CenterX + prevNode.CenterExt / 4,
                            prevNode.CenterY - prevNode.CenterExt / 4,
                            prevNode.CenterExt / 2);
                    }

                    currentNode = prevNode.UpperRight;
                }
                else if (currentNode.CenterX > cenX && currentNode.CenterY < cenY)
                {
                    currentNode = currentNode.DownLeft;

                    if (currentNode == null)
                    {
                        prevNode.DownLeft = new Node(prevNode.CenterX - prevNode.CenterExt / 4,
                            prevNode.CenterY + prevNode.CenterExt / 4,
                            prevNode.CenterExt / 2);
                    }

                    currentNode = prevNode.DownLeft;
                }
                else if (currentNode.CenterX < cenX && currentNode.CenterY < cenY)
                {
                    currentNode = currentNode.DownRight;
                    if (currentNode == null)
                    {
                        prevNode.DownRight = new Node(prevNode.CenterX + prevNode.CenterExt / 4,
                            prevNode.CenterY + prevNode.CenterExt / 4,
                            prevNode.CenterExt / 2);
                    }

                    currentNode = prevNode.DownRight;
                }
                else
                {
                    throw new ArgumentException("Quartile not found which is weird and should never happen");
                }
            }


            currentNode.BoundingBoxes.Add(boundingBox);


            return boundingBox.Id;
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
