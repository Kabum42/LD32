using UnityEngine;
using System.Collections;

public class SkeletonScript : MonoBehaviour {

	public GameObject main;
	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject feet;
	public GameObject bigCarrot;
	public GameObject auxCarrot;

	private float sonic = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		leftHand.transform.position = transform.position;
		rightHand.transform.position = transform.position;
		feet.transform.position = transform.position;



		//Debug.Log (this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).shortNameHash);
		//Debug.Log (Animator.StringToHash("Idle"));
		/*
		if (this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Balling")) {
			sonic += Time.deltaTime*1000f;
			while (sonic > 360) {
				sonic -= 360;
			}

			this.gameObject.transform.RotateAround(this.gameObject.transform.position + new Vector3(0, 0.1f, 0), this.gameObject.transform.right, -sonic);
			//feet.transform.RotateAround(this.gameObject.transform.position + new Vector3(0, 0.1f, 0), this.gameObject.transform.right, -sonic);

			//this.gameObject.transform.localEulerAngles = new Vector3(0 -sonic, this.gameObject.transform.localEulerAngles.y, this.gameObject.transform.localEulerAngles.z); 
		}
		*/

		if (main.GetComponent<MainScript> ().killAnimation && main.GetComponent<MainScript> ().killAnimationTimer >= -0.5f) {

			if (auxCarrot == null) { 
				auxCarrot = (GameObject)Instantiate (bigCarrot);
				leftHand.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.L/armaIzq").gameObject.SetActive (false);
				rightHand.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.R/armaDer").gameObject.SetActive (false);
			}

			auxCarrot.transform.position = this.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.R/Carrot").transform.position;
			auxCarrot.transform.eulerAngles = this.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.R/Carrot").transform.eulerAngles;
			auxCarrot.transform.RotateAround (auxCarrot.transform.position, auxCarrot.transform.right, 180f);
		} else if (!main.GetComponent<MainScript> ().killAnimation) {
			auxCarrot = null;
			leftHand.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.L/armaIzq").gameObject.SetActive (true);
			rightHand.gameObject.transform.FindChild ("Armature.001/Stomach/Chest/Hand.R/armaDer").gameObject.SetActive (true);
		} else {
			auxCarrot = null;
		}
	
	}

	void FixedUpdate() {

		this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, this.transform.eulerAngles.y + 180f, this.transform.eulerAngles.z);
		leftHand.transform.eulerAngles = new Vector3 (leftHand.transform.eulerAngles.x, leftHand.transform.eulerAngles.y + 180f, leftHand.transform.eulerAngles.z);
		rightHand.transform.eulerAngles = new Vector3 (rightHand.transform.eulerAngles.x, rightHand.transform.eulerAngles.y + 180f, rightHand.transform.eulerAngles.z);
		feet.transform.eulerAngles = new Vector3 (feet.transform.eulerAngles.x, feet.transform.eulerAngles.y + 180f, feet.transform.eulerAngles.z);

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
				Vector3 oldRotation = leftHand.transform.eulerAngles;
				
				leftHand.transform.LookAt (main.GetComponent<MainScript> ().targetIzq.transform);


				Vector3 newRotation = leftHand.transform.eulerAngles;
				
				leftHand.transform.eulerAngles = new Vector3(oldRotation.x, newRotation.y +30f, oldRotation.z);
				//leftHand.transform.RotateAround (transform.position, Vector3.up, 20f);
			} else {
				leftHand.transform.eulerAngles = new Vector3(0, 0, 0);
			}
			
			if (main.GetComponent<MainScript> ().targetDer != null) {
				Vector3 oldRotation = rightHand.transform.eulerAngles;
				
				rightHand.transform.LookAt (main.GetComponent<MainScript> ().targetDer.transform);
				Vector3 newRotation = rightHand.transform.eulerAngles;
				
				rightHand.transform.eulerAngles = new Vector3(oldRotation.x, newRotation.y -30f, oldRotation.z);
				//rightHand.transform.RotateAround (transform.position, Vector3.up, -20f);
			}
			else {
				rightHand.transform.eulerAngles = new Vector3(0, 0, 0);
			}

			if (main.GetComponent<MainScript> ().targetIzq == main.GetComponent<MainScript> ().targetDer) {
				Vector3 oldRotation = this.transform.eulerAngles;
				
				this.transform.LookAt (main.GetComponent<MainScript> ().targetIzq.transform);
				Vector3 newRotation = this.transform.eulerAngles;
				
				this.transform.eulerAngles = new Vector3(oldRotation.x, newRotation.y, oldRotation.z);
			}
			else {
				Vector3 median = (main.GetComponent<MainScript> ().targetIzq.transform.position + main.GetComponent<MainScript> ().targetDer.transform.position);
				median = new Vector3(median.x/2f, median.y/2f, median.z/2f);

				Vector3 oldRotation = this.transform.eulerAngles;

				this.transform.LookAt (median);
				Vector3 newRotation = this.transform.eulerAngles;

				this.transform.eulerAngles = new Vector3(oldRotation.x, newRotation.y, oldRotation.z);
			}
		}



		this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, this.transform.eulerAngles.y - 180f, this.transform.eulerAngles.z);
		leftHand.transform.eulerAngles = new Vector3 (leftHand.transform.eulerAngles.x, leftHand.transform.eulerAngles.y - 180f, leftHand.transform.eulerAngles.z);
		rightHand.transform.eulerAngles = new Vector3 (rightHand.transform.eulerAngles.x, rightHand.transform.eulerAngles.y - 180f, rightHand.transform.eulerAngles.z);
		feet.transform.eulerAngles = new Vector3 (feet.transform.eulerAngles.x, feet.transform.eulerAngles.y - 180f, feet.transform.eulerAngles.z);


	}
}
