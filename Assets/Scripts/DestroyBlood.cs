using UnityEngine;
using System.Collections;

public class DestroyBlood : MonoBehaviour
{
	void Awake ()
	{
		Destroy (this.gameObject, this.GetComponent<ParticleSystem> ().duration + this.GetComponent<ParticleSystem> ().startLifetime);    
	}
}
