using System;
using UnityEngine;

class NoForcingFunctionCreator : ForcingFunctionCreator
{
    public override AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        NoForcingFunctionConfig NoForcingFunctionConfig = (abstractForcingFunctionConfig as NoForcingFunctionConfig);
        if (NoForcingFunctionConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert control config of type " + abstractForcingFunctionConfig.GetType() + " to NoForcingFunctionConfig.");
        }
        return new NoForcingFunction(NoForcingFunctionConfig, t_final);
    }
}

[Serializable]
public class NoForcingFunctionConfig : AbstractForcingFunctionConfig { }

public class NoForcingFunction : AbstractForcingFunction
{
    // NoForcingFunction-specific members

    public NoForcingFunction(NoForcingFunctionConfig config, float t_final) : base(config as AbstractForcingFunctionConfig, t_final) { }
    public NoForcingFunctionConfig NoForcingFunctionConfig => this.AbstractForcingFunctionConfig as NoForcingFunctionConfig;

    // Compute current value
    public override void Compute(float t) { }
}