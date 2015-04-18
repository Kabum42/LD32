using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour {

	Rigidbody my_rigidbody;
	float jumpMagnitude = 20000f;
	bool canJump = true;
	float hitDist = 0.1f;

	// Use this for initialization
	void Start () {

		my_rigidbody = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Space) && canJump) {

			my_rigidbody.AddForce (Vector3.up * jumpMagnitude);
			canJump = false;
		}

		if (Physics.Raycast (transform.position, -Vector3.up, hitDist)) {

			canJump = true;
		}
	}
}
