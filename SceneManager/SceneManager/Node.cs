using System;
using System.Collections.Generic;

namespace SceneManager
{
    public class Node
    {
        public Node(int centerX, int centerY, int size_of_sceneManager, int level)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterExt = size_of_sceneManager / (int)Math.Pow(2, level);
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


        public void PlaceNodeAsChild( Node childNode)
        {
            if (this.CenterX > childNode.CenterX && this.CenterY > childNode.CenterY)
            {
                this.UpperLeft = childNode;
            }
            else if (this.CenterX < childNode.CenterX && this.CenterY > childNode.CenterY)
            {
                this.UpperRight = childNode;
            }
            else if (this.CenterX > childNode.CenterX && this.CenterY < childNode.CenterY)
            {
                this.DownLeft = childNode;
            }
            else if (this.CenterX < childNode.CenterX && this.CenterY < childNode.CenterY)
            {
                this.DownRight = childNode;
            }
        }

        public bool IsPointInside(int x, int y)
        {
            return (x >= CenterX - CenterExt && x <= CenterX + CenterExt) &&
                   (y >= CenterY - CenterExt && y <= CenterY + CenterExt);
        }
    }
}