using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

	public enum State
	{
		EMPTY,
		TOUCHING,
		HOLDING
	};

	public OVRInput.Controller Controller = OVRInput.Controller.LTouch;
	public State mHandState = State.EMPTY;
	public Rigidbody AttachPoint = null;
	public bool IgnoreContactPoint = false;
	private Rigidbody mHeldObject;
	private FixedJoint mTempJoint;

	private Vector3 motionTrack;
	private Vector3 lastMotionTrack;

	private float timeSinceLastCall = 1;
	private float previousVelocityx = 0f;
	private float currentVelocityx = 0f;

    private float slapSensitivity = 0.05f;

	private Vector3 lastTouchingPos;
	private Vector3 emptyTouchingPos;

	private GameObject RotatableCube;

	private int face = 0;
	//1 = Back
	//2 = Left
	//3 = Right
	//4 = Top
	//5 = Bottom
	//6 = Front

	private Quaternion lastRotation, currentRotation;

	void Start ()
	{
		if (AttachPoint == null)
		{
			AttachPoint = GetComponent<Rigidbody>();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		//print ("////////////////////////////////");
		lastTouchingPos = OVRInput.GetLocalControllerPosition(Controller);

		if (mHandState == State.EMPTY)
		{
			GameObject temp = collider.gameObject;

			if (temp != null && temp.layer == LayerMask.NameToLayer("grabbable") && temp.GetComponent<Rigidbody>() != null)
			{
				mHeldObject = temp.GetComponent<Rigidbody>();
				mHandState = State.TOUCHING;
			}
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (mHandState != State.HOLDING)
		{
			if (collider.gameObject.layer == LayerMask.NameToLayer("grabbable"))
			{
				mHeldObject = null;
				mHandState = State.EMPTY;
			}
		}
		RotatableCube = GameObject.FindGameObjectWithTag("Rotatable");


		if (collider.tag == "Back") { // check which face being collidered
			emptyTouchingPos = OVRInput.GetLocalControllerPosition (Controller);
			Vector3 diff = lastTouchingPos - emptyTouchingPos;
			if (Mathf.Sign (diff.x) == 1 && diff.x >= slapSensitivity) {
				if (RotatableCube.GetComponent<Rotate> () != null) {
					RotatableCube.GetComponent<Rotate> ().rotate (RotatableCube.GetComponent<Collider>().attachedRigidbody, -1);
				}
			} else if (Mathf.Sign (diff.x) == -1 && diff.x <= -slapSensitivity) {
				if (RotatableCube.GetComponent<Rotate> () != null) {
					RotatableCube.GetComponent<Rotate> ().rotate (RotatableCube.GetComponent<Collider>().attachedRigidbody, 1);
				}
			} else if (Mathf.Sign (diff.y) == 1 && diff.y >= slapSensitivity) {
				if (RotatableCube.GetComponent<Rotate> () != null) {
                    RotatableCube.GetComponent<Rotate>().rotate(RotatableCube.GetComponent<Collider>().attachedRigidbody, -2);
                }
			} else if (Mathf.Sign (diff.y) == -1 && diff.y <= -slapSensitivity) {
				if (RotatableCube.GetComponent<Rotate> () != null) {
					RotatableCube.GetComponent<Rotate> ().rotate (RotatableCube.GetComponent<Collider>().attachedRigidbody, 2);
				}
			}
		}
        else if (collider.tag == "Left")
        { // check which face being collidered
            emptyTouchingPos = OVRInput.GetLocalControllerPosition(Controller);
            Vector3 diff = lastTouchingPos - emptyTouchingPos;
            if (Mathf.Sign(diff.x) == 1 && diff.x >= slapSensitivity)
            {
                if (RotatableCube.GetComponent<Rotate>() != null)
                {
                    RotatableCube.GetComponent<Rotate>().rotate(RotatableCube.GetComponent<Collider>().attachedRigidbody, -1);
                }
            }
            else if (Mathf.Sign(diff.x) == -1 && diff.x <= -slapSensitivity)
            {
                if (RotatableCube.GetComponent<Rotate>() != null)
                {
                    RotatableCube.GetComponent<Rotate>().rotate(RotatableCube.GetComponent<Collider>().attachedRigidbody, 1);
                }
            }
            else if (Mathf.Sign(diff.y) == 1 && diff.y >= slapSensitivity)
            {
                if (RotatableCube.GetComponent<Rotate>() != null)
                {
                    RotatableCube.GetComponent<Rotate>().rotate(RotatableCube.GetComponent<Collider>().attachedRigidbody, -2);
                }
            }
            else if (Mathf.Sign(diff.y) == -1 && diff.y <= -slapSensitivity)
            {
                if (RotatableCube.GetComponent<Rotate>() != null)
                {
                    RotatableCube.GetComponent<Rotate>().rotate(RotatableCube.GetComponent<Collider>().attachedRigidbody, 2);
                }
            }
        }

        // HEY DO THIS
        // Fix rotations when positioned differently around the cube.

    }

	void Update () {
        /*
		Ray raydirection = new Ray(transform.position, transform.forward);
		Debug.DrawRay (transform.position, transform.forward, Color.black, 1);
		RaycastHit hit;

		if (Physics.Raycast(raydirection, out hit, 1000)) {
			if (hit.collider.tag == "Back") {
				print (" O WOW");
				face = 1;
					//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
				}
			else if (hit.collider.tag == "Left") {
				print (" O Left");
				face = 2;
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "right") {
				print (" O Right");
				face = 3;
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Top") {
				print (" O Top");
				face = 4;
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Bottom") {
				print (" O Bottom");
				face = 5;
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Front") {
				print (" O Front");
				face = 6;
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
		}
        */

		if (mHandState == State.TOUCHING) {
		}


		timeSinceLastCall += Time.deltaTime;
		if(mHandState == State.HOLDING){
		}

		switch (mHandState)
		{
		case State.TOUCHING:
			
			if (mTempJoint == null && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f)
			{
				mHeldObject.velocity = Vector3.zero;
				//mTempJoint = mHeldObject.gameObject.AddComponent<FixedJoint>();
				//mTempJoint.connectedBody = AttachPoint;
				mHandState = State.HOLDING;
			}
			break;
		case State.HOLDING:
			break;
		}
		previousVelocityx = currentVelocityx;
	}

	private void throwObject()//should check parameters of rotation here to rotate cube
	{
		//mHeldObject.velocity = OVRInput.GetLocalControllerVelocity(Controller);	//for moving objects
		print(OVRInput.GetLocalControllerVelocity(Controller));
		/*mHeldObject.angularVelocity = GetAngularVelocity();
		mHeldObject.maxAngularVelocity = mHeldObject.angularVelocity.magnitude;*/
	}

	private Vector3 GetAngularVelocity(){
		Quaternion deltaRotation = currentRotation * Quaternion.Inverse (lastRotation);
		return new Vector3 (Mathf.DeltaAngle (0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle (0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle (0, deltaRotation.eulerAngles.z));
	}
}
