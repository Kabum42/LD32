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
	float hitDist = 2;
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
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Input.GetKey (KeyCode.Space)) {
		
			if (canBounce && keyReleased) {
				my_rigidbody.velocity = new Vector3 (0, 0, 0);
				my_rigidbody.AddForce (bounceDist * normalDir + new Vector3 (0, verticalJump, 0), ForceMode.Impulse);
				canBounce = false;
				bounceTimer = 0;
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
				}
				
				if (canDoubleJump && keyReleased) {
					
					my_rigidbody.velocity = new Vector3 (0, 0, 0);
					my_rigidbody.AddForce (Vector3.up * doubleJumpMagnitude, ForceMode.Impulse);
					canDoubleJump = false;
					keyReleased = false;
				}
			}	
		}
		
		if (Input.GetKeyUp (KeyCode.Space))
			keyReleased = true;

		//check collision with walls an apply forces
		
		if (!canJump && keyReleased && bounceTimer > 0.5) {
			if (Physics.Raycast (transform.position, Vector3.forward, out hit, hitDist, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, -Vector3.forward, out hit, hitDist, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, Vector3.left, out hit, hitDist, obstacles)) {
				canDoubleJump = false;
				canJump = false;
				canBounce = true;
				normalDir = hit.normal;
				
			} else if (Physics.Raycast (transform.position, Vector3.right, out hit, hitDist, obstacles)) {
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
