using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateFloorParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.rotation *= Quaternion.Euler(new Vector3(0,0.2f,0));
	}
}
