using UnityEngine;
using System.Collections;

public class CrackProjector : MonoBehaviour
{
	int crackNum;
	float testTime = 3;
	bool test = true;
	Transform transform;
	
	void Start ()
	{
		transform = this.GetComponent<Transform> ();
	}

	// Use this for initialization
	void Update ()
	{
	
		testTime -= 0.1f;
	
		if (testTime <= 0 && true) {
		
			createCrack (Vector3.zero);
			testTime = -1;
			test = false;
		}

	}
	
	void createCrack (Vector3 position)
	{
		crackNum = projectorManager.currentCrack;
		if (crackNum >= projectorManager.MAX_CRACK - 1)
			crackNum = 0;
		
		projectorManager.currentCrack = crackNum;
		
		if (!projectorManager.crackArray [crackNum].activeSelf)
			projectorManager.crackArray [crackNum].SetActive (true);
			
		projectorManager.crackArray [crackNum].transform.position = new Vector3 (transform.position.x, transform.position.y + 16, transform.position.z);
		
		projectorManager.currentCrack += 1;
	}
}
