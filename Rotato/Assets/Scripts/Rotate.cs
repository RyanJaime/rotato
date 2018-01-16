using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 15;
    private int timesHitHorizontal = 0;
    private int timesHitVertical = 0;
    private int timesHitRolling = 0;

    //0 = front
    //1 = right
    //2 = back
    //3 = left
    private int frontFace = 0;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            timesHitHorizontal--;
            changeFace(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            timesHitHorizontal++;
            changeFace(false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            timesHitVertical++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            timesHitVertical--;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            timesHitRolling++;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            timesHitRolling--;
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(timesHitVertical * 90, timesHitHorizontal * 90, timesHitRolling*90), Time.deltaTime * speed);
        
    }

    void changeFace(bool increase)
    {
        if (increase) { frontFace++; }
        else { frontFace--; }

        if(frontFace > 3) { frontFace -= 4; }
        if(frontFace < 0) { frontFace += 4; }
        print(frontFace);
    }
}
