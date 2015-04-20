using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManagerB : MonoBehaviour {

	Queue<PathRequestB> pathRequestQueue = new Queue<PathRequestB>();
	PathRequestB currentPathRequest;

	static PathRequestManagerB instance;
	PathfindingB pathfinding;

	bool isProcessingPath;

	void Awake() {
		instance = this;
		pathfinding = GetComponent<PathfindingB>();
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[]> callback){
		PathRequestB newRequest = new PathRequestB(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	void TryProcessNext(){
		if (!isProcessingPath && pathRequestQueue.Count > 0){
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	public void finishedProcessingPath(Vector3[] path){
		currentPathRequest.callback(path);
		isProcessingPath = false;
		TryProcessNext();
	}
	struct PathRequestB {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[]> callback;

		public PathRequestB(Vector3 _start, Vector3 _end, Action<Vector3[]> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}
