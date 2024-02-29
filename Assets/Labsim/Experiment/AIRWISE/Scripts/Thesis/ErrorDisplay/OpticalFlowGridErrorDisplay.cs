using System;
using UnityEngine;

class OpticalFlowGridErrorDisplayCreator : ErrorDisplayCreator
{
    public override AbstractErrorDisplay Create(AbstractErrorDisplayConfig abstractErrorDisplayConfig, Rigidbody rb, AbstractForcingFunction forcingFunction)
    {
        OpticalFlowGridErrorDisplayConfig OpticalFlowGridErrorDisplayConfig = (abstractErrorDisplayConfig as OpticalFlowGridErrorDisplayConfig);
        if (OpticalFlowGridErrorDisplayConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> OpticalFlowGridErrorCreator.create(): Cannot convert control config of type " + abstractErrorDisplayConfig.GetType() + " to OpticalFlowGridErrorDisplayConfig.");
        }
        return new OpticalFlowGridErrorDisplay(OpticalFlowGridErrorDisplayConfig, rb, forcingFunction);
    }
}

[Serializable]
public class OpticalFlowGridErrorDisplayConfig : AbstractErrorDisplayConfig { }

public class OpticalFlowGridErrorDisplay : AbstractErrorDisplay
{
    // Rigidbody-related members
    private Rigidbody m_left;
    private Rigidbody m_right;
    private Rigidbody m_leftError;
    private Rigidbody m_rightError;

    // ErrorDisplay-related member
    private float m_currVal;
    private float m_refVal;
    private float m_error;

    // Logger members
    public int CurrValLoggerIdx, RefValLoggerIdx, ErrorLoggerIdx;

    private Material m_materialNegativeDefault, m_materialNegativeError, m_materialPositiveDefault, m_materialPositiveError;

    public OpticalFlowGridErrorDisplay(OpticalFlowGridErrorDisplayConfig config, Rigidbody rb, AbstractForcingFunction forcingFunction) : base(config as AbstractErrorDisplayConfig, rb, forcingFunction)
    {
        this.m_left = GameObject.Find("Left").GetComponent<Rigidbody>();
        this.m_right = GameObject.Find("Right").GetComponent<Rigidbody>();
        this.m_leftError = GameObject.Find("LeftError").GetComponent<Rigidbody>();
        this.m_rightError = GameObject.Find("RightError").GetComponent<Rigidbody>();
        m_materialNegativeDefault = GameObject.Find("Negative").GetComponent<Renderer>().materials[0];
        m_materialNegativeError = GameObject.Find("Negative").GetComponent<Renderer>().materials[1];
        m_materialPositiveDefault = GameObject.Find("Positive").GetComponent<Renderer>().materials[0];
        m_materialPositiveError = GameObject.Find("Positive").GetComponent<Renderer>().materials[1];

        this.CurrValLoggerIdx = Logger.Instance.GetEntry("CurrVal");
        this.RefValLoggerIdx = Logger.Instance.GetEntry("RefVal");
        this.ErrorLoggerIdx = Logger.Instance.GetEntry("Error");
    }

    public override void ComputeError()
    {
        // Fetch aircraft velocity
        this.m_currVal = AeroFrame.GetAbsoluteVelocity(this.m_rb).x;
        this.m_refVal = this.m_forcingFunction.val;
        this.m_error = (this.m_currVal - this.m_refVal) / (this.m_forcingFunction.dA);

        Logger.Instance.AddEntry(this.CurrValLoggerIdx, this.m_currVal);
        Logger.Instance.AddEntry(this.RefValLoggerIdx, this.m_refVal);
        Logger.Instance.AddEntry(this.ErrorLoggerIdx, this.m_error);
    }

    public override void Move()
    {
        AeroFrame.SetAbsoluteVelocity(this.m_leftError, new Vector3(this.m_refVal, 0.0f, 0.0f));
        AeroFrame.SetAbsoluteVelocity(this.m_rightError, new Vector3(this.m_refVal, 0.0f, 0.0f));
    }

    public override void Display()
    {
        Material materialDefault, materialError;
        // Assign material to renderer
        if (this.m_error > 0.0f) {
            materialDefault = this.m_materialPositiveDefault;
            materialError = this.m_materialPositiveError;
        }
        else
        {
            materialDefault = this.m_materialNegativeDefault;
            materialError = this.m_materialNegativeError;
        }
        this.m_left.GetComponent<Renderer>().sharedMaterial = materialDefault;
        this.m_right.GetComponent<Renderer>().sharedMaterial = materialDefault;
        this.m_leftError.GetComponent<Renderer>().sharedMaterial = materialError;
        this.m_rightError.GetComponent<Renderer>().sharedMaterial = materialError;
        this.m_left.GetComponent<Renderer>().material.SetFloat("_Blend", 1);
        this.m_right.GetComponent<Renderer>().material.SetFloat("_Blend", 1);
        this.m_leftError.GetComponent<Renderer>().material.SetFloat("_Blend", Mathf.Abs(this.m_error));
        this.m_rightError.GetComponent<Renderer>().material.SetFloat("_Blend", Mathf.Abs(this.m_error));
    }
}
