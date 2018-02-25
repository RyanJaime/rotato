using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToTitleScene : MonoBehaviour {
    private bool isSceneLoaded = false; // has title scene been loaded yet?
    private GameObject HandObjectRef;
    private Hand HandScriptInstance;
    // Use this for initialization
    void Start () {
        HandObjectRef = GameObject.FindGameObjectWithTag("Hand");
        HandScriptInstance = HandObjectRef.GetComponent<Hand>();
        //SongListScript = songList.GetComponent<rotateSongList>();
        //SceneManager.LoadScene("title");
    }

    IEnumerator LoadSelectionSceneAsync()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the Scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("selection");
        //HandScriptInstance.setSongSelectorAfterSceneChange();
        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    // Update is called once per frame
    void Update () {
        if (!isSceneLoaded)
        {
            
            isSceneLoaded = true;
            StartCoroutine(LoadSelectionSceneAsync());
        }
        //if (Input.GetKey(KeyCode.T)) { SceneManager.LoadScene("title"); }
    }
}
