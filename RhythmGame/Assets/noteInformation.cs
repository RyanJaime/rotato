using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteInformation : MonoBehaviour {
    public bool hasGoneThruCorrectFace;
	// Use this for initialization
	void Start () {
        hasGoneThruCorrectFace = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider face)
    {
        if (face.tag == "RightPhysical")
        {
            hasGoneThruCorrectFace = true;
        }
    }
}
