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
		print ("////////////////////////////////");
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
					print ("000000000000000000000");
				}
			} else if (Mathf.Sign (diff.x) == -1 && diff.x <= -0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, 1);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, 1);
					print ("1111111111111111111111111");
				}
			} else if (Mathf.Sign (diff.y) == 1 && diff.y >= 0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, -2);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, -2);
					print ("222222222222222222222222222");
				}
			} else if (Mathf.Sign (diff.y) == -1 && diff.y <= -0.05) { // need to fine tune this
				if (collider.attachedRigidbody.GetComponent<Rotate> () != null) {
					//collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody.GetComponent<Rotate> ().rotate (collider.gameObject.transform.parent.gameObject.GetComponent<Collider>().attachedRigidbody, 2);
					collider.attachedRigidbody.GetComponent<Rotate> ().rotate (collider.attachedRigidbody, 2);
					print ("3333333333333333333333333");
				}
			}
		}
			

		/*else if ((OVRInput.GetLocalControllerVelocity (Controller).y) > 0.2f) {
			//print ("here");
			if (mHeldObject != null && mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
				mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, 2);
				//print ("00000000000000000000000000here");
			}
		}
		else if ((OVRInput.GetLocalControllerVelocity (Controller).y) < -0.2f) {
			//print ("there");
			if (mHeldObject != null && mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
				mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, -2);
				//print ("11111111111111111111111111111111there");
			}
		}*/

		// HEY DO THIS
		// Fix rotations when positioned differently around the cube.

	}

	void Update () {
		// HEY DO THIS
		// To let the cube rotate in one direction multiple times,
		// We need to check if it leaves and re-enters the TOUCHING state. so check if it goes to EMPTY and if it does, uuuuh
		// set previousVelocityx to -previousVelocityx or just something that's the opposite sign.
		if (mHandState == State.TOUCHING) {
			//print ("WOW");
		}


		timeSinceLastCall += Time.deltaTime;
		//print("deltaTime: " + Time.deltaTime);
		if(mHandState == State.HOLDING){
			/*
			lastRotation = currentRotation;
			currentRotation = mHeldObject.transform.rotation;
			lastMotionTrack = motionTrack;
			motionTrack = OVRInput.GetLocalControllerPosition (Controller);
			*/
		}

		switch (mHandState)
		{
		case State.TOUCHING:
			/*currentVelocityx = OVRInput.GetLocalControllerVelocity (Controller).x;

			if (currentVelocityx > 0.1 || currentVelocityx < -0.1) {
				//print ("velocity on x: " + OVRInput.GetLocalControllerVelocity (Controller).x);
			}

			if (currentVelocityx > 0.3f && Mathf.Sign(previousVelocityx) != Mathf.Sign(currentVelocityx)) {
				//print (OVRInput.GetLocalControllerVelocity (Controller));
				if (mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
					//if (timeSinceLastCall >= 1)
					mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, 1);
				}
			} else if (currentVelocityx < -0.3f && Mathf.Sign(previousVelocityx) != Mathf.Sign(currentVelocityx)) {
				if (mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
					mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, -1);
				}
			}
*/
			if (mTempJoint == null && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f)
			{
				mHeldObject.velocity = Vector3.zero;
				//mTempJoint = mHeldObject.gameObject.AddComponent<FixedJoint>();
				//mTempJoint.connectedBody = AttachPoint;
				mHandState = State.HOLDING;
			}
			break;
		case State.HOLDING:
			/*
			if (OVRInput.GetLocalControllerVelocity (Controller).x > 0.1 || OVRInput.GetLocalControllerVelocity (Controller).x < -0.1) {
				print ("velocity on x: " + OVRInput.GetLocalControllerVelocity (Controller).x);
			}
			if ((OVRInput.GetLocalControllerVelocity (Controller).x) > 0.5f) {
				//print (OVRInput.GetLocalControllerVelocity (Controller));
				if (mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
					//if (timeSinceLastCall >= 1)
					mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, 1);
				}
			} else if ((OVRInput.GetLocalControllerVelocity (Controller).x) < -0.2f) {
				if (mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
					mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, -1);
				}
			}



			if (OVRInput.GetDown (OVRInput.Button.One, Controller)) { // I used GetDown instead of Get so it rotates properly instead of flippin out
				print ("a button pressed while holding an object");
				if (mHeldObject.GetComponent<Rotate> () != null) { // check if the held object has the Rotate script
					mHeldObject.GetComponent<Rotate> ().rotate (mHeldObject, 1); // I made a new function in Rotate.cs so I can call it from here ( can't call Rotate.Update i think)
				}
			}

			if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) < 0.5f) // mTempJoint != null && 
			{
				Object.DestroyImmediate(mTempJoint);
				mTempJoint = null;
				throwObject();
				mHandState = State.EMPTY;
			}
			*/
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
