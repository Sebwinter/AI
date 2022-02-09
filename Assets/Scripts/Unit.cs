using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    Vector3[] path;
    int targetIndex;

    Camera cam;
    public LayerMask movementMask;

    private void Start()
    {
        cam = Camera.main;
                
    }

    private void Update()
    {
        //Player is always facing the mouse;

        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);

        }

        if (Input.GetMouseButton(0))
        {

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                RequestPath.requestPath(transform.position, hit.point, OnPathFound);

            }

        }
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWayPoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed);
            yield return null;
        }
    }
}
