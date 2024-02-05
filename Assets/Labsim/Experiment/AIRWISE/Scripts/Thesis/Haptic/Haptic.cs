using System;
using UnityEngine;

public class HapticFactory
{
    private HapticCreator m_creator;
    private AbstractHapticConfig m_abstractConfig;
    public AbstractHaptic Build(HapticConfig config, Rigidbody rb, Manager manager)
    {
        string hapticConfigFile = Constants.streamingAssetsPath + Constants.HapticConfigFile;
        System.IO.FileInfo fi = new System.IO.FileInfo(hapticConfigFile);
        // Check if file is there  
        if (!fi.Exists)
        {
            Debug.LogError("<color=Red>Error: </color> " + this.GetType() + ".Build(): Cannot find haptic config file " + Constants.HapticConfigFile);
        }

        switch ((AbstractHaptic.HapticModes)config.mode)
        {
            case AbstractHaptic.HapticModes.velocityTrim:
                m_creator = new VelocityTrimHapticCreator((AbstractHaptic.HapticAxisModes)config.axisMode);
                m_abstractConfig = Utilities.Read<VelocityTrimHapticConfig>(hapticConfigFile);
                break;
            case AbstractHaptic.HapticModes.coulomb:
                m_creator = new CoulombHapticCreator((AbstractHaptic.HapticAxisModes)config.axisMode);
                m_abstractConfig = Utilities.Read<CoulombHapticConfig>(hapticConfigFile);
                break;
            case AbstractHaptic.HapticModes.fajen:
                m_creator = new FajenHapticCreator((AbstractHaptic.HapticAxisModes)config.axisMode);
                m_abstractConfig = Utilities.Read<FajenHapticConfig>(hapticConfigFile);
                break;
            case AbstractHaptic.HapticModes.undefined:
            default:
                m_creator = new NoHapticCreator((AbstractHaptic.HapticAxisModes)config.axisMode);
                m_abstractConfig = Utilities.Read<NoHapticConfig>(hapticConfigFile);
                break;
        }
        return m_creator.Create(m_abstractConfig, rb, manager);
    }
}

abstract class HapticCreator
{
    public AbstractHaptic.HapticAxisModes axisMode;
    public HapticCreator(AbstractHaptic.HapticAxisModes axisMode)
    {
        this.axisMode = axisMode;
    }
    public abstract AbstractHaptic Create(AbstractHapticConfig config, Rigidbody rb, Manager manager);
}


[Serializable]
public class HapticConfig
{
    public int mode;
    public int axisMode;
}

[Serializable]
public class AbstractHapticConfig {
    public float trimX0, trimY0;

    public UInt16 Lat0 = 20, Lat1 = 200, Lat2 = 400, Lat3 = 600, Lat4 = 800, Lat5 = 1000, Lat6 = 1200, Lat7 = 1400, Lat8 = 1600;
    public UInt16 Longi0 = 20, Longi1 = 200, Longi2 = 400, Longi3 = 600, Longi4 = 800, Longi5 = 1000, Longi6 = 1200, Longi7 = 1400, Longi8 = 1600;

    public UInt16 X0, X1, X2, X3, X4, X5, X6, X7, X8;
    public UInt16 Y0, Y1, Y2, Y3, Y4, Y5, Y6, Y7, Y8;
 }

public abstract class AbstractHaptic
{
    public enum HapticModes
    {
        undefined = -1,
        velocityTrim = 0,
        coulomb = 1,
        fajen = 2,
    };
    public enum HapticAxisModes
    {
        undefined = -1,
        latLongi = 0,
        xY = 1
    };
    public HapticModes m_mode = HapticModes.undefined;
    public HapticAxisModes m_axisMode = HapticAxisModes.undefined;
    protected AbstractHapticConfig AbstractHapticConfig { get; private set; } = null;
    protected float mass;

    // Rigidbody-related members
    protected Rigidbody m_rb;

    // Haptic-related members
    protected int mode;
    protected int axisMode;

    // Default trim position
    public float TrimX, TrimY;

    // Default force profile values
    protected UInt16[] profileLat, profileLongi, profileX, profileY;
    
    // Measured position    
    protected float MeasuredX, MeasuredY;

