using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteInformation : MonoBehaviour {
    public bool hasGoneThruCorrectFace;

	private noteCollision comboKeeper;

	// Use this for initialization
	void Start () {
        hasGoneThruCorrectFace = false;
		comboKeeper = GameObject.FindGameObjectWithTag ("Rotatable").GetComponent<noteCollision> ();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider face)
    {
        if (face.tag == "RightPhysical")
        {
            hasGoneThruCorrectFace = true;
        }
        else if (face.tag == "destroyMissedNotes")// && hasGoneThruCorrectFace == false)
        {
            print(face.tag);
            StartCoroutine(destroyAfterSeconds(1));
			if (comboKeeper){ comboKeeper.combo = 0; }
            //play particle. change color.
            this.gameObject.GetComponent<Renderer>().enabled = false;
            //Destroy(gameObject);
        }
    }

    private IEnumerator destroyAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
