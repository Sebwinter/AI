using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyAI : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    RequestPath requestManager; 
    MyGrid grid;
    public float movementSpeed = 1.5f;

    Camera cam;
    public LayerMask movementMask;

    public void Awake()
    {
        requestManager = GetComponent<RequestPath>();
        grid = GetComponent<MyGrid>();
        cam = Camera.main;

    }
    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                FindPath(seeker.position, hit.point);
                       
            }

        }

    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            { 
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                { 
					if (openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];
				}
			}

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);


            if (currentNode == targetNode)
            {

                pathSuccess = true;
                break;
            }

            foreach (Node neighour in grid.GetNeighbours(currentNode))
            {
                if (!neighour.walkable || closedSet.Contains(neighour))
                {
                    continue;
                }

                int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighour);
                if (newMovementCost < neighour.gCost || !openSet.Contains(neighour))
                {
                    neighour.gCost = newMovementCost;
                    neighour.hCost = GetDistance(neighour, targetNode);
                    neighour.parent = currentNode;

                    if (!openSet.Contains(neighour))
                    {
                        openSet.Add(neighour);
                    }


                }
            }
        }
        yield return null;
        
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }



    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
    int GetDistance(Node nodeA, Node nodeB )
    {
        int DistanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int DistanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(DistanceX > DistanceY)
        {
            return 14 * DistanceX + 10 * DistanceY;
        }

        return 14 * DistanceY + 10 * DistanceX;
    }

}
