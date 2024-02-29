using System;
using UnityEngine;
public class ActuationFactory
{
    private ActuationCreator m_creator;
    private AbstractActuationConfig m_abstractConfig;
    public AbstractActuation Build(ActuationConfig config, FilterFactory filterFactory, AbstractControl control, Rigidbody rb, Rotor[] rotors)
    {
        string actuationConfigFile = Constants.streamingAssetsPath + Constants.ActuationConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(actuationConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> AbstractActuation.Build(): Cannot find actuation config file " + Constants.ActuationConfigFile);
        }

        switch ((AbstractActuation.ActuationModes)config.mode)
        {
            case AbstractActuation.ActuationModes.direct:
                m_creator = new DirectActuationCreator();
                m_abstractConfig = Utilities.Read<DirectActuationConfig>(actuationConfigFile);
                break;
            case AbstractActuation.ActuationModes.X4:
                m_creator = new X4ActuationCreator();
                m_abstractConfig = Utilities.Read<X4ActuationConfig>(actuationConfigFile);
                break;
            case AbstractActuation.ActuationModes.undefined:
            default:
                //TODO : bail out early + Log
                break;
        }
        return m_creator.Create(m_abstractConfig, filterFactory, rb, control);
    }
}

abstract class ActuationCreator
{
    public abstract AbstractActuation Create(AbstractActuationConfig abstractActuationConfig, FilterFactory filterFactory, Rigidbody rb, AbstractControl control);
}

[Serializable]
public class ActuationConfig
{
    public int mode;
    [NonSerialized]
    public AbstractActuationConfig AbstractActuationConfig;
    public float rotationToForce;
}

[Serializable]
public class AbstractActuationConfig { }

public abstract class AbstractActuation
{
    protected AbstractActuationConfig AbstractActuationConfig { get; private set; } = null;

    public enum ActuationModes
    {
        undefined = -1,
        direct = 0,
        X4 = 1,
    };
    public ActuationModes m_mode = ActuationModes.undefined;

    public ActuationModes Mode
    {
        get
        {
            return m_mode;
        }
        private set
        {
            this.m_mode = value;
        }
    }

    // Rigidbody-related members
    protected Rigidbody m_rb;

    // Control-related members
    protected AbstractControl m_control;

    protected const float tMin = -100000;
    protected const float tMax = 100000;

    // Measured position, attitude, velocity and angular velocity
    protected int MeasuredXLoggerIdx, MeasuredYLoggerIdx, MeasuredZLoggerIdx,
        MeasuredPhiLoggerIdx, MeasuredThetaLoggerIdx, MeasuredPsiLoggerIdx,
        MeasuredXDotLoggerIdx, MeasuredYDotLoggerIdx, MeasuredZDotLoggerIdx,
        MeasuredPhiDotLoggerIdx, MeasuredThetaDotLoggerIdx, MeasuredPsiDotLoggerIdx;

    public AbstractActuation(AbstractActuationConfig abstractActuationConfig, FilterFactory filterFactory, Rigidbody rb, AbstractControl control)
    {
        this.m_rb = rb;
        this.m_control = control;
        this.AbstractActuationConfig = abstractActuationConfig;

        this.MeasuredXLoggerIdx = Logger.Instance.GetEntry("MeasuredX");
        this.MeasuredYLoggerIdx = Logger.Instance.GetEntry("MeasuredY");
        this.MeasuredZLoggerIdx = Logger.Instance.GetEntry("MeasuredZ");

        this.MeasuredPhiLoggerIdx = Logger.Instance.GetEntry("MeasuredPhi");
        this.MeasuredThetaLoggerIdx = Logger.Instance.GetEntry("MeasuredTheta");
        this.MeasuredPsiLoggerIdx = Logger.Instance.GetEntry("MeasuredPsi");

        this.MeasuredXDotLoggerIdx = Logger.Instance.GetEntry("MeasuredXDot");
        this.MeasuredYDotLoggerIdx = Logger.Instance.GetEntry("MeasuredYDot");
        this.MeasuredZDotLoggerIdx = Logger.Instance.GetEntry("MeasuredZDot");

        this.MeasuredPhiDotLoggerIdx = Logger.Instance.GetEntry("MeasuredPhiDot");
        this.MeasuredThetaDotLoggerIdx = Logger.Instance.GetEntry("MeasuredThetaDot");
        this.MeasuredPsiDotLoggerIdx = Logger.Instance.GetEntry("MeasuredPsiDot");

    }
    public virtual void ComputeActuation()
    {
        Vector3 position = AeroFrame.GetPosition(this.m_rb);
        Vector3 attitude = AeroFrame.GetAngles(this.m_rb);
        Vector3 velocity = AeroFrame.GetAbsoluteVelocity(this.m_rb);
        Vector3 angularVelocity = AeroFrame.GetAngularVelocity(this.m_rb);

        Logger.Instance.AddEntry(this.MeasuredXLoggerIdx, position.x);
        Logger.Instance.AddEntry(this.MeasuredYLoggerIdx, position.y);
        Logger.Instance.AddEntry(this.MeasuredZLoggerIdx, position.z);

        Logger.Instance.AddEntry(this.MeasuredPhiLoggerIdx, attitude.x);
        Logger.Instance.AddEntry(this.MeasuredThetaLoggerIdx, attitude.y);
        Logger.Instance.AddEntry(this.MeasuredPsiLoggerIdx, attitude.z);

        Logger.Instance.AddEntry(this.MeasuredXDotLoggerIdx, velocity.x);
        Logger.Instance.AddEntry(this.MeasuredYDotLoggerIdx, velocity.y);
        Logger.Instance.AddEntry(this.MeasuredZDotLoggerIdx, velocity.z);

        Logger.Instance.AddEntry(this.MeasuredPhiDotLoggerIdx, angularVelocity.x);
        Logger.Instance.AddEntry(this.MeasuredThetaDotLoggerIdx, angularVelocity.y);
        Logger.Instance.AddEntry(this.MeasuredPsiDotLoggerIdx, angularVelocity.z);
    }
    public abstract void FilterActuation();
    public abstract void HandlePropellers();

}