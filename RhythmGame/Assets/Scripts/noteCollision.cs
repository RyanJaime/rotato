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
	public ParticleSystem ampNoiseLines;
	public ParticleSystem noteExplosion;
    //public ParticleSystem leftSpeakerParticle, rightSpeakerParticle;

    public int combo;
	private int comboMultiplier;
	private ParticleSystem.MainModule ampMain;
	private ParticleSystem.MainModule noteMain;

    public int getPlayerScore()
    {
        return playerScore;
    }

	// Use this for initialization
	void Start () {
        playerScore = 0;
        combo = 0;
		comboMultiplier = 1;
        isColliding = false;
        handObjects = GameObject.FindGameObjectsWithTag ("Hand");
		localIsPunchingL = handObjects [1].GetComponent<Hand> ().isPunching;
		localIsPunchingR = handObjects [0].GetComponent<Hand> ().isPunching;
		lTouch = handObjects [1].GetComponent<Hand> ().Controller;
		rTouch = handObjects [0].GetComponent<Hand> ().Controller;
		lHapticsClip = new OVRHapticsClip (lVibeClip);
		rHapticsClip = new OVRHapticsClip (rVibeClip);
		if (ampNoiseLines)
			ampMain = ampNoiseLines.main;
		if (noteExplosion) {
			noteMain = noteExplosion.main;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (handObjects.Length > 0)
        {
            localIsPunchingL = handObjects[1].GetComponent<Hand>().isPunching;
            localIsPunchingR = handObjects[0].GetComponent<Hand>().isPunching;
        }
		if (combo == 0) {
			comboMultiplier = 1;
			ampMain.startColor = Color.white;
			noteMain.startColor = Color.white;
		} if (combo == 10) {
			comboMultiplier = 2;
			ampMain.startColor = Color.red;
			noteMain.startColor = Color.red;
		} else if (combo == 20) {
			comboMultiplier = 3;
			ampMain.startColor = Color.yellow;
			noteMain.startColor = Color.yellow;
		} else if (combo == 30) {
			comboMultiplier = 4;
			ampMain.startColor = Color.green;
			noteMain.startColor = Color.green;
		} else if (combo == 40) {
			comboMultiplier = 5;
			ampMain.startColor = Color.blue;
			noteMain.startColor = Color.blue;
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
        if (note.CompareTag("note")) {
            isColliding = true;
			bool temp = note.GetComponent<noteInformation> ().hasGoneThruCorrectFace;
            //note.GetComponent<noteInformation>().hasGoneThruCorrectFace
			//note.GetComponent<obstacleMovement> ().colliding = true;
			if (localIsPunchingL && temp) {
				ampNoiseLines.Play ();
				Instantiate (noteExplosion, note.transform.position, Quaternion.identity);
				lVibrate = true;
				lHapticsClip.Reset ();
				Destroy (note.gameObject);
				playerScore += comboMultiplier;
				combo++;
				//blackSquiggle.Play();
				//StartCoroutine(soundParticle(0.5f));
				innerScore.text = playerScore.ToString ();
				//scoreText1.text = scoreText2.text = scoreText3.text = scoreText4.text = playerScore.ToString();
				//comboText.text = combo.ToString();
				if (lTouch == OVRInput.Controller.LTouch) {
					OVRHaptics.LeftChannel.Mix (lHapticsClip);
				} 
					
				handObjects [1].GetComponent<Hand> ().isPunching = false;
			} else if (localIsPunchingR && temp) {
				ampNoiseLines.Play ();
				Instantiate (noteExplosion, note.transform.position, Quaternion.identity);
				rVibrate = true;
				rHapticsClip.Reset ();
				Destroy (note.gameObject);
				playerScore += comboMultiplier;
				combo++;
				//blackSquiggle.Play();
				//StartCoroutine(soundParticle(0.5f));

				innerScore.text = playerScore.ToString ();
				//scoreText1.text = scoreText2.text = scoreText3.text = scoreText4.text = playerScore.ToString();
				//comboText.text = combo.ToString(); 
				if (rTouch == OVRInput.Controller.RTouch) {
					OVRHaptics.RightChannel.Mix (rHapticsClip);
				} 
				handObjects [0].GetComponent<Hand> ().isPunching = false;
			}
		} /* else if (note.CompareTag("Hand")) { // Bug happens when transferring scenes. LocalIsPunching never gets modified.
			//print("play black squiggle!");
			blackSquiggle.Play();
		} */

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
