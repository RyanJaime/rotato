using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rHand : MonoBehaviour {

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
	private Rotate CubeScript;
	private Collider CubeCollider;

	public Text helpfulText;

	public bool isPunching = false;
	public bool isFist = false;

	public GameObject noteEatingFace;
	public bool isFaceEatingNote = false;

	private Quaternion lastRotation, currentRotation;


	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;

	void Start ()
	{
		if (AttachPoint == null) {
			AttachPoint = GetComponent<Rigidbody>();
		}
		RotatableCube = GameObject.FindGameObjectWithTag("Rotatable");
		CubeScript = RotatableCube.GetComponent<Rotate> ();
		CubeCollider = RotatableCube.GetComponent<Collider> ();

		lHapticsClip = new OVRHapticsClip (lVibeClip);
		rHapticsClip = new OVRHapticsClip (rVibeClip);
	}

	void OnTriggerEnter(Collider collider)
	{
		//print ("////////////////////////////////");
		lastTouchingPos = OVRInput.GetLocalControllerPosition(Controller);

		/*if (collider.tag == "Back") { // check which face being collidered
			helpfulText.text = "Back";
		} else if (collider.tag == "Left") { // check which face being collidered
			helpfulText.text = "Left";
		}  else if (collider.tag == "Right") { // check which face being collidered
			helpfulText.text = "Right";
		}	else if (collider.tag == "Front") { // check which face being collidered
			helpfulText.text = "Front";
		}	else if (collider.tag == "Front") { // check which face being collidered
			helpfulText.text = "Front";
		}	else if (collider.tag == "Bottom") { // check which face being collidered
			helpfulText.text = "Bottom";
		}*/
		noteEatingFace = GameObject.FindGameObjectWithTag ("Right");
		isFaceEatingNote = noteEatingFace.GetComponent<noteCollision> ().isColliding;

		if (collider.tag == "Cube" && isFist && isFaceEatingNote) {
			helpfulText.text = "PUNCH";
			isPunching = true;

		}

	}

	void OnTriggerExit(Collider collider)
	{
		/*if (OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f && OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f) {
			isPunching = true;
		} else {
			isPunching = false;
		}*/

		helpfulText.text = "KICK";
		// RotatableCube = GameObject.FindGameObjectWithTag("Rotatable");
		emptyTouchingPos = OVRInput.GetLocalControllerPosition (Controller);
		Vector3 diff = lastTouchingPos - emptyTouchingPos;
		float directionOne = 0f, directionTwo = 0f;
		int axisOne = 0, axisTwo = 0;
		if (collider.tag == "Front") {
			directionOne = diff.x; 	directionTwo = diff.y;
			axisOne = -1; 			axisTwo = -2;
		} else if (collider.tag == "Back") {
			directionOne = diff.x; 	directionTwo = diff.y;
			axisOne = 1; 			axisTwo = 2;
		} else if (collider.tag == "Left") {
			directionOne = diff.z; 	directionTwo = diff.y;
			axisOne = 1; 			axisTwo = 3;
		} else if (collider.tag == "Right") {
			directionOne = diff.z;	directionTwo = diff.y;
			axisOne = -1;			axisTwo = -3;
		} else if (collider.tag == "Top") {
			directionOne = diff.x;	directionTwo = diff.z;
			axisOne = 3;			axisTwo = -2;
		} else if (collider.tag == "Bottom"){
			directionOne = diff.x;	directionTwo = diff.z;
			axisOne = -3;			axisTwo = 2;
		}

		if (!CubeScript.moving && isFist == false) {
			if (CubeScript != null) {
				if 		(Mathf.Sign (directionOne) == 1 && directionOne >= slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, axisOne);
				else if (Mathf.Sign (directionOne) == -1 && directionOne <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisOne);
				else if (Mathf.Sign (directionTwo) == 1 && directionTwo >= slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, axisTwo);
				else if (Mathf.Sign (directionTwo) == -1 && directionTwo <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisTwo);
			}
		}

	}

	void Update () {

		timeSinceLastCall += Time.deltaTime;
		//isPunching = false;
		if(mHandState == State.HOLDING){
		}

		if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f) {
			if (Controller == OVRInput.Controller.RTouch) {
				if (noteEatingFace.GetComponent<noteCollision> ().rVibrate) {
					OVRHaptics.RightChannel.Mix (rHapticsClip);
					noteEatingFace.GetComponent<noteCollision> ().rVibrate = false;
				}
				transform.GetComponent<SphereCollider> ().center = new Vector3 (0.03f, -0.03f, -0.03f);
			}
			isFist = true;
		} else {
			transform.GetComponent<SphereCollider> ().center = new Vector3 (-0.01f, -0.02f, 0.03f);
			isFist = false;
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
