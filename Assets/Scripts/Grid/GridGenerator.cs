using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
        [SerializeField] private GameObject wall;
        [SerializeField] private UnityEvent<MazeGrid> onFinishGeneratingGrid = new UnityEvent<MazeGrid>();
        
        private int cellSize = 6;
        
        public void GenerateGrid(Vector2Int size)
        { 
            MazeGrid grid = CreateGrid(size);
                
            GenerateVisualCells(grid);

            onFinishGeneratingGrid.Invoke(grid);
        }

        private MazeGrid CreateGrid(Vector2Int size)
        {
                return new MazeGrid(size.y, size.x, transform.position, cellSize);
        }

        private void GenerateVisualCells(MazeGrid grid)
        {
                int size = grid.CellSize;
                for (int x = 0; x < grid.Height; x++)
                {
                        for (int z = 0; z < grid.Width; z++)
                        {

                                MazeCell mazeCell = new MazeCell(x, z, size);
                                InitializeVisuals(mazeCell);
                                
                                grid.AddCell(mazeCell);
                        }
                }
        }

        /*
         * Instantiates walls and floors for the MazeCell
         */
        private void InitializeVisuals(MazeCell mazeCell)
        {
                int x = mazeCell.X;
                int z = mazeCell.Z;
                int size = mazeCell.Size;

                int upscaledX = x * size;
                int upscaledZ = z * size;
                float downScaledSize = size / 2f;
                
                
                GameObject floor = Instantiate (wall, new Vector3 (upscaledX, -(downScaledSize), upscaledZ), Quaternion.identity) as GameObject;

                
                mazeCell.FloorMesh = floor.GetComponent<MeshRenderer>();
                floor.name = "Floor " + x + "," + z;
                floor.transform.Rotate (Vector3.right, 90f);
                
                mazeCell.SetFloorColor(Color.black);

                if (z == 0)
                {
                        GameObject westWall = Instantiate(wall,
                                new Vector3(upscaledX, 0, (upscaledZ) - (downScaledSize)),
                                Quaternion.identity);
                        westWall.name = "West Wall " + x + "," + z;
                        mazeCell.AddWall(westWall, Directions.West);
                }

                GameObject eastWall = Instantiate (wall, new Vector3 (upscaledX, 0, upscaledZ + (downScaledSize)), Quaternion.identity) as GameObject;
                eastWall.name = "East Wall " + x + "," + z;

                mazeCell.AddWall(eastWall, Directions.East);

                if (x == 0)
                {
                        GameObject northWall = Instantiate(wall,
                                new Vector3((upscaledX) - (downScaledSize), 0, upscaledZ),
                                Quaternion.identity);
                        northWall.name = "North Wall " + x + "," + z;
                        northWall.transform.Rotate (Vector3.up * 90f);
                        mazeCell.AddWall(northWall, Directions.North);
                } 

                GameObject southWall = Instantiate(wall,
                        new Vector3((upscaledX) + (downScaledSize), 0, upscaledZ), Quaternion.identity);
                southWall.name = "South Wall " + x + "," + z;
                southWall.transform.Rotate (Vector3.up * 90f);
                mazeCell.AddWall(southWall, Directions.South);
        }

}
