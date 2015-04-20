using UnityEngine;
using System.Collections;

public class DialogControl : MonoBehaviour
{
	bool isStart = true;
	bool isJump = false;
	bool isAttack = false;
	Transform transform;
	float dt;
	public GameObject moveText;
	public GameObject jumpText;
	public GameObject attackText;
	public GameObject executeText;
	
	// Use this for initialization
	void Awake ()
	{
		transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		moveText.transform.position = transform.position + new Vector3 (0, 6 + Mathf.Cos (Time.fixedTime * 5f) * 0.2f, 0);
		jumpText.transform.position = transform.position + new Vector3 (0, 8 + Mathf.Cos (Time.fixedTime * 5f) * 0.2f, 0);
		attackText.transform.position = transform.position + new Vector3 (0, 8 + Mathf.Cos (Time.fixedTime * 5f) * 0.2f, 0);
		executeText.transform.position = transform.position + new Vector3 (0, 8 + Mathf.Cos (Time.fixedTime * 5f) * 0.2f, 0);
		
		if (isStart) {
			moveText.SetActive (true);
		}
		
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
		
			if (isStart) {
				isStart = false;
				moveText.SetActive (false);	
				isJump = true;
			}
		}
		
		if (isJump) {
			jumpText.SetActive (true);
		}
		
		if (Input.GetAxis ("Jump") != 0 && isJump) {
			isJump = false;
			jumpText.SetActive (false);	
			isAttack = true;
		}
		
		if (isAttack) {
			attackText.SetActive (true);
		}
		
		if (Input.GetMouseButtonDown (0) && isAttack) {
			isAttack = false;
			attackText.SetActive (false);	
		}
	}
}
