﻿using System;
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
            Debug.Assert(sceneManager.insertBoundingBox(30, 30, 24, 24) == 1);
            Debug.Assert(sceneManager._root.UpperLeft != null);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);


            //insert node inbetween
            Debug.Assert(sceneManager.insertBoundingBox(2, 1, 30, 30) == 2);
            Debug.Assert(sceneManager._root.UpperLeft.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperLeft.DownRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager.insertBoundingBox(1, 1, 24, 24) == 3);
            Debug.Assert(sceneManager._root.UpperLeft.UpperLeft.BoundingBoxes.Count == 1);
            //Insert two bounding boxes into the same node
            Debug.Assert(sceneManager.insertBoundingBox(1, 1, 24, 24) == 4);
            Debug.Assert(sceneManager._root.UpperLeft.UpperLeft.BoundingBoxes.Count == 2);

            //Insert bounding box where we have to add a parent node since it wasn't there before
            Debug.Assert(sceneManager.insertBoundingBox(55, 10, 24, 24) == 5);
            Debug.Assert(sceneManager._root.UpperRight != null);
            Debug.Assert(sceneManager.insertBoundingBox(89, 10, 24, 24) == 6);
            Debug.Assert(sceneManager._root.UpperRight.UpperRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.UpperRight.BoundingBoxes.Count == 0);
            Debug.Assert(sceneManager._root.UpperRight.UpperLeft.BoundingBoxes.Count == 1);

            //Check where boxes on edges are placed
            Debug.Assert(sceneManager.insertBoundingBox(50, 50, 3, 3) == 7);



            //TestMultipleBoundingBoxes
            Debug.Assert(sceneManager.insertBoundingBox(18, 69, 12, 12) == 8);
            Debug.Assert(sceneManager._root.DownLeft != null);
            Debug.Assert(sceneManager.insertBoundingBox(40, 90, 25, 25) == 9);
            Debug.Assert(sceneManager._root.DownLeft.BoundingBoxes.Count == 0);
            Debug.Assert(sceneManager._root.DownLeft.DownRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.DownLeft.UpperLeft.BoundingBoxes.Count == 1);



            Debug.Assert(sceneManager.insertBoundingBox(56, 56, 3, 3) == 10);
            Debug.Assert(sceneManager.insertBoundingBox(56, 56, 3, 3) == 11);
            Debug.Assert(sceneManager._root.DownRight.BoundingBoxes.Count == 2);


            Debug.Assert(sceneManager.insertBoundingBox(92, 56, 3, 3) == 12);

            Debug.Assert(sceneManager._root.DownRight.UpperRight.BoundingBoxes.Count == 1);
            Debug.Assert(sceneManager._root.DownRight.UpperLeft.BoundingBoxes.Count == 2);





            //TODO::Fix bug if squared equals 1  without area


            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }
    }
}
