using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class noteCollision : MonoBehaviour {

	public bool localIsPunchingR;
	public bool localIsPunchingL;

	public bool isColliding;
	GameObject[] handObjects;

	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;

	private OVRInput.Controller lTouch;
	private OVRInput.Controller rTouch;

	public bool lVibrate = false;
	public bool rVibrate = false;

    public int playerScore;
    public TextMesh innerScore; //scoreText1, scoreText2, scoreText3, scoreText4, comboText;

    public ParticleSystem blackSquiggle;
    //public ParticleSystem leftSpeakerParticle, rightSpeakerParticle;

    private int combo;

    public int getPlayerScore()
    {
        return playerScore;
    }

	// Use this for initialization
	void Start () {
        playerScore = 0;
        combo = 0;
        isColliding = false;
        handObjects = GameObject.FindGameObjectsWithTag ("Hand");
		localIsPunchingL = handObjects [1].GetComponent<Hand> ().isPunching;
		localIsPunchingR = handObjects [0].GetComponent<Hand> ().isPunching;
		lTouch = handObjects [1].GetComponent<Hand> ().Controller;
		rTouch = handObjects [0].GetComponent<Hand> ().Controller;
		lHapticsClip = new OVRHapticsClip (lVibeClip);
		rHapticsClip = new OVRHapticsClip (rVibeClip);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (handObjects.Length > 0)
        {
            localIsPunchingL = handObjects[1].GetComponent<Hand>().isPunching;
            localIsPunchingR = handObjects[0].GetComponent<Hand>().isPunching;
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (!other.CompareTag("note"))
        {
            print("play black squiggle!");
            combo = 0;
            comboText.text = combo.ToString();
            blackSquiggle.Play();
        }*/
    }
    void OnTriggerStay(Collider note)
    {
        if (note.CompareTag("note"))
        {
            isColliding = true;
            //note.GetComponent<noteInformation>().hasGoneThruCorrectFace
        }
        if (note.CompareTag("Hand")) //change this so it only triggers when u miss a note timing
        { 

            //print("play black squiggle!");
            //blackSquiggle.Play();
        }
		//note.GetComponent<obstacleMovement> ().colliding = true;
		if (note.CompareTag("note") && localIsPunchingL && note.GetComponent<noteInformation>().hasGoneThruCorrectFace) {
			lHapticsClip.Reset();
            lVibrate = true;
            Destroy (note.gameObject);
            playerScore++;
            combo++;
            //blackSquiggle.Play();
            //StartCoroutine(soundParticle(0.5f));
            innerScore.text = playerScore.ToString();
            //scoreText1.text = scoreText2.text = scoreText3.text = scoreText4.text = playerScore.ToString();
            //comboText.text = combo.ToString();
            if (lTouch == OVRInput.Controller.LTouch) {
                OVRHaptics.LeftChannel.Mix (lHapticsClip);
			} 
				
			handObjects[1].GetComponent<Hand> ().isPunching = false;
		}
		if (note.CompareTag("note") && localIsPunchingR && note.GetComponent<noteInformation>().hasGoneThruCorrectFace) {
            rHapticsClip.Reset();
			rVibrate = true;
			Destroy (note.gameObject);
            playerScore++;
            combo++;
            //blackSquiggle.Play();
            //StartCoroutine(soundParticle(0.5f));

            innerScore.text = playerScore.ToString();
            //scoreText1.text = scoreText2.text = scoreText3.text = scoreText4.text = playerScore.ToString();
            //comboText.text = combo.ToString(); 
            if (rTouch == OVRInput.Controller.RTouch) {
                OVRHaptics.RightChannel.Mix (rHapticsClip);
			} 
			handObjects[0].GetComponent<Hand> ().isPunching = false; 
		}

	}
    void OnTriggerExit(Collider note)
    {
        isColliding = false;
        //note.GetComponent<obstacleMovement> ().colliding = false;
    }

    private IEnumerator soundParticle(float seconds)
    {
        //leftSpeakerParticle.Play(); rightSpeakerParticle.Play();
        yield return new WaitForSeconds(seconds);
        //leftSpeakerParticle.Stop(); rightSpeakerParticle.Stop();
    }
    void playBlackSquiggleParticle()
    {
        blackSquiggle.Play();
    }
}
