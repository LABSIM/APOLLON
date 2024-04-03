using System;
using UnityEngine;

public class ForcingFunctionFactory
{
    private ForcingFunctionCreator m_creator;
    private AbstractForcingFunctionConfig m_abstractConfig;
    public AbstractForcingFunction Build(ForcingFunctionConfig config)
    {
        string forcingFunctionConfigFile = Constants.streamingAssetsPath + Constants.ForcingFunctionConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(forcingFunctionConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".Build(): Cannot find forcingFunction config file " + Constants.ForcingFunctionConfigFile);
        }

        switch ((AbstractForcingFunction.ForcingFunctionModes)config.mode)
        {
            case AbstractForcingFunction.ForcingFunctionModes.fixedSine:
                m_creator = new FixedSineForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<FixedSineForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.variableSine:
                m_creator = new VariableSineForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<VariableSineForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.fixedSquare:
                m_creator = new FixedSquareForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<FixedSquareForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.variableSquare:
                m_creator = new VariableSquareForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<VariableSquareForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.fixedTrapezoidal:
                m_creator = new FixedTrapezoidalForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<FixedTrapezoidalForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.variableTrapezoidal:
                m_creator = new VariableTrapezoidalForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<VariableTrapezoidalForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
            case AbstractForcingFunction.ForcingFunctionModes.undefined:
            default:
                m_creator = new NoForcingFunctionCreator();
                m_abstractConfig = Utilities.Read<NoForcingFunctionConfig>(forcingFunctionConfigFile);
                break;
        }
        return m_creator.Create(m_abstractConfig, config.t_final);
    }
}

abstract class ForcingFunctionCreator
{
    public abstract AbstractForcingFunction Create(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final);
}

[Serializable]
public class ForcingFunctionConfig
{
    public int mode;
    public float t_final;
    public AbstractForcingFunctionConfig AbstractForcingFunctionConfig;

}

[Serializable]
public class AbstractForcingFunctionConfig
{
    // ForcingFunction-related config
    protected float t_final;
}

public abstract class AbstractForcingFunction
{
    protected AbstractForcingFunctionConfig AbstractForcingFunctionConfig { get; private set; } = null;
    public float val { get; protected set; }
    public float dA { get; protected set; }
    public float t_final { get; private set; }
    public float A { get; protected set; }
    public float average { get; protected set; }
    public float f { get; protected set; }
    public float tau { get; protected set; }

    // Logger members
    private int ValLoggerIdx;

    public enum ForcingFunctionModes
    {
        undefined = -1,
        fixedSine = 0,
        variableSine = 1,
        fixedSquare = 2,
        variableSquare = 3,
        fixedTrapezoidal = 4,
        variableTrapezoidal = 5,
    };
    public ForcingFunctionModes m_mode = ForcingFunctionModes.undefined;

    public ForcingFunctionModes Mode
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

    public AbstractForcingFunction(AbstractForcingFunctionConfig abstractForcingFunctionConfig, float t_final)
    {
        this.AbstractForcingFunctionConfig = abstractForcingFunctionConfig;
        this.t_final = t_final;
        this.ValLoggerIdx = Logger.Instance.GetEntry("Val");
    }

    // Declare abstract methods to be implemented in child classes
    public abstract void Compute(double elapsed);
}