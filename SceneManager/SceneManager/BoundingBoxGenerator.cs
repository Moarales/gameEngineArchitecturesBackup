namespace SceneManager
{
    public class BoundingBoxGenerator
    {
        public BoundingBox GetBoundingBox(int cenX, int cenY, int extX, int extY)
        {
            var bb = new BoundingBox(_id, cenX, cenY, extX, extY);
            _id++;
            return bb;
        }

        private int _id;
    }
}