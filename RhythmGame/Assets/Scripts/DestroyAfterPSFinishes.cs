using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPSFinishes : MonoBehaviour {

	public ParticleSystem ps;
	
	// Update is called once per frame
	void Update () {
		if (ps.isStopped) {
			Destroy (gameObject);
		}
	}
}
