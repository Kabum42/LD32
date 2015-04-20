using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Transform target;
	public Rigidbody targetRigidbody;
	float jumpForce = 8f;
	public Rigidbody rigidbody;
	float speed = 5f;
	Vector3[] path;
	int targetIndex;

	public LayerMask obstacles;

	float elapsedTime = 0f;
	float reactionTime;

	/*void Start(){
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
	}*/

	void Awake(){
		rigidbody = GetComponent<Rigidbody>();
		targetRigidbody = target.GetComponent<Rigidbody>();
		obstacles = LayerMask.GetMask("Obstacle");
		reactionTime = Vector3.Distance(transform.position, target.position + targetRigidbody.velocity) * 0.04f;
		//rigidbody.AddForce(Vector3.up * jumpForce);
	}

	void FixedUpdate(){
		if (elapsedTime >= reactionTime){
			if (Vector3.Distance(transform.position, target.position) > 3){
				PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
				reactionTime = Vector3.Distance(transform.position, target.position) * 0.04f;
				if (reactionTime < 0.1f) reactionTime = 0.1f;
			}
			elapsedTime = 0f;
		}
		else elapsedTime += Time.deltaTime;

	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
		if (pathSuccessful){
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath(){
		if (path == null || path.Length == 0)
			yield return null;



		//Vector3 currentWaypoint = path[0];
		Vector3 currentWaypoint;
		if (path.Length > 0){
		
			currentWaypoint = path[0];

			while(true){
				if (transform.position == currentWaypoint){
					targetIndex++;
					if (targetIndex >= path.Length){
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				Vector3 targetDirection = (currentWaypoint - transform.position).normalized;
				targetDirection.y *= 0f;
				//transform.position = Vector3.MoveTowards(transform.position,currentWaypoint, speed * Time.deltaTime);
				rigidbody.AddForce(targetDirection * speed * 100f * Time.deltaTime);
				//transform.Rotate(Vector3.up * Vector3.Angle(transform.forward, (currentWaypoint - transform.position).normalized));

				if(Physics.Raycast(transform.position-Vector3.down, targetDirection, 3f, obstacles)){
					if (Physics.Raycast (transform.position-Vector3.down, Vector3.down, 0.95f, obstacles)){
						//rigidbody.AddForce(Vector3.up * jumpForce); 
						rigidbody.velocity = new Vector3(0, jumpForce, 0);


					}
					//transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up * jumpForce, Time.deltaTime);
					//rigidbody.velocity = new Vector3(0, jumpForce, 0);
					//rigidbody.inertiaTensor = 
				}
				
				yield return null;
			}
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one/2);
				
				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
