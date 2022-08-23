using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using Random = UnityEngine.Random;

public abstract class MazeAlgorithm
{

    public MazeGrid MazeGrid { get; set; }
    
    public bool VisualFeedback { get; set; }

    public MazeAlgorithm(MazeGrid grid)
    {
        MazeGrid = grid;
    }

    protected void AddVisualFeedback(MazeCell cell, Color color)
    {
        if (!VisualFeedback) return;
        
        cell.SetFloorColor(color);
    }

    public abstract void Initialize();
    
    // Kinda the "Update" function of Unity, it acts like a loop.
    public abstract void GenerateMaze();

    // Determines if it should stop looping based if the algorithm is done generating.
    public abstract bool IsDone();

}
