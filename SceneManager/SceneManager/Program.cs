using System;
using System.Diagnostics;

namespace SceneManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var sceneManager = new SceneManager(100);
            Debug.Assert(sceneManager.insertBoundingBox(30, 30, 24, 24) == 0);
            Debug.Assert(sceneManager._root.UpperLeft != null);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);

            Debug.Assert(sceneManager.insertBoundingBox(2, 1, 1, 1) == 1);


            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }
    }
}
