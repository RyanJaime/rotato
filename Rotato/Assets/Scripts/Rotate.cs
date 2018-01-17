using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 15;
	private Quaternion targetOrientation = Quaternion.identity;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			targetOrientation = Quaternion.Euler(0, -90, 0) * targetOrientation;
        }
		if( Input.GetKeyDown( KeyCode.LeftArrow ) ){
			
			targetOrientation = Quaternion.Euler(0, 90, 0) * targetOrientation;
		}
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			targetOrientation = Quaternion.Euler(90, 0, 0) * targetOrientation;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
			targetOrientation = Quaternion.Euler(-90, 0, 0) * targetOrientation;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
			targetOrientation = Quaternion.Euler(0, 0, 90) * targetOrientation;
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
			targetOrientation = Quaternion.Euler(0, 0, -90) * targetOrientation;
        }

		this.transform.rotation = Quaternion.Lerp(
			this.transform.rotation,
			targetOrientation,
			Mathf.Clamp01(Time.deltaTime*speed));
    }
}