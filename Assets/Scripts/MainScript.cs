using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
	
	public GameObject player;
	Vector3 movement;
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	public int floorMask; 
	public int targetableMask; // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	public float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	public GameObject targetIzq = null;
	public GameObject targetDer = null;
	
	public GameObject mirillaIzq;
	public GameObject mirillaDer;
	
	public GameObject nextMirilla = null;
	private GameObject nextTarget = null;
	
	private float maxCooldown = 1f;
	private float cooldown = 1f;
	
	public bool killAnimation = false;
	public float killAnimationTimer = 0.9f;

	public GameObject conejomierda;
	
	// Use this for initialization
	void Start () {
		
		playerRigidbody = player.GetComponent<Rigidbody> ();
		
		floorMask = LayerMask.GetMask("Floor");
		targetableMask = LayerMask.GetMask("Targetable");
		
	}
	
	// Update is called once per frame
	void Update() {

		/*
		GameObject bla = conejomierda.transform.FindChild("Armature.001/Stomach/Chest/Head/Lower Ear.L").gameObject;
		player.transform.position = bla.transform.position;
		player.transform.rotation = bla.transform.rotation;
		*/
		
		transform.position = player.transform.position;
		
		if (killAnimation) {
			
			killAnimationTimer -= Time.deltaTime*(1f/Time.timeScale);
			if (killAnimationTimer <= -0.9f) {
				killAnimationTimer = 0.9f;
				killAnimation = false;

				player.GetComponent<Animator> ().SetTrigger("Executed");
				player.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().SetTrigger("Executed");
				player.GetComponent<SkeletonScript> ().leftHand.GetComponent<Animator> ().SetTrigger("Executed");
				player.GetComponent<SkeletonScript> ().rightHand.GetComponent<Animator> ().SetTrigger("Executed");
				
			}
		}
		
		cooldown += Time.deltaTime;
		if (cooldown >= maxCooldown) {
			cooldown = maxCooldown;
		}

		float aux = (maxCooldown - 0.15f) / 0.75f;

		mirillaIzq.transform.localScale = new Vector3(0.15f + 0.15f * aux, 0.15f + 0.15f * aux, 0.15f + 0.15f * aux);
		mirillaDer.transform.localScale = new Vector3(0.15f + 0.15f * aux, 0.15f + 0.15f * aux, 0.15f + 0.15f * aux);

		
		if (Input.GetMouseButtonDown (0) && (targetIzq != null || targetDer != null)) {
			
			player.GetComponent<AudioSource> ().Play ();
			cooldown = 0;
			maxCooldown = 1f;
			
			if (nextTarget!= null) {
				nextTarget.GetComponent<LifeScript>().lifePoints -= 10f;
				nextTarget.GetComponent<shotEmission>().createEmission(nextTarget.transform.position - player.transform.position, nextTarget.transform.position + new Vector3(0, 2, 0));
				
				if (nextTarget.GetComponent<LifeScript>().lifePoints <= 0f) {

					
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
		} else if (Input.GetMouseButton (0) && (targetIzq != null || targetDer != null)) {
			
			maxCooldown -= Time.deltaTime/2f;
			if (maxCooldown <= 0.15f) { maxCooldown = 0.15f; }
			
			if (cooldown >= maxCooldown) {
				cooldown = 0;
				player.GetComponent<AudioSource> ().Play ();
				
				if (nextTarget!= null) {
					nextTarget.GetComponent<LifeScript>().lifePoints -= 10f;
					nextTarget.GetComponent<shotEmission>().createEmission(nextTarget.transform.position - player.transform.position, nextTarget.transform.position + new Vector3(0, 2, 0));
					
					if (nextTarget.GetComponent<LifeScript>().lifePoints <= 0f) {

						
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
			if (Physics.Raycast(ray, out hit, 100f, targetableMask)) {
				if (hit.transform.gameObject == player) {
					targetIzq = null;
					targetDer = null;
					
					nextMirilla = null;
					nextTarget = null;
				}
			}
			
			
			if (nextMirilla == null) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 100f, targetableMask)) {
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
				if (Physics.Raycast(ray, out hit, 100f, targetableMask)) {
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
				if (Physics.Raycast(ray, out hit, 100f, targetableMask)) {
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

		
		
		//invisibleFloor.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
		
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		if (!killAnimation) {
			Move (h, v);
		}

		
		
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
		
		
		//Facing();


		
	}
	
	void Move(float h, float v) {

		if (h == 0 && v == 0) {
			player.GetComponent<Animator> ().SetBool ("Moving", false);
			player.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().SetBool ("Moving", false);
			player.GetComponent<SkeletonScript> ().leftHand.GetComponent<Animator> ().SetBool ("Moving", false);
			player.GetComponent<SkeletonScript> ().rightHand.GetComponent<Animator> ().SetBool ("Moving", false);
		} else {
			player.GetComponent<Animator>().SetBool("Moving", true);
			player.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().SetBool ("Moving", true);
			player.GetComponent<SkeletonScript> ().leftHand.GetComponent<Animator> ().SetBool ("Moving", true);
			player.GetComponent<SkeletonScript> ().rightHand.GetComponent<Animator> ().SetBool ("Moving", true);
		}
		
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
				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				
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