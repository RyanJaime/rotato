using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSync : MonoBehaviour {
    public double totalDSP;
    // Use this for initialization
    void Start () {
        totalDSP = AudioSettings.dspTime; // digital signal processor
        print("Str time: " + totalDSP);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
