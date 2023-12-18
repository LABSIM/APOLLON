using System;
using UnityEngine;

public class ErrorDisplayFactory
{
    private ErrorDisplayCreator m_creator;
    private AbstractErrorDisplayConfig m_abstractConfig;
    public AbstractErrorDisplay Build(ErrorDisplayConfig config, AbstractForcingFunction forcingFunction, Rigidbody rb)
    {
        string ErrorDisplayConfigFile = Constants.streamingAssetsPath + Constants.ErrorDisplayConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(ErrorDisplayConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> AbstractErrorDisplay.Build(): Cannot find ErrorDisplay config file " + Constants.ErrorDisplayConfigFile);
        }

        switch ((AbstractErrorDisplay.ErrorDisplayModes)config.mode)
        {
            case AbstractErrorDisplay.ErrorDisplayModes.noErrorDisplay:
                m_creator = new NoErrorDisplayCreator();
                m_abstractConfig = Utilities.Read<NoErrorDisplayConfig>(ErrorDisplayConfigFile);
                break;
            case AbstractErrorDisplay.ErrorDisplayModes.opticalFlowGrid:
                m_creator = new OpticalFlowGridErrorDisplayCreator();
                m_abstractConfig = Utilities.Read<OpticalFlowGridErrorDisplayConfig>(ErrorDisplayConfigFile);
                break;
            case AbstractErrorDisplay.ErrorDisplayModes.undefined:
            default:
                //TODO : bail out early + Log
                break;
        }
        return m_creator.Create(m_abstractConfig, rb, forcingFunction);
    }
}

abstract class ErrorDisplayCreator
{
    public abstract AbstractErrorDisplay Create(AbstractErrorDisplayConfig abstractErrorDisplayConfig, Rigidbody rb, AbstractForcingFunction forcingFunction);
}

[Serializable]
public class ErrorDisplayConfig
{
    public int mode;
    public AbstractErrorDisplayConfig AbstractErrorDisplayConfig;

}

[Serializable]
public class AbstractErrorDisplayConfig
{
    // ErrorDisplay-related config
    protected float k_time = 0.5f;
}

public abstract class AbstractErrorDisplay
{
    protected AbstractErrorDisplayConfig AbstractErrorDisplayConfig { get; private set; } = null;

    public enum ErrorDisplayModes 
    {
        undefined = -1,
        noErrorDisplay = 0,
        opticalFlowGrid = 1,
    };
    public ErrorDisplayModes m_mode = ErrorDisplayModes.undefined;

    public ErrorDisplayModes Mode
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
    protected Rigidbody m_rb { get; private set; }

    // ForcingFunction-related members
    protected AbstractForcingFunction m_forcingFunction { get; private set; }

    // Declare abstract methods to be implemented in child classes
    public abstract void ComputeError();
    public abstract void Move();
    public abstract void Display();

    public AbstractErrorDisplay(AbstractErrorDisplayConfig abstractErrorDisplayConfig, Rigidbody rb, AbstractForcingFunction forcingFunction)
    {
        this.AbstractErrorDisplayConfig = abstractErrorDisplayConfig;
        this.m_rb = rb;
        this.m_forcingFunction = forcingFunction;
    }
}