using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 1f; // was 15
	public OVRInput.Controller Controller = OVRInput.Controller.LTouch;
	public bool moving = false;

	private Quaternion targetOrientation = Quaternion.identity;
	private Quaternion startPos;
	private Quaternion endPos;
	private bool isLerping;
	private float timeStartedLerping;

    // Use this for initialization
    void Start () {
		startPos = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		moving = false;

		if (Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.Get(OVRInput.Button.One, Controller)) {
			targetOrientation = Quaternion.Euler(0, -90, 0) * targetOrientation;
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			targetOrientation = Quaternion.Euler(0, 90, 0) * targetOrientation;
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			targetOrientation = Quaternion.Euler(90, 0, 0) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			targetOrientation = Quaternion.Euler(-90, 0, 0) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.A)) {
			targetOrientation = Quaternion.Euler(0, 0, 90) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.D)) {
			targetOrientation = Quaternion.Euler(0, 0, -90) * targetOrientation;
        }

		/*
		if (this.transform.rotation != targetOrientation) {
			this.transform.rotation = Quaternion.Lerp (
				startPos,
				targetOrientation,
				Time.deltaTime * speed
				//Mathf.Clamp01 (Time.deltaTime * speed)
			);
			moving = true;
		}
		*/
    }

	public void FixedUpdate(){
		if (isLerping) {
			float timeSinceStarted = Time.time - timeStartedLerping;
			float percentageComplete = timeSinceStarted / 0.1f;
			//print ("PERC%: " + percentageComplete);
			this.transform.rotation = Quaternion.Lerp (
				startPos,
				endPos,
				percentageComplete
				/*Mathf.Clamp01 (Time.deltaTime * speed)*/
			);
			if (percentageComplete >= 1.0f) {
				isLerping = false;
				startPos = transform.rotation;
			}
		}
	}

	public void startLerping(){
		isLerping = true;
		timeStartedLerping = Time.time;
		startPos = this.transform.rotation;
		endPos = targetOrientation;
	}

	public void rotate(Rigidbody obj, int dir) // takes a Rigidbody gameObject and a number which determines which direction it will rotate.
	{
		if (dir == 1)
		{
			targetOrientation = Quaternion.Euler (0, -90, 0) * startPos;//targetOrientation;
		}

		else if (dir == -1)
		{
			targetOrientation = Quaternion.Euler(0, 90, 0) * startPos;//targetOrientation;
		}

		else if (dir == 2)
		{
			targetOrientation = Quaternion.Euler(90, 0, 0) * startPos;//targetOrientation;
		}

		else if (dir == -2)
		{
			targetOrientation = Quaternion.Euler(-90, 0 , 0) * startPos;//targetOrientation;
		}

		else if (dir == 3)
		{
			targetOrientation = Quaternion.Euler(0, 0, 90) * startPos;//targetOrientation;
		}

		else if (dir == -3)
		{
			targetOrientation = Quaternion.Euler(0, 0, -90) * startPos;//targetOrientation;
		}
		if (!isLerping) {
			startLerping ();
		}
		/*
		obj.transform.rotation = Quaternion.Lerp(
			obj.transform.rotation,
			targetOrientation,
			Mathf.Clamp01(Time.deltaTime*speed)
		);
		*/
	}
}