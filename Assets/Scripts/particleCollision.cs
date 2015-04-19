using UnityEngine;
using System.Collections;

public class particleCollision : MonoBehaviour
{
	float myHeight;
	int projNum;
	
	public ParticleSystem part;
	public ParticleCollisionEvent[] collisionEvents;
	
	Quaternion rotation = Quaternion.identity;
	


	// Use this for initialization
	void Start ()
	{
		part = GetComponent<ParticleSystem> ();
		collisionEvents = new ParticleCollisionEvent[16];
		rotation.eulerAngles = new Vector3 (90, 0, 0);
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
				
				projectorManager.projectorArray [projNum].transform.position = new Vector3 (pos.x, pos.y + 1, pos.z);
				projectorManager.projectorArray [projNum].transform.rotation = rotation;
				
				projectorManager.currentProj += 1;
				//Instantiate (Resources.Load ("BloodProjector1"), new Vector3 (pos.x, pos.y + 1, pos.z), rotation);
			}
			i++;
		}
	}
	
}
