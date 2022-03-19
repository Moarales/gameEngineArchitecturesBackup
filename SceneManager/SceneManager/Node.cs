using System.Collections.Generic;

namespace SceneManager
{
    public class Node
    {
        public Node(int centerX, int centerY, int centerExt)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterExt = centerExt;
        }

        public List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        public Node UpperLeft;
        public Node UpperRight;
        public Node DownLeft;
        public Node DownRight;
        public int CenterY;
        public int CenterX;
        public int CenterExt;
    }
}