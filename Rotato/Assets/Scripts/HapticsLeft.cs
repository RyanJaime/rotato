using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsLeft : MonoBehaviour {
	public OVRInput.Controller Controller = OVRInput.Controller.LTouch;
	OVRHapticsClip myHapticsClip;
	public AudioClip VibeClip;
	// Use this for initialization
	void Start () {
		myHapticsClip = new OVRHapticsClip (VibeClip);
	}

	// Update is called once per frame
	void Update () {
		if (Controller == OVRInput.Controller.LTouch) {
			if (OVRInput.Get (OVRInput.Button.One, Controller)) {
				OVRHaptics.LeftChannel.Mix (myHapticsClip);
			}
		} 
		//else {print ("HOW?");}
	}
}
