using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ampHitAnimation : MonoBehaviour {

    public int[] shapeKeyArr;
    shapeKeyController shapeKeyControllerScriptInstance;
	// Use this for initialization
	void Start () {
        shapeKeyControllerScriptInstance = GetComponent<shapeKeyController> ();
	}
	
	// Update is called once per frame
	public void animate () {
        for(int i = 0; i < shapeKeyArr.Length; i++)
        {
            shapeKeyControllerScriptInstance.callScaleCoroutine(shapeKeyArr[i]);
        }
		
	}
}
