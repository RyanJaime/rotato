using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteCollision : MonoBehaviour {

	public bool localIsPunchingR;
	public bool localIsPunchingL;

	public bool isColliding;
	GameObject[] handObjects;

	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;

	private OVRInput.Controller lTouch;
	private OVRInput.Controller rTouch;

	public bool lVibrate = false;
	public bool rVibrate = false;

	// Use this for initialization
	void Start () {
		handObjects = GameObject.FindGameObjectsWithTag ("Hand");
		localIsPunchingL = handObjects [0].GetComponent<Hand> ().isPunching;
		localIsPunchingR = handObjects [1].GetComponent<Hand> ().isPunching;
		lTouch = handObjects [0].GetComponent<Hand> ().Controller;
		rTouch = handObjects [1].GetComponent<Hand> ().Controller;

		lHapticsClip = new OVRHapticsClip (lVibeClip);
		rHapticsClip = new OVRHapticsClip (rVibeClip);
	}
	
	// Update is called once per frame
	void Update () {
		localIsPunchingL = handObjects [0].GetComponent<Hand> ().isPunching;
		localIsPunchingR = handObjects [1].GetComponent<Hand> ().isPunching;
	}

	void OnTriggerEnter(Collider note){
		isColliding = true;
		//note.GetComponent<obstacleMovement> ().colliding = true;
		if (note.CompareTag("note") && localIsPunchingL) {
			//lHapticsClip.Reset();
			//lHapticsClip.WriteSample(255);
			//lHapticsClip.WriteSample(0);
			//lHapticsClip.WriteSample(0);
			lVibrate = true;
			Destroy (note.gameObject);
			if (lTouch == OVRInput.Controller.LTouch) {
				//OVRHaptics.LeftChannel.Mix (lHapticsClip);
			} 
				
			handObjects[0].GetComponent<Hand> ().isPunching = false;
		}
		if (note.CompareTag("note") && localIsPunchingR) {
			//rHapticsClip.Reset();
			//rHapticsClip.WriteSample(255);
			//rHapticsClip.WriteSample(0);
			//rHapticsClip.WriteSample(0);
			rVibrate = true;
			Destroy (note.gameObject);
			if (rTouch == OVRInput.Controller.RTouch) {
				//OVRHaptics.RightChannel.Mix (rHapticsClip);
			} 
			handObjects[1].GetComponent<Hand> ().isPunching = false;
		}

	}

	void OnTriggerStay(Collider note){
		
		if (note.CompareTag("note")) {
			//print (note);
			//Destroy (note.gameObject);
		}
	}

	void OnTriggerExit(Collider note){
		isColliding = false;
		//note.GetComponent<obstacleMovement> ().colliding = false;
	}
}
