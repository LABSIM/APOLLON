using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class PositionYawControlCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        PositionYawControlConfig PositionYawControlConfig = (abstractControlConfig as PositionYawControlConfig);
        if (PositionYawControlConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> PositionYawControlCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to PositionYawControlConfig.");
        }
        return new PositionYawControl(PositionYawControlConfig, rb, mapping);
    }
}

[Serializable]
public class PositionYawControlConfig : AbstractControlConfig
{
    public float k_xi;
    public float k_v;
    public float k_eta;
    public float k_mu;
    public float k_Omega;
}

public class PositionYawControl : AbstractControl
{
    // PositionYawControl-specific members
    // TODO: comment mettre une valeur par défaut ?
    private Vector3 PositionDesired { get; set; }
    private float YawDesired { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, 
        YawDesiredLoggerIdx;

    public PositionYawControl(PositionYawControlConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry("PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry("PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry("PositionDesiredZ");
        this.YawDesiredLoggerIdx = Logger.Instance.GetEntry("YawDesired");
    }
    public PositionYawControlConfig PositionYawControlConfig => this.AbstractControlConfig as PositionYawControlConfig;

    // Fetch desired altitude and velocity from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.PositionDesired = ((PositionMapping)this.m_mapping).PositionDesired;
                this.YawDesired = ((PositionMapping)this.m_mapping).OtherAxisDesired;
            }
        }
        else
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.PositionDesired = ((PositionMapping)this.m_mapping).DefaultPositionDesired;
                this.YawDesired = ((PositionMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
        }
        Logger.Instance.AddEntry(this.PositionDesiredXLoggerIdx, this.PositionDesired.x);
        Logger.Instance.AddEntry(this.PositionDesiredYLoggerIdx, this.PositionDesired.y);
        Logger.Instance.AddEntry(this.PositionDesiredZLoggerIdx, this.PositionDesired.z);
        Logger.Instance.AddEntry(this.YawDesiredLoggerIdx, this.YawDesired);
    }

    // Compute control based on fetched mapping values
    public override void Compute()
    {
        // v^d = \dot{\xi}^d - k_\xi\xi
        // V^d = -k_\xi\xi
        Vector3 velocityDesired = -this.PositionYawControlConfig.k_xi * PositionDesired;
        Vector3 vTilde = AeroFrame.GetAbsoluteVelocity(this.m_rb) - velocityDesired;
        float T = AeroFrame.GetMass(this.m_rb) * Vector3.Magnitude(AeroFrame.GetGravity(this.m_rb) + this.PositionYawControlConfig.k_v * vTilde);

        Matrix4x4 R = AeroFrame.GetRotationMatrix(this.m_rb);
        Vector3 y1 = R * Constants.e1;
        Vector3 y3 = R * Constants.e3;
        Vector3 yDesired1 = Utilities.ZYXToMatrix(new Vector3(0.0f, 0.0f, this.YawDesired)) * Constants.e1;
        Vector3 yDesired3 = new Vector3();
        if (T != 0)
        {
            yDesired3 = AeroFrame.GetMass(this.m_rb) / T * (AeroFrame.GetGravity(this.m_rb) + this.PositionYawControlConfig.k_v * vTilde);
        }
        Vector3 omegaBody = AeroFrame.GetAngularVelocity(this.m_rb);
        Vector3 omegaBodyDesired = R.transpose * (this.PositionYawControlConfig.k_eta * Vector3.Cross(y1, yDesired1) + this.PositionYawControlConfig.k_mu * Vector3.Cross(y3, yDesired3));
        Vector3 tau = -this.PositionYawControlConfig.k_Omega * (omegaBody - omegaBodyDesired) + Vector3.Cross(omegaBody, AeroFrame.GetInertiaTensor(this.m_rb) * omegaBodyDesired);

        this.Order = new Vector4(-T, tau[0], tau[1], tau[2]);
    }
}