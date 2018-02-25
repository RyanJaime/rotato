using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioEffects : MonoBehaviour {

    private AudioLowPassFilter lpf;
	// Use this for initialization
	void Start () {
        lpf = GetComponent<AudioLowPassFilter>();
	}

    // Update is called once per frame
    private void Update()
    {
        lpf.cutoffFrequency += 2;
    }
    void UpdateCutOffFrq () {
        lpf.cutoffFrequency += 100;
	}
}
