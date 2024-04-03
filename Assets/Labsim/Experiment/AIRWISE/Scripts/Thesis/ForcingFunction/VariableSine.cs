using System;
using UnityEngine;

class VariableSineForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        VariableSineForcingFunctionConfig VariableSineForcingFunctionConfig = (abstractForcingFunctionConfig as VariableSineForcingFunctionConfig);
        if (VariableSineForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to VariableSineForcingFunctionConfig.");
        }
        return new VariableSineForcingFunction(VariableSineForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class VariableSineForcingFunctionConfig : AbstractForcingFunctionConfig
{
    public float A;
    public float average;
    public float f;
    public float tau;
}

public class VariableSineForcingFunction : AbstractForcingFunction
{
    public VariableSineForcingFunction(VariableSineForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final)
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
    public VariableSineForcingFunctionConfig VariableSineForcingFunctionConfig => this.AbstractForcingFunctionConfig as VariableSineForcingFunctionConfig;

    // Compute current value
    public override void Compute(double t) { }
}