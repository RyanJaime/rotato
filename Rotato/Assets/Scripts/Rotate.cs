using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 20; // was 15
	private Quaternion targetOrientation = Quaternion.identity;


	public OVRInput.Controller Controller = OVRInput.Controller.LTouch;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.Get(OVRInput.Button.One, Controller))
        {
			//print ("rotateigngn!");
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


	public void rotate(Rigidbody obj, int dir) // takes a Rigidbody gameObject and a number which determines which direction it will rotate.
	{
		if (dir == 1)
		{
			//print ("rotateigngn!");
			targetOrientation = Quaternion.Euler(0, -90, 0) * targetOrientation;
		}

		else if (dir == -1)
		{
			targetOrientation = Quaternion.Euler(0, 90, 0) * targetOrientation;
		}

		else if (dir == 2)
		{
			targetOrientation = Quaternion.Euler(90, 0, 0) * targetOrientation;
		}

		else if (dir == -2)
		{
			targetOrientation = Quaternion.Euler(-90, 0 , 0) * targetOrientation;
		}

		else if (dir == 3)
		{
			targetOrientation = Quaternion.Euler(0, 0, 90) * targetOrientation;
		}

		else if (dir == -3)
		{
			targetOrientation = Quaternion.Euler(0, 0, -90) * targetOrientation;
		}



		obj.transform.rotation = Quaternion.Lerp(
			obj.transform.rotation,
			targetOrientation,
			Mathf.Clamp01(Time.deltaTime*speed)
		);

	}

}