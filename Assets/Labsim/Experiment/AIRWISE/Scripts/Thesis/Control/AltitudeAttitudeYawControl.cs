using System;
using UnityEngine;

class AltitudeAttitudeYawControlCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        AltitudeAttitudeYawControlConfig AltitudeAttitudeYawControlConfig = (abstractControlConfig as AltitudeAttitudeYawControlConfig);
        if (AltitudeAttitudeYawControlConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> AltitudeAttitudeYawControlCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to AltitudeAttitudeYawControlConfig.");
        }
        return new AltitudeAttitudeYawControl(AltitudeAttitudeYawControlConfig, rb, mapping);
    }
}

[Serializable]
public class AltitudeAttitudeYawControlConfig : AbstractControlConfig
{
    public float k_z;
    public float k_vz;
    public float k_eta;
    public float k_mu;
    public float k_Omega;
}

public class AltitudeAttitudeYawControl : AbstractControl
{
    // AltitudeAttitudeYawControl-specific members
    private float AltitudeDesired { get; set; }
    private Vector3 AttitudeDesired { get; set; }

    // Logger members
    private int AltitudeDesiredLoggerIdx, 
        AttitudeDesiredXLoggerIdx, AttitudeDesiredYLoggerIdx, AttitudeDesiredZLoggerIdx;

    public AltitudeAttitudeYawControl(AltitudeAttitudeYawControlConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig) 
    { 
        rb.useGravity = true;
        rb.isKinematic = false;
        this.AltitudeDesiredLoggerIdx = Logger.Instance.GetEntry("AltitudeDesired");
        this.AttitudeDesiredXLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredX");
        this.AttitudeDesiredYLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredY");
        this.AttitudeDesiredZLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredZ");
    }
    public AltitudeAttitudeYawControlConfig AltitudeAttitudeYawControlConfig => this.AbstractControlConfig as AltitudeAttitudeYawControlConfig;


    // Fetch desired altitude and attitude from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(AltitudeAttitudeMapping))
            {
                this.AltitudeDesired = ((AltitudeAttitudeMapping)this.m_mapping).AltitudeDesired;
                this.AttitudeDesired = ((AltitudeAttitudeMapping)this.m_mapping).AttitudeDesired;
            }
        } else
        {
            if (this.m_mapping.GetType() == typeof(AltitudeAttitudeMapping))
            {
                this.AltitudeDesired = ((AltitudeAttitudeMapping)this.m_mapping).DefaultAltitudeDesired;
                this.AttitudeDesired = ((AltitudeAttitudeMapping)this.m_mapping).DefaultAttitudeDesired;
            }
        }
        Logger.Instance.AddEntry(this.AltitudeDesiredLoggerIdx, this.AltitudeDesired);
        Logger.Instance.AddEntry(this.AttitudeDesiredXLoggerIdx, this.AttitudeDesired.x);
        Logger.Instance.AddEntry(this.AttitudeDesiredYLoggerIdx, this.AttitudeDesired.y);
        Logger.Instance.AddEntry(this.AttitudeDesiredZLoggerIdx, this.AttitudeDesired.z);
    }

    // Compute control based on fetched mapping values
    public override void Compute()
    {
        float zTilde = AeroFrame.GetPosition(this.m_rb).z - this.AltitudeDesired;
        float vzDesired = -this.AltitudeAttitudeYawControlConfig.k_z * zTilde;
        float vzTilde = AeroFrame.GetAbsoluteVelocity(this.m_rb).z - vzDesired;
        Matrix4x4 R = AeroFrame.GetRotationMatrix(this.m_rb);
        float T = AeroFrame.GetMass(this.m_rb) / R[2, 2] * (AeroFrame.GetGravity(this.m_rb)[2] + this.AltitudeAttitudeYawControlConfig.k_vz * vzTilde);

        Matrix4x4 RDesired = Utilities.ZYXToMatrix(this.AttitudeDesired);

        Vector3 eta = R * Constants.e1;
        Vector3 mu = R * Constants.e3;
        Vector3 etaDesired = RDesired * Constants.e1;
        Vector3 muDesired = RDesired * Constants.e3;
        Vector3 omegaBody = AeroFrame.GetAngularVelocity(this.m_rb);
        Vector3 omegaBodyDesired = R.transpose * (this.AltitudeAttitudeYawControlConfig.k_eta * Vector3.Cross(eta, etaDesired) + this.AltitudeAttitudeYawControlConfig.k_mu * Vector3.Cross(mu, muDesired));
        Vector3 tau = -this.AltitudeAttitudeYawControlConfig.k_Omega * (omegaBody - omegaBodyDesired) + Vector3.Cross(omegaBody, AeroFrame.GetInertiaTensor(this.m_rb) * omegaBodyDesired);

        this.Order = new Vector4(-T, tau[0], tau[1], tau[2]);
    }
}