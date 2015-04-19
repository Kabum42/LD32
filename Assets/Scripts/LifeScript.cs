using UnityEngine;
using System.Collections;

public class LifeScript : MonoBehaviour {

	public float lifePoints = 100f;
	public bool alive = true;
	public GameObject player;
	public GameObject main;
	public float killDistance = 3f;
	public int targetableMask;

	// Use this for initialization
	void Start () {
	
		targetableMask = LayerMask.GetMask("Targetable");

		GameObject whateverGameObject = this.gameObject.transform.FindChild("default").gameObject;
		
		MeshRenderer gameObjectRenderer = whateverGameObject.GetComponent<MeshRenderer>();
		Material newMaterial = new Material(Shader.Find("Standard"));

		newMaterial.mainTexture = whateverGameObject.GetComponent<MeshRenderer> ().material.mainTexture;
		gameObjectRenderer.material = newMaterial;

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

		if (alive) {
			Highlight ();
		}
	}

	void Highlight() {

		this.gameObject.transform.FindChild("default").gameObject.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 1f);

		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(camRay, out hit, 100f, targetableMask))
		{
			if (hit.transform.gameObject == this.gameObject) {
				this.gameObject.transform.FindChild("default").gameObject.GetComponent<MeshRenderer> ().material.color = new Color (1f, 0f, 0f);
			}
		}

	}

}
