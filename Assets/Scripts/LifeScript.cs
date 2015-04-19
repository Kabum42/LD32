using UnityEngine;
using System.Collections;

public class LifeScript : MonoBehaviour {

	public float lifePoints = 100f;
	public bool alive = true;
	public GameObject player;
	public GameObject main;
	public float killDistance = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (lifePoints <= 0f && alive) {
			if (Input.GetButton ("Fire1") && !main.GetComponent<MainScript>().killAnimation) {
				if (Vector3.Distance(transform.position, player.transform.position) < killDistance) {
					main.GetComponent<MainScript>().killAnimation = true;
					main.GetComponent<MainScript>().killAnimationTimer = 0.9f;
					main.GetComponent<MainScript>().GetComponent<AudioSource> ().Play ();
					alive = false;
					transform.position = new Vector3(0, -99999, 0);
				}
			}
		}
	}

}
