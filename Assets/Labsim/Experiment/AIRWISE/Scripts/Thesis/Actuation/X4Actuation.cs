using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class X4ActuationCreator : ActuationCreator
{
    public override AbstractActuation Create(AbstractActuationConfig abstractActuationConfig, FilterFactory filterFactory, Rigidbody rb, AbstractControl control)
    {
        X4ActuationConfig X4ActuationConfig = (abstractActuationConfig as X4ActuationConfig);
        if (X4ActuationConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> X4ActuationCreator.create(): Cannot convert control config of type " + abstractActuationConfig.GetType() + " to X4ActuationConfig.");
        }
        return new X4Actuation(X4ActuationConfig, filterFactory, rb, control);
    }
}

[Serializable]
public class X4ActuationConfig : AbstractActuationConfig
{
    public FilterConfig filterThrust = new FilterConfig();
}

public class X4Actuation : AbstractActuation
{
    // X4Actuation-specific members
    private static readonly float PropLever = Mathf.Sqrt(0.5f * 0.5f + 0.5f * 0.5f);
    public float toto => X4Actuation.PropLever;
    private float RotationToForce { get; set; } = 1.0f;
    private Matrix4x4 A_inv { get; set; } = new Matrix4x4();
    private Rotor[] m_rotors;

    // Filter-related members
    private AbstractFilter filter0, filter1, filter2, filter3;
    private Vector4 TUnfiltered;
    private Vector4 T;

    // Logger members
    private int T0LoggerIdx, T1LoggerIdx, T2LoggerIdx, T3LoggerIdx;

    public X4Actuation(X4ActuationConfig config, FilterFactory filterFactory, Rigidbody rb, AbstractControl control) : base(config, filterFactory, rb, control)
    {
        this.RotationToForce = 1.0f;
        this.A_inv.SetColumn(0, new Vector4(
            (float)1 / 4,
            (float)1 / 4,
            (float)1 / 4,
            (float)1 / 4));
        this.A_inv.SetColumn(1, new Vector4(
            (float)Math.Sqrt(2) / (4 * PropLever),
            (float)Math.Sqrt(2) / (4 * PropLever),
            (float)-Math.Sqrt(2) / (4 * PropLever),
            (float)-Math.Sqrt(2) / (4 * PropLever)));
        this.A_inv.SetColumn(2, new Vector4(
            (float)Math.Sqrt(2) / (4 * PropLever),
            (float)-Math.Sqrt(2) / (4 * PropLever),
            (float)-Math.Sqrt(2) / (4 * PropLever),
            (float)Math.Sqrt(2) / (4 * PropLever)));
        this.A_inv.SetColumn(3, new Vector4(
            (float)Math.Sqrt(2) / (4 * this.RotationToForce),
            (float)-Math.Sqrt(2) / (4 * this.RotationToForce),
            (float)Math.Sqrt(2) / (4 * this.RotationToForce),
            (float)-Math.Sqrt(2) / (4 * this.RotationToForce)));
        this.m_rotors = this.m_rb.transform.parent.GetComponentsInChildren<Rotor>();
        this.filter0 = filterFactory.Build(config.filterThrust, rb);
        this.filter1 = filterFactory.Build(config.filterThrust, rb);
        this.filter2 = filterFactory.Build(config.filterThrust, rb);
        this.filter3 = filterFactory.Build(config.filterThrust, rb);

        this.T0LoggerIdx = Logger.Instance.GetEntry("T0");
        this.T1LoggerIdx = Logger.Instance.GetEntry("T1");
        this.T2LoggerIdx = Logger.Instance.GetEntry("T2");
        this.T3LoggerIdx = Logger.Instance.GetEntry("T3");
    }

    public override void ComputeActuation()
    {
        base.ComputeActuation();
        float T = this.m_control.Order[0];
        Vector3 tau = new Vector3(this.m_control.Order[1], this.m_control.Order[2], this.m_control.Order[3]);
        this.TUnfiltered = this.A_inv * new Vector4(T, tau[0], tau[1], tau[2]);
    }

    public override void FilterActuation()
    {
        this.T = new Vector4(
            this.filter0.Filter(this.TUnfiltered[0]),
            this.filter1.Filter(this.TUnfiltered[1]),
            this.filter2.Filter(this.TUnfiltered[2]),
            this.filter3.Filter(this.TUnfiltered[3]));
        Logger.Instance.AddEntry(this.T0LoggerIdx, this.T[0]);
        Logger.Instance.AddEntry(this.T1LoggerIdx, this.T[1]);
        Logger.Instance.AddEntry(this.T2LoggerIdx, this.T[2]);
        Logger.Instance.AddEntry(this.T3LoggerIdx, this.T[3]);
    }

    public override void HandlePropellers()
    {
        for (int i = 0; i < 4; i++)
        {
            AeroFrame.ApplyRelativeForce(this.m_rotors[i].m_rb, new Vector3(0.0f, 0.0f, this.T[i]));
        }
    }
}