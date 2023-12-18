using System;
using UnityEngine;

class NoErrorDisplayCreator : ErrorDisplayCreator
{
    public override AbstractErrorDisplay Create(AbstractErrorDisplayConfig abstractErrorDisplayConfig, Rigidbody rb, AbstractForcingFunction forcingFunction)
    {
        NoErrorDisplayConfig NoErrorDisplayConfig = (abstractErrorDisplayConfig as NoErrorDisplayConfig);
        if (NoErrorDisplayConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> NoErrorCreator.create(): Cannot convert control config of type " + abstractErrorDisplayConfig.GetType() + " to NoErrorDisplayConfig.");
        }
        return new NoErrorDisplay(NoErrorDisplayConfig, rb, forcingFunction);
    }
}

[Serializable]
public class NoErrorDisplayConfig : AbstractErrorDisplayConfig { }

public class NoErrorDisplay : AbstractErrorDisplay
{
    public NoErrorDisplay(NoErrorDisplayConfig config, Rigidbody rb, AbstractForcingFunction forcingFunction) : base(config as AbstractErrorDisplayConfig, rb, forcingFunction) { }

    public override void ComputeError() { }

    public override void Move() { }

    public override void Display() { }
}
