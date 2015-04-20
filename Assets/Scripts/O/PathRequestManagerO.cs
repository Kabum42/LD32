using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManagerO : MonoBehaviour {

	Queue<PathRequestO> pathRequestQueue = new Queue<PathRequestO>();
	PathRequestO currentPathRequest;

	static PathRequestManagerO instance;
	PathfindingO pathfinding;

	bool isProcessingPath;

	void Awake() {
		instance = this;
		pathfinding = GetComponent<PathfindingO>();
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[]> callback){
		PathRequestO newRequest = new PathRequestO(pathStart, pathEnd, callback);
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
	struct PathRequestO {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[]> callback;

		public PathRequestO(Vector3 _start, Vector3 _end, Action<Vector3[]> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}
