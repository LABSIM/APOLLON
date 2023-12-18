using System;
using UnityEngine;

public class MappingFactory
{
    private MappingCreator m_creator;
    private AbstractMappingConfig m_abstractConfig;
    public AbstractMapping Build(MappingConfig config, FilterFactory filterFactory, Rigidbody rb)
    {
        string mappingConfigFile = Constants.streamingAssetsPath + Constants.MappingConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(mappingConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> AbstractMapping.Build(): Cannot find mapping config file " + Constants.MappingConfigFile);
        }

        switch ((AbstractMapping.MappingModes)config.mode)
        {
            case AbstractMapping.MappingModes.altitudeAttitude:
                m_creator = new AltitudeAttitudeMappingCreator();
                m_abstractConfig = Utilities.Read<AltitudeAttitudeMappingConfig>(mappingConfigFile);
                break;
            case AbstractMapping.MappingModes.altitudeTranslationalVelocity:
                m_creator = new AltitudeTranslationalVelocityMappingCreator();
                m_abstractConfig = Utilities.Read<AltitudeTranslationalVelocityMappingConfig>(mappingConfigFile);
                break;
            case AbstractMapping.MappingModes.position:
                m_creator = new PositionMappingCreator();
                m_abstractConfig = Utilities.Read<PositionMappingConfig>(mappingConfigFile);
                break;
            case AbstractMapping.MappingModes.xYIncrementZOtherAxis:
                m_creator = new XYIncrementZOtherAxisMappingCreator();
                m_abstractConfig = Utilities.Read<XYIncrementZOtherAxisMappingConfig>(mappingConfigFile);
                break;
            case AbstractMapping.MappingModes.incrementXYZOtherAxis:
                m_creator = new IncrementXYZOtherAxisMappingCreator();
                m_abstractConfig = Utilities.Read<IncrementXYZOtherAxisMappingConfig>(mappingConfigFile);
                break;
            case AbstractMapping.MappingModes.undefined:
            default:
                //TODO: bail out early + Log
                break;
        }
        return m_creator.Create(m_abstractConfig, filterFactory, rb);
    }
}

abstract class MappingCreator
{
    public abstract AbstractMapping Create(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb);
}

[Serializable]
public class MappingConfig
{
    public int mode = -1;
}

[Serializable]
public class AbstractMappingConfig
{
    protected float k_time = 0.5f;
}

public abstract class AbstractMapping
{
    protected AbstractMappingConfig AbstractMappingConfig { get; set; } = null;
    public bool ConnectedToBrunner { get; protected set; } = BrunnerHandle.libOperational;

    public enum MappingModes
    {
        undefined = -1,
        altitudeAttitude = 1,
        altitudeTranslationalVelocity = 2,
        position = 3,
        xYIncrementZOtherAxis = 4,
        incrementXYZOtherAxis = 5,
    };
    public MappingModes m_mode = MappingModes.undefined;

    public MappingModes Mode
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

    // Declare abstract methods to be implemented in child classes
    public abstract void FilterMapping();

    // Declare virtual methods to be completed in child classes
    public virtual void ReadMapping()
    {
        BrunnerHandle.Instance.ExecuteOneStep();
    }

    public AbstractMapping(Rigidbody rb)
    {
        this.m_rb = rb;
    }
}