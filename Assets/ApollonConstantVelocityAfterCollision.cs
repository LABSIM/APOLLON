using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonConstantVelocityAfterCollision : MonoBehaviour
{

    private UnityEngine.Vector3 m_constantVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(UnityEngine.Collision other) 
    {

        var forward_velocity = gameObject.GetComponent<UnityEngine.Rigidbody>().velocity.z;

        // stop
        gameObject.GetComponent<UnityEngine.Rigidbody>().velocity = UnityEngine.Vector3.zero;
        
        // bump up about 2m
        gameObject.transform.Translate(UnityEngine.Vector3.up * 2.0f, UnityEngine.Space.World);

        // keep moving forward
        gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(
            UnityEngine.Vector3.forward * forward_velocity,
            UnityEngine.ForceMode.VelocityChange
        );

        // // log
        // UnityEngine.Debug.Log(
        //     "<color=Blue>Info: </color> ApollonConstantVelocityAfterCollision.OnCollisionEnter() : found " + this.m_constantVelocity
        // );

    } /* OnCollisionEnter() */

    // private void OnCollisionStay(UnityEngine.Collision other) 
    // {

    //     // apply new velocity from previous magnitude
    //     if(this.m_constantVelocity != UnityEngine.Vector3.zero) {

    //         var normalized_normal = other.GetContact(0).normal.Normalize();
    //         gameObject.GetComponent<UnityEngine.Rigidbody>().velocity =  * this.m_constantVelocity.magnitude;
    //         this.m_constantVelocity = UnityEngine.Vector3.zero;
        
    //         // log
    //         UnityEngine.Debug.Log(
    //             "<color=Blue>Info: </color> ApollonConstantVelocityAfterCollision.OnCollisionStay() : found " 
    //             + gameObject.GetComponent<UnityEngine.Rigidbody>().velocity
    //         );

    //     }

    // } /* OnCollisionStay() */

    // private void OnCollisionExit(UnityEngine.Collision other) 
    // {

    //     // apply new velocity from previous magnitude
    //     if(this.m_constantVelocity != UnityEngine.Vector3.zero) {


    //         this.m_constantVelocity = UnityEngine.Vector3.zero;
        
    //         // log
    //         UnityEngine.Debug.Log(
    //             "<color=Blue>Info: </color> ApollonConstantVelocityAfterCollision.OnCollisionExit() : found " 
    //             + gameObject.GetComponent<UnityEngine.Rigidbody>().velocity
    //         );

    //     }

    // } /* OnCollisionExit() */

}
