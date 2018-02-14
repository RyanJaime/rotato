using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsRight : MonoBehaviour {
	public OVRInput.Controller Controller = OVRInput.Controller.RTouch;
	OVRHapticsClip myHapticsClip;
	public AudioClip VibeClip;
	// Use this for initialization
	void Start () {
		myHapticsClip = new OVRHapticsClip (VibeClip);
	}
	
	// Update is called once per frame
	void Update () {
		if (Controller == OVRInput.Controller.RTouch) {
			if (OVRInput.Get (OVRInput.Button.One, Controller)) {
				OVRHaptics.RightChannel.Mix (myHapticsClip);
			}
		} 
		//else {print ("WOW");}
	}
}
