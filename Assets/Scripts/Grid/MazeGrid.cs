using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGrid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
        public Vector3 OriginPosition { get; set; }
        public MazeCell[,] Grid { get; set; }

        public MazeGrid(int x, int z, Vector3 originPosition, int cellSize)
        {
            Height = x;
            Width = z;
            OriginPosition = originPosition;
            CellSize = cellSize;
            
            Grid = new MazeCell[x, z];
        }

        /*
         * Gets all neighbours of given cell and returns them.
         */
        public Dictionary<Directions, MazeCell> GetNeighbours(MazeCell cell)
        {
            Dictionary<Directions, MazeCell> neighbours = new Dictionary<Directions, MazeCell>();

            foreach (Directions direction in Enum.GetValues(typeof(Directions)))
            {
                MazeCell cellByDirection = GetCellByDirection(cell, direction);

                if (cellByDirection == null) continue;

                neighbours.Add(direction, cellByDirection);
            }
            
            return neighbours;
        }

        /*
        * Returns true if cell has neighbours that are visited.
        */
        public bool HasVisitedNeighbour(MazeCell cell)
        {
            return GetNeighboursByVisitedStatus(cell, true).Count > 0;
        }
        
        /*
         * Returns true if cell has neighbours that aren't visited.
         */
        public bool HasNonVisitedNeighbour(MazeCell cell)
        {
            return GetNeighboursByVisitedStatus(cell, false).Count > 0;
        }
        
        /*
         * Get all neighbours of given cell based on if they are visited or not and returns them.
         */
        public Dictionary<Directions, MazeCell> GetNeighboursByVisitedStatus(MazeCell cell, bool visited)
        {
            Dictionary<Directions, MazeCell> neighbours = new Dictionary<Directions, MazeCell>();
         
            foreach (KeyValuePair<Directions,MazeCell> element in GetNeighbours(cell))
            {
                Directions direction = element.Key;
                MazeCell mazeCell = element.Value;
             
                if (mazeCell.Visited == visited)
                {
                    neighbours.Add(direction, mazeCell);
                }
            }

            return neighbours;
        }
        
        /*
         * Gets all neighbours of given cell that are visited/non visited.
         * Returns a random neighbour based on the visit status.
         * Warning: Pair could be empty if cell doesn't have visited/non visited neighbours.
         */
        public KeyValuePair<Directions, MazeCell>? GetRandomNeighbourByVisitedStatus(MazeCell cell, bool visitStatus)
        {
            Dictionary<Directions, MazeCell> neighbours = GetNeighboursByVisitedStatus(cell, visitStatus);

            if (neighbours.Count == 0) return null;

            int randomIndex = Random.Range(0, neighbours.Count);

            return neighbours.ElementAt(randomIndex);;
        }

        /*
         * Calculates where the cell is based on the given cell and the direction of the given cell.
         * Returns cell based on currentCell and its direction.
         */
        public MazeCell GetCellByDirection(MazeCell currentCell, Directions direction)
        {
            int x = currentCell.X;
            int z = currentCell.Z;
            
            switch (direction)
            {
                case Directions.North:
                    return GetCell(x - 1, z);
                case Directions.South:
                    return GetCell(x + 1, z);
                case Directions.East:
                    return GetCell(x, z + 1);
                case Directions.West:
                    return GetCell(x, z - 1);
            }

            return null;
        }

        /*
         * Returns cell based on their respective X and Z.
         * Warning: could be null when X or Z is out of bounds (off the grid).
         */
        public MazeCell GetCell(int x, int z)
        {
            if (IsOutOfBounds(x, z)) return null;

            return Grid[x, z];
        }
        
        public void Destroy()
        {

            for (int x = 0; x < Height; x++)
            {
                for (int z = 0; z < Width; z++)
                {
                    MazeCell cell = GetCell(x, z);
                    
                    cell.DestroyWalls();
                    cell.DestroyFloor();
                }
            }
            

            // Prevent garbage collection
            Grid = null;
        }

        public void AddCell(MazeCell cell)
        {
            Grid[cell.X, cell.Z] = cell;
        }

        public bool IsOutOfBounds(int x, int z)
        {
            return x >= Height || x < 0 || z < 0 || z >= Width;
        }
    }
