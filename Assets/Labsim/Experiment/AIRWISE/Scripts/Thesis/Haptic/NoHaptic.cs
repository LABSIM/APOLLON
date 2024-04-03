using System;
using System.Collections.Generic;
using UnityEngine;

class NoHapticCreator : HapticCreator
{
    public NoHapticCreator(AbstractHaptic.HapticAxisModes axisMode) : base(axisMode) { }
    public override AbstractHaptic Create(AbstractHapticConfig abstractHapticConfig, Rigidbody rb, Manager manager)
    {
        NoHapticConfig NoHapticConfig = (abstractHapticConfig as NoHapticConfig);
        if (NoHapticConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert haptic config of type " + abstractHapticConfig.GetType() + " to NoHapticConfig.");
        }
        return new NoHaptic(NoHapticConfig, rb, manager);
    }
}

[Serializable]
public class NoHapticConfig : AbstractHapticConfig { }

public class NoHaptic : AbstractHaptic
{
    public NoHaptic(NoHapticConfig config, Rigidbody rb, Manager manager) : base(config as AbstractHapticConfig, rb, manager) { }

    public override void FetchCriterion() { }
    public override void ComputeForce() { }
    public override void Actuate() { }
}