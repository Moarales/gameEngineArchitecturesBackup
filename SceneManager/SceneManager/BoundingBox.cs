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
            ExtY = ExtY;
        }

        public int Id { get; }
        public int CenX { get; set; }
        public int ExtX { get; set; }
        public int CenY { get; set; }
        public int ExtY { get; set; }
    }
}