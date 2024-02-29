using System;
using UnityEngine;

class FajenHapticCreator : HapticCreator
{
    public FajenHapticCreator(AbstractHaptic.HapticAxisModes axisMode) : base(axisMode) { }
    public override AbstractHaptic Create(AbstractHapticConfig abstractHapticConfig, Rigidbody rb, Manager manager)
    {
        FajenHapticConfig FajenHapticConfig = (abstractHapticConfig as FajenHapticConfig);
        if (FajenHapticConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".create(): Cannot convert haptic config of type " + abstractHapticConfig.GetType() + " to FajenHapticConfig.");
        }
        return new FajenHaptic(FajenHapticConfig, rb, manager);
    }
}

[Serializable]
public class FajenHapticConfig : AbstractHapticConfig
{
    // Constant parameters
    public float b, kG, c1, c2;
    public float kO, c3, c4;

    // Parameters to depend on trajectory portion
    //TODO: envoyé par Nawfel après lecture de la scène
    public float xG, yG;
    public float[] xO, yO;
    public float errD;

    // Solver parameters
    public int order;
    public float tSolver, dt;
    public int nStepSolver;
}

public class FajenHaptic : AbstractHaptic
{
    // FajenHaptic-specific members
    private SteeringDynamics m_SteeringDynamics;

    private float PositionXToSend, PositionYToSend;
    private int PositionXToSendLoggerIdx, PositionYToSendLoggerIdx;

    public FajenHaptic(FajenHapticConfig config, Rigidbody rb, Manager manager) : base(config as AbstractHapticConfig, rb, manager)
    {
        this.m_SteeringDynamics = rb.GetComponentsInChildren<SteeringDynamics>()[0];
        this.m_SteeringDynamics.Configure(config.b, config.kG, config.c1, config.c2, config.kO, config.c3, config.c4, this.m_rb, config.order, config.tSolver, config.dt, config.nStepSolver);
        this.m_SteeringDynamics.SetGoalAndObstacles(config.xG, config.yG, config.xO, config.yO, config.errD);

        Vector4 X0 = new Vector4();

        this.Initialize(manager.GetElapsedTime(), manager.GetElapsedTime() + config.tSolver, X0);

        this.PositionXToSendLoggerIdx = Logger.Instance.GetEntry("PositionXToSend");
        this.PositionYToSendLoggerIdx = Logger.Instance.GetEntry("PositionYToSend");

        //TODO: nettoyer pour ajouter les conditions propres à la loi haptique par Fajen
        //Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.SecurityFactorKey, this.SecurityFactor);
        //Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.ReactionTimeKey, this.ReactionTime);
        //Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.kXKey, this.kX);
        //Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.kYKey, this.kY);
    }

    public void Initialize(float t0, float tf, Vector4 X0)
    {
        this.m_SteeringDynamics.Initialize(
            t0, tf, 
            new Vector4(AeroFrame.ComputePhi(this.m_rb), AeroFrame.ComputePhiDot(this.m_rb), AeroFrame.GetPosition(this.m_rb).x, AeroFrame.GetPosition(this.m_rb).y), 
            Utilities.Norm(AeroFrame.GetAbsoluteVelocity(this.m_rb)));
    }


    public override void FetchCriterion(float tElapsed)
    {
        base.FetchCriterion(tElapsed);
        UnityEngine.Debug.Log(this.m_SteeringDynamics.GetResolutionResultAtTime(tElapsed));
    }
    public override void ComputeForce() { }
    public override void Actuate(float elapsed)
    {
        BrunnerHandle.Instance.WriteTrimPositionXY(this.PositionXToSend, this.PositionYToSend);
        Logger.Instance.AddEntry(this.PositionXToSendLoggerIdx, this.PositionXToSend);
        Logger.Instance.AddEntry(this.PositionYToSendLoggerIdx, this.PositionYToSend);
    }
}