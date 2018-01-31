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

	private Vector3 lastTouchingPos;
	private Vector3 emptyTouchingPos;

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

		if (collider.gameObject.layer == LayerMask.NameToLayer ("rotatable")) {
			emptyTouchingPos = OVRInput.GetLocalControllerPosition (Controller);
			Vector3 diff = lastTouchingPos - emptyTouchingPos;
			//print ("diff.y: " + diff.y);
			if (Mathf.Sign (diff.x) == 1 && diff.x >= 0.05) { //if positive 
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, -1);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, -1);
					//print ("000000000000000000000");
				}
			} else if (Mathf.Sign (diff.x) == -1 && diff.x <= -0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, 1);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, 1);
					//print ("1111111111111111111111111");
				}
			} else if (Mathf.Sign (diff.y) == 1 && diff.y >= 0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, -2);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, -2);
					//print ("222222222222222222222222222");
				}
			} else if (Mathf.Sign (diff.y) == -1 && diff.y <= -0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, 2);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, 2);
					//print ("3333333333333333333333333");
				}
			}
		}

		// HEY DO THIS
		// Fix rotations when positioned differently around the cube.

	}

	void Update () {
		Ray raydirection = new Ray(transform.position, transform.forward);
		Debug.DrawRay (transform.position, transform.forward, Color.black, 1);
		RaycastHit hit;

		if (Physics.Raycast(raydirection, out hit, 1000)) {
			if (hit.collider.tag == "Back") {
				print (" O WOW");
					//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
				}
			else if (hit.collider.tag == "Left") {
				print (" O Left");
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "right") {
				print (" O Right");
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Top") {
				print (" O Top");
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Bottom") {
				print (" O Bottom");
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else if (hit.collider.tag == "Front") {
				print (" O Front");
				//hit.transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
		}


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
