using System;
using UnityEngine;

class AltitudeTranslationalVelocityMappingCreator : MappingCreator
{
    public override AbstractMapping Create(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb)
    {
        AltitudeTranslationalVelocityMappingConfig AltitudeTranslationalVelocityMappingConfig = (abstractMappingConfig as AltitudeTranslationalVelocityMappingConfig);
        if (AltitudeTranslationalVelocityMappingConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> AltitudeTranslationalVelocityMappingCreator.create(): Cannot convert control config of type " + abstractMappingConfig.GetType() + " to AltitudeTranslationalVelocityMappingConfig.");
        }
        return new AltitudeTranslationalVelocityMapping(AltitudeTranslationalVelocityMappingConfig, filterFactory, rb);
    }
}

[Serializable]
public class AltitudeTranslationalVelocityMappingConfig : AbstractMappingConfig
{
    public float DefaultDesiredZ = 0.0f;
    public float DefaultDesiredPsi = 0.0f;
    public float DefaultDesiredVx = 0.0f;
    public float DefaultDesiredVy = 0.0f;
    public float DefaultDesiredVz = 0.0f;
    public FilterConfig filterZ = new FilterConfig();
    public FilterConfig filterVx = new FilterConfig();
    public FilterConfig filterVy = new FilterConfig();
    public FilterConfig filterPsi = new FilterConfig();
}

public class AltitudeTranslationalVelocityMapping : AbstractMapping
{
    // AltitudeTranslationalVelocityMappingConfig property
    private AltitudeTranslationalVelocityMappingConfig AltitudeTranslationalVelocityMappingConfig => this.AbstractMappingConfig as AltitudeTranslationalVelocityMappingConfig;

    // Unfiltered AltitudeTranslationalVelocityMapping-specific members
    public float AltitudeDesired { get; protected set; }
    public Vector3 VelocityDesired { get; protected set; }
    public float YawDesired { get; protected set; }
    public float DefaultAltitudeDesired { get; protected set; }
    public Vector3 DefaultVelocityDesired { get; protected set; }
    public float DefaultYawDesired { get; protected set; }

    // Filter members
    private AbstractFilter FilterZ { get; set; }
    private AbstractFilter FilterVx { get; set; }
    private AbstractFilter FilterVy { get; set; }
    private AbstractFilter FilterVz { get; set; }
    private AbstractFilter FilterPsi { get; set; }

    // AltitudeTranslationalVelocityMapping-specific members
    public float AltitudeDesiredUnfiltered { get; protected set; }
    public Vector3 VelocityDesiredUnfiltered { get; protected set; }
    public float YawDesiredUnfiltered { get; protected set; }

    // Logger members
    private int AltitudeDesiredLoggerIdx, 
        VelocityDesiredXLoggerIdx, VelocityDesiredYLoggerIdx, VelocityDesiredZLoggerIdx, 
        YawDesiredLoggerIdx;

