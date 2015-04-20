using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour {

	//public Transform seeker, target;
	PathRequestManager requestManager;
	Grid grid;
	int searchRange = 200;

	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid> ();
	}

	/*void Update() {
		FindPath(seeker.position, target.position);
	}*/

	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

		int nodesSearched = 0;
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		Heap<Node> openSet = new Heap<Node>(grid.maxHeapSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add (startNode);

		while (openSet.Count > 0 && nodesSearched < searchRange) {
			nodesSearched++;
			Node currentNode = openSet.RemoveFirst();
			/*for (int i = 1; i < openSet.Count; i++) {
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);*/
			closedSet.Add(currentNode);

			if (currentNode == targetNode){
				pathSuccess = true;
				//RetracePath(startNode, targetNode);
				break;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)){

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
		/*if (pathSuccess){
			waypoints = RetracePath(startNode,targetNode);
		}*/
		if (!pathSuccess){
			Node closestNodeToTarget = new Node(0, Vector3.up, 0, 0);
			closestNodeToTarget.hCost = 2147483647;
			foreach (Node n in closedSet){
				if(n.hCost < closestNodeToTarget.hCost) closestNodeToTarget = n;
			}
			targetNode = closestNodeToTarget;
		}
		pathSuccess = true;
		waypoints = RetracePath(startNode,targetNode);
		requestManager.finishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

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

	Vector3[]SimplifyPath(List<Node>path){
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

	int GetDistance(Node nodeA, Node nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY)
			return 14*distY + 10*(distX-distY);
		return 14*distX + 10*(distY-distX);
	}
}

