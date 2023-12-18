using System;
using UnityEngine;

class VariableSquareForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        VariableSquareForcingFunctionConfig VariableSquareForcingFunctionConfig = (abstractForcingFunctionConfig as VariableSquareForcingFunctionConfig);
        if (VariableSquareForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to VariableSquareForcingFunctionConfig.");
        }
        return new VariableSquareForcingFunction(VariableSquareForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class VariableSquareForcingFunctionConfig : AbstractForcingFunctionConfig
{
    public float A;
    public float average;
    public float f;
    public float tau;
}

public class VariableSquareForcingFunction : AbstractForcingFunction
{
    public VariableSquareForcingFunction(VariableSquareForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final)
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
    public VariableSquareForcingFunctionConfig VariableSquareForcingFunctionConfig => this.AbstractForcingFunctionConfig as VariableSquareForcingFunctionConfig;

    // Compute current value
    public override void Compute(float t)
    {

    }
}