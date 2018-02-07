using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	private Rigidbody mHeldObject;
	private FixedJoint mTempJoint;


	private float previousVelocityx = 0f;
	private float currentVelocityx = 0f;

    private float slapSensitivity = 0.05f;

	private Vector3 lastTouchingPos;
	private Vector3 emptyTouchingPos;

	private GameObject RotatableCube;
	private Rotate CubeScript;
	private Collider CubeCollider;

	public Text helpfulText;

	public bool isPunching, isFist;

	public GameObject noteEatingFace;
	public bool isFaceEatingNote = false;

	private Quaternion lastRotation, currentRotation;


	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;

	void Start ()
	{
        isPunching = isFist = false;
		if (AttachPoint == null) {
			AttachPoint = GetComponent<Rigidbody>();
		}
		RotatableCube = GameObject.FindGameObjectWithTag("Rotatable");
		CubeScript = RotatableCube.GetComponent<Rotate> ();
		CubeCollider = RotatableCube.GetComponent<Collider> ();

		lHapticsClip = new OVRHapticsClip (lVibeClip);
        rHapticsClip = new OVRHapticsClip (rVibeClip);

        noteEatingFace = GameObject.FindGameObjectWithTag("Right");
        if (noteEatingFace != null && noteEatingFace.GetComponent<noteCollision>() != null) {
            print("faceearting");
            isFaceEatingNote = noteEatingFace.GetComponent<noteCollision>().isColliding; }
    }

	void OnTriggerEnter(Collider collider)
	{
		lastTouchingPos = OVRInput.GetLocalControllerPosition(Controller);
        // check which face being collidered
        /*      if (collider.tag == "Back")     { helpfulText.text = "Back"; } 
         * else if (collider.tag == "Left")     { helpfulText.text = "Left"; }  
         * else if (collider.tag == "Right")    { helpfulText.text = "Right"; }
         * else if (collider.tag == "Front")    { helpfulText.text = "Front"; }
         * else if (collider.tag == "Front")    { helpfulText.text = "Front"; }	
         * else if (collider.tag == "Bottom")   { helpfulText.text = "Bottom"; }    */

        if (collider.tag == "Cube" && isFist && isFaceEatingNote) {
			//helpfulText.text = "PUNCH";
            isPunching = true;
		}
	}

	void OnTriggerExit(Collider collider) // Rotates Cube WRT global axes when one of the six frozen (position & rotation) faces are slapped.
	{
		//helpfulText.text = "KICK";
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
        if (CubeScript != null){
            if (!CubeScript.moving && isFist == false && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f) {
				if 		(Mathf.Sign (directionOne) == 1 && directionOne >= slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, axisOne);
				else if (Mathf.Sign (directionOne) == -1 && directionOne <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisOne);
				else if (Mathf.Sign (directionTwo) == 1 && directionTwo >= slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, axisTwo);
				else if (Mathf.Sign (directionTwo) == -1 && directionTwo <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisTwo);
			}
		}
    }

	void Update () {

        // noteCollision script with isColliding variable needs more frames than Start to GetComponent, so keep trying until we get it
        if (noteEatingFace.GetComponent<noteCollision>() == null) 
        {
            print("attemping to GetComponent<noteCollision>().isColliding");
            isFaceEatingNote = noteEatingFace.GetComponent<noteCollision>().isColliding;
        }

		if(mHandState == State.HOLDING){
		}

		if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f ) // Neutral? Punch? Swipe?
        { //&& OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f
            print("It's me");
            if (Controller == OVRInput.Controller.RTouch) { // Right Controller, Punch
				if (noteEatingFace != null && noteEatingFace.GetComponent <noteCollision>() != null && noteEatingFace.GetComponent<noteCollision> ().rVibrate) {
					OVRHaptics.RightChannel.Mix (rHapticsClip);
					noteEatingFace.GetComponent<noteCollision> ().rVibrate = false;
				}
					
				transform.GetComponent<SphereCollider> ().center = new Vector3 (0.03f, -0.03f, -0.03f);

			} else { // Left Controller, Punch
				if (noteEatingFace.GetComponent<noteCollision> ().lVibrate) {
					OVRHaptics.LeftChannel.Mix (lHapticsClip);
					noteEatingFace.GetComponent<noteCollision> ().lVibrate = false;
				}

				transform.GetComponent<SphereCollider> ().center = new Vector3 (-0.03f, -0.03f, -0.03f);
			}
			isFist = true;
		} else { // Both Hands, Neutral/Swipe
            print("It's also me");
			transform.GetComponent<SphereCollider> ().center = new Vector3 (-0.01f, -0.04f, 0f);
			isFist = false;
		}
	
		switch (mHandState)
		{
		case State.TOUCHING:
			if (mTempJoint == null && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f)
			{
				mHeldObject.velocity = Vector3.zero;
				mHandState = State.HOLDING;
			}
			break;
		}
		previousVelocityx = currentVelocityx;
	}
}
