using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class AltitudeTranslationalVelocityControlCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        AltitudeTranslationalVelocityControlConfig AltitudeTranslationalVelocityControlConfig = (abstractControlConfig as AltitudeTranslationalVelocityControlConfig);
        if (AltitudeTranslationalVelocityControlConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> AltitudeTranslationalVelocityControlCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to AltitudeTranslationalVelocityControlConfig.");
        }
        return new AltitudeTranslationalVelocityControl(AltitudeTranslationalVelocityControlConfig, rb, mapping);
    }
}

[Serializable]
public class AltitudeTranslationalVelocityControlConfig : AbstractControlConfig
{
    public float k_z;
    public float k_vz;
    public float k_eta;
    public float k_mu;
    public float k_Omega;
    public float k_v;
}

public class AltitudeTranslationalVelocityControl : AbstractControl
{
    // AltitudeTranslationalVelocityControl-specific members
    // TODO: comment mettre une valeur par défaut ?
    private float AltitudeDesired { get; set; }
    private Vector3 VelocityDesired { get; set; }
    private float YawDesired { get; set; }

    // Logger members
    private int AltitudeDesiredLoggerIdx, 
        VelocityDesiredXLoggerIdx, VelocityDesiredYLoggerIdx, VelocityDesiredZLoggerIdx, 
        YawDesiredLoggerIdx;

    public AltitudeTranslationalVelocityControl(AltitudeTranslationalVelocityControlConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig) { 
        this.AltitudeDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "AltitudeDesired");
        this.VelocityDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredX");
        this.VelocityDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredY");
        this.VelocityDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredZ");
        this.YawDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "YawDesired");
    }
    public AltitudeTranslationalVelocityControlConfig AltitudeTranslationalVelocityControlConfig => this.AbstractControlConfig as AltitudeTranslationalVelocityControlConfig;

    // Fetch desired altitude and velocity from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
            {
                this.AltitudeDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).AltitudeDesired;
                this.VelocityDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).VelocityDesired;
                this.YawDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).YawDesired;
            }
        }
        else
        {
            if (this.m_mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
            {
                this.AltitudeDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).DefaultAltitudeDesired;
                this.VelocityDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).DefaultVelocityDesired;
                this.YawDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).DefaultYawDesired;
            }
        }
        Logger.Instance.AddEntry(this.AltitudeDesiredLoggerIdx, this.AltitudeDesired);
        Logger.Instance.AddEntry(this.VelocityDesiredXLoggerIdx, this.VelocityDesired.x);
        Logger.Instance.AddEntry(this.VelocityDesiredYLoggerIdx, this.VelocityDesired.y);
        Logger.Instance.AddEntry(this.VelocityDesiredZLoggerIdx, this.VelocityDesired.z);
        Logger.Instance.AddEntry(this.YawDesiredLoggerIdx, this.YawDesired);
    }

    // Compute control based on fetched mapping values
    public override void Compute()
    {
        Vector3 res = this.VelocityDesired;
        float zTilde = AeroFrame.GetPosition(this.m_rb).z - this.AltitudeDesired;
        res[2] = -this.AltitudeTranslationalVelocityControlConfig.k_z * zTilde;
        this.VelocityDesired = res;
        Vector3 vTilde = AeroFrame.GetAbsoluteVelocity(this.m_rb) - this.VelocityDesired;
        float T = AeroFrame.GetMass(this.m_rb) * Vector3.Magnitude(AeroFrame.GetGravity(this.m_rb) + this.AltitudeTranslationalVelocityControlConfig.k_vz * vTilde);

        Matrix4x4 R = AeroFrame.GetRotationMatrix(this.m_rb);
        Vector3 eta = R * Constants.e1;
        Vector3 mu = R * Constants.e3;
        Vector3 etaDesired = Utilities.ZYXToMatrix(new Vector3(0.0f, 0.0f, this.YawDesired)) * Constants.e1;
        Vector3 muDesired = new Vector3();
        if (T != 0)
        {
            muDesired = AeroFrame.GetMass(this.m_rb) / T * (AeroFrame.GetGravity(this.m_rb) + this.AltitudeTranslationalVelocityControlConfig.k_v * vTilde);
        }
        Vector3 omegaBody = AeroFrame.GetAngularVelocity(this.m_rb);
        Vector3 omegaBodyDesired = R.transpose * (this.AltitudeTranslationalVelocityControlConfig.k_eta * Vector3.Cross(eta, etaDesired) + this.AltitudeTranslationalVelocityControlConfig.k_mu * Vector3.Cross(mu, muDesired));
        Vector3 tau = -this.AltitudeTranslationalVelocityControlConfig.k_Omega * (omegaBody - omegaBodyDesired) + Vector3.Cross(omegaBody, AeroFrame.GetInertiaTensor(this.m_rb) * omegaBodyDesired);

        this.Order = new Vector4(-T, tau[0], tau[1], tau[2]);
    }
}