using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSongList : MonoBehaviour {
    // this script activates the score pedestal and it lerps out of the floor, with the score appearing on top.
    public float speed = 1f; // was 15
    //public OVRInput.Controller Controller = OVRInput.Controller.LTouch;
    public bool moving = false;

    private Quaternion targetOrientation = Quaternion.identity;
    private Quaternion startPos;
    private Quaternion endPos;
    private bool isLerping;
    private float timeStartedLerping;

    // Use this for initialization
    void Start()
    {
        startPos = transform.rotation;
    }

    public void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / 0.6f;
            //print ("PERC%: " + percentageComplete);
            this.transform.rotation = Quaternion.Lerp(
                startPos,
                endPos,
                percentageComplete
            /*Mathf.Clamp01 (Time.deltaTime * speed)*/
            );
            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
                startPos = transform.rotation;
            }
        }
    }

    public void startLerping()
    {
        isLerping = true;
        timeStartedLerping = Time.time;
        startPos = this.transform.rotation;
        endPos = targetOrientation;
    }

    public void rotate(Rigidbody obj, int dir) // takes a Rigidbody gameObject and a number which determines which direction it will rotate.
    {
        if (dir == 1)
        {
            targetOrientation = Quaternion.Euler(0, -90, 0) * startPos;//targetOrientation;
        }

        else if (dir == -1)
        {
            targetOrientation = Quaternion.Euler(0, 90, 0) * startPos;//targetOrientation;
        }

        else if (dir == 2)
        {
            targetOrientation = Quaternion.Euler(90, 0, 0) * startPos;//targetOrientation;
        }

        else if (dir == -2)
        {
            targetOrientation = Quaternion.Euler(-90, 0, 0) * startPos;//targetOrientation;
        }

        else if (dir == 3)
        {
            targetOrientation = Quaternion.Euler(0, 0, 90) * startPos;//targetOrientation;
        }

        else if (dir == -3)
        {
            targetOrientation = Quaternion.Euler(0, 0, -90) * startPos;//targetOrientation;
        }
        if (!isLerping)
        {
            startLerping();
        }
    }
}
