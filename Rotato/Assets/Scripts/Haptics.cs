using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haptics : MonoBehaviour {

	public AudioClip VibeClip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown (OVRInput.RawButton.RIndexTrigger)) {
		}
		if (OVRInput.GetDown (OVRInput.RawButton.LIndexTrigger)) {
		}
	}
}