    public AltitudeTranslationalVelocityMapping(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb) : base(rb)
    {
        this.AbstractMappingConfig = abstractMappingConfig;
        this.AltitudeDesiredUnfiltered = AltitudeTranslationalVelocityMappingConfig.DefaultDesiredZ;
        this.VelocityDesiredUnfiltered = new Vector3(AltitudeTranslationalVelocityMappingConfig.DefaultDesiredVx, AltitudeTranslationalVelocityMappingConfig.DefaultDesiredVy, AltitudeTranslationalVelocityMappingConfig.DefaultDesiredVz);
        this.YawDesiredUnfiltered = AltitudeTranslationalVelocityMappingConfig.DefaultDesiredPsi;
        this.FilterZ = filterFactory.Build(AltitudeTranslationalVelocityMappingConfig.filterZ, rb);
        this.FilterVx = filterFactory.Build(AltitudeTranslationalVelocityMappingConfig.filterVx, rb);
        this.FilterVy = filterFactory.Build(AltitudeTranslationalVelocityMappingConfig.filterVy, rb);
        this.FilterPsi = filterFactory.Build(AltitudeTranslationalVelocityMappingConfig.filterPsi, rb);
        this.AltitudeDesired = this.AltitudeDesiredUnfiltered;
        this.VelocityDesired = this.VelocityDesiredUnfiltered;
        this.YawDesired = this.YawDesiredUnfiltered;
        this.DefaultAltitudeDesired = this.AltitudeDesiredUnfiltered;
        this.DefaultVelocityDesired = this.VelocityDesiredUnfiltered;
        this.DefaultYawDesired = this.YawDesiredUnfiltered;

        this.AltitudeDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "AltitudeDesired");
        this.VelocityDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredX");
        this.VelocityDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredY");
        this.VelocityDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "VelocityDesiredZ");
        this.YawDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "YawDesired");

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.AltitudeDesiredKey, this.DefaultAltitudeDesired);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.VelocityDesiredKey, this.DefaultVelocityDesired);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.YawDesiredKey, this.DefaultYawDesired);
    }

    // Define objectives
    public override void ReadMapping()
    {
        base.ReadMapping();
        if (BrunnerHandle.Instance.GetReturnBrunner())
        {
            Vector3 velocityBodyDesired = new Vector3();
            if (Math.Abs(BrunnerHandle.Instance.GetX()) > Constants.epsilon)
            {
                velocityBodyDesired.x = (BrunnerHandle.Instance.GetX() - Constants.epsilon) / 100.0f * Parameters.VMax;
            }
            if (Math.Abs(BrunnerHandle.Instance.GetY()) > Constants.epsilon)
            {
                velocityBodyDesired.y = (BrunnerHandle.Instance.GetY() - Constants.epsilon) / 100.0f * Parameters.VMax;
            }
            this.AltitudeDesiredUnfiltered += -Parameters.JoystickToPosition * (Convert.ToSingle(BrunnerHandle.Instance.GetHatUp()) - Convert.ToSingle(BrunnerHandle.Instance.GetHatDown()));
            Matrix4x4 R = AeroFrame.GetRotationMatrix(this.m_rb);
            this.VelocityDesiredUnfiltered = R * velocityBodyDesired;
            this.YawDesiredUnfiltered += Parameters.JoystickToPosition * (Convert.ToSingle(BrunnerHandle.Instance.GetHatRight()) - Convert.ToSingle(BrunnerHandle.Instance.GetHatLeft()));

            if (!this.ConnectedToBrunner)
            {
                Debug.Log(
                        "<color=Blue>Info: </color> " + this.GetType() + ".AltitudeTranslationalVelocityMapping(): using desired altitude and velocity values from sidestick inputs again. ");
                this.ConnectedToBrunner = true;
            }
        }
        else if (this.ConnectedToBrunner)
        {
            Debug.LogWarning(
                    "<color=Orange>Warning: </color> " + this.GetType() + ".AltitudeTranslationalVelocityMapping(): could not read sidestick inputs. Using default desired altitude and velocity values. ");
            this.ConnectedToBrunner = false;
        }
    }

    // Filter objectives
    public override void FilterMapping()
    {
        this.AltitudeDesired = this.FilterZ.Filter(this.AltitudeDesiredUnfiltered);
        this.VelocityDesired = new Vector3(this.FilterVx.Filter(this.VelocityDesiredUnfiltered[0]), this.FilterVy.Filter(this.VelocityDesiredUnfiltered[1]), this.VelocityDesiredUnfiltered[2]);
        this.YawDesired = this.FilterPsi.Filter(this.YawDesiredUnfiltered);
        
        Logger.Instance.AddEntry(this.AltitudeDesiredLoggerIdx, this.AltitudeDesired);
        Logger.Instance.AddEntry(this.VelocityDesiredXLoggerIdx, this.VelocityDesired.x);
        Logger.Instance.AddEntry(this.VelocityDesiredYLoggerIdx, this.VelocityDesired.y);
        Logger.Instance.AddEntry(this.VelocityDesiredZLoggerIdx, this.VelocityDesired.z);
        Logger.Instance.AddEntry(this.YawDesiredLoggerIdx, this.YawDesired);
            //TODO: log unfiltered values in ReadMapping ?
    }
}