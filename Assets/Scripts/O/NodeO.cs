using UnityEngine;
using System.Collections;

public class NodeO : IOHeapItem<NodeO> {

	public bool walkable;//int height;
	public int height;
	public Vector3 worldPos;

	public int gCost;
	public int hCost;

	public NodeO parent;
	int heapIndex;

	public int gridX, gridY;

	public int startingDistance;

	public int OfCost {
		get {
			return Mathf.Abs(startingDistance - hCost) + gCost;
		}
	}

	public NodeO(int _height/*bool _walkable*/, Vector3 _worldPos, int _gridX, int _gridY){
		//walkable = _walkable;
		height = _height;
		worldPos = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int HeapIndex{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}

	public int CompareTo(NodeO nodeToCompare){
		int compare = OfCost.CompareTo(nodeToCompare.OfCost);
		/*if (compare == 0) {
			compare = -hCost.CompareTo(nodeToCompare.hCost);
		}*/
		return -compare;
	}
}
