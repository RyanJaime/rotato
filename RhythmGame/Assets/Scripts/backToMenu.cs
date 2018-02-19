using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backToMenu : MonoBehaviour {
    private GameObject audioSrc;
    public GameObject Speaker, Colliders, floor, sphere, dLight, pLight;
    // Use this for initialization
    void Start () {
        //audioSrc = GameObject.FindGameObjectWithTag("audioSource");
    }
	
	// Update is called once per frame
	void Update () {
        audioSrc = GameObject.FindGameObjectWithTag("audioSource");
    }
    public void setAudio()
    {
        audioSrc = GameObject.FindGameObjectWithTag("audioSource");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (audioSrc != null)
            {
                //Destroy(Speaker); Destroy(Colliders); Destroy(floor); Destroy(sphere); Destroy(dLight); Destroy(pLight);
                audioSrc.GetComponent<AudioSource>().Stop();
                SceneManager.LoadScene("end");
            }
        }
    }
}
