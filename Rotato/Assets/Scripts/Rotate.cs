using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    private int rotateUpDown = 0;
    private int rotateLR = 0;

    private float oldAngleHorizontal;
    private float oldAngleVertical;

    private Quaternion startingRotation;
    public float speed = 15;
    private int timesHitHorizontal = 0;
    private int timesHitVertical = 0;

    // Use this for initialization
    void Start () {
        startingRotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            timesHitHorizontal--;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            timesHitHorizontal++;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            timesHitVertical++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            timesHitVertical--;
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(timesHitVertical * 90, timesHitHorizontal * 90, 0), Time.deltaTime * speed);
    }
}
