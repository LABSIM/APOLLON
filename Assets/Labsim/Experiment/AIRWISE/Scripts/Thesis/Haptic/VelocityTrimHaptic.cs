using System;
using UnityEngine;

class VelocityTrimHapticCreator : HapticCreator
{
    public VelocityTrimHapticCreator(AbstractHaptic.HapticAxisModes axisMode): base(axisMode) {}
    public override AbstractHaptic Create(AbstractHapticConfig abstractHapticConfig, Rigidbody rb, Manager manager)
    {
        VelocityTrimHapticConfig VelocityTrimHapticConfig = (abstractHapticConfig as VelocityTrimHapticConfig);
        if (VelocityTrimHapticConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert haptic config of type " + abstractHapticConfig.GetType() + " to VelocityTrimHapticConfig.");
        }
        return new VelocityTrimHaptic(VelocityTrimHapticConfig, rb, manager, this.axisMode);
    }
}

[Serializable]
public class VelocityTrimHapticConfig : AbstractHapticConfig { }

public class VelocityTrimHaptic : AbstractHaptic
{
    public VelocityTrimHaptic(VelocityTrimHapticConfig config, Rigidbody rb, Manager manager, AbstractHaptic.HapticAxisModes axisMode) : base(config, rb, manager) { }

    public override void FetchCriterion()
    {
        base.FetchCriterion();
        Debug.Log("FetchCriterion from VelocityTrimHaptic");
    }

    // Apply to current RigidBody provided force and torque expressed in aero frame using Euler-Cardan ZYX angles
    public override void ComputeForce()
    {
        Debug.Log("ComputeForce from VelocityTrimHaptic");
        // BrunnerHandle.Instance.WriteTrimPosition(0.5f, -0.5f);
        // BrunnerHandle.Instance.WriteForceScaleFactor(200, 50);
        // BrunnerHandle.Instance.WriteFriction(10, 50);
        // BrunnerHandle.Instance.WriteTrimPositionXY(0.5f, -0.5f);
        // BrunnerHandle.Instance.WriteFrictionByVelocity(0.0f, 0.0f);
        // BrunnerHandle.Instance.WriteAxisRangeLimit(-50, 50, -75, 75);
    }

    public override void Actuate()
    {
        Debug.Log("Actuate from VelocityTrimHaptic");
    }
}