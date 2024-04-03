using System.Numerics;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    private Rigidbody m_rb 
        = new();
    public Rigidbody Rb 
        => this.m_rb;

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_defaultPos = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 DefaultPos { get { return this.m_defaultPos; } set { this.m_defaultPos = value; } }

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_defaultRot = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 DefaultRot { get { return this.m_defaultRot; } set { this.m_defaultRot = value; } }

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initPos = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitPos { get { return this.m_initPos; } set { this.m_initPos = value; } }

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initRot = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitRot { get { return this.m_initRot; } set { this.m_initRot = value; } }

    private void Awake()
    {
        this.m_rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        // Save rotor initial position and rotation at start
        this.DefaultPos = AeroFrame.GetPosition(this.Rb);
        this.DefaultRot = AeroFrame.GetAngles(this.Rb);
        this.InitPos = this.DefaultPos;
        this.InitRot = this.DefaultRot;
    }

    public void ComputeInitPosRot(UnityEngine.Vector3 refPos, UnityEngine.Vector3 refRot)
    {
        this.InitPos = this.DefaultPos + refPos;
        this.InitRot = this.DefaultRot + refRot;
    }

    public void ResetRigidBody()
    {
        // Reset rotor position and rotation to values saved at start
        AeroFrame.SetPosition(this.Rb, this.InitPos);
        AeroFrame.SetAngles(this.Rb, this.InitRot);
        AeroFrame.SetAbsoluteVelocity(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.SetAngularVelocity(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.ApplyRelativeForce(this.Rb, UnityEngine.Vector3.zero);
        AeroFrame.ApplyRelativeTorque(this.Rb, UnityEngine.Vector3.zero);

    }

}
