﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extendScorePedestal : MonoBehaviour {
    // this script activates the score pedestal and it lerps out of the floor, with the score appearing on top.

    public bool startLerping = false;
    float lerpTime = 1f;
    float currentLerpTime;

    float moveDistance = 10f;

    Vector3 startPos;
    Vector3 endPos;

    private GameObject singleHex;
    public TextMesh pedestalScore;
    GameObject noteCollisionObject;

    protected void Start()
    {
        singleHex = GameObject.FindGameObjectWithTag("singleHex");
        singleHex.gameObject.SetActive(false);
        noteCollisionObject = GameObject.FindGameObjectWithTag("Rotatable");
        pedestalScore.gameObject.SetActive(false);
        startPos = transform.localScale;
        endPos = transform.localScale + new Vector3(0, 0, 40f);
        currentLerpTime = 0;
    }

    protected void Update()
    {
        if (startLerping)
        {
            singleHex.gameObject.SetActive(true);
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            
            transform.localScale = Vector3.Lerp(startPos, endPos, perc);
            if (perc >= 1)
            {    
                pedestalScore.gameObject.SetActive(true);
                pedestalScore.text = noteCollisionObject.GetComponent<noteCollision>().getPlayerScore().ToString();
            }
        }
    }
}
