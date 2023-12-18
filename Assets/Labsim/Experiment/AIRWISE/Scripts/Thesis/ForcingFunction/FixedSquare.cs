using System;
using UnityEngine;

class FixedSquareForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        FixedSquareForcingFunctionConfig FixedSquareForcingFunctionConfig = (abstractForcingFunctionConfig as FixedSquareForcingFunctionConfig);
        if (FixedSquareForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to FixedSquareForcingFunctionConfig.");
        }
        return new FixedSquareForcingFunction(FixedSquareForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class FixedSquareForcingFunctionConfig : AbstractForcingFunctionConfig
{
    public float A;
    public float average;
    public float f;
    public float tau;
}

public class FixedSquareForcingFunction : AbstractForcingFunction
{
    public FixedSquareForcingFunction(FixedSquareForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final)
    {
        this.dA = config.A;
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
    public FixedSquareForcingFunctionConfig FixedSquareForcingFunctionConfig => this.AbstractForcingFunctionConfig as FixedSquareForcingFunctionConfig;
    
    // Compute current value
    public override void Compute(float t)
    {
        this.val = this.A * Mathf.Sign(Mathf.Sin(2.0f * Mathf.PI * this.f * (t + this.tau))) + this.average;
    }
}