using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedSong : MonoBehaviour {

    public string currentSelectedSong;
    public List<AudioClip> AudioList = new List<AudioClip>();

	// Use this for initialization
	void Start () {
        AudioList.Add(Resources.Load("jonWick") as AudioClip);
        AudioList.Add(Resources.Load("irodori") as AudioClip);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider songTile)
    {
        print("songTile " + songTile);
        switch (songTile.tag)
        {
            case "LEDSpirals":
                currentSelectedSong = "LEDSpirals";
                // set byte file for MIDIparser to load
                break;
            case "Irodori":
                currentSelectedSong = "Irodori";
                // set byte file for MIDIparser to load
                break;
            default:
                break;
        }
    }
}
