using System;
using UnityEngine;

class DirectActuationCreator : ActuationCreator
{
    public override AbstractActuation Create(AbstractActuationConfig abstractActuationConfig, FilterFactory filterFactory, Rigidbody rb, AbstractControl control)
    {
        DirectActuationConfig DirectActuationConfig = (abstractActuationConfig as DirectActuationConfig);
        if (DirectActuationConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> DirectActuationCreator.create(): Cannot convert control config of type " + abstractActuationConfig.GetType() + " to DirectActuationConfig.");
        }
        return new DirectActuation(DirectActuationConfig, filterFactory, rb, control);
    }
}

[Serializable]
public class DirectActuationConfig : AbstractActuationConfig
{
    public FilterConfig filterThrust = new FilterConfig();
    public FilterConfig filterTorque = new FilterConfig();
}

public class DirectActuation : AbstractActuation
{
    // Actuation-specific members
    private float T;
    private Vector3 Torque;

    // Filter-related members
    private float TUnfiltered;
    private Vector3 TorqueUnfiltered;
    private AbstractFilter filterT;
    private AbstractFilter filterTorque;

    // Logger members
    private int TDesiredLoggerIdx, 
        TorqueDesiredXLoggerIdx, TorqueDesiredYLoggerIdx, TorqueDesiredZLoggerIdx;

    public DirectActuation(DirectActuationConfig config, FilterFactory filterFactory, Rigidbody rb, AbstractControl control) : base(config, filterFactory, rb, control)
    {
        this.TUnfiltered = 0.0f;
        this.TorqueUnfiltered = new Vector3();
        this.filterT = filterFactory.Build(config.filterThrust, rb);
        this.filterTorque = filterFactory.Build(config.filterTorque, rb);
        this.T = 0.0f;
        this.Torque = new Vector3();
        
        this.TDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TDesired");
        this.TorqueDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredX");
        this.TorqueDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredY");
        this.TorqueDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "TorqueDesiredZ");
    }

    public override void ComputeActuation()
    {
        base.ComputeActuation();
        this.TUnfiltered = this.m_control.Order[0];
        this.TorqueUnfiltered = new Vector3(this.m_control.Order[1], this.m_control.Order[2], this.m_control.Order[3]);
    }

    public override void FilterActuation()
    {
        this.T = this.filterT.Filter(this.TUnfiltered);
        this.Torque = new Vector3(this.filterTorque.Filter(this.TorqueUnfiltered[0]), this.filterTorque.Filter(this.TorqueUnfiltered[1]), this.filterTorque.Filter(this.TorqueUnfiltered[2]));
        Logger.Instance.AddEntry(this.TDesiredLoggerIdx, this.T);
        Logger.Instance.AddEntry(this.TorqueDesiredXLoggerIdx, this.Torque.x);
        Logger.Instance.AddEntry(this.TorqueDesiredYLoggerIdx, this.Torque.y);
        Logger.Instance.AddEntry(this.TorqueDesiredZLoggerIdx, this.Torque.z);
        // TODO: log unfiltered values
    }

    public override void HandlePropellers()
    {
        // Directly apply thrust and torque
        AeroFrame.ApplyRelativeForceAndTorque(this.m_rb, new Vector3(0.0f, 0.0f, this.T), this.Torque);
    }
}