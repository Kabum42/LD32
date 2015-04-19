using UnityEngine;
using System.Collections;

public class particleCollision : MonoBehaviour
{
	float myHeight;
	int projNum;
	
	public ParticleSystem part;
	public ParticleCollisionEvent[] collisionEvents;
	
	private Transform myTransform;
	
	Quaternion rotation = Quaternion.identity;
	
	// Use this for initialization
	void Awake ()
	{
		part = GetComponent<ParticleSystem> ();
		collisionEvents = new ParticleCollisionEvent[16];
		rotation = part.transform.rotation;
		myTransform = GetComponent<Transform> ();
	}
	
	void OnParticleCollision (GameObject other)
	{
		int safeLength = part.GetSafeCollisionEventSize ();
		//if (collisionEvents.Length < safeLength)
		collisionEvents = new ParticleCollisionEvent[safeLength];
		
		int numCollisionEvents = part.GetCollisionEvents (other, collisionEvents);
		Transform body = other.GetComponent<Transform> ();
		int i = 0;
		while (i < numCollisionEvents) {
			if (body) {
				
				Vector3 pos = collisionEvents [i].intersection;
				
				projNum = projectorManager.currentProj;
				if (projNum >= projectorManager.MAX_PROJ)
					projNum = 0;
				
				projectorManager.currentProj = projNum;
				
				if (!projectorManager.projectorArray [projNum].activeSelf)
					projectorManager.projectorArray [projNum].SetActive (true);
				
				//projectorManager.projectorArray [projNum].transform.position = new Vector3 (myTransform.position.x, myTransform.position.y, myTransform.position.z);
				
			
				projectorManager.projectorArray [projNum].transform.position = new Vector3 (myTransform.position.x, myTransform.position.y, myTransform.position.z); 
				
				Vector3 direction = new Vector3 (pos.x - projectorManager.projectorArray [projNum].transform.position.x,
				                                pos.y - projectorManager.projectorArray [projNum].transform.position.y,
				                                pos.z - projectorManager.projectorArray [projNum].transform.position.z);
				                         
				direction.Normalize ();   
				
				                         
				                                
				projectorManager.projectorArray [projNum].transform.LookAt (projectorManager.projectorArray [projNum].transform.position + direction);
				                           
				
				projectorManager.currentProj += 1;
			}
			i++;
		}
	}
	
}
