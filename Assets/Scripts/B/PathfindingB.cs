using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathfindingB : MonoBehaviour {
	
	PathRequestManagerB requestManager;
	GridB grid;
	int searchRange = 50;

	void Awake() {
		requestManager = GetComponent<PathRequestManagerB>();
		grid = GetComponent<GridB> ();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

		int nodesSearched = 0;
		Vector3[] waypoints = new Vector3[0];
		//bool pathSuccess = false;

		NodeB startNode = grid.NodeFromWorldPoint (startPos);
		NodeB targetNode = grid.NodeFromWorldPoint (targetPos);

		HeapB<NodeB> openSet = new HeapB<NodeB>(grid.maxHeapSize);
		HashSet<NodeB> closedSet = new HashSet<NodeB>();
		openSet.Add (startNode);

		while (openSet.Count > 0 && nodesSearched < searchRange) {
			nodesSearched++;
			NodeB currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			foreach (NodeB neighbour in grid.GetNeighbours(currentNode)){

				if (neighbour.height - currentNode.height > 1/*!neighbour.walkable*/ || closedSet.Contains (neighbour)){
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
					else
						openSet.UpdateItem(neighbour);
				}
			}
		}
		yield return null;

		NodeB farthestNodeToTarget = new NodeB(0, Vector3.zero, 0, 0);
		farthestNodeToTarget.hCost = -1;
		foreach (NodeB n in closedSet){
			if(n.hCost > farthestNodeToTarget.hCost) farthestNodeToTarget = n;
		}
		targetNode = farthestNodeToTarget;

		/*NodeB optimalEscapeTarget = new NodeB(0, Vector3.zero, 0, 0);
		optimalEscapeTarget.hCost = 0; optimalEscapeTarget.gCost = 2147483647;
		foreach (NodeB n in closedSet){
			if(n.BfCost < optimalEscapeTarget.BfCost) optimalEscapeTarget = n;
		}*/

		waypoints = RetracePath(startNode,targetNode);
		requestManager.finishedProcessingPath(waypoints);
	}

	Vector3[] RetracePath(NodeB startNode, NodeB endNode){
		List<NodeB> path = new List<NodeB>();
		NodeB currentNode = endNode;

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

	Vector3[]SimplifyPath(List<NodeB>path){
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

	int GetDistance(NodeB nodeA, NodeB nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY)
			return 14*distY + 10*(distX-distY);
		return 14*distX + 10*(distY-distX);
	}
}
