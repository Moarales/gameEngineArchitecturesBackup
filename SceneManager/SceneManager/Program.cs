using System;
using System.Diagnostics;

namespace SceneManager
{
    class Program
    {
        static void Main(string[] args)
        {

            //TODO:: check if node is to big to insert
            //TODO:: Root is always created could be optimized
            var sceneManager = new SceneManager(100);
            Debug.Assert(sceneManager.insertBoundingBox(30, 30, 24, 24) == 0);
            Debug.Assert(sceneManager._root.UpperLeft != null);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager.insertBoundingBox(2, 1, 30, 30) == 1);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperLeft.DownRight.BoundingBoxes.Count == 1);



            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }
    }
}
