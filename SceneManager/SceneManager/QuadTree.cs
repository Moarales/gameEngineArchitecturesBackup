namespace SceneManager
{
    public class QuadTree
    {
        public Node Root;


        public void InsertBoundingBox(BoundingBox bb)
        {
            if (Root == null) Root = new Node(bb);
        }
    }
}