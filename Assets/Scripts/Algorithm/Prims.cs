
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Prims : MazeAlgorithm
    {

        // list of cells that are called "frontiers", a frontier is practically a unvisited neighbour of a cell.
        private List<MazeCell> frontierCells;

        public override void Initialize()
        {
            frontierCells = new List<MazeCell>();

            int randomX = Random.Range(0, MazeGrid.Height);
            int randomZ = Random.Range(0, MazeGrid.Width);

            MazeCell startCell = MazeGrid.GetCell(randomX, randomZ);

            AddFrontierCells(startCell);
            
            AddVisualFeedback(startCell, Color.black);
            startCell.Visited = true;
        }

        public override void GenerateMaze()
        {

            MazeCell randomFrontierCell = GetRandomFrontierCell();
            AddVisualFeedback(randomFrontierCell, Color.grey);
            
            KeyValuePair<Directions, MazeCell>? randomVisitedNeighbour = MazeGrid.GetRandomNeighbourByVisitedStatus(randomFrontierCell, true);
            if(!randomVisitedNeighbour.HasValue) return;
            
            // Get the direction and cell of a random visited neighbour
            KeyValuePair<Directions, MazeCell> element = randomVisitedNeighbour.Value;

            Directions direction = element.Key;
            MazeCell randomVisitedCell = element.Value;

            // destroy walls if random frontier cell isn't visited yet.
            if (!randomFrontierCell.Visited)
            {
                randomFrontierCell.DestroyWall(direction);
                randomVisitedCell.DestroyWall(DirectionUtil.GetOppositeDirection(direction));
            }

            // Remove randon frontier cell because it isn't a frontier cell anymore
            frontierCells.Remove(randomFrontierCell);
            randomFrontierCell.Visited = true;
            
            AddFrontierCells(randomFrontierCell);
            
        }

        /*
         * Gets all non visited neighbours of given cell and adds them to the frontier list
         */ 
        private void AddFrontierCells(MazeCell cell)
        {
            // get all non visited neighbours based on the random frontier cell
            List<MazeCell> frontierCells = GetFrontierCells(cell);
            
            // add all non visited neighbours to the frontier list
            frontierCells.ForEach(cell => AddFrontierCell(cell)); 
        }
        
     public override bool IsDone()
     {
         return frontierCells.Count == 0;
     }

     /*
      * Returns a list based on the neighbours of the cell and their visited status, if a cell is not visited it is identified as "frontier cell"
      */
     private List<MazeCell> GetFrontierCells(MazeCell cell)
     {
         return MazeGrid.GetNeighboursByVisitedStatus(cell, false).Values.ToList();
     }

     private void AddFrontierCell(MazeCell cell)
        {
            if (cell.Visited) return;

            frontierCells.Add(cell);
            
            AddVisualFeedback(cell, Color.red);
        }

     private MazeCell GetRandomFrontierCell()
     {
         int index = Random.Range(0, frontierCells.Count);
         
         return frontierCells[index];
     }

     public Prims(MazeGrid grid) : base(grid)
     {
     }
    }
