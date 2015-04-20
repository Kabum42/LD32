using UnityEngine;
using System.Collections;

public class UnitCerdo : MonoBehaviour {

	public Transform target;
	Rigidbody targetRigidbody;
	float jumpForce = 8f *2f;
	Rigidbody rigidbody;
	float speed = 5f;
	Vector3[] path;
	int targetIndex;

	float attackRange = 20f;
	float attackJump = 20f * 2f;
	float attackForce = 1000f;
	bool ableToAttack = false;
	bool chargingAttack = false;
	bool attacking = false;
	float attackTimer = 0f;
	float attackTimeCharge = 1f;
	float attackCooldownTimer = 0.0001f; // 'cause of EXPLOSION
	float attackCooldownMark = 3f;

	LayerMask obstacles;
	LayerMask characters;

	float elapsedTime = 0f;
	float reactionTime;
	

	void Awake(){
		rigidbody = GetComponent<Rigidbody>();
		targetRigidbody = target.GetComponent<Rigidbody>();
		obstacles = LayerMask.GetMask("Obstacle");
		characters = LayerMask.GetMask("Targetable", "Player");
		reactionTime = Vector3.Distance(transform.position, target.position + targetRigidbody.velocity) * 0.04f;
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

		if(ableToAttack == false && rigidbody.velocity.y <= 0f && Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f, obstacles)){
			if (attackCooldownTimer >= attackCooldownMark){
				ableToAttack = true;
				attackCooldownTimer = 0f;
			}
			else {
				if (attackCooldownTimer == 0f){
					RaycastHit[] affected = Physics.SphereCastAll(transform.position, 100f, new Vector3(0,1,0), 100f, characters);
					foreach(RaycastHit item in affected){
						if(!item.transform.Equals(this.transform)) item.rigidbody.AddForce((item.transform.position - transform.position + Vector3.up * 2f).normalized * attackForce / Mathf.Sqrt(Vector3.Distance(item.transform.position, transform.position)));
					}
					this.rigidbody.velocity = new Vector3(0,0,0);
					// Animacion
				}
				attackCooldownTimer += Time.deltaTime;
			}
			attacking = false;
		}

		if (Vector3.Distance(transform.position, target.position) <= attackRange && ableToAttack)
			ChargeAttack();

		if (attacking){
		    if (rigidbody.velocity.y < 0f){
				rigidbody.AddForce((target.position - transform.position) * speed * 75f * Time.deltaTime);
				rigidbody.AddForce(Vector3.down*100f);
			}
		}

	}






	private void ChargeAttack(){
		if(!chargingAttack) chargingAttack = true;

		rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);

		if(attackTimer >= attackTimeCharge){
			attackTimer = 0f;
			Attack();
		}
		else
			attackTimer += Time.deltaTime;
	}

	private void Attack(){
		chargingAttack = false;
		attacking = true;
		rigidbody.velocity = Vector3.up * attackJump;
		ableToAttack = false;
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

		Vector3 currentWaypoint;
		if (path.Length > 0 && !chargingAttack && !attacking){ // && !chargingAttack
		
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
				rigidbody.AddForce(targetDirection * speed * 100f * Time.deltaTime);

				if(Physics.Raycast(transform.position-Vector3.down, targetDirection, 3f, obstacles)){
					if (Physics.Raycast (transform.position-Vector3.down, Vector3.down, 2f, obstacles)){
						//rigidbody.AddForce(Vector3.up * jumpForce); 
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
