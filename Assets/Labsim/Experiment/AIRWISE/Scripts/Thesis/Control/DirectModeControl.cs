using System;
using UnityEngine;


class DirectModeControlCreator : ControlCreator
{
    public override AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping)
    {
        DirectModeControlConfig DirectModeControlConfig = (abstractControlConfig as DirectModeControlConfig);
        if (DirectModeControlConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> DirectModeControlCreator.create(): Cannot convert control config of type " + abstractControlConfig.GetType() + " to DirectModeControlConfig.");
        }
        return new DirectModeControl(DirectModeControlConfig, rb, mapping);
    }
}


[Serializable]
public class DirectModeControlConfig : AbstractControlConfig { }

public class DirectModeControl : AbstractControl
{
    // DirectModeControl-specific members
    private float ForceDesired { get; set; }
    private Vector3 TorqueDesired { get; set; }

    // Logger members
    private int ForceDesiredLoggerIdx, 
        TorqueDesiredXLoggerIdx, TorqueDesiredYLoggerIdx, TorqueDesiredZLoggerIdx;

    public DirectModeControl(DirectModeControlConfig config, Rigidbody rb, AbstractMapping mapping) : base(rb, mapping, config) { 
        this.ForceDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceDesired");
        this.TorqueDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredX");
        this.TorqueDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredY");
        this.TorqueDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredZ");
    }
    public DirectModeControlConfig DirectModeControlConfig => this.AbstractControlConfig as DirectModeControlConfig;

    // Fetch desired force and torque from mapping
    public override void FetchMapping()
    {
        if (this.m_mapping.ConnectedToBrunner)
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.ForceDesired = ((PositionMapping)this.m_mapping).PositionDesired.z;
                this.TorqueDesired = new Vector3(
                    ((PositionMapping)this.m_mapping).PositionDesired.x,
                    ((PositionMapping)this.m_mapping).PositionDesired.y,
                    ((PositionMapping)this.m_mapping).OtherAxisDesired
                );
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.ForceDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired.z;
                this.TorqueDesired = new Vector3(
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired.x,
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).PositionDesired.y,
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).OtherAxisDesired
                );
            }
        }
        else
        {
            if (this.m_mapping.GetType() == typeof(PositionMapping))
            {
                this.ForceDesired = ((PositionMapping)this.m_mapping).DefaultPositionDesired.z;
                this.TorqueDesired = new Vector3(
                    ((PositionMapping)this.m_mapping).DefaultPositionDesired.x,
                    ((PositionMapping)this.m_mapping).DefaultPositionDesired.y,
                    ((PositionMapping)this.m_mapping).DefaultOtherAxisDesired
                );
            }
            else if (this.m_mapping.GetType() == typeof(XYIncrementZOtherAxisMapping))
            {
                this.ForceDesired = ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired.z;
                this.TorqueDesired = new Vector3(
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired.x,
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultPositionDesired.y,
                    ((XYIncrementZOtherAxisMapping)this.m_mapping).DefaultOtherAxisDesired
                );
            }
        }
        Logger.Instance.AddEntry(this.ForceDesiredLoggerIdx, this.ForceDesired);
        Logger.Instance.AddEntry(this.TorqueDesiredXLoggerIdx, this.TorqueDesired.x);
        Logger.Instance.AddEntry(this.TorqueDesiredYLoggerIdx, this.TorqueDesired.y);
        Logger.Instance.AddEntry(this.TorqueDesiredZLoggerIdx, this.TorqueDesired.z);
    }

    // Compute control based on fetched mapping values
    public override void Compute()
    {
        this.Order = new Vector4(this.ForceDesired, TorqueDesired[0], TorqueDesired[1], TorqueDesired[2]);
    }
}