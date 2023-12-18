using UnityEngine;

public class ErrorDisplay
{
    // Rigidbody-related members
    private readonly Rigidbody m_rb;
    private Rigidbody m_left;
    private Rigidbody m_right;
    private Rigidbody m_leftError;
    private Rigidbody m_rightError;

    // ForcingFunction-related member
    private readonly AbstractForcingFunction m_forcingFunction;
    private float m_currVal;
    private float m_refVal;
    private float m_error;

    private Material m_materialNegativeDefault, m_materialNegativeError, m_materialPositiveDefault, m_materialPositiveError;

    public ErrorDisplay(Rigidbody rb, AbstractForcingFunction forcingFunction)
    {
        this.m_rb = rb;
        this.m_forcingFunction = forcingFunction;
        this.m_left = GameObject.Find("Left").GetComponent<Rigidbody>();
        this.m_right = GameObject.Find("Right").GetComponent<Rigidbody>();
        this.m_leftError = GameObject.Find("LeftError").GetComponent<Rigidbody>();
        this.m_rightError = GameObject.Find("RightError").GetComponent<Rigidbody>();
        m_materialNegativeDefault = GameObject.Find("Negative").GetComponent<Renderer>().materials[0];
        m_materialNegativeError = GameObject.Find("Negative").GetComponent<Renderer>().materials[1];
        m_materialPositiveDefault = GameObject.Find("Positive").GetComponent<Renderer>().materials[0];
        m_materialPositiveError = GameObject.Find("Positive").GetComponent<Renderer>().materials[1];
    }

    public void ComputeError()
    {
        // Fetch aircraft velocity
        this.m_currVal = AeroFrame.GetAbsoluteVelocity(this.m_rb).x;
        this.m_refVal = this.m_forcingFunction.val;
        this.m_error = (this.m_currVal - this.m_refVal) / (this.m_forcingFunction.dA);
    }

    public void Move()
    {
        AeroFrame.SetAbsoluteVelocity(this.m_leftError, new Vector3(this.m_refVal, 0.0f, 0.0f));
        AeroFrame.SetAbsoluteVelocity(this.m_rightError, new Vector3(this.m_refVal, 0.0f, 0.0f));
    }

    public void Display()
    {
        Material materialDefault, materialError;

        // Assign material to renderer
        if (this.m_error > 0.0f) 
        {
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
