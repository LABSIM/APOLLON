using System.Numerics;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    private Rigidbody m_rb 
        = new();
    public Rigidbody Rb 
        => this.m_rb;

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initPos 
        = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitPos
        => this.m_initPos;

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initRot 
        = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitRot 
        => this.m_initRot;

    private void Awake()
    {
        this.m_rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        // Save rotor initial position and rotation at start
        this.m_initPos = AeroFrame.GetPosition(this.Rb);
        this.m_initRot = AeroFrame.GetAngles(this.Rb);
    }
    public void ResetRigidBody()
    {
        UnityEngine.Debug.Log("Rotor.ResetRigidBody " + this.InitPos);
        // Reset rotor position and rotation to values saved at start
        AeroFrame.SetPosition(this.Rb, this.InitPos);
        AeroFrame.SetAngles(this.Rb, this.InitRot);
        AeroFrame.SetAbsoluteVelocity(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.SetAngularVelocity(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.ApplyRelativeForce(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.ApplyRelativeTorque(this.Rb, UnityEngine.Vector3.zero);

    }

}
