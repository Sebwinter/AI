using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //public Transform target;
    //float speed = 20;
    //Vector3[] path;
    //int targetIndex;

    public float movementSpeed = 1.5f;

    Camera cam;

    public LayerMask movementMask;

   

    void Start()
    {
        cam = Camera.main;

    }

    private void Update()
    {
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

        //Movement
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                
                
                //FindPath(seeker.position, hit.point);


            }

        }
    }

    //public void PathFound(Vector3[] newPath, bool correctPath)
    //{
    //    if(correctPath)
    //    {
    //        path = newPath;
    //        targetIndex = 0;
    //        StopCoroutine("MoveToPoint");
    //        StartCoroutine("MoveToPoint");
    //    }
    //}

    //IEnumerable MoveToPoint()
    //{
    //    Vector3 currentPath = path[0];
    //    while (true)
    //    {
    //        if (transform.position == currentPath)
    //        {
    //            targetIndex++;

    //            if (targetIndex >= path.Length)
    //            {
    //                yield break;
    //            }

    //            currentPath = path[targetIndex];

    //        }

    //        transform.position = Vector3.MoveTowards(transform.position, currentPath, movementSpeed * Time.deltaTime);
    //        yield return null;
    //    }

    //}


}
