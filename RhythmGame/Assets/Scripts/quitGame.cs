﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        print("hit");
        if (other.CompareTag("Hand"))
        {
            print("QUIT");
            Application.Quit();
        }
    }
}
