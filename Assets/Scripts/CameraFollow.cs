using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public MainScript main;
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.
	
	Vector3 offset;                     // The initial offset from the target.
	
	void Start ()
	{
		// Calculate the initial offset.
		offset = transform.position - target.position;
	}
	
	void FixedUpdate ()
	{
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = target.position + offset;
		
		// Smoothly interpolate between the camera's current position and it's target position.


		if (main.killAnimation) {

			if (main.killAnimationTimer <= 0.9f) {
				transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime*(1f/Time.timeScale));
				
				float value = 0f;
				if (main.killAnimationTimer > 0) { value = 0.9f - main.killAnimationTimer; }
				else { value = 0.9f + main.killAnimationTimer; }
				
				transform.position += transform.forward*value*0.9f;
				
				Time.timeScale = 0.1f;
				Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
			}



		} else {

			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

			Time.timeScale = 1f;
			Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);

		}
	}
}
