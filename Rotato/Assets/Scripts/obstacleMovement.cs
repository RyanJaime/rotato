using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleMovement : MonoBehaviour {

	public float speed = 4.0f;
	public enum State
	{
		XMOVE,
		NEGXMOVE,
		NEGYMOVE,
		ZMOVE,
		NEGZMOVE
	};

	public State moveState = State.XMOVE;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (moveState) {
		case State.XMOVE:
			transform.position = new Vector3 (transform.position.x + speed, transform.position.y, transform.position.z);
			break;

		case State.NEGXMOVE:
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
			break;

		case State.NEGYMOVE:
			transform.position = new Vector3 (transform.position.x, transform.position.y - speed, transform.position.z);
			break;

		case State.ZMOVE:
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + speed);
			break;

		case State.NEGZMOVE:
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - speed);
			break;
		}
	}
}
