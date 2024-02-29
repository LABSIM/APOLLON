using System;
using UnityEngine;

class AltitudeAttitudeMappingCreator : MappingCreator
{
    public override AbstractMapping Create(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb)
    {
        AltitudeAttitudeMappingConfig AltitudeAttitudeMappingConfig = (abstractMappingConfig as AltitudeAttitudeMappingConfig);
        if (AltitudeAttitudeMappingConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> AltitudeAttitudeMappingCreator.create(): Cannot convert mapping config of type " + abstractMappingConfig.GetType() + " to AltitudeAttitudeMappingConfig.");
        }
        return new AltitudeAttitudeMapping(AltitudeAttitudeMappingConfig, filterFactory, rb);
    }
}

[Serializable]
public class AltitudeAttitudeMappingConfig : AbstractMappingConfig
{
    public float DefaultDesiredZ = 0.0f;
    public float DefaultDesiredPhi = 0.0f;
    public float DefaultDesiredTheta = 0.0f;
    public float DefaultDesiredPsi = 0.0f;
    public FilterConfig filterZ = new FilterConfig();
    public FilterConfig filterPhi = new FilterConfig();
    public FilterConfig filterTheta = new FilterConfig();
    public FilterConfig filterPsi = new FilterConfig();
}
public class AltitudeAttitudeMapping : AbstractMapping
{
    // AltitudeAttitudeMappingConfig property
    private AltitudeAttitudeMappingConfig AltitudeAttitudeMappingConfig => this.AbstractMappingConfig as AltitudeAttitudeMappingConfig;

    // AltitudeAttitudeMapping-specific members
    public float AltitudeDesired { get; private set; }
    public Vector3 AttitudeDesired { get; private set; }
    public float DefaultAltitudeDesired { get; private set; }
    public Vector3 DefaultAttitudeDesired { get; private set; }

    // Filter members
    private AbstractFilter FilterZ { get; set; }
    private AbstractFilter FilterPhi { get; set; }
    private AbstractFilter FilterTheta { get; set; }
    private AbstractFilter FilterPsi { get; set; }

    // Logger members
    private int AltitudeDesiredLoggerIdx, 
        AttitudeDesiredXLoggerIdx, AttitudeDesiredYLoggerIdx, AttitudeDesiredZLoggerIdx;

    public AltitudeAttitudeMapping(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb) : base(rb)
    {
        this.AbstractMappingConfig = abstractMappingConfig;
        this.AltitudeDesired = AltitudeAttitudeMappingConfig.DefaultDesiredZ;
        this.AttitudeDesired = new Vector3(AltitudeAttitudeMappingConfig.DefaultDesiredPhi, AltitudeAttitudeMappingConfig.DefaultDesiredTheta, AltitudeAttitudeMappingConfig.DefaultDesiredPsi);
        this.DefaultAltitudeDesired = this.AltitudeDesired;
        this.DefaultAttitudeDesired = this.AttitudeDesired;
        this.FilterZ = filterFactory.Build(AltitudeAttitudeMappingConfig.filterZ, rb);
        this.FilterPhi = filterFactory.Build(AltitudeAttitudeMappingConfig.filterPhi, rb);
        this.FilterTheta = filterFactory.Build(AltitudeAttitudeMappingConfig.filterTheta, rb);
        this.FilterPsi = filterFactory.Build(AltitudeAttitudeMappingConfig.filterPsi, rb);

        this.AltitudeDesiredLoggerIdx = Logger.Instance.GetEntry("AltitudeDesiredMeasured");
        this.AttitudeDesiredXLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredMeasuredX");
        this.AttitudeDesiredYLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredMeasuredY");
        this.AttitudeDesiredZLoggerIdx = Logger.Instance.GetEntry("AttitudeDesiredMeasuredZ");

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.AltitudeDesiredKey, this.DefaultAltitudeDesired);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.AttitudeDesiredKey, this.DefaultAttitudeDesired);
    }

    // Define objectives
    public override void ReadMapping()
    {
        base.ReadMapping();
        // Define desired altitude + attitude + yaw
        if (BrunnerHandle.Instance.GetReturnBrunner())
        {
            Vector3 attitudeDesired = this.AttitudeDesired;
            attitudeDesired.x = BrunnerHandle.Instance.GetDDL();
            attitudeDesired.y = BrunnerHandle.Instance.GetDDM();
            this.AttitudeDesired = attitudeDesired;

            float altitudeDesired = this.AltitudeDesired + -Parameters.JoystickToPositionAltitude * (Convert.ToSingle(BrunnerHandle.Instance.GetHatUp()) - Convert.ToSingle(BrunnerHandle.Instance.GetHatDown()));
            this.AltitudeDesired = altitudeDesired;
            if (!this.ConnectedToBrunner)
            {
                Debug.Log(
                        "<color=Blue>Info: </color> " + this.GetType() + ".AltitudeAttitudeMapping(): using desired altitude and attitude values from sidestick inputs again. ");
                this.ConnectedToBrunner = true;
            }
        }
        else if (this.ConnectedToBrunner)
        {
            Debug.LogWarning(
                    "<color=Orange>Warning: </color> " + this.GetType() + ".AltitudeAttitudeMapping(): could not read sidestick inputs. Using default desired altitude and attitude values. ");
            this.ConnectedToBrunner = false;
        }
    }

    // Filter objectives
    public override void FilterMapping()
    {
        this.AltitudeDesired = this.FilterZ.Filter(this.AltitudeDesired);
        this.AttitudeDesired = new Vector3(this.FilterPhi.Filter(this.AttitudeDesired[0]), this.FilterTheta.Filter(this.AttitudeDesired[1]), this.FilterPsi.Filter(this.AttitudeDesired[2]));
            Logger.Instance.AddEntry(this.AltitudeDesiredLoggerIdx, this.AltitudeDesired);
            Logger.Instance.AddEntry(this.AttitudeDesiredXLoggerIdx, this.AttitudeDesired.x);
            Logger.Instance.AddEntry(this.AttitudeDesiredYLoggerIdx, this.AttitudeDesired.y);
            Logger.Instance.AddEntry(this.AttitudeDesiredZLoggerIdx, this.AttitudeDesired.z);
    }
}