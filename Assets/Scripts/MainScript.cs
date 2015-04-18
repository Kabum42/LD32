using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{

	public GameObject player;
	Vector3 movement;
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	public int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	public GameObject target;

	// Use this for initialization
	void Start ()
	{

		playerRigidbody = player.GetComponent<Rigidbody> ();

		floorMask = LayerMask.GetMask ("Floor");
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		/*
		if(Input.GetMouseButtonDown(0))
			Debug.Log("Pressed left click.");
		if(Input.GetMouseButtonDown(1))
			Debug.Log("Pressed right click.");
		*/

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);


		if (Input.GetMouseButtonDown (1)) {

			target = null;

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.tag == "Targetable") {
					target = hit.transform.gameObject;
				}
			}
		}


		Facing ();
		//Debug.Log ("LOL");
	
	}

	void Move (float h, float v)
	{

		float speed = 10f;

		movement.Set (h, 0f, v);

		movement = movement.normalized * speed; //* Time.deltaTime;

		//playerRigidbody.MovePosition (player.transform.position + movement);
		
		transform.position = Vector3.Lerp (player.transform.position, player.transform.position + movement, Time.deltaTime); 

	}

	void Facing ()
	{


		if (target == null) {

			// GIRA SEGUN EL RATON Y EL SUELO
			// Create a ray from the mouse cursor on screen in the direction of the camera.
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			// Create a RaycastHit variable to store information about what was hit by the ray.
			RaycastHit floorHit;
			
			// Perform the raycast and if it hits something on the floor layer...
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
				// Create a vector from the player to the point on the floor the raycast from the mouse hit.
				Vector3 playerToMouse = floorHit.point - player.transform.position;
				
				// Ensure the vector is entirely along the floor plane.
				playerToMouse.y = 0f;
				
				// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
				Quaternion newRotation = Quaternion.LookRotation (-playerToMouse);
				
				// Set the player's rotation to this new rotation.
				playerRigidbody.MoveRotation (newRotation);
			}

		} else {

			// GIRA MIRANDO AL TARGET
			Vector3 playerToTarget = target.transform.position - player.transform.position;
			
			// Ensure the vector is entirely along the floor plane.
			playerToTarget.y = 0f;
			
			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (-playerToTarget);
			
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);

		}

        
	}

}
