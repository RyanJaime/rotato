using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSpawners : MonoBehaviour
{

    public GameObject spawner0;
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;
    //public GameObject spawner5;
    /*
    public GameObject spawner6;
    public GameObject spawner7;
    public GameObject spawner8;

    public GameObject spawnerRight00;
    public GameObject spawnerRight01;
    public GameObject spawnerRight02;

    public GameObject spawnerLeft00;
    public GameObject spawnerLeft01;
    public GameObject spawnerLeft02; */

    public static List<GameObject> spawnerList;
    //public static createSpawners current;

    public static bool ready = false;
    // Use this for initialization
    void Start()
    {
        spawnerList = new List<GameObject>();
        GameObject obj00 = Instantiate(spawner0);
        
        GameObject obj01 = Instantiate(spawner1);
        GameObject obj02 = Instantiate(spawner2);
        GameObject obj10 = Instantiate(spawner3);
        GameObject obj11 = Instantiate(spawner4);
        //GameObject obj12 = Instantiate(spawner5);
        /*
        GameObject obj20 = Instantiate(spawner20);
        GameObject obj21 = Instantiate(spawner21);
        GameObject obj22 = Instantiate(spawner22);

        GameObject objRight00 = Instantiate(spawnerRight00);
        GameObject objRight01 = Instantiate(spawnerRight01);
        GameObject objRight02 = Instantiate(spawnerRight02);

        GameObject objLeft00 = Instantiate(spawnerLeft00);
        GameObject objLeft01 = Instantiate(spawnerLeft01);
        GameObject objLeft02 = Instantiate(spawnerLeft02);*/

        spawnerList.Add(obj00);
        
        spawnerList.Add(obj01);
        spawnerList.Add(obj02);
        spawnerList.Add(obj10);
        spawnerList.Add(obj11);
        //spawnerList.Add(obj12);
        /*
        spawnerList.Add(obj20);
        spawnerList.Add(obj21);
        spawnerList.Add(obj22);

        spawnerList.Add(objRight00);
        spawnerList.Add(objRight01);
        spawnerList.Add(objRight02);

        spawnerList.Add(objLeft00);
        spawnerList.Add(objLeft01);
        spawnerList.Add(objLeft02);*/

        if (spawnerList.Count > 0)
        {
            ready = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
