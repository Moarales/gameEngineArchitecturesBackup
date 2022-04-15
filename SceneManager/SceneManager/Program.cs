using System;
using System.Diagnostics;

namespace SceneManager
{
    class Program
    {
        static void Main(string[] args)
        {

            //TODO:: Root is always created could be optimized
            var sceneManager = new SceneManager(128);
            Debug.Assert(sceneManager.insertBoundingBox(30, 30, 16, 16) == 1);
            Debug.Assert(sceneManager._root.UpperLeft != null);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes[0].CenX == 30);


            //insert node inbetween
            Debug.Assert(sceneManager.insertBoundingBox(2, 1, 30, 30) == 2);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperLeft.DownRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager.insertBoundingBox(1, 1, 24, 24) == 3);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 2);
            //Insert two bounding boxes into the same node
            Debug.Assert(sceneManager.insertBoundingBox(1, 1, 24, 24) == 4);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 3);

            //Insert bounding box where we have to add a parent node since it wasn't there before
            Debug.Assert(sceneManager.insertBoundingBox(65, 10, 24, 24) == 5);
            Debug.Assert(sceneManager._root.UpperRight != null);
            Debug.Assert(sceneManager.insertBoundingBox(100, 10, 24, 24) == 6);
            Debug.Assert(sceneManager._root.UpperRight.UpperRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperRight.BoundingBoxes.Count == 0);
            Debug.Assert(sceneManager._root.UpperRight.UpperLeft.BoundingBoxes.Count == 1);

            //Check where boxes on edges are placed
            Debug.Assert(sceneManager.insertBoundingBox(64, 64, 64, 64) == 7);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);



            //TestMultipleBoundingBoxes
            Debug.Assert(sceneManager.insertBoundingBox(18, 69, 12, 12) == 8);
            Debug.Assert(sceneManager._root.DownLeft != null);
            Debug.Assert(sceneManager.insertBoundingBox(56, 97, 25, 25) == 9);
            Debug.Assert(sceneManager._root.DownLeft.BoundingBoxes.Count == 0);
            Debug.Assert(sceneManager._root.DownLeft.DownRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.DownLeft.UpperLeft.BoundingBoxes.Count == 1);


            Debug.Assert(sceneManager.insertBoundingBox(65, 65, 3, 3) == 10);
            Debug.Assert(sceneManager.insertBoundingBox(65, 65, 3, 3) == 11);
            Debug.Assert(sceneManager._root.DownRight.BoundingBoxes.Count == 2);
            Debug.Assert(sceneManager.insertBoundingBox(128, 65, 3, 3) == 12);
            Debug.Assert(sceneManager._root.DownRight.UpperRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.DownRight.UpperLeft.BoundingBoxes.Count == 2);
            Debug.Assert(sceneManager._root.DownRight.UpperLeft.BoundingBoxes[0].CenX == 65);

            //insert bb with extX 1 and extY 1
            Debug.Assert(sceneManager.insertBoundingBox(128, 65, 1, 1) == 13);
            Debug.Assert(sceneManager._root.DownRight.UpperRight.UpperLeft.BoundingBoxes.Count == 1);


            //Searching
            sceneManager.clearScene();
            //search the whole scene
            Debug.Assert(sceneManager.searchAnyBoundingBoxInCube(64, 64, 128) == 0);
            Debug.Assert(sceneManager.insertBoundingBox(65, 65, 3, 3) == 14);
            Debug.Assert(sceneManager.searchAnyBoundingBoxInCube(64, 64, 128) == 14);
            //should still find this bb
            Debug.Assert(sceneManager.searchAnyBoundingBoxInCube(110, 110, 128) == 14);
            //should not find this bb
            Debug.Assert(sceneManager.searchAnyBoundingBoxInCube(110, 110, 10) == 0);


            sceneManager.removeBoundingBox(14);


            //insert small node
            Debug.Assert(sceneManager.insertBoundingBox(65, 65, 3, 3) == 15);

            //insert bigger node on top
            Debug.Assert(sceneManager.insertBoundingBox(65, 65, 30, 30) == 16);
            //remove bigger node
            sceneManager.removeBoundingBox(16);
            Debug.Assert(sceneManager._root.DownRight.BoundingBoxes[0].ExtX == 3);
            sceneManager.removeBoundingBox(15);

            //only empty root should now be inside
            Debug.Assert(sceneManager._root.GetChildNodes().Count == 0);


            //insert small node
            Debug.Assert(sceneManager.insertBoundingBox(1, 1, 3, 3) == 17);

            //insert smale  node on rightSide
            Debug.Assert(sceneManager.insertBoundingBox(55, 1, 3, 3) == 18);

            Debug.Assert(sceneManager._root.UpperLeft.UpperLeft != null);
            Debug.Assert(sceneManager._root.UpperLeft.UpperRight != null);

            //remove one node now there should only be 1 childNode
            sceneManager.removeBoundingBox(18);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);

            sceneManager.removeBoundingBox(17);

            Debug.Assert(sceneManager._root.BoundingBoxes.Count == 0);


            Console.WriteLine("Press any key for exit");
            Console.ReadKey();


        }
    }
}
