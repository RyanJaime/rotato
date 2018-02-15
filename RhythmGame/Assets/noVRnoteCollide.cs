using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noVRnoteCollide : MonoBehaviour {
    private int debugScore;
	// Use this for initialization
	void Start () {
        debugScore = 0;
	}
	
	// Update is called once per frame
	void Update () {

    }
    void OnTriggerStay(Collider note)
    {
        if (note.CompareTag("note") && Input.GetKeyDown(KeyCode.K))
        {
            Destroy(note.gameObject);
            print("debugScore: " + ++debugScore);
            //blackSquiggle.Play();

        }
    }
}
