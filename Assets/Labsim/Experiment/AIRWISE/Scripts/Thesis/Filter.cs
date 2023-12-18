using System;
using UnityEngine;

public class FilterFactory
{
    private FilterCreator m_creator;
    private float m_tSampling = Time.fixedDeltaTime;

    public AbstractFilter Build(FilterConfig config, Rigidbody rb)
    {
        //TODO: change to Enum and switch cases
        int mode = config.mode;
        if (mode == Constants.gain) { m_creator = new GainFilterCreator(); }
        else if (mode == Constants.firstOrder) { m_creator = new FirstOrderFilterCreator(); }
        else if (mode == Constants.secondOrder) { m_creator = new SecondOrderFilterCreator(); }
        else
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".Build(): Cannot instantiate " + mode + " type objectives.");
        }
        return m_creator.Create(config, rb, this.m_tSampling);
    }
}

public abstract class FilterCreator
{
    public abstract AbstractFilter Create(FilterConfig config, Rigidbody rb, float tSampling);
}

public class GainFilterCreator : FilterCreator
{
    public override AbstractFilter Create(FilterConfig config, Rigidbody rb, float tSampling)
    {
        return new GainFilter(config, rb, tSampling);
    }
}

public class FirstOrderFilterCreator : FilterCreator
{
    public override AbstractFilter Create(FilterConfig config, Rigidbody rb, float tSampling)
    {
        return new FirstOrderFilter(config, rb, tSampling);
    }
}

public class SecondOrderFilterCreator : FilterCreator
{
    public override AbstractFilter Create(FilterConfig config, Rigidbody rb, float tSampling)
    {
        return new SecondOrderFilter(config, rb, tSampling);
    }
}

[Serializable]
public class FilterConfig
{
    public int mode;

    // Default values
    public float K;

    public float tau;

    public float zeta;
    public float omega0;
}

public abstract class AbstractFilter
{
    // Rigidbody-related members
    protected Rigidbody m_rb;

    // Filter-related members
    protected int mode;
    protected float tSampling;
    protected float output;
    protected float outputPrev;
    protected float outputPrevPrev;

    // Declare abstract methods to be implemented in child classes
    public abstract float Filter(float input);

    public AbstractFilter(Rigidbody rb, float tSampling)
    {
        this.m_rb = rb;
        this.tSampling = tSampling;
    }
}
public class GainFilter : AbstractFilter
{
    // GainFilter-specific members
    private float K;

    public GainFilter(FilterConfig config, Rigidbody rb, float tSampling) : base(rb, tSampling)
    {
        this.K = config.K;
    }

    // Define objectives
    public override float Filter(float input)
    {
        // Define desired position + attitude
        this.output = K * input;
        return this.output;
    }
}
public class FirstOrderFilter : AbstractFilter
{
    // FirstOrderFilter-specific members
    private float tau;

    public FirstOrderFilter(FilterConfig config, Rigidbody rb, float tSampling) : base(rb, tSampling)
    {
        this.tau = config.tau;
    }

    // Define objectives
    public override float Filter(float input)
    {
        // Define desired position + attitude
        this.output = tSampling / (tSampling + this.tau) * input + this.tau / (tSampling + this.tau) * this.outputPrev;
        this.outputPrev = this.output;
        return this.output;
    }
}
public class SecondOrderFilter : AbstractFilter
{
    // SecondOrderFilter-specific members
    private float zeta;
    private float omega0;

    public SecondOrderFilter(FilterConfig config, Rigidbody rb, float tSampling) : base(rb, tSampling)
    {
        this.zeta = config.zeta;
        this.omega0 = config.omega0;
    }

    // Define objectives
    public override float Filter(float input)
    {
        this.output = (Mathf.Pow(this.omega0 * tSampling, 2) * input + (2 * this.zeta * this.omega0 * tSampling + 2) * this.outputPrev - this.outputPrevPrev) / (Mathf.Pow(this.omega0 * tSampling, 2) + 2 * this.zeta * this.omega0 * tSampling + 1);
        this.outputPrevPrev = this.outputPrev;
        this.outputPrev = this.output;
        return this.output;
    }
}