using System;
using UnityEngine;

class FixedSineForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        FixedSineForcingFunctionConfig FixedSineForcingFunctionConfig = (abstractForcingFunctionConfig as FixedSineForcingFunctionConfig);
        if (FixedSineForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to FixedSineForcingFunctionConfig.");
        }
        return new FixedSineForcingFunction(FixedSineForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class FixedSineForcingFunctionConfig : AbstractForcingFunctionConfig
{
    public float A;
    public float average;
    public float f;
    public float tau;
}

public class FixedSineForcingFunction : AbstractForcingFunction
{
    // FixedSineForcingFunction-specific members

    public FixedSineForcingFunction(FixedSineForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final)
    {
        this.dA = 2 * config.A;
        this.A = config.A;
        this.average = config.average;
        this.f = config.f;
        this.tau = config.tau;

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.ForcingFunctionKey, Logger.Utilities.DAKey, this.dA);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.ForcingFunctionKey, Logger.Utilities.AKey, this.A);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.ForcingFunctionKey, Logger.Utilities.AverageKey, this.average);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.ForcingFunctionKey, Logger.Utilities.FKey, this.f);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.FixedValuesKey, Logger.Utilities.ForcingFunctionKey, Logger.Utilities.TauKey, this.tau);
    }
    public FixedSineForcingFunctionConfig FixedSineForcingFunctionConfig => this.AbstractForcingFunctionConfig as FixedSineForcingFunctionConfig;

    // Compute current value
    public override void Compute(float t)
    {
        this.val = this.A * Mathf.Sin(2.0f * Mathf.PI * this.f * (t + this.tau)) + this.average;
    }
}