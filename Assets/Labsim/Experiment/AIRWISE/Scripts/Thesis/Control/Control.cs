using System;
using UnityEngine;

public class ControlFactory
{
    private ControlCreator m_creator;
    private AbstractControlConfig m_abstractConfig;
    public AbstractControl Build(ControlConfig config, AbstractMapping mapping, Rigidbody rb)
    {
        string controlConfigFile = Constants.streamingAssetsPath + Constants.ControlConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(controlConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> AbstractControl.Build(): Cannot find control config file " + Constants.ControlConfigFile);
        }

        switch ((AbstractControl.ControlModes)config.mode)
        {
            case AbstractControl.ControlModes.directMode:
                m_creator = new DirectModeControlCreator();
                m_abstractConfig = Utilities.Read<DirectModeControlConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.openLoop:
                m_creator = new OpenLoopControlCreator();
                m_abstractConfig = Utilities.Read<OpenLoopControlConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.altitudeAttitudeYaw:
                m_creator = new AltitudeAttitudeYawControlCreator();
                if (mapping.GetType() != typeof(AltitudeAttitudeMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read altitude + attitude inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<AltitudeAttitudeYawControlConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.altitudeTranslationalVelocity:
                m_creator = new AltitudeTranslationalVelocityControlCreator();
                if (mapping.GetType() != typeof(AltitudeTranslationalVelocityMapping) && mapping.GetType() != typeof(XYIncrementZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read altitude + velocity inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<AltitudeTranslationalVelocityControlConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.positionYaw:
                m_creator = new PositionYawControlCreator();
                if (mapping.GetType() != typeof(PositionMapping) && mapping.GetType() != typeof(XYIncrementZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read position inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<PositionYawControlConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.directAltitudeSingleIntegratorVelocityDirectYaw:
                m_creator = new DirectAltitudeSingleIntegratorVelocityDirectYawCreator();
                // if (mapping.GetType() != typeof(PositionMapping) && mapping.GetType() != typeof(XYIncrementZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read altitude + velocity inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<DirectAltitudeSingleIntegratorVelocityDirectYawConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.directPitchSingleIntegratorVelocityDirectYaw:
                m_creator = new DirectPitchSingleIntegratorVelocityDirectYawCreator();
                // if (mapping.GetType() != typeof(PositionMapping) && mapping.GetType() != typeof(XYIncrementZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read altitude + velocity inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<DirectPitchSingleIntegratorVelocityDirectYawConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.directAltitudeDoubleIntegratorPositionDirectYaw:
                m_creator = new DirectAltitudeDoubleIntegratorPositionDirectYawCreator();
                if (mapping.GetType() != typeof(IncrementXYZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read altitude + position inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<DirectAltitudeDoubleIntegratorPositionDirectYawConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.directPitchDoubleIntegratorPositionDirectYaw:
                m_creator = new DirectPitchDoubleIntegratorPositionDirectYawCreator();
                if (mapping.GetType() != typeof(IncrementXYZOtherAxisMapping)) Debug.LogError("<color=Red>Error: </color> ControlFactory.Build(): Cannot read attitude + position inputs from " + mapping.GetType() + " type mapping.");
                m_abstractConfig = Utilities.Read<DirectPitchDoubleIntegratorPositionDirectYawConfig>(controlConfigFile);
                break;
            case AbstractControl.ControlModes.undefined:
            default:
                //TODO : bail out early + Log
                break;
        }
        return m_creator.Create(m_abstractConfig, rb, mapping);
    }
}

abstract class ControlCreator
{
    public abstract AbstractControl Create(AbstractControlConfig abstractControlConfig, Rigidbody rb, AbstractMapping mapping);
}

[Serializable]
public class ControlConfig
{
    public int mode;
    public AbstractControlConfig AbstractControlConfig;

}

[Serializable]
public class AbstractControlConfig
{
    // Control-related config
    public float k_time = 0.5f;
}

public abstract class AbstractControl
{
    protected AbstractControlConfig AbstractControlConfig { get; private set; } = null;

    public enum ControlModes 
    {
        undefined = -1,
        directMode = 0,
        openLoop = 1,
        altitudeAttitudeYaw = 2,
        altitudeTranslationalVelocity = 3,
        directAltitudeSingleIntegratorVelocityDirectYaw = 4,
        directPitchSingleIntegratorVelocityDirectYaw = 5,
        positionYaw = 6,
        directAltitudeDoubleIntegratorPositionDirectYaw = 7,
        directPitchDoubleIntegratorPositionDirectYaw = 8,
    };
    public ControlModes m_mode = ControlModes.undefined;

    public ControlModes Mode
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

    // Mapping-related members
    public AbstractMapping m_mapping;

    // Actuation members
    public Vector4 Order { get; protected set; }

    // Declare abstract methods to be implemented in child classes
    public abstract void FetchMapping();
    public abstract void Compute();

    public AbstractControl(Rigidbody rb, AbstractMapping mapping, AbstractControlConfig abstractControlConfig)
    {
        this.m_rb = rb;
        this.m_mapping = mapping;
        this.AbstractControlConfig = abstractControlConfig;
    }

    //void decelerate(float altitude)
    //{
    //    altitudeTranslationalVelocityControl(altitude, new Vector3(0.0f, 0.0f, 0.0f));
    //}

    //void computeDistanceToWalls(float[] distances, out float minDistance, out int minIndex)
    //{
    //    for (var i = 0; i < this.hit_points.Length; i++)
    //    {
    //        computeDistanceToWallInDirection(i);
    //    }
    //    minDistance = distances.Min();
    //    minIndex = Array.IndexOf(distances, distances.Min());
    //}

    //void computeDistanceToWallInDirection(int index)
    //{
    //    RaycastHit hit = new RaycastHit();

    //    Vector3 dir = Constants.mUnityAero * rayDirections[index];
    //    Physics.Raycast(this.m_rb.transform.position + dir, this.m_rb.transform.TransformDirection(dir), out hit);
    //    Debug.DrawRay(this.m_rb.transform.position + dir, this.m_rb.transform.TransformDirection(dir) * hit.distance, colors[index], 5f);

    //    if (hit.collider)
    //    {
    //        this.hit_points[index] = Constants.mAeroUnity * hit.point;
    //        this.distances[index] = hit.distance;
    //    }
    //}
}