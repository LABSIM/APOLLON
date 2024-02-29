using System;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

class DirectAltitudeSingleIntegratorVelocityDirectYawCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        DirectAltitudeSingleIntegratorVelocityDirectYawConfig DirectAltitudeSingleIntegratorVelocityDirectYawConfig = (abstractControlConfig as DirectAltitudeSingleIntegratorVelocityDirectYawConfig);
        if (DirectAltitudeSingleIntegratorVelocityDirectYawConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> DirectAltitudeSingleIntegratorVelocityDirectYawCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to DirectAltitudeSingleIntegratorVelocityDirectYawConfig.");
        }
        return new DirectAltitudeSingleIntegratorVelocityDirectYaw(DirectAltitudeSingleIntegratorVelocityDirectYawConfig, rb, mapping);
    }
}

[Serializable]
public class DirectAltitudeSingleIntegratorVelocityDirectYawConfig : AbstractControlConfig { }

public class DirectAltitudeSingleIntegratorVelocityDirectYaw : AbstractControl
{
    // DirectAltitudeSingleIntegratorVelocityDirectYaw-specific members
    private Vector3 PositionDesired { get; set; }
    private float OtherAxisDesired { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, OtherAxisDesiredLoggerIdx;

    public DirectAltitudeSingleIntegratorVelocityDirectYaw(DirectAltitudeSingleIntegratorVelocityDirectYawConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig) 
    { 
        rb.useGravity = false;
        rb.isKinematic = false;
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry("PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry("PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry("PositionDesiredZ");
        this.OtherAxisDesiredLoggerIdx = Logger.Instance.GetEntry("OtherAxisDesired");
    }
    public DirectAltitudeSingleIntegratorVelocityDirectYawConfig DirectAltitudeSingleIntegratorVelocityDirectYawConfig => this.AbstractControlConfig as DirectAltitudeSingleIntegratorVelocityDirectYawConfig;

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
            else if (this.m_mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
            {
                this.PositionDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).VelocityDesired;
                this.OtherAxisDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).YawDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired;
                this.OtherAxisDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).OtherAxisDesired;
            }
        }
        else
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.PositionDesired = ((PositionMapping)this.m_mapping).DefaultPositionDesired;
                this.OtherAxisDesired = ((PositionMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
            {
                this.PositionDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).DefaultVelocityDesired;
                this.OtherAxisDesired = ((AltitudeTranslationalVelocityMapping)this.m_mapping).DefaultYawDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired;
                this.OtherAxisDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultOtherAxisDesired;
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
        AeroFrame.SetAngles(this.m_rb, new Vector3(.0f, .0f, -this.OtherAxisDesired));
        AeroFrame.IncrementPosition(this.m_rb, new Vector3(.0f, .0f, this.PositionDesired.z));
        AeroFrame.SetRelativeVelocity(this.m_rb, new Vector3(this.PositionDesired.x, this.PositionDesired.y, .0f));
        this.Order = new Vector4();
    }
}