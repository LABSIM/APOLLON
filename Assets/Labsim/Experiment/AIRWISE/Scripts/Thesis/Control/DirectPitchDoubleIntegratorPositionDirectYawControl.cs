using System;
using UnityEngine;

class DirectPitchDoubleIntegratorPositionDirectYawCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        DirectPitchDoubleIntegratorPositionDirectYawConfig DirectPitchDoubleIntegratorPositionDirectYawConfig = (abstractControlConfig as DirectPitchDoubleIntegratorPositionDirectYawConfig);
        if (DirectPitchDoubleIntegratorPositionDirectYawConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> DirectPitchDoubleIntegratorPositionDirectYawCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to SingleIntegratorControlConfig.");
        }
        return new DirectPitchDoubleIntegratorPositionDirectYaw(DirectPitchDoubleIntegratorPositionDirectYawConfig, rb, mapping);
    }
}

[Serializable]
public class DirectPitchDoubleIntegratorPositionDirectYawConfig : AbstractControlConfig { }

public class DirectPitchDoubleIntegratorPositionDirectYaw : AbstractControl
{
    // DirectPitchDoubleIntegratorPositionDirectYaw-specific members
    private Vector3 PositionDesired { get; set; }
    private float OtherAxisDesired { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, OtherAxisDesiredLoggerIdx;

    public DirectPitchDoubleIntegratorPositionDirectYaw(DirectPitchDoubleIntegratorPositionDirectYawConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig)
    {
        rb.useGravity = false;
        rb.isKinematic = false;
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry("PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry("PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry("PositionDesiredZ");
        this.OtherAxisDesiredLoggerIdx = Logger.Instance.GetEntry("OtherAxisDesired");
    }
    public DirectPitchDoubleIntegratorPositionDirectYawConfig DirectPitchDoubleIntegratorPositionDirectYawConfig => this.AbstractControlConfig as DirectPitchDoubleIntegratorPositionDirectYawConfig;

    // Fetch desired position and attitude from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.PositionDesired = ((PositionMapping)this.m_mapping).PositionDesired;
                this.OtherAxisDesired = ((PositionMapping)this.m_mapping).OtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired;
                this.OtherAxisDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).OtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(IncrementXYZOtherAxisMapping))
            {
                this.PositionDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).PositionDesired;
                this.OtherAxisDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).OtherAxisDesired;
            }
        }
        else
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.PositionDesired = ((PositionMapping)this.m_mapping).DefaultPositionDesired;
                this.OtherAxisDesired = ((PositionMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired;
                this.OtherAxisDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(IncrementXYZOtherAxisMapping))
            {
                this.PositionDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).PositionDesired;
                this.OtherAxisDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).OtherAxisDesired;
            }
        }
        Logger.Instance.AddEntry(this.PositionDesiredXLoggerIdx, this.PositionDesired.x);
        Logger.Instance.AddEntry(this.PositionDesiredYLoggerIdx, this.PositionDesired.y);
        Logger.Instance.AddEntry(this.PositionDesiredZLoggerIdx, this.PositionDesired.z);
        Logger.Instance.AddEntry(this.OtherAxisDesiredLoggerIdx, this.OtherAxisDesired);
    }

    // Compute control based on fetched mapping values
    public override void Compute()
    {
        AeroFrame.SetAngles(this.m_rb, new Vector3(.0f, 0.0f, -this.OtherAxisDesired));
        AeroFrame.SetPosition(this.m_rb, this.PositionDesired);
        this.Order = new Vector4();
    }
}