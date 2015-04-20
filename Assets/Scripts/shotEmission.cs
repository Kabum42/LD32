using UnityEngine;
using System.Collections;

public class shotEmission : MonoBehaviour
{
	Transform myTransform;
	GameObject myParticles;
	GameObject myBlood;
	// Use this for initialization
	void Awake ()
	{
		myTransform = GetComponent<Transform> ();
		
		Vector3 direction = new Vector3 (1, 0, 0);
		direction.Normalize ();
		Vector3 position = new Vector3 (myTransform.position.x, myTransform.position.y + 2f, myTransform.position.z);
		createEmission (direction, position);
		createShot (direction, position);
	}
	
	public void createEmission (Vector3 direction, Vector3 position)
	{
		myParticles = (GameObject)(Instantiate (Resources.Load ("Blood"), new Vector3 (position.x, position.y, position.z), Quaternion.identity));
		myParticles.transform.LookAt (myTransform.position + new Vector3 (0, 2, 0) + direction);
	}
	
	void createShot (Vector3 direction, Vector3 position)
	{
		myBlood = (GameObject)(Instantiate (Resources.Load ("BloodSpatter"), new Vector3 (position.x, position.y, position.z), Quaternion.identity));
		myBlood.transform.LookAt (myTransform.position + new Vector3 (0, 2, 0) + direction);
	}
}
