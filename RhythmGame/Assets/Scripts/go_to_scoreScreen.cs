using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go_to_scoreScreen : MonoBehaviour {

    AudioSource audioSource;
    //dioClip audioClip;
    // Use this for initialization
    void Start () {
        //audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.M))
        {
            audioSource.Stop();
        }
        if (!audioSource.isPlaying)
        {
            //audioSource.clip = otherClip;
            //audioSource.Play();
            SceneManager.LoadScene("scoreScreen");
        }
    }
}
