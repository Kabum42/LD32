using UnityEngine;
using System.Collections;

public class SkeletonScript : MonoBehaviour {

	public GameObject main;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject feet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		leftHand.transform.position = transform.position;
		rightHand.transform.position = transform.position;
		feet.transform.position = transform.position;
	
	}

	void FixedUpdate() {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Vector3 direction = new Vector3 (h, 0, v);
		direction.Normalize();

		Vector3 originalRotation = feet.transform.eulerAngles;
		feet.transform.LookAt (feet.transform.position + direction);
		Vector3 destinationRotation = feet.transform.eulerAngles;
		feet.transform.eulerAngles = originalRotation;


		if ( Mathf.Abs(destinationRotation.y +360f -originalRotation.y) < Mathf.Abs(destinationRotation.y -originalRotation.y) ) {
			destinationRotation.y += 360f;
		}
		else if ( Mathf.Abs(destinationRotation.y -360f -originalRotation.y) < Mathf.Abs(destinationRotation.y -originalRotation.y) ) {
			destinationRotation.y -= 360f;
		}



		feet.transform.eulerAngles = Vector3.Lerp (feet.transform.eulerAngles, destinationRotation, 0.1f);
		//feet.transform.LookAt(feet.transform.position + direction);
		this.transform.eulerAngles = feet.transform.eulerAngles;

		if (main.GetComponent<MainScript> ().targetIzq == null && main.GetComponent<MainScript> ().targetDer == null) {
			leftHand.transform.eulerAngles = feet.transform.eulerAngles;
			rightHand.transform.eulerAngles = feet.transform.eulerAngles;
		} else {
			if (main.GetComponent<MainScript> ().targetIzq != null) {
				leftHand.transform.LookAt (main.GetComponent<MainScript> ().targetIzq.transform);
				//leftHand.transform.RotateAround (transform.position, Vector3.up, 20f);
			} else {
				leftHand.transform.eulerAngles = new Vector3(0, 0, 0);
			}
			
			if (main.GetComponent<MainScript> ().targetDer != null) {
				rightHand.transform.LookAt (main.GetComponent<MainScript> ().targetDer.transform);
				//rightHand.transform.RotateAround (transform.position, Vector3.up, -20f);
			}
			else {
				rightHand.transform.eulerAngles = new Vector3(0, 0, 0);
			}
		}

	}
}
