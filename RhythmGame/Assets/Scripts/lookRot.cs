using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookRot : MonoBehaviour
{

    public Camera cam;

    protected void Update()
    {
        Vector3 directionToCamera
          = Camera.main.transform.position - transform.position;

        Quaternion targetRotation
          = Quaternion.LookRotation(-directionToCamera, Vector3.up);
        transform.rotation = targetRotation;
    }
}
