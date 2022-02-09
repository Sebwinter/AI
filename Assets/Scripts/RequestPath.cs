using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RequestPath : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static RequestPath instance;
    bool isProcessingPath;

    MyAI Ai;

    private void Awake()
    {
        instance = this;
        Ai = GetComponent<MyAI>();
    }
    public static void requestPath(Vector3 pathStart,Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestsQueue.Count>0)
        {
            currentPathRequest = pathRequestsQueue.Dequeue();
            isProcessingPath = true;
            Ai.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 Start, Vector3 End, Action<Vector3[], bool> CallBack)
        {
            pathStart = Start;
            pathEnd = End;
            callBack = CallBack;
        }

    }
   
}
