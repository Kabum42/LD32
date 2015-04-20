using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour
{
	Rigidbody my_rigidbody;
	public float jumpMagnitude = 600;
	public float doubleJumpMagnitude = 540;
	public bool canJump = true;
	public bool canDoubleJump = false;
	public bool canBounce = false;
	float hitDist = 0.1f;
	public float bounceDist = 500;
	public float verticalJump = 400;
	bool keyReleased = false;
	Vector3 normalDir;
	LayerMask obstacles;
	RaycastHit hit;
	float bounceTimer = 0.6f;

	// Use this for initialization
	void Start ()
	{
		normalDir = new Vector3 ();
		my_rigidbody = GetComponent<Rigidbody> ();
		obstacles = LayerMask.GetMask ("Obstacle");
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.Space))
			keyReleased = true;

		if (my_rigidbody.velocity.y < -0.1f) {
			this.GetComponent<Animator> ().SetBool ("Falling", true);
			this.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().SetBool ("Falling", true);
		} else {
			this.GetComponent<Animator> ().SetBool ("Falling", false);
			this.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().SetBool ("Falling", false);
			/*
			if (this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Falling")) {
				this.GetComponent<Animator> ().Play("Idle");
				this.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().Play("Idle");
			}
			*/
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{




		if (Input.GetKeyDown (KeyCode.Space)) {
		
			if (canBounce && keyReleased) {

				if (!Physics.Raycast (transform.position, -Vector3.up, out hit, 0.5f, obstacles)) {
					my_rigidbody.velocity = new Vector3 (0, 0, 0);
					my_rigidbody.AddForce (bounceDist * normalDir + new Vector3 (0, verticalJump, 0), ForceMode.Impulse);
					canBounce = false;
					bounceTimer = 0;
				}
			}
			
			if (bounceTimer <= 0.5) {
				bounceTimer += 0.1f;
			}
	
			if (my_rigidbody.velocity.y <= 0) {
				
				if (canJump) {
					
					my_rigidbody.AddForce (Vector3.up * jumpMagnitude, ForceMode.Impulse);
					canJump = false;
					canDoubleJump = true;
					keyReleased = false;
					this.GetComponent<Animator> ().Play("Jumping");
					this.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().Play("Jumping");
				}
				
				if (canDoubleJump && keyReleased) {
					
					my_rigidbody.velocity = new Vector3 (0, 0, 0);
					my_rigidbody.AddForce (Vector3.up * doubleJumpMagnitude, ForceMode.Impulse);
					canDoubleJump = false;
					keyReleased = false;
					this.GetComponent<Animator> ().Play("Balling");
					this.GetComponent<SkeletonScript> ().feet.GetComponent<Animator> ().Play("Balling");
				}
			}	
		}
		


		//check collision with walls an apply forces
		
		if (!canJump && keyReleased && bounceTimer > 0.5) {
			if (Physics.Raycast (transform.position, Vector3.forward, out hit, 1, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, -Vector3.forward, out hit, 1, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, Vector3.left, out hit, 1, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, Vector3.right, out hit, 1, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
			} else
				canBounce = false;	
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		canJump = true;
		canBounce = false;	
		canDoubleJump = false;
	}
}
