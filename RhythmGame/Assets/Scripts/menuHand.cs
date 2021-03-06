﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuHand : MonoBehaviour {

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

	private Quaternion lastRotation, currentRotation;

	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;

    private Text scoreText;

    private GameObject[] menuHands;
    private bool isMenuHandScriptEnabledL;
    private bool isMenuHandScriptEnabledR;

    void Start ()
	{
        menuHands = GameObject.FindGameObjectsWithTag("Hand");
        isMenuHandScriptEnabledL = menuHands[0].GetComponent<menuHand>();
        isMenuHandScriptEnabledR = menuHands[1].GetComponent<menuHand>();

        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        scoreText.enabled = false;

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
		lastTouchingPos = OVRInput.GetLocalControllerPosition(Controller);

		if (collider.tag == "Cube" && isFist) {
			//helpfulText.text = "PUNCH";
			isPunching = true;
		}

	}

	void OnTriggerExit(Collider collider)
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

		if (!CubeScript.moving && isFist == false && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f) {
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

		if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f) //&& OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f)
                {
			if (Controller == OVRInput.Controller.RTouch) {
				transform.GetComponent<SphereCollider> ().center = new Vector3 (0.03f, -0.03f, -0.03f);

			} else {
				transform.GetComponent<SphereCollider> ().center = new Vector3 (-0.03f, -0.03f, -0.03f);
			}
			isFist = true;
		} else {
			transform.GetComponent<SphereCollider> ().center = new Vector3 (-0.01f, -0.04f, 0f);
			isFist = false;
		}

		if (isPunching || Input.GetKey(KeyCode.K)) {
            //transform.
            helpfulText.enabled = false;
            
            //transform.gameObject.GetComponent<Hand>().enabled = true;
            StartCoroutine(LoadLevelAfterDelay(3));
            //SceneManager.LoadScene("main");
            
            //scoreText = GameObject.FindGameObjectsWithTag("scoreTextTag");
            scoreText.enabled = true;
            if (isMenuHandScriptEnabledL || isMenuHandScriptEnabledR)
            {
                menuHands[0].GetComponent<menuHand>().isFist = true; // temp bugfix
                menuHands[1].GetComponent<menuHand>().isFist = true;
                menuHands[0].GetComponent<Hand>().enabled = true;
                menuHands[1].GetComponent<Hand>().enabled = true;
                menuHands[0].GetComponent<menuHand>().enabled = false;
                menuHands[1].GetComponent<menuHand>().enabled = false;
            }
            helpfulText.enabled = false;
            //this.enabled = false;
        }
	}

	private Vector3 GetAngularVelocity(){
		Quaternion deltaRotation = currentRotation * Quaternion.Inverse (lastRotation);
		return new Vector3 (Mathf.DeltaAngle (0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle (0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle (0, deltaRotation.eulerAngles.z));
	}
    private IEnumerator LoadLevelAfterDelay(int seconds)
    {
        helpfulText.text = "LOADING";
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("main");
        helpfulText.text = "done";
        //helpfulText.enabled = false;

    }
}


