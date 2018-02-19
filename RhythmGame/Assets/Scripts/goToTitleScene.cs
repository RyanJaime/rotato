using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToTitleScene : MonoBehaviour {
    public GameObject lHand, rHand;
	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("title");
        lHand.GetComponent<Hand>().enabled = true;
        rHand.GetComponent<Hand>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
