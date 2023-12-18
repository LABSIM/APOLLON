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

    public AbstractActuation(AbstractActuationConfig abstractActuationConfig, FilterFactory filterFactory, Rigidbody rb, AbstractControl control)
    {
        this.m_rb = rb;
        this.m_control = control;
        this.AbstractActuationConfig = abstractActuationConfig;
    }
    public abstract void ComputeActuation();
    public abstract void FilterActuation();
    public abstract void HandlePropellers();

}