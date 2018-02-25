using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class postGameOptions : MonoBehaviour {
    private GameObject HandObjectRef;
    private Hand HandScriptInstance;
    private GameObject CollisionObjectRef;
    private noteCollision NoteCollisionScriptInstance;
    // Use this for initialization
    void Start () {
        HandObjectRef = GameObject.FindGameObjectWithTag("Hand");
        HandScriptInstance = HandObjectRef.GetComponent<Hand>();
        CollisionObjectRef = GameObject.FindGameObjectWithTag("Rotatable");
        NoteCollisionScriptInstance = CollisionObjectRef.GetComponent<noteCollision>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Z))
        {
            print("Z pressed, going to selection scene");
            // reset score and other variables.
            HandScriptInstance.levelStarted = false;
            NoteCollisionScriptInstance.playerScore = 0;
            NoteCollisionScriptInstance.combo = 0;
            SceneManager.LoadScene("selection");
        }
        if (Input.GetKey(KeyCode.X))
        {
            print("X pressed, going to Quit");
            Application.Quit();
        }
    }
}
