using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlatformImpedance : MonoBehaviour
{
    // Input members
    [SerializeField]
    private Rigidbody m_inputRb;

    // Input to output factor
    [SerializeField]
    [Range(0.1f,1.0f)]
    private float kImpedance = 0.5f;
    // bornes de débattement angulaire

    // Output members
    [SerializeField]
    private Rigidbody m_outputRb;

    private void FixedUpdate()
    {
        UnityFrame.SetAngles(this.m_outputRb, kImpedance * this.TrimUnder5(this.TrimOver180(UnityFrame.GetAngles(this.m_inputRb))));
    }

    private Vector3 TrimOver180(Vector3 angles)
    {
        Vector3 res = angles;
        if (res[0] >= 180.0f) { res[0] -= 360.0f; }
        if (res[1] >= 180.0f) { res[1] -= 360.0f; }
        if (res[2] >= 180.0f) { res[2] -= 360.0f; }
        return res;
    }

    private Vector3 TrimUnder5(Vector3 angles)
    {
        Vector3 res = angles;
        if (res[0] <= 5.0f) { res[0] = .0f; }
        if (res[1] <= 5.0f) { res[1] = .0f; }
        if (res[2] <= 5.0f) { res[2] = .0f; }
        return res; 
    }
}
