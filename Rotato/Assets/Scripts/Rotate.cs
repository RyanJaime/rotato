using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = .000002000f; // was 15
	public OVRInput.Controller Controller = OVRInput.Controller.LTouch;
	public bool moving = false;

	private Quaternion targetOrientation = Quaternion.identity;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		moving = false;

		if (Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.Get(OVRInput.Button.One, Controller)) {
			targetOrientation = Quaternion.Euler(0, -90, 0) * targetOrientation;
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			targetOrientation = Quaternion.Euler(0, 90, 0) * targetOrientation;
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			targetOrientation = Quaternion.Euler(90, 0, 0) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			targetOrientation = Quaternion.Euler(-90, 0, 0) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.A)) {
			targetOrientation = Quaternion.Euler(0, 0, 90) * targetOrientation;
        } else if (Input.GetKeyDown(KeyCode.D)) {
			targetOrientation = Quaternion.Euler(0, 0, -90) * targetOrientation;
        }

		if (this.transform.rotation != targetOrientation) {
			this.transform.rotation = Quaternion.Lerp (
				this.transform.rotation,
				targetOrientation,
				Mathf.Clamp01 (Time.deltaTime * speed)
			);
			moving = true;
		}

    }


	public void rotate(Rigidbody obj, int dir) // takes a Rigidbody gameObject and a number which determines which direction it will rotate.
	{
		if (dir == 1)
		{
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
			
		/*
		obj.transform.rotation = Quaternion.Lerp(
			obj.transform.rotation,
			targetOrientation,
			Mathf.Clamp01(Time.deltaTime*speed)
		);
		*/

	}

}