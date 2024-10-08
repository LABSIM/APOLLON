﻿using System;
using UnityEngine;

class DirectAltitudeDoubleIntegratorPositionDirectYawCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        DirectAltitudeDoubleIntegratorPositionDirectYawConfig DirectAltitudeDoubleIntegratorPositionDirectYawConfig = (abstractControlConfig as DirectAltitudeDoubleIntegratorPositionDirectYawConfig);
        if (DirectAltitudeDoubleIntegratorPositionDirectYawConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> DirectAltitudeDoubleIntegratorPositionDirectYawCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to DirectAltitudeDoubleIntegratorPositionDirectYawConfig.");
        }
        return new DirectAltitudeDoubleIntegratorPositionDirectYaw(DirectAltitudeDoubleIntegratorPositionDirectYawConfig, rb, mapping);
    }
}

[Serializable]
public class DirectAltitudeDoubleIntegratorPositionDirectYawConfig : AbstractControlConfig { }

public class DirectAltitudeDoubleIntegratorPositionDirectYaw : AbstractControl
{
    // DirectAltitudeDoubleIntegratorPositionDirectYaw-specific members
    private Vector3 PositionDesired { get; set; }
    private float OtherAxisDesired { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, OtherAxisDesiredLoggerIdx;

    public DirectAltitudeDoubleIntegratorPositionDirectYaw(DirectAltitudeDoubleIntegratorPositionDirectYawConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig)
    {
        rb.useGravity = false;
        rb.isKinematic = false;
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry("PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry("PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry("PositionDesiredZ");
        this.OtherAxisDesiredLoggerIdx = Logger.Instance.GetEntry("OtherAxisDesired");
    }
    public DirectAltitudeDoubleIntegratorPositionDirectYawConfig DirectAltitudeDoubleIntegratorPositionDirectYawConfig => this.AbstractControlConfig as DirectAltitudeDoubleIntegratorPositionDirectYawConfig;

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
                this.PositionDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).DefaultPositionDesired;
                this.OtherAxisDesired = ((IncrementXYZOtherAxisMapping)this.m_mapping).DefaultOtherAxisDesired;
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
        // Vector3 positionRelative = new Vector3(this.PositionDesired.x, this.PositionDesired.y, 0.0f);
        // AeroFrame.SetPosition(this.m_rb, AeroFrame.GetRotationMatrix(this.m_rb) * positionRelative);
        // Vector3 altitudeRelative = new Vector3(0.0f, 0.0f, this.PositionDesired.z);
        // AeroFrame.IncrementPosition(this.m_rb, AeroFrame.GetRotationMatrix(this.m_rb) * altitudeRelative);
        AeroFrame.SetPosition(this.m_rb,this.PositionDesired);
        AeroFrame.SetAngles(this.m_rb, new Vector3(.0f, .0f, -this.OtherAxisDesired));
        this.Order = new Vector4();
    }
}