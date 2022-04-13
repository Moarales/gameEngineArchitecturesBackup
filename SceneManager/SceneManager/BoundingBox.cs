using System;

namespace SceneManager
{
    public class BoundingBox
    {
        public BoundingBox(int id, int cenX = 0, int cenY = 0, int extX = 0, int extY = 0)
        {
            Id = id;
            CenX = cenX;
            CenY = cenY;
            ExtX = extX;
            ExtY = extY;
            RecalculateDiagonalSquared();
        }

        public int Id { get; }
        public int CenX { get; set; }
        public int ExtX { get; private set; }

        public void SetExtX(int extX)
        {
            ExtX = extX;
            RecalculateDiagonalSquared();
        }
        public int CenY { get; }
        public int ExtY { get; private set; }

        public void SetExtY(int extY)
        {
            ExtY = extY;
            RecalculateDiagonalSquared();
        }

        public int DiagonalSquared { get; private set; }

        private void RecalculateDiagonalSquared()
        {
            DiagonalSquared = (int)(Math.Pow(ExtX, 2) + Math.Pow(ExtY, 2));
        }

    }
}