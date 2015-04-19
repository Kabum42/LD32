using UnityEngine;
using System.Collections;

public class shotEmission : MonoBehaviour
{
	Transform myTransform;
	GameObject myParticles;
	// Use this for initialization
	void Start ()
	{
		myTransform = GetComponent<Transform> ();
		
		Vector3 direction = new Vector3 (1, 0, 0);
		direction.Normalize ();
		Vector3 position = new Vector3 (myTransform.position.x, myTransform.position.y + 1.89f, myTransform.position.z);
		createEmission (direction, position);
	}
	
	void createEmission (Vector3 direction, Vector3 position)
	{
		myParticles = (GameObject)(Instantiate (Resources.Load ("Blood"), new Vector3 (position.x, position.y, position.z), Quaternion.identity));

		myParticles.transform.LookAt (myTransform.position + direction);

	}
}
