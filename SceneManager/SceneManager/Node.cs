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
        public Node ParentNode;
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
            childNode.ParentNode = this;
            if (this.CenterX >= childNode.CenterX && this.CenterY >= childNode.CenterY)
            {
                this.UpperLeft = childNode;
            }
            else if (this.CenterX < childNode.CenterX && this.CenterY >= childNode.CenterY)
            {
                this.UpperRight = childNode;
            }
            else if (this.CenterX >= childNode.CenterX && this.CenterY < childNode.CenterY)
            {
                this.DownLeft = childNode;
            }
            else if (this.CenterX < childNode.CenterX && this.CenterY < childNode.CenterY)
            {
                this.DownRight = childNode;
            }
            else
            {
                throw new Exception("Couldn't place child inside parentNode");
            }
        }

        public bool IsBoundingBoxCenterInsideNode(BoundingBox boundingBox)
        {
            
            return (boundingBox.CenX >= CenterX - CenterExt/2 && boundingBox.CenX <= CenterX + CenterExt/2) &&
                   (boundingBox.CenY >= CenterY - CenterExt/2 && boundingBox.CenY <= CenterY + CenterExt/2)&& boundingBox.DiagonalSquared<= DiagonalSquared;
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

        public int NumberOfChildNodes()
        {
            int i = 0;
            if (UpperRight != null)
            {
                i++;
            }
            if (UpperLeft != null)
            {
                i++;
            }
            if (DownLeft != null)
            {
                i++;
            }
            if (DownRight != null)
            {
                i++;
            }

            return i;
        }

        public List<Node> GetChildNodes()
        {
            var list = new List<Node>();

            if (UpperLeft != null)
            {
                list.Add(UpperLeft);
            }

            if (UpperRight != null)
            {
                list.Add(UpperRight);
            }

            if (DownLeft != null)
            {
                list.Add(DownLeft);
            }

            if (DownRight != null)
            {
                list.Add(DownRight);
            }

            return list;
        }

        public void DeleteChildNode(Node childNode)
        {

            if (childNode == null)
            {
                throw new ArgumentException("Can't delete empty childnode");
            }


            //finds the given node and deletes the ref
            if (UpperLeft == childNode)
            {
                UpperLeft = null;
            }else if(UpperRight == childNode)
            {
                UpperRight = null;
            }
            else if (DownLeft == childNode)
            {
                DownLeft = null;
            }
            else if (DownRight == childNode)
            {
                DownRight = null;
            }

        }
    }
}