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


        public void UpdateNode(int centerX, int centerY, int sceneSize, int level)
        {
            CenterX = centerX;
            CenterY = centerY;
            CenterExt = sceneSize / (int)Math.Pow(2, level);
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

        public bool IsBoundingBoxCenterInsideNode(BoundingBox boundingBox)
        {
            
            return (boundingBox.CenX >= CenterX - CenterExt/2 && boundingBox.CenX <= CenterX + CenterExt/2) &&
                   (boundingBox.CenY >= CenterY - CenterExt/2 && boundingBox.CenY <= CenterY + CenterExt/2);
        }


        /// <summary>
        /// Returns bb id or 0 if not found
        /// Returns -1 if this Node or any children do not intercept with box
        /// </summary>
        public int FindCollidingBoundingBoxes(int centerX, int centerY, int ext)
        {

            //if this node cann't contain a colliding bb return 0
            if (!CouldContainCollidingBoundingBox(centerX, centerY, ext))
            {
                return -1;
            }
            var collidedBb = BoundingBoxes.Find(bb =>
            {
                return (centerX - ext / 2 <= bb.CenX + bb.ExtX / 2 && centerX + ext / 2 >= bb.CenX - bb.ExtX / 2) &&
                       (centerY - ext / 2 <= bb.CenY + bb.ExtY / 2 && centerY + ext / 2 >= bb.CenY - bb.ExtY / 2);

            });

            return collidedBb?.Id ?? 0;
        }


        /// <summary>
        /// Returns true if currentBox could collide with any bounding box inside node
        /// </summary>
        private bool CouldContainCollidingBoundingBox(int centerX, int centerY, int ext)
        {
            return (centerX - ext / 2 <= this.CenterX + this.CenterExt  && centerX + ext / 2 >= this.CenterX - this.CenterExt) &&
                   (centerY - ext / 2 <= this.CenterY + this.CenterExt && centerY + ext / 2 >= this.CenterY - this.CenterExt);

        }
    }
}