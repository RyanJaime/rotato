using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    public GameObject obstacle;
    public Vector3 spawnValues;

    public float spawnPosX;
    public float spawnPosZ;
    public Vector3 rotationVector = new Vector3(0, 0, 0);
    private Quaternion obstacleRotation;


    List<GameObject> obstacleArr = new List<GameObject>();
    GameObject clone;

    // Use this for initialization
    void Start()
    {
        obstacleRotation = Quaternion.Euler(rotationVector);
        //StartCoroutine (waitSpawner ());
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (obstacleArr.Count > 0)
        {
            if (obstacleArr[0].transform.position.x < -60 || obstacleArr[0].transform.position.y < -10 || obstacleArr[0].transform.position.z < -60)
            {
                GameObject toBeDestroyed = obstacleArr[0];
                obstacleArr.Remove(toBeDestroyed);
                Destroy(toBeDestroyed);
            }
        }
    }
    */

    /*IEnumerator waitSpawner(){

		//spawnWait = Random.Range (spawnLeastWait, spawnMostWait);
		while(!stop){
			Vector3 spawnPosition = new Vector3 (0,0,0);
			clone = Instantiate (obstacle, spawnPosition = transform.TransformPoint (spawnPos, 0, 0), gameObject.transform.rotation);
			obstacleArr.Add (clone);
			yield return new WaitForSeconds (spawnWait);
		}
	}*/

    //execute params means that whatever the masterFreqGen obj sends to a particular spawner, execute those parameters

    //ticks / 10 = YieldInstruction length of block



    public void createObstacle()//int ticks)
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        clone = Instantiate(obstacle, spawnPosition = transform.TransformPoint(spawnPosX, 0, spawnPosZ), obstacleRotation);
        //clone.transform.localScale = new Vector3(1f, ticks / 10, 1f); // fiddle with ticks/10, the Yscale of each obstacle
        obstacleArr.Add(clone);
    }
}
