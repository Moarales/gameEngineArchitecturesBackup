using System;

namespace SceneManager
{
    public sealed class BoundingBoxManager
    {
        private static int _id;

        static BoundingBoxManager()
        {
            _id = 0;
        }

        private BoundingBoxManager()
        {
            _id = 0;
        }

        public static BoundingBoxManager Instance { get; } = new BoundingBoxManager();


        public BoundingBox GetBoundingBox(int cenX, int cenY, int extX, int extY)
        {
            if (cenX < 0) throw new ArgumentException("CenX is smaller then 0");
            if (cenY < 0) throw new ArgumentException("CenY is smaller then 0");
            var bb = new BoundingBox(_id, cenX, cenY, extX, extY);
            _id++;
            return bb;
        }
    }
}