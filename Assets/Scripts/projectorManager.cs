using UnityEngine;
using System.Collections;

public class projectorManager : MonoBehaviour
{
	public static GameObject[] projectorArray;
	public static GameObject[] crackArray;
	public static int currentProj;
	public static int currentCrack;
	public static int MAX_PROJ = 250;
	public static int MAX_CRACK = 3;
	string projectorType;
	

	// Use this for initialization
	void Start ()
	{
		crackArray = new GameObject[MAX_CRACK];
		
		for (int j = 0; j< crackArray.Length; j++) {
			
			Quaternion rotation = Quaternion.identity;
			rotation.eulerAngles = new Vector3 (90, Random.Range (0, 360), 0);
			
			crackArray [j] = (GameObject)(Instantiate (Resources.Load ("CrackProjector"), new Vector3 (0, 0, 0), rotation));
			crackArray [j].SetActive (false);
		}
		
		projectorArray = new GameObject[MAX_PROJ];
		
		for (int i = 0; i< projectorArray.Length; i++) {
		
			projectorArray [i] = (GameObject)(Instantiate (Resources.Load ("BloodProjector1"), new Vector3 (0, 0, 0), Quaternion.identity));
			projectorArray [i].SetActive (false);
		}
	}
}
