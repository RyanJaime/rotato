using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shapeKeyController : MonoBehaviour {
	private float mScale = 0.0f;
	private bool ifScaleUp = true;

    public void callScaleCoroutine(int shapeKey)
    {
        StartCoroutine(Scale(ifScaleUp, shapeKey));
    }

    IEnumerator Scale(bool ifScaleUp, int shapeKey) // recursively calls to scale amp up then back to normal
    {
        if (!ifScaleUp) // scale back down to normal
        {
            if (mScale <= 0.0f) // if it has scaled back down.(sometimes it shrinks past 0 to -5, -10)
            {
                mScale = 0.0f;
                GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(shapeKey, mScale); // set scale back to 0
                yield return null;
            }
            else // scale back down until scale is 0 again
            {
                GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(shapeKey, mScale -= 5);
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(Scale(ifScaleUp, shapeKey)); // Recursively call coroutine
            }
        }
        if (ifScaleUp) // first it scales up
        {
            if (mScale > 100.0f) { ifScaleUp = !ifScaleUp; } // next call it will scale back down to normal
            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(shapeKey, mScale += 5);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Scale(ifScaleUp, shapeKey)); // Recursively call coroutine
        }
    }
}
