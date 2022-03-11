using System.Collections.Generic;

namespace SceneManager
{
    public class Node
    {
        public Node(BoundingBox bb)
        {
            BoundingBoxes.Add(bb);
        }

        public List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        public Node UpperLeft;
        public Node UpperRight;
        public Node DownLeft;
        public Node DownRight;
    }
}