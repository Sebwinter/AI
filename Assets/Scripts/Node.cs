using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;
    public Node(bool Walkable, Vector3 WorldPos, int GridX, int GridY)
    {
        walkable = Walkable;
        worldPosition = WorldPos;
        gridX = GridX;
        gridY = GridY;

    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}
