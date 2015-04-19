using UnityEngine;
using System.Collections;

public class projectorManager : MonoBehaviour
{
	public static GameObject[] projectorArray;
	public static int currentProj;
	public static int MAX_PROJ = 250;
	string projectorType;
	

	// Use this for initialization
	void Start ()
	{
		projectorArray = new GameObject[MAX_PROJ];
		
		for (int i = 0; i< projectorArray.Length; i++) {
		
			projectorArray [i] = (GameObject)(Instantiate (Resources.Load ("BloodProjector1"), new Vector3 (0, 0, 0), Quaternion.identity));
			projectorArray [i].SetActive (false);
		}
	}
}
