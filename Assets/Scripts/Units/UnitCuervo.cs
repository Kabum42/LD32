using UnityEngine;
using System.Collections;

public class UnitCuervo : MonoBehaviour {

	public Transform target;
	Rigidbody targetRigidbody;
	Rigidbody rigidbody;
	public float speed = 200f;
	public float attackSpeed = 350f;
	public float heightOfFlight = 3f;
	public float forcedDrag = 1f;

	public LayerMask obstacles;

	public int pathCount = -1;
	int pathFee = 3;
	Vector3 currentPathPoint;
	public bool attacking = false;
	float stalkRadius = 10f;
	
	void Awake(){
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.drag = forcedDrag;
		targetRigidbody = target.GetComponent<Rigidbody>();
		obstacles = LayerMask.GetMask("Obstacle");
		heightOfFlight *= 2f;
		currentPathPoint = transform.position;
	}

	void FixedUpdate(){

		if (!attacking){
			// Busqueda de camino / Atacar
			if(Vector3.Distance(transform.position, currentPathPoint) < 2.5f){
				if (pathCount >= pathFee && !Physics.Raycast(transform.position + Vector3.up, (target.transform.position - transform.position).normalized, Vector3.Distance(transform.position, target.transform.position), obstacles)){
					pathCount = 0 + (int)Random.value * 3;
					attacking = true;
					rigidbody.velocity = (target.transform.position - transform.position).normalized;
				}
				else{
					currentPathPoint = RequestNewPathPoint();
					pathCount++;
				}
			}
			if(!attacking){
				// Evitar obstaculos
				while(Physics.Raycast (transform.position + Vector3.up * 2f, currentPathPoint - transform.position, rigidbody.velocity.magnitude, obstacles)){
					currentPathPoint = RequestNewPathPoint();
				}

				rigidbody.AddForce((currentPathPoint - transform.position) * speed * Time.deltaTime);

			}
		}
		else{
			Vector2 targetVector = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
			Vector2 movementVector = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
			if(Vector2.Angle(movementVector, targetVector) < 90f && Mathf.Abs(target.transform.position.y - transform.position.y) > 1f){
				rigidbody.AddForce((target.transform.position - transform.position) * attackSpeed * Time.deltaTime);
			}
			else{
				attacking = false;
				currentPathPoint = RequestNewPathPoint();
			}
		}
	}

	private Vector3 RequestNewPathPoint(){
		Vector2 newPoint = Random.insideUnitCircle * stalkRadius;
		Vector3 finalPoint = new Vector3(newPoint.x + target.transform.position.x, heightOfFlight, newPoint.y + target.transform.position.z);
		return finalPoint;
	}
}
