using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticDroneAnimate : MonoBehaviour
{

    // the speed the blade can rotate
    public float targetSpeed = 5000.0f;

    // axis to rotate on
    private Vector3 axis = new(0.0f,0.0f,1.0f);

    private float currentSpeed;
 
    private void OnEnable()
    {
        currentSpeed = 0;
        StartCoroutine(Animate());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator Animate()
    {
        while (enabled)
        {          
            // interpolate the currentSpeed to match the targetSpeed
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime);

            // rotate the blade every frame
            transform.Rotate(axis, currentSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

}
