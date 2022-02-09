using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
 
public class MyGrid : MonoBehaviour
{
    public Transform player;

    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }
     

    public void Update()
    {
        //CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        //used to get the bottom left corner of the map.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; 
        for(int x = 0; x < gridSizeX; x ++)
        {
            for (int y = 0; y <gridSizeY; y ++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

            }
        }

    }

    public Node NodeFromWorldPoint(Vector3 worldPostion)
    {
        float percentX = (worldPostion.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPostion.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

  
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <=1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighours;
    }

    //Gizmo Tests


    public List<Node> path;

    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if(grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);

            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if(path != null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                              
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));

            }
        }

    }

  
}
