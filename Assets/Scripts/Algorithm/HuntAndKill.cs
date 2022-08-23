using UnityEngine;

    public class HuntAndKill : MazeAlgorithm
    {
        private int currentRow;
        private int currentColumn;

        private bool courseComplete;

        public override void Initialize()
        {
            currentRow = 0;
            currentColumn = 0;
            courseComplete = false;
        }

        public override void GenerateMaze()
        {

            Kill(); // Will run until it hits a dead end.
            Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any the maze generation stops.
            
        }

        public override bool IsDone()
        {
            return courseComplete;
        }

        /*
         * Searches for a non visited cell with visited neighbours
         * if a cell like that is found it destroys walls next to the current cell, update the currentRow / currentColumn and go straight back to kill modus.
         */
        private void Hunt() {
            courseComplete = true; // Set it to this, and see if we can prove otherwise below!

            
            for (int x = 0; x < MazeGrid.Height; x++) {
                for (int z = 0; z < MazeGrid.Width; z++)
                {
                    MazeCell cell = MazeGrid.GetCell(x, z);

                    AddVisualFeedback(cell, Color.gray);
                    
                    
                    if (!cell.Visited && MazeGrid.HasVisitedNeighbour(cell)) {
                        // Found a unvisited cell so we go to kill modus.
                        courseComplete = false;
                        currentRow = x;
                        currentColumn = z;

                        AddVisualFeedback(cell, Color.blue);

                        MazeCell currentCell = GetCurrentCell();
                        
                        DestroyAdjacentWall (currentCell);
                        currentCell.Visited = true;
                        return;
                    }
                }
            }
        }

        /*
         * Kill phase loops while the current cell doesn't have any non visited neighbours
         * When it is looping it destroys walls in random directions of cells that are not visited.
         * Current cell is modified by the "IncrementOrDecrementByDirection" (since that function modifies currentRow/currentColumn)
         */
        private void Kill() {
         while (MazeGrid.HasNonVisitedNeighbour(GetCurrentCell())) {
                 MazeCell currentCell = GetCurrentCell();

                 Directions direction = DirectionUtil.GetRandomDirection();
                 MazeCell chosenCell = MazeGrid.GetCellByDirection(currentCell, direction);

                 if (chosenCell != null && !chosenCell.Visited)
                 {
                     IncrementOrDecrementByDirection(direction);

                     AddVisualFeedback(chosenCell, Color.red);
                     
                     
                     currentCell.DestroyWall(direction);
                     chosenCell.DestroyWall(DirectionUtil.GetOppositeDirection(direction));
                 }
                 
                 currentCell.Visited = true;
         }
        }
        
        /*
         * Loops till it can find a neighbour cell (perspective of given cell) and destroys the wall in a random direction.
         */
        private void DestroyAdjacentWall(MazeCell cell) {
            bool destroyedWall = false;

            while (!destroyedWall) {
                Directions direction = DirectionUtil.GetRandomDirection();
                MazeCell chosenCell = MazeGrid.GetCellByDirection(cell, direction);

                if (chosenCell != null && chosenCell.Visited)
                {
                    cell.DestroyWall(direction);
                    chosenCell.DestroyWall(DirectionUtil.GetOppositeDirection(direction));
                    destroyedWall = true;
                    
                }
            }

        }

        /*
        * Increments or decrements currentRow / currentColumn based on the given direction.
        */
        private void IncrementOrDecrementByDirection(Directions direction)
        {
            if (direction.Equals(Directions.North)) currentRow--;
            else if (direction.Equals(Directions.South)) currentRow++;
            else if (direction.Equals(Directions.East)) currentColumn++;
            else if (direction.Equals(Directions.West)) currentColumn--;
        }
        
        /*
        * Returns cell based on current row and current column (x, z)
        */
        private MazeCell GetCurrentCell()
        {
            return MazeGrid.GetCell(currentRow, currentColumn);
        }

        public HuntAndKill(MazeGrid grid) : base(grid)
        {
        }
    }