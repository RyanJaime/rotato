using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private GameObject songList;
    private rotateSongList SongListScript;
    

    public GameObject songListSelector;
    public selectedSong selectedSongInstance;

    private Collider CubeCollider;

	public Text helpfulText, scoreText;

	public bool isPunching, isFist, levelStarted;

    public GameObject noteEatingFace, backButton;
    private backToMenu backButtonScript;
    public bool isFaceEatingNote = false;

	private Quaternion lastRotation, currentRotation;


	OVRHapticsClip lHapticsClip;
	OVRHapticsClip rHapticsClip;
	public AudioClip lVibeClip;
	public AudioClip rVibeClip;
    private GameObject scorePedestal;
    private GameObject[] handsArray;

    //public ParticleSystem blackSquiggle;

    void Start ()
	{
        handsArray = GameObject.FindGameObjectsWithTag("Hand");
        scorePedestal = GameObject.FindGameObjectWithTag("scorePedestal");
        //songListSelector = GameObject.FindGameObjectWithTag("songSelectorHitbox");
        //selectedSongInstance = songListSelector.GetComponent<selectedSong>();
        //SongListScript = songList.GetComponent<rotateSongList>();
        isPunching = isFist = levelStarted = false;
		if (AttachPoint == null) {
			AttachPoint = GetComponent<Rigidbody>();
		}
		RotatableCube = GameObject.FindGameObjectWithTag("Rotatable");
		CubeScript = RotatableCube.GetComponent<Rotate> ();
        songList = GameObject.FindGameObjectWithTag("songList");
        

		CubeCollider = RotatableCube.GetComponent<Collider> ();

		lHapticsClip = new OVRHapticsClip (lVibeClip);
        rHapticsClip = new OVRHapticsClip (rVibeClip);

        backButton = GameObject.FindGameObjectWithTag("backPlane");
        //backButtonScript = backButton.GetComponent<backToMenu>();

        //scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        //scoreText.enabled = false;

        noteEatingFace = GameObject.FindGameObjectWithTag("Rotatable");
        //noteEatingFace =  GameObject.FindGameObjectWithTag("Cube"); // was Right
        if (noteEatingFace != null && noteEatingFace.GetComponent<noteCollision>() != null) {
            //print("faceearting");
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
        if (collider.tag ==  "end")
        {
            print("QUIT");
            Application.Quit();
        }
        if (collider.tag == "Cube")
        {
            //print("tag: " + collider.tag + " isFist: " + isFist + " isFaceEatingNote: " + isFaceEatingNote);
        }
        if (collider.tag == "Cube" && isFist)// && isFaceEatingNote)
        {
            helpfulText.text = "PUNCH";
            isPunching = true;
        }
        if ((!levelStarted && collider.tag == "Cube" && isFist) || Input.GetKey(KeyCode.K))
        { // first time you punch the cube will start the level
            if (!handsArray[0].GetComponent<Hand>().levelStarted && !handsArray[1].GetComponent<Hand>().levelStarted)
            {
                if(selectedSongInstance != null) { 
                    levelStarted = true;
                    switch (selectedSongInstance.currentSelectedSong)
                    {
                        //print("in SWITCH " + selectedSongInstance.currentSelectedSong);
                        case "LEDSpirals":
                            print("in SWITCH " + selectedSongInstance.currentSelectedSong);
                            helpfulText.text = "LOADING";
                            noteEatingFace.GetComponent<noteCollision>().playerScore = 0;
                            SceneManager.LoadScene("jonWick");
                            helpfulText.enabled = false;
                            //backButtonScript.setAudio();
                            // set byte file for MIDIparser to load
                            break;
                        case "Irodori":
                            print("in SWITCH " + selectedSongInstance.currentSelectedSong);
                            helpfulText.text = "LOADING";
                            noteEatingFace.GetComponent<noteCollision>().playerScore = 0;
                            SceneManager.LoadScene("main");
                            helpfulText.enabled = false;
                            //backButtonScript.setAudio();
                            // set byte file for MIDIparser to load
                            break;
                        case "Quit":
                            Application.Quit();
                            break;
                        default:
                            print("in SWITCH " + selectedSongInstance.currentSelectedSong);
                            break;
                    }
                }
            }
        }
	}

	void OnTriggerExit(Collider collider) // Rotates Cube WRT global axes when one of the six frozen (position & rotation) faces are slapped.
	{
		helpfulText.text = "KICK";
		emptyTouchingPos = OVRInput.GetLocalControllerPosition (Controller);
		Vector3 diff = lastTouchingPos - emptyTouchingPos;
		float directionOne = 0f, directionTwo = 0f;
		int axisOne = 0, axisTwo = 0;
		if (collider.tag == "Front") {
            //print("front rot");
			directionOne = diff.x; 	directionTwo = diff.y;
			axisOne = -1; 			axisTwo = -2;
		} else if (collider.tag == "Back") {
            //print("back rot");
            directionOne = diff.x; 	directionTwo = diff.y;
			axisOne = 1; 			axisTwo = 2;
		} else if (collider.tag == "Left") {
            //print("ledft rot");
            directionOne = diff.z; 	directionTwo = diff.y;
			axisOne = 1; 			axisTwo = 3;
		} else if (collider.tag == "Right") {
            //print("right rot");
            directionOne = diff.z;	directionTwo = diff.y;
			axisOne = -1;			axisTwo = -3;
		} else if (collider.tag == "Top") {
            //print("top tier rot");
            directionOne = diff.x;	directionTwo = diff.z;
			axisOne = 3;			axisTwo = -2;
		} else if (collider.tag == "Bottom"){
            //print("bottomom rot");
            directionOne = diff.x;	directionTwo = diff.z;
			axisOne = -3;			axisTwo = 2;
		}
        if (CubeScript == null)
        {
            print("cUBEsCRIPT nULL, LUL");
        }
        if (CubeScript != null){
            if (!CubeScript.moving && isFist == false && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f) {
				if 		(Mathf.Sign (directionOne) == 1 && directionOne >= slapSensitivity)     CubeScript.rotate (CubeCollider.attachedRigidbody, axisOne);
				else if (Mathf.Sign (directionOne) == -1 && directionOne <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisOne);
				else if (Mathf.Sign (directionTwo) == 1 && directionTwo >= slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, axisTwo);
				else if (Mathf.Sign (directionTwo) == -1 && directionTwo <= -slapSensitivity) 	CubeScript.rotate (CubeCollider.attachedRigidbody, -axisTwo);
                if (levelStarted == false) {
                    if (Mathf.Sign(directionOne) == 1 && directionOne >= slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, axisOne);
                    else if (Mathf.Sign(directionOne) == -1 && directionOne <= -slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, -axisOne);
                    else if (Mathf.Sign(directionTwo) == 1 && directionTwo >= slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, axisTwo);
                    else if (Mathf.Sign(directionTwo) == -1 && directionTwo <= -slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, -axisTwo);
                }
            }
		}
    }
    void debugRotations(string face, Vector3 diff)  // similar to OnTriggerExit, to test using keyboard inputs.
    {
        //emptyTouchingPos = OVRInput.GetLocalControllerPosition(Controller);
        //Vector3 diff = new Vector3(1, 1, 1);// lastTouchingPos - emptyTouchingPos;
        float directionOne = 0f, directionTwo = 0f;
        int axisOne = 0, axisTwo = 0;
        if (face == "Front")
        {
            directionOne = diff.x; directionTwo = diff.y;
            axisOne = -1; axisTwo = -2;
        }
        else if (face == "Back")
        {
            directionOne = diff.x; directionTwo = diff.y;
            axisOne = 1; axisTwo = 2;
        }
        else if (face == "Left")
        {
            directionOne = diff.z; directionTwo = diff.y;
            axisOne = 1; axisTwo = 3;
        }
        else if (face == "Right")
        {
            directionOne = diff.z; directionTwo = diff.y;
            axisOne = -1; axisTwo = -3;
        }
        else if (face == "Top")
        {
            directionOne = diff.x; directionTwo = diff.z;
            axisOne = 3; axisTwo = -2;
        }
        else if (face == "Bottom")
        {
            directionOne = diff.x; directionTwo = diff.z;
            axisOne = -3; axisTwo = 2;
        }
        if (CubeScript != null)
        {
            //if (!CubeScript.moving && isFist == false && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f)
            //{
                if (Mathf.Sign(directionOne) == 1 && directionOne >= slapSensitivity) CubeScript.rotate(CubeCollider.attachedRigidbody, axisOne);
                else if (Mathf.Sign(directionOne) == -1 && directionOne <= -slapSensitivity) CubeScript.rotate(CubeCollider.attachedRigidbody, -axisOne);
                else if (Mathf.Sign(directionTwo) == 1 && directionTwo >= slapSensitivity) CubeScript.rotate(CubeCollider.attachedRigidbody, axisTwo);
                else if (Mathf.Sign(directionTwo) == -1 && directionTwo <= -slapSensitivity) CubeScript.rotate(CubeCollider.attachedRigidbody, -axisTwo);
                if (levelStarted == false)
                {
                    if (Mathf.Sign(directionOne) == 1 && directionOne >= slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, axisOne);
                    else if (Mathf.Sign(directionOne) == -1 && directionOne <= -slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, -axisOne);
                    else if (Mathf.Sign(directionTwo) == 1 && directionTwo >= slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, axisTwo);
                    else if (Mathf.Sign(directionTwo) == -1 && directionTwo <= -slapSensitivity) SongListScript.rotate(CubeCollider.attachedRigidbody, -axisTwo);
                }
            //}
        }
    }
    public void setSongSelectorNull()
    {
        print("setSongSelectorNull function called");
        songList = null;
        songListSelector = null;
        songList = null;
        songListSelector = null;

    }


    public void setSongSelectorAfterSceneChange()
    {
        songList = GameObject.FindGameObjectWithTag("songList");
        songListSelector = GameObject.FindGameObjectWithTag("songSelectorHitbox");
        if (songList != null) { SongListScript = songList.GetComponent<rotateSongList>(); }
        if (songListSelector != null) { selectedSongInstance = songListSelector.GetComponent<selectedSong>(); }
    }

	void FixedUpdate () {
        if (Input.GetKey(KeyCode.W))
        {
            print("rotate!");
            debugRotations("Front", new Vector3(1,0,0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            debugRotations("Front", new Vector3(-1,-1,-1));
        }
        if (Input.GetKey(KeyCode.T))
        {
            print("T pressed, going to title scene");
            SceneManager.LoadScene("title");
        }
        if (Input.GetKey(KeyCode.J))
        {
            levelStarted = true;
            SceneManager.LoadScene("jonWick");
            songListSelector = GameObject.FindGameObjectWithTag("songSelectorHitbox");
            selectedSongInstance = songListSelector.GetComponent<selectedSong>();
            SongListScript = songList.GetComponent<rotateSongList>();
        }
        if (songList == null || songListSelector == null || selectedSongInstance == null || SongListScript == null)
        {
            print("SETTING SONG SELECTOR STUFF");
            setSongSelectorAfterSceneChange();
        }

        if (scorePedestal != null && OVRInput.Get(OVRInput.Button.One))
        {
            //scorePedestal.GetComponent<extendScorePedestal>().elevateFunction();
            GameObject.FindGameObjectWithTag("pedestal").SetActive(true);
            scorePedestal.GetComponent<extendScorePedestal>().startLerping = true;
        }
        isFaceEatingNote = noteEatingFace.GetComponent<noteCollision>().isColliding; // true when note colliding with cube's hitbox

		if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f ) // Neutral? Punch? Swipe?
        { //&& OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) >= 0.5f
            //print("It's me");
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
            //print("It's also me");
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
    private IEnumerator LoadLevelAfterDelay(int seconds)
    {
        helpfulText.text = "LOADING";
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("jonWick");
        // helpfulText.text = "done";
        helpfulText.enabled = false;
    }
}
