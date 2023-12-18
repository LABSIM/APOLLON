using System;
using UnityEngine;


class OpenLoopControlCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        OpenLoopControlConfig OpenLoopControlConfig = (abstractControlConfig as OpenLoopControlConfig);
        if (OpenLoopControlConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> OpenLoopControlCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to OpenLoopControlConfig.");
        }
        return new OpenLoopControl(OpenLoopControlConfig, rb, mapping);
    }
}

[Serializable]
public class OpenLoopControlConfig : AbstractControlConfig { }

public class OpenLoopControl : AbstractControl
{

    // OpenLoopControl-specific members
    private Vector3 PositionDesired { get; set;}
    private float YawDesired { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, YawDesiredLoggerIdx;

    public OpenLoopControl(OpenLoopControlConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config as AbstractControlConfig) { 
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredZ");
        this.YawDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "YawDesired");
    }
    public OpenLoopControlConfig OpenLoopControlConfig => this.AbstractControlConfig as OpenLoopControlConfig;

    // Fetch desired position and attitude from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping)){
                this.PositionDesired = ((PositionMapping)this.m_mapping).PositionDesired;
                this.YawDesired = ((PositionMapping)this.m_mapping).OtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping)){
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired;
                this.YawDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).OtherAxisDesired;
            }
        } else
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping)){
                this.PositionDesired = ((PositionMapping)this.m_mapping).DefaultPositionDesired;
                this.YawDesired = ((PositionMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping)){
                this.PositionDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired;
                this.YawDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultOtherAxisDesired;
            }
        }
        Logger.Instance.AddEntry(this.PositionDesiredXLoggerIdx, this.PositionDesired.x);
        Logger.Instance.AddEntry(this.PositionDesiredYLoggerIdx, this.PositionDesired.y);
        Logger.Instance.AddEntry(this.PositionDesiredZLoggerIdx, this.PositionDesired.z);
        Logger.Instance.AddEntry(this.YawDesiredLoggerIdx, this.YawDesired);
    }

    // Compute control based on fetched mapping values
    // Set current RigidBody in provided position and attitude expressed in aero frame using Euler-Cardan ZYX angles
    public override void Compute()
    {
        AeroFrame.SetPosition(this.m_rb, this.PositionDesired);
        AeroFrame.SetAngles(this.m_rb, new Vector3(0.0f, 0.0f, this.YawDesired));
        this.Order = new Vector4();
    }
}