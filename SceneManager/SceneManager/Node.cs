using System;
using System.Collections.Generic;

namespace SceneManager
{
    public class Node
    {
        public Node(int centerX, int centerY, int size, int level)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterExt = size / (int)Math.Pow(2, level);
            Level = level;
            DiagonalSquared = 2 * (int)Math.Pow(CenterExt, 2);
        }


        public List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        public Node UpperLeft;
        public Node UpperRight;
        public Node DownLeft;
        public Node DownRight;
        public int CenterY;
        public int CenterX;
        public int CenterExt { get; private set; }
        public int Level { get; private set; }
        public int DiagonalSquared { get; private set; }


        public void UpdateNode(int centerX, int centerY, int size, int level)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterExt = size / (int)Math.Pow(2, level);
            Level = level;
            DiagonalSquared = 2 * (int)Math.Pow(CenterExt, 2);
        }
    }
}