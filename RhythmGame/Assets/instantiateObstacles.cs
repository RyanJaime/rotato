using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateObstacles : MonoBehaviour
{
    // alternative to MIDIparser, for earlier playtesting
    // guarentees equal spacing between drum beats of Square hammer song

    public GameObject obstacle;
    GameObject clone;
    public float distanceBetween1 = 3.87f;
    public float distanceBetween2 = 5.84f;
    private float startingXpos = 162.09f;
    private int maxObstacles = 62;

    // Use this for initialization
    void Start()
    {
        spawnToSnareBeat();
    }

    private void spawnToSnareBeat()
    {
        for (int i = 0; i < maxObstacles; i++)
        {
            //print("distanceBetween: " + distanceBetween + " i: " + i);
            //Vector3 spawnPosition = new Vector3(0, (startingYPosition - distanceBetween * i), -1);
            //clone = Instantiate(obstacle, spawnPosition, Quaternion.identity);
            Vector3 spawnPos1 = new Vector3((startingXpos + (distanceBetween2+ distanceBetween1) * i), 0.14f, 0);
            Vector3 spawnPos2 = spawnPos1 + new Vector3((distanceBetween1 * i+1), 0, 0); //spawnPos1 + new Vector3((distanceBetween2* i), 0, 0);
            clone = Instantiate(obstacle, spawnPos1, Quaternion.identity);
            clone = Instantiate(obstacle, spawnPos2, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}