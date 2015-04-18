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

	public GameObject invisibleFloor;


	private GameObject targetIzq = null;
	private GameObject targetDer = null;

	public GameObject mirillaIzq;
	public GameObject mirillaDer;

	public GameObject nextMirilla = null;
	private GameObject nextTarget = null;

	private float maxCooldown = 1f;
	private float cooldown = 1f;

	public bool killAnimation = false;
	public float killAnimationTimer = 0.9f;


	// Use this for initialization
	void Start ()
	{

		playerRigidbody = player.GetComponent<Rigidbody> ();

		floorMask = LayerMask.GetMask ("Floor");
	
	}
	
	// Update is called once per frame

	void Update() {

		transform.position = player.transform.position;

		if (killAnimation) {
			killAnimationTimer -= Time.deltaTime*10f;
			if (killAnimationTimer <= -0.9f) {
				killAnimationTimer = 0.9f;
				killAnimation = false;
			}
		}

		cooldown += Time.deltaTime;
		if (cooldown >= maxCooldown) {
			cooldown = maxCooldown;
		}
		
		if (Input.GetMouseButtonDown (0)) {

			player.GetComponent<AudioSource> ().Play ();
			cooldown = 0;
			maxCooldown = 1f;
			
			if (nextTarget!= null) {
				nextTarget.GetComponent<LifeScript>().lifePoints -= 10f;

				if (nextTarget.GetComponent<LifeScript>().lifePoints <= 0f) {

					killAnimation = true;
					killAnimationTimer = 0.9f;
					this.GetComponent<AudioSource> ().Play ();

					if (targetIzq == targetDer) {
						targetIzq = null;
						targetDer = null;

						nextMirilla = null;
						nextTarget = null;
					}
					else if (nextTarget == targetIzq) {
						targetIzq = targetDer;
						nextTarget = targetDer;
					}
					else if (nextTarget == targetDer) {
						targetDer = targetIzq;
						nextTarget = targetIzq;
					}
				}
				else {
					// EL TARGET AL QUE LE DISPARO, SIGUE VIVO
					if (nextTarget == targetIzq) {
						nextTarget = targetDer;
					}
					else if (nextTarget == targetDer) {
						nextTarget = targetIzq;
					}
				}
			}
		} else if (Input.GetMouseButton (0)) {
			
			maxCooldown -= Time.deltaTime/2f;
			if (maxCooldown <= 0.15f) { maxCooldown = 0.15f; }
			
			if (cooldown >= maxCooldown) {
				cooldown = 0;
				player.GetComponent<AudioSource> ().Play ();
				
				if (nextTarget!= null) {
					nextTarget.GetComponent<LifeScript>().lifePoints -= 10f;

					if (nextTarget.GetComponent<LifeScript>().lifePoints <= 0f) {

						killAnimation = true;
						killAnimationTimer = 0.9f;
						this.GetComponent<AudioSource> ().Play ();

						if (targetIzq == targetDer) {
							targetIzq = null;
							targetDer = null;

							nextMirilla = null;
							nextTarget = null;
						}
						else if (nextTarget == targetIzq) {
							targetIzq = targetDer;
							nextTarget = targetDer;
						}
						else if (nextTarget == targetDer) {
							targetDer = targetIzq;
							nextTarget = targetIzq;
						}
					}
					else {
						// EL TARGET AL QUE LE DISPARO, SIGUE VIVO
						if (nextTarget == targetIzq) {
							nextTarget = targetDer;
						}
						else if (nextTarget == targetDer) {
							nextTarget = targetIzq;
						}
					}

				}
			}
		}
		else {
			maxCooldown = 1f;
		}


		if (Input.GetMouseButtonDown (1)) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.gameObject == player) {
					targetIzq = null;
					targetDer = null;
					
					nextMirilla = null;
					nextTarget = null;
				}
			}

			
			if (nextMirilla == null) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.tag == "Targetable") {
						targetIzq = hit.transform.gameObject;
						targetDer = hit.transform.gameObject;
						
						nextMirilla = mirillaDer;
						nextTarget = targetIzq;
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

				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

				
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
