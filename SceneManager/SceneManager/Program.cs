using System;

namespace SceneManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var sceneManager = new SceneManager();
            var boundingBoxGenerator = new BoundingBoxGenerator();

            var bb1 = boundingBoxGenerator.GetBoundingBox(1, 1, 1, 1);
            var bb2 = boundingBoxGenerator.GetBoundingBox(2, 1, 1, 1);

            Console.WriteLine(bb1.Id);
            Console.WriteLine(bb2.Id);

            Console.ReadKey();
        }
    }
}