    // Measured forces
    protected float MeasuredForceX, MeasuredForceY;
    protected int MeasuredXLoggerIdx, MeasuredYLoggerIdx, MeasuredForceXLoggerIdx, MeasuredForceYLoggerIdx, 
        ForceXToSendLoggerIdx, ForceYToSendLoggerIdx,
        ForceProfileLat0LoggerIdx, ForceProfileLat1LoggerIdx, ForceProfileLat2LoggerIdx, ForceProfileLat3LoggerIdx, ForceProfileLat4LoggerIdx, ForceProfileLat5LoggerIdx, ForceProfileLat6LoggerIdx, ForceProfileLat7LoggerIdx, ForceProfileLat8LoggerIdx, 
        ForceProfileLongi0LoggerIdx, ForceProfileLongi1LoggerIdx, ForceProfileLongi2LoggerIdx, ForceProfileLongi3LoggerIdx, ForceProfileLongi4LoggerIdx, ForceProfileLongi5LoggerIdx, ForceProfileLongi6LoggerIdx, ForceProfileLongi7LoggerIdx, ForceProfileLongi8LoggerIdx, 
        ForceProfileX0LoggerIdx, ForceProfileX1LoggerIdx, ForceProfileX2LoggerIdx, ForceProfileX3LoggerIdx, ForceProfileX4LoggerIdx, ForceProfileX5LoggerIdx, ForceProfileX6LoggerIdx, ForceProfileX7LoggerIdx, ForceProfileX8LoggerIdx, 
        ForceProfileY0LoggerIdx, ForceProfileY1LoggerIdx, ForceProfileY2LoggerIdx, ForceProfileY3LoggerIdx, ForceProfileY4LoggerIdx, ForceProfileY5LoggerIdx, ForceProfileY6LoggerIdx, ForceProfileY7LoggerIdx, ForceProfileY8LoggerIdx;

