using System;
using UnityEngine;


class XYIncrementZOtherAxisMappingCreator : MappingCreator
{
    public override AbstractMapping Create(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb)
    {
        XYIncrementZOtherAxisMappingConfig XYIncrementZOtherAxisMappingConfig = (abstractMappingConfig as XYIncrementZOtherAxisMappingConfig);
        if (XYIncrementZOtherAxisMappingConfig == null)
        {
            Debug.LogError("<color=Red>Error: </color> XYIncrementZOtherAxisMappingCreator.create(): Cannot convert mapping config of type " + abstractMappingConfig.GetType() + " to XYIncrementZOtherAxisMappingConfig.");
        }
        return new XYIncrementZOtherAxisMapping(abstractMappingConfig, filterFactory, rb);
    }
}

[Serializable]
public class XYIncrementZOtherAxisMappingConfig : AbstractMappingConfig
{
    public float DefaultDesiredX = 0.0f;
    public float DefaultDesiredY = 0.0f;
    public float DefaultDesiredZ = 0.0f;
    public float DefaultDesiredOtherAxis = 0.0f;
    public FilterConfig filterX = new FilterConfig();
    public FilterConfig filterY = new FilterConfig();
    public FilterConfig filterZ = new FilterConfig();
}
public class XYIncrementZOtherAxisMapping : AbstractMapping
{
    // XYIncrementZOtherAxisMappingConfig property
    private XYIncrementZOtherAxisMappingConfig XYIncrementZOtherAxisMappingConfig => this.AbstractMappingConfig as XYIncrementZOtherAxisMappingConfig;

    // XYIncrementZOtherAxisMapping-specific members
    public Vector3 PositionDesired { get; private set; }
    public float OtherAxisDesired { get; private set; }
    public Vector3 DefaultPositionDesired { get; private set; }
    public float DefaultOtherAxisDesired { get; private set; }

    // Filter members
    private AbstractFilter FilterX { get; set; }
    private AbstractFilter FilterY { get; set; }
    private AbstractFilter FilterZ { get; set; }

    // Logger members
    private int PositionDesiredXLoggerIdx, PositionDesiredYLoggerIdx, PositionDesiredZLoggerIdx, 
        OtherAxisDesiredLoggerIdx;
        
    public XYIncrementZOtherAxisMapping(AbstractMappingConfig abstractMappingConfig, FilterFactory filterFactory, Rigidbody rb) : base(rb)
    {
        this.AbstractMappingConfig = abstractMappingConfig;
        this.PositionDesired = new Vector3(XYIncrementZOtherAxisMappingConfig.DefaultDesiredX, XYIncrementZOtherAxisMappingConfig.DefaultDesiredY, XYIncrementZOtherAxisMappingConfig.DefaultDesiredZ);
        this.OtherAxisDesired = XYIncrementZOtherAxisMappingConfig.DefaultDesiredOtherAxis;
        this.DefaultPositionDesired = this.PositionDesired;
        this.DefaultOtherAxisDesired = this.OtherAxisDesired;
        this.FilterX = filterFactory.Build(XYIncrementZOtherAxisMappingConfig.filterX, rb);
        this.FilterY = filterFactory.Build(XYIncrementZOtherAxisMappingConfig.filterY, rb);
        this.FilterZ = filterFactory.Build(XYIncrementZOtherAxisMappingConfig.filterZ, rb);
        
        this.PositionDesiredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredX");
        this.PositionDesiredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredY");
        this.PositionDesiredZLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "PositionDesiredZ");
        this.OtherAxisDesiredLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "OtherAxisDesired");

        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.PositionDesiredKey, this.DefaultPositionDesired);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.DefaultValuesKey, Logger.Utilities.OtherAxisDesiredKey, this.DefaultOtherAxisDesired);
    }

    // Define objectives
    public override void ReadMapping()
    {
        base.ReadMapping();
        // Define desired position + attitude
        if (BrunnerHandle.Instance.GetReturnBrunner())
        {
            Vector3 positionDesired = new Vector3
            {
                x = BrunnerHandle.Instance.GetX(),
                y = BrunnerHandle.Instance.GetY()
            };
            positionDesired.z = this.PositionDesired.z -Parameters.JoystickToPosition * (Convert.ToSingle(BrunnerHandle.Instance.GetHatUp()) - Convert.ToSingle(BrunnerHandle.Instance.GetHatDown()));
            this.PositionDesired = positionDesired;
            this.OtherAxisDesired += -Parameters.JoystickToPosition * (Convert.ToSingle(BrunnerHandle.Instance.GetHatRight()) - Convert.ToSingle(BrunnerHandle.Instance.GetHatLeft()));
            Logger.Instance.AddEntry(this.PositionDesiredXLoggerIdx, this.PositionDesired.x);
            Logger.Instance.AddEntry(this.PositionDesiredYLoggerIdx, this.PositionDesired.y);
            Logger.Instance.AddEntry(this.PositionDesiredZLoggerIdx, this.PositionDesired.z);
            Logger.Instance.AddEntry(this.OtherAxisDesiredLoggerIdx, this.OtherAxisDesired);
            if (!this.ConnectedToBrunner)
            {
                Debug.Log(
                        "<color=Blue>Info: </color> " + this.GetType() + ".XYIncrementZOtherAxisMapping(): using desired position value from sidestick inputs again. ");
                this.ConnectedToBrunner = true;
            }
        }
        else if (this.ConnectedToBrunner)
        {
            Debug.LogWarning(
                    "<color=Orange>Warning: </color> " + this.GetType() + ".XYIncrementZOtherAxisMapping(): could not read sidestick inputs. Using default desired position value. ");
            this.ConnectedToBrunner = false;
        }
    }

    // Filter objectives
    public override void FilterMapping()
    {
        this.PositionDesired = new Vector3(this.FilterX.Filter(this.PositionDesired[0]), this.FilterY.Filter(this.PositionDesired[1]), this.FilterZ.Filter(this.PositionDesired[2]));
    }
}