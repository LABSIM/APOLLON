using System;
using UnityEngine;

class FixedTrapezoidalForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        FixedTrapezoidalForcingFunctionConfig FixedTrapezoidalForcingFunctionConfig = (abstractForcingFunctionConfig as FixedTrapezoidalForcingFunctionConfig);
        if (FixedTrapezoidalForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to FixedTrapezoidalForcingFunctionConfig.");
        }
        return new FixedTrapezoidalForcingFunction(FixedTrapezoidalForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class FixedTrapezoidalForcingFunctionConfig : AbstractForcingFunctionConfig
{
    public float A;
    public float average;
    public float f;
    public float tau;
}

public class FixedTrapezoidalForcingFunction : AbstractForcingFunction
{
    public FixedTrapezoidalForcingFunction(FixedTrapezoidalForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final)
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
    public FixedTrapezoidalForcingFunctionConfig FixedTrapezoidalForcingFunctionConfig => this.AbstractForcingFunctionConfig as FixedTrapezoidalForcingFunctionConfig;

    // Compute current value
    public override void Compute(double t) { }
}