    public AbstractHaptic(AbstractHapticConfig abstractHapticConfig, Rigidbody rb, Manager manager)
    {
        this.m_rb = rb;
        this.AbstractHapticConfig = abstractHapticConfig;
        
        this.TrimX = abstractHapticConfig.trimX0;
        this.TrimY = abstractHapticConfig.trimY0;
        this.MeasuredX = BrunnerHandle.Instance.GetX();
        this.MeasuredY = BrunnerHandle.Instance.GetY();

        this.profileLat = new UInt16[9];
        this.profileLongi = new UInt16[9];
        this.profileX = new UInt16[9];
        this.profileY = new UInt16[9];

        this.profileLat[0] = abstractHapticConfig.Lat0;
        this.profileLat[1] = abstractHapticConfig.Lat1;
        this.profileLat[2] = abstractHapticConfig.Lat2;
        this.profileLat[3] = abstractHapticConfig.Lat3;
        this.profileLat[4] = abstractHapticConfig.Lat4;
        this.profileLat[5] = abstractHapticConfig.Lat5;
        this.profileLat[6] = abstractHapticConfig.Lat6;
        this.profileLat[7] = abstractHapticConfig.Lat7;
        this.profileLat[8] = abstractHapticConfig.Lat8;
        this.profileLongi[0] = abstractHapticConfig.Longi0;
        this.profileLongi[1] = abstractHapticConfig.Longi1;
        this.profileLongi[2] = abstractHapticConfig.Longi2;
        this.profileLongi[3] = abstractHapticConfig.Longi3;
        this.profileLongi[4] = abstractHapticConfig.Longi4;
        this.profileLongi[5] = abstractHapticConfig.Longi5;
        this.profileLongi[6] = abstractHapticConfig.Longi6;
        this.profileLongi[7] = abstractHapticConfig.Longi7;
        this.profileLongi[8] = abstractHapticConfig.Longi8;
        
        this.profileX = this.profileLongi;
        this.profileY = this.profileLat;

        this.MeasuredXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "MeasuredX");
        this.MeasuredYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "MeasuredY");

        this.MeasuredForceXLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "MeasuredForceX");
        this.MeasuredForceYLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "MeasuredForceY");

        this.ForceXToSendLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceXToSendLoggerIdx");
        this.ForceYToSendLoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceYToSendLoggerIdx");

        this.ForceProfileLat0LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat0");
        this.ForceProfileLat1LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat1");
        this.ForceProfileLat2LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat2");
        this.ForceProfileLat3LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat3");
        this.ForceProfileLat4LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat4");
        this.ForceProfileLat5LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat5");
        this.ForceProfileLat6LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat6");
        this.ForceProfileLat7LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat7");
        this.ForceProfileLat8LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLat8");

        this.ForceProfileLongi0LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi0");
        this.ForceProfileLongi1LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi1");
        this.ForceProfileLongi2LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi2");
        this.ForceProfileLongi3LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi3");
        this.ForceProfileLongi4LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi4");
        this.ForceProfileLongi5LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi5");
        this.ForceProfileLongi6LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi6");
        this.ForceProfileLongi7LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi7");
        this.ForceProfileLongi8LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileLongi8");
        
        this.ForceProfileX0LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX0");
        this.ForceProfileX1LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX1");
        this.ForceProfileX2LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX2");
        this.ForceProfileX3LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX3");
        this.ForceProfileX4LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX4");
        this.ForceProfileX5LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX5");
        this.ForceProfileX6LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX6");
        this.ForceProfileX7LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX7");
        this.ForceProfileX8LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileX8");
        
        this.ForceProfileY0LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY0");
        this.ForceProfileY1LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY1");
        this.ForceProfileY2LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY2");
        this.ForceProfileY3LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY3");
        this.ForceProfileY4LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY4");
        this.ForceProfileY5LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY5");
        this.ForceProfileY6LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY6");
        this.ForceProfileY7LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY7");
        this.ForceProfileY8LoggerIdx = Logger.Instance.GetEntry(this.GetType() + Logger.Instance.GetTextSep() + "ForceProfileY8");
    }

    // Set default trim position
    private void SetInitialTrimPosition(){
        BrunnerHandle.Instance.WriteTrimPositionXY(this.TrimX, this.TrimY);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.TrimX0Key, this.TrimX);
        Logger.Instance.AddTrialConfigEntry(Logger.Utilities.InitialConditionsKey, Logger.Utilities.TrimY0Key, this.TrimY);
    }

    // Set default force profile
    private void SetInitialForceProfile(){
        BrunnerHandle.Instance.WriteForceProfile(
            this.profileLat[0], 
            this.profileLat[1], 
            this.profileLat[2], 
            this.profileLat[3], 
            this.profileLat[4], 
            this.profileLat[5], 
            this.profileLat[6], 
            this.profileLat[7], 
            this.profileLat[8], 
            this.profileLongi[0], 
            this.profileLongi[1], 
            this.profileLongi[2], 
            this.profileLongi[3], 
            this.profileLongi[4], 
            this.profileLongi[5], 
            this.profileLongi[6], 
            this.profileLongi[7], 
            this.profileLongi[8]);
        Logger.Instance.AddEntry(this.ForceProfileLat0LoggerIdx, this.profileLat[0].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat1LoggerIdx, this.profileLat[1].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat2LoggerIdx, this.profileLat[2].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat3LoggerIdx, this.profileLat[3].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat4LoggerIdx, this.profileLat[4].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat5LoggerIdx, this.profileLat[5].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat6LoggerIdx, this.profileLat[6].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat7LoggerIdx, this.profileLat[7].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLat8LoggerIdx, this.profileLat[8].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi0LoggerIdx, this.profileLongi[0].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi1LoggerIdx, this.profileLongi[1].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi2LoggerIdx, this.profileLongi[2].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi3LoggerIdx, this.profileLongi[3].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi4LoggerIdx, this.profileLongi[4].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi5LoggerIdx, this.profileLongi[5].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi6LoggerIdx, this.profileLongi[6].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi7LoggerIdx, this.profileLongi[7].ToString("#"));
        Logger.Instance.AddEntry(this.ForceProfileLongi8LoggerIdx, this.profileLongi[8].ToString("#"));
    }

    // Set haptic initial conditions
    public void SetHapticInitialConditions(){
        this.SetInitialTrimPosition();
        this.SetInitialForceProfile();
    }

    // Declare abstract methods to be implemented in child classes
    public virtual void FetchCriterion(float tElapsed) {
        this.MeasuredX = BrunnerHandle.Instance.GetX();
        this.MeasuredY = BrunnerHandle.Instance.GetY();
        this.MeasuredForceX = BrunnerHandle.Instance.GetForceLongitudinal();
        this.MeasuredForceY = -BrunnerHandle.Instance.GetForceLateral();

        Logger.Instance.AddEntry (this.MeasuredXLoggerIdx, this.MeasuredX);
        Logger.Instance.AddEntry (this.MeasuredYLoggerIdx, this.MeasuredY);
        Logger.Instance.AddEntry (this.MeasuredForceXLoggerIdx, this.MeasuredForceX);
        Logger.Instance.AddEntry (this.MeasuredForceYLoggerIdx, this.MeasuredForceY);
    }
    public abstract void ComputeForce();
    public abstract void Actuate(float elapsed);
}