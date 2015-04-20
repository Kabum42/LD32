using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathfindingO : MonoBehaviour {
	
	PathRequestManagerO requestManager;
	GridO grid;
	int searchRange = 100;
	int startingDistance;

	void Awake() {
		requestManager = GetComponent<PathRequestManagerO>();
		grid = GetComponent<GridO> ();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

		int nodesSearched = 0;
		Vector3[] waypoints = new Vector3[0];
		//bool pathSuccess = false;

		NodeO startNode = grid.NodeFromWorldPoint (startPos);
		NodeO targetNode = grid.NodeFromWorldPoint (targetPos);

		HeapO<NodeO> openSet = new HeapO<NodeO>(grid.maxHeapSize);
		HashSet<NodeO> closedSet = new HashSet<NodeO>();
		openSet.Add (startNode);

		startingDistance = startNode.hCost;

		while (openSet.Count > 0 && nodesSearched < searchRange) {
			nodesSearched++;
			NodeO currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			foreach (NodeO neighbour in grid.GetNeighbours(currentNode)){

				if (neighbour.height - currentNode.height > 1 || closedSet.Contains (neighbour)){
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;
					neighbour.startingDistance = startingDistance;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
					else
						openSet.UpdateItem(neighbour);
				}
			}
		}
		yield return null;

		NodeO farthestNodeFromOrigin = new NodeO(0, Vector3.zero, 0, 0);
		farthestNodeFromOrigin.gCost = -1;
		foreach (NodeO n in closedSet){
			if(n.gCost > farthestNodeFromOrigin.gCost) farthestNodeFromOrigin = n;
		}
		targetNode = farthestNodeFromOrigin;

		waypoints = RetracePath(startNode,targetNode);
		requestManager.finishedProcessingPath(waypoints);
	}

	Vector3[] RetracePath(NodeO startNode, NodeO endNode){
		List<NodeO> path = new List<NodeO>();
		NodeO currentNode = endNode;

		while (currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);

		path.Reverse();
		grid.path = path;

		return waypoints;
	}

	Vector3[]SimplifyPath(List<NodeO>path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++){
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld){
				waypoints.Add(path[i].worldPos);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int GetDistance(NodeO nodeA, NodeO nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY)
			return 14*distY + 10*(distX-distY);
		return 14*distX + 10*(distY-distX);
	}
}
