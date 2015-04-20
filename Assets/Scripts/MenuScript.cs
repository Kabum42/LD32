using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	private float aux_angle = 0;

	// Use this for initialization
	void Start () {
	
	//	Application.LoadLevel ("RubenScene");

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		aux_angle += Time.deltaTime*90;
		if (aux_angle > 360f) {
			aux_angle -= 360f;
		}

		float aux_scale = 0.6f + 0.05f + Mathf.Sin (Mathf.Deg2Rad*aux_angle) * 0.05f;

		this.gameObject.transform.FindChild("Bunnysher").transform.localScale = new Vector3(aux_scale, aux_scale, aux_scale);
		this.gameObject.transform.FindChild("Logo2").transform.localScale = new Vector3(aux_scale, aux_scale, aux_scale);

		if (Input.anyKey) {
			Application.LoadLevel ("RubenScene");
		}
	}
}
