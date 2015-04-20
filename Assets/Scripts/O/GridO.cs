using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridO : MonoBehaviour {

	public Transform player;
	public bool displayGridGizmos;
	public bool onlyDisplayPathGizmos;

	public LayerMask heightMask;
	public LayerMask obstacles;
	public int cubeSize = 2;
	public int maxHeight;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	NodeO[,] grid;
	public List<NodeO> path;


	float nodeDiameter;
	int gridSizeX, gridSizeY;
	public int maxHeapSize{
		get{
			return gridSizeX * gridSizeY;
		}
	}

	void Awake() {
		maxHeight = 10 * cubeSize;
		obstacles = LayerMask.GetMask("Obstacle");
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}


	void CreateGrid() {
		grid = new NodeO[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

				RaycastHit hit;
				Physics.Raycast(worldPoint + Vector3.up * maxHeight, -Vector3.up, out hit, (float)maxHeight + 1f, obstacles);

				int height = (maxHeight - (int) hit.distance) / cubeSize;
				grid[x,y] = new NodeO(height, worldPoint, x, y);

			}
		}
	}


	public List<NodeO> GetNeighbours(NodeO node){
		List<NodeO> neighbours = new List<NodeO>();

		for (int x = -1; x <= 1; x++){
			for (int y = -1; y <= 1; y++){
				if( x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					if(grid[checkX, checkY].height - node.height <= 1) //walkable
						neighbours.Add(grid[checkX, checkY]);
				}

			}
		}
		return neighbours;
	}


	public NodeO NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid [x, y];
	}


	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));
		if (displayGridGizmos){
			if (onlyDisplayPathGizmos){
				if (path != null){
					foreach (NodeO n in path){
						Gizmos.color = Color.black;
						Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter-.1f));
					}
				}
			}
			else{
				if (grid != null) {
					NodeO playerNode = NodeFromWorldPoint(player.position);
					foreach(NodeO n in grid){

						Gizmos.color = (n.walkable)?Color.white:Color.red;

						if (playerNode == n) Gizmos.color = Color.blue;

						if (path != null)
							if (path.Contains(n))
								Gizmos.color = Color.black;

						Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter-.1f));
					}
				}
			}
		}
	}
}
