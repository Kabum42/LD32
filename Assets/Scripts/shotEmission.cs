using UnityEngine;
using System.Collections;

public class shotEmission : MonoBehaviour
{
	Transform myTransform;
	ParticleSystem myParticles;
	// Use this for initialization
	void Start ()
	{
		myTransform = GetComponent<Transform>();
	}
	
	void createEmission (Vector3 direction)
	{
		myPArticles = Instantiate (Resources.Load ("BloodProjector1"), Quaternion.identity)
	}
}
