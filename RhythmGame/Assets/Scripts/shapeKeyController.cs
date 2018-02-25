using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shapeKeyController : MonoBehaviour {

	public int shapeKey;
	private float mScale = 0.0f;
	private bool scaleUp = true;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("Scale", 0.0f, 0.01f);
	}

	// Update is called once per frame
	void Scale () {
		if (scaleUp) {
			GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (shapeKey, mScale+=5); //(shapeKeyIndex, scaleSpeed)
			if (mScale >= 100.0f) {
				scaleUp = !scaleUp;
			}
		} else {
			GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (shapeKey, mScale-=5); //(shapeKeyIndex, scaleSpeed)
			if (mScale <= 0.0f) {
                //scaleUp = !scaleUp;
                CancelInvoke("Scale");
			}
		}
	}
}
