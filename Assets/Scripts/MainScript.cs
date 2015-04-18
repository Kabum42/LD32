using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	public GameObject player;
	Vector3 movement;
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	public int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	public GameObject invisibleFloor;


	private GameObject targetIzq = null;
	private GameObject targetDer = null;

	public GameObject mirillaIzq;
	public GameObject mirillaDer;

	public GameObject nextMirilla = null;
	private GameObject lastMirilla = null;

	// Use this for initialization
	void Start () {

		playerRigidbody = player.GetComponent<Rigidbody> ();

		floorMask = LayerMask.GetMask("Floor");
	
	}
	
	// Update is called once per frame
	void Update() {

		if (Input.GetMouseButtonDown (1)) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.gameObject == player) {
					targetIzq = null;
					targetDer = null;
					
					nextMirilla = null;
				}
			}

			
			if (nextMirilla == null) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.tag == "Targetable") {
						targetIzq = hit.transform.gameObject;
						targetDer = hit.transform.gameObject;
						
						nextMirilla = mirillaDer;
					}
				}
			}
			else if (nextMirilla == mirillaDer) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.tag == "Targetable") {
						
						if (targetDer == hit.transform.gameObject) {
							targetIzq = hit.transform.gameObject;
							nextMirilla = mirillaDer;
						}
						else {
							targetDer = hit.transform.gameObject;
							nextMirilla = mirillaIzq;
						}
						
					}
				}
			}
			else if (nextMirilla == mirillaIzq) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.tag == "Targetable") {
						
						if (targetIzq == hit.transform.gameObject) {
							targetDer = hit.transform.gameObject;
							nextMirilla = mirillaIzq;
						}
						else {
							targetIzq = hit.transform.gameObject;
							nextMirilla = mirillaDer;
						}
						
					}
				}
			}
			
			
		}

	}

	void FixedUpdate () {
		/*
		if(Input.GetMouseButtonDown(0))
			Debug.Log("Pressed left click.");
		if(Input.GetMouseButtonDown(1))
			Debug.Log("Pressed right click.");
		*/

		invisibleFloor.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);


		if (targetIzq != null) {
			mirillaIzq.transform.position = targetIzq.transform.position + new Vector3 (0, 2, 0);
			mirillaIzq.transform.LookAt (Camera.main.transform.position, Vector3.up);
			mirillaIzq.transform.Rotate (new Vector3 (90, 0, 0));
		} else {
			mirillaIzq.transform.position = new Vector3(0, -999999, 0);
		}

		if (targetDer != null) {
			mirillaDer.transform.position = targetDer.transform.position + new Vector3 (0, 2, 0);
			mirillaDer.transform.LookAt (Camera.main.transform.position, Vector3.up);
			mirillaDer.transform.Rotate (new Vector3 (90, 0, 0));
		} else {
			mirillaDer.transform.position = new Vector3(0, -999999, 0);
		}


		Facing();
		//Debug.Log ("LOL");
	
	}

	void Move(float h, float v) {

		float speed = 6f;

		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (player.transform.position + movement);

	}

    void Facing()
    {


		if (targetIzq == null || true) {

			// GIRA SEGUN EL RATON Y EL SUELO
			// Create a ray from the mouse cursor on screen in the direction of the camera.
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Create a RaycastHit variable to store information about what was hit by the ray.
			RaycastHit floorHit;
			
			// Perform the raycast and if it hits something on the floor layer...
			if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
			{
				// Create a vector from the player to the point on the floor the raycast from the mouse hit.
				Vector3 playerToMouse = floorHit.point - player.transform.position;
				
				// Ensure the vector is entirely along the floor plane.
				playerToMouse.y = 0f;
				
				// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
				Quaternion newRotation = Quaternion.LookRotation(-playerToMouse);
				
				// Set the player's rotation to this new rotation.
				playerRigidbody.MoveRotation(newRotation);
			}

		} else {

			// GIRA MIRANDO AL TARGET
			Vector3 playerToTarget = targetIzq.transform.position - player.transform.position;
			
			// Ensure the vector is entirely along the floor plane.
			playerToTarget.y = 0f;
			
			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation(-playerToTarget);
			
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation(newRotation);

		}

        
    }

}
