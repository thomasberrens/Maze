using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public MeshRenderer FloorMesh { get; set; }
    
    public Dictionary<Directions, GameObject> Walls { get; private set; }

    public bool Visited { get; set; }

    public int X { get; set; }
    public int Z { get; set; }
    
    public int Size { get; set; }

    public MazeCell(int x, int z, int size)
    {
        X = x;
        Z = z;
        Size = size;
        
        Walls = new Dictionary<Directions, GameObject>();
    }

    public void SetFloorColor(Color color)
    {
        FloorMesh.material.color = color;
    }

    public void DestroyWall(Directions directions)
    {

        if (!Walls.ContainsKey(directions)) return;

        GameObject wall = Walls[directions];

        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
        
        Walls.Remove(directions);
    }

    public void DestroyWalls()
    {
        foreach (GameObject wall in Walls.Values)
        {
            GameObject.Destroy(wall);
        }
    }

    public void DestroyFloor()
    {
        GameObject.Destroy(FloorMesh.gameObject);
    }

    public void AddWall(GameObject wall, Directions direction)
    {
        Walls.Add(direction, wall);
    }
}
