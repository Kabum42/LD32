using UnityEngine;
using System.Collections;

public class UnitGato : MonoBehaviour {

	public Transform target;
	Rigidbody targetRigidbody;
	float jumpForce = 8f;
	Rigidbody rigidbody;
	float speed = 5f;
	Vector3[] path;
	int targetIndex;

	LayerMask obstacles;

	float elapsedTime = 0f;
	float reactionTime;

	float attackElapsedTime = 0f;
	float attackSpeed = 1f;

	int state = 0;
	float comfortZone = 20f;
	float comfortZoneThreshold = 5f;
	bool followingPath = false;
	

	void Awake(){
		rigidbody = GetComponent<Rigidbody>();
		targetRigidbody = target.GetComponent<Rigidbody>();
		obstacles = LayerMask.GetMask("Obstacle");
		reactionTime = Vector3.Distance(transform.position, target.position + targetRigidbody.velocity) * 0.04f;
	}

	void FixedUpdate(){


		if(Vector3.Distance(transform.position, target.position) < comfortZone - comfortZoneThreshold){
			if(state != -1) ChangeState(-1);
		}
		else if (Vector3.Distance(transform.position, target.position) > comfortZone + comfortZoneThreshold){
			if(state != 1) ChangeState(1);
		}
		else if (Vector3.Distance(transform.position, target.position) < Mathf.Abs(comfortZone - comfortZoneThreshold/2)){
			if(state != 0){
				ChangeState(0);
				rigidbody.velocity = new Vector3(rigidbody.velocity.x / 2f, rigidbody.velocity.y, rigidbody.velocity.z / 2f);
			}
		}
		



		if(state == -1){
			if (elapsedTime >= reactionTime){
				PathRequestManagerB.RequestPath (transform.position, target.position, OnBPathFound);
				reactionTime = Vector3.Distance(transform.position, target.position) * 0.04f;
				if (reactionTime < 0.1f) reactionTime = 0.1f;
				elapsedTime = 0f;
			}
			else elapsedTime += Time.deltaTime;
		}
		if(state == 0){
			if(!Physics.Raycast(transform.position + Vector3.up, target.position-transform.position, Vector3.Distance(transform.position, target.position), obstacles)){
				Attack();
			}
			else if(!followingPath){
				followingPath = true;
				PathRequestManagerO.RequestPath (transform.position, target.position, OnBPathFound);
			}
		}
		if(state == 1){
			if (elapsedTime >= reactionTime){
				PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
				reactionTime = Vector3.Distance(transform.position, target.position) * 0.04f;
				if (reactionTime < 0.1f) reactionTime = 0.1f;
				elapsedTime = 0f;
			}
			else elapsedTime += Time.deltaTime;
		}
	}



	private void Attack(){

		StopCoroutine("FollowPath");
		followingPath = false;

		if (attackElapsedTime >= attackSpeed){
			Debug.Log ("Bang!"); //shoot
			attackElapsedTime = 0;
		}
		else
			attackElapsedTime += Time.deltaTime;
	}

	private void ChangeState(int newState){
		StopCoroutine("FollowPath");
		followingPath = false;
		state = newState;
		reactionTime = Vector3.Distance(transform.position, target.position) * 0.04f;
		elapsedTime = 0f;
		//ATTACK ELAPSED TIME RESET?
	}


	public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
		if (pathSuccessful){
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
			followingPath = true;
		}
	}

	public void OnBPathFound(Vector3[] newPath){
		path = newPath;
		StopCoroutine("FollowPath");
		StartCoroutine("FollowPath");
		followingPath = true;
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
						followingPath = false;
						StopCoroutine("FollowPath");
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				Vector3 targetDirection = (currentWaypoint - transform.position).normalized;
				targetDirection.y *= 0f;

				rigidbody.AddForce(targetDirection * speed * 100f * Time.deltaTime);


				if(Physics.Raycast(transform.position+Vector3.up, targetDirection, 3f, obstacles)){
					if (Physics.Raycast (transform.position-Vector3.down, Vector3.down, 0.95f, obstacles)){
						rigidbody.velocity = new Vector3(0, jumpForce, 0);
					}
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
