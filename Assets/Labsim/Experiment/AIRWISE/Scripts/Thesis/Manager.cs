using System;
using UnityEngine;

public class Manager
{
    // MonoBehaviour-related members
    public QuadController QuadController { get; set; }
    private Config Config  { get; set; }

    // Logger-related members
    private LoggerConfig LoggerConfig  { get; set; }

    // // InitialConditions-related member
    // private InitialConditions InitialConditions { get; set; }

    // Forcing function-related member
    private ForcingFunctionConfig ForcingFunctionConfig  { get; set; }
    private ForcingFunctionFactory ForcingFunctionFactory  { get; set; }
    private AbstractForcingFunction ForcingFunction { get; set; }

    // Mapping-related members
    private MappingConfig MappingConfig  { get; set; }
    private MappingFactory MappingFactory  { get; set; }
    public AbstractMapping Mapping { get; set; }

    // Filter-related members
    private FilterConfig FilterConfig  { get; set; }
    private FilterFactory FilterFactory  { get; set; }
    private AbstractFilter Filter { get; set; }

    // Control-related members
    private ControlConfig ControlConfig  { get; set; }
    private ControlFactory ControlFactory  { get; set; }
    public AbstractControl Control { get; set; }

    // Actuation-related members
    private ActuationConfig ActuationConfig  { get; set; }
    private ActuationFactory ActuationFactory  { get; set; }
    public AbstractActuation Actuation { get; set; }

    // Haptic-related members
    private HapticConfig HapticConfig  { get; set; }
    private HapticFactory HapticFactory  { get; set; }
    private AbstractHaptic Haptic { get; set; }

    // Display-related member
    private ErrorDisplayConfig ErrorDisplayConfig  { get; set; }
    private ErrorDisplayFactory ErrorDisplayFactory  { get; set; }
    private AbstractErrorDisplay ErrorDisplay { get; set; }

    private TimeSpan m_currElapsed;
    private float m_tElapsed, m_tFinal, m_arrivalRadius;
    private int m_nbTrials, m_currTrial;


    #region singleton pattern
    private static readonly System.Lazy<Manager> _lazyManager
        = new System.Lazy<Manager>(
            () =>
            {
                var manager = new Manager();
                return manager;
            }
        );

    public static Manager Instance => _lazyManager.Value;

    private Manager() { }
    ~Manager()
    {
        this.Dispose();
    }

    public void Dispose()
    {
        this.SetDisposeConditions();
    }

    #endregion 
    

    public void Instantiate(QuadController mb, TimeSpan elapsed)
    {
        UnityEngine.Debug.Log(Constants.ConfigFilePath);
        // Initiate Controller variable
        this.QuadController = mb;
        UnityEngine.Debug.Log("Manager.Instantiate()" + Manager.Instance.QuadController.Rb);

        this.m_currElapsed = elapsed;
        this.m_tElapsed = Utilities.FromTimeSpanToFloatElapsed(this.m_currElapsed);
        // First Initialization upon QuadController Awake
        this.m_currTrial = 0;
    }

    public void Initialize(Rigidbody rb, TimeSpan elapsed, DateTime timestamp, Rotor[] rotors)
    {
        UnityEngine.Debug.Log("Manager.Initialize()" + rb);
        // Initialize inner members
        this.m_currElapsed = elapsed;
        this.m_tElapsed = Utilities.FromTimeSpanToFloatElapsed(this.m_currElapsed);
        this.m_currTrial += 1;
        UnityEngine.Debug.Log("manager.Initialize()" + Manager.Instance.GetCurrTrial());

        // Build manager
        this.Build(timestamp, rb, rotors);

        // Set initial conditions
        this.SetInitialConditions();
            Debug.Log(
                "<color=blue>Info: </color> " + this.GetType() + ".Initialize(): Entering block " + this.m_currTrial.ToString("#") + "."
            );
        if (this.DuringTrial()) {
        }

        // Initialize logger
        Logger.Instance.Initialize();
    }

    private void Build(DateTime timestamp, Rigidbody rb, Rotor[] rotors) 
    {
        UnityEngine.Debug.Log("Manager.Build()" + rb);

        this.BuildConfig();

        // Initiate logger
        Logger.Instance.Configure(this.LoggerConfig, timestamp, this.m_currElapsed, rb);
        
        // Define initial conditions
        // this.InitialConditions = this.Config.InitialConditions;

        // Initiate forcing function
        this.ForcingFunction = this.ForcingFunctionFactory.Build(this.ForcingFunctionConfig);

        // Initiate sidestick to objectives mapping
        this.Mapping = this.MappingFactory.Build(this.MappingConfig, this.FilterFactory, rb);

        // Initiate control law
        this.Control = this.ControlFactory.Build(this.ControlConfig, this.Mapping, rb);

        // Initiate actuation
        this.Actuation = this.ActuationFactory.Build(this.ActuationConfig, this.FilterFactory, this.Control, rb, rotors);

        // Initiate haptic
        this.Haptic = this.HapticFactory.Build(this.HapticConfig, rb, this);

        // Define error display
        this.ErrorDisplay = this.ErrorDisplayFactory.Build(this.ErrorDisplayConfig, this.ForcingFunction, rb);

        // Log built configuration
        Logger.Instance.BuildConfig(
            this.ForcingFunction.GetType(),
            this.Mapping.GetType(),
            this.Control.GetType(),
            this.Actuation.GetType(),
            this.Haptic.GetType(),
            this.ErrorDisplay.GetType()
        );
    }

    public void BuildConfig()
    {        
        // Initiate config variable
        this.Config = Utilities.Read<Config>(Constants.ConfigFilePath);
        
        this.m_tFinal = this.Config.tFinal;
        this.m_nbTrials = this.Config.nbTrials;
        this.m_arrivalRadius = this.Config.arrivalRadius;

        this.LoggerConfig = this.Config.LoggerConfig;
        this.ForcingFunctionConfig = this.Config.ForcingFunctionConfig;
        this.MappingConfig = this.Config.MappingConfig;
        this.ControlConfig = this.Config.ControlConfig;
        this.ActuationConfig = this.Config.ActuationConfig;
        this.HapticConfig = this.Config.HapticConfig;
        this.ErrorDisplayConfig = this.Config.ErrorDisplayConfig;

        // Initiate factories
        this.ForcingFunctionFactory = new ForcingFunctionFactory();
        this.MappingFactory = new MappingFactory();
        this.FilterFactory = new FilterFactory();
        this.ControlFactory = new ControlFactory();
        this.ActuationFactory = new ActuationFactory();
        this.HapticFactory = new HapticFactory();
        this.ErrorDisplayFactory = new ErrorDisplayFactory();
    }

    public void Reset()
    {
        UnityEngine.Debug.Log("Manager.Reset()");
        // Reset logger
        Logger.Instance.Reset();

        // Reset conditions
        this.SetResetConditions();

        // Leave manager inner members left for GC to clean
    }

    // Define initial conditions
    private void SetInitialConditions()
    {
        // this.InitialConditions.SetInitialConditions(rb);
        this.Haptic.SetHapticInitialConditions();
        Logger.Instance.SetHeaders();
    }

    // Define reset conditions
    private void SetResetConditions()
    {
        // this.InitialConditions.SetResetConditions(rb);
        // N.B.: Haptic reset conditions = initial conditions as session loops over trials
        this.Haptic.SetHapticInitialConditions();
        Logger.Instance.Flush();
    }

    // Define dispose conditions
    private void SetDisposeConditions()
    {
        this.Haptic.SetHapticDisposeConditions();
    }

    // Compute routine: return false to stop, true to continue
    public bool Compute(TimeSpan elapsed)
    {
        this.m_currElapsed = elapsed;
        this.m_tElapsed = Utilities.FromTimeSpanToFloatElapsed(this.m_currElapsed);
        
        // // Pause simulation if criterion reached
        // if (this.PauseTrialOnButtonCondition())
        // {
        //     Debug.Log(
        //         "<color=blue>Info: </color> " + this.GetType() + ".Compute(): Pausing run on Button 1 click."
        //     );
        //     Debug.Break();
        //     return true; 
        // }

        // Bail out early when iteration criterion reached
        if (this.EndTrialOnIterationCountCondition() || this.EndTrialOnFinalTimeCondition() || this.EndTrialOnTaskEndCondition()) { 
            return true; 
        };

        // Update elapsed time-based members
        Logger.Instance.SaveElapsed(elapsed);
        this.ForcingFunction.Compute(this.m_tElapsed);

        // Update Brunner input-based members 
        this.Mapping.ReadMapping();
        this.Mapping.FilterMapping();
        if (this.EndTrialOnButtonCondition()) { return true; };
        
        this.Control.FetchMapping();
        this.Control.Compute();
        
        this.Actuation.ComputeActuation();
        this.Actuation.FilterActuation();
        this.Actuation.HandlePropellers();
        
        this.Haptic.FetchCriterion(this.m_tElapsed);
        this.Haptic.ComputeForce();
        this.Haptic.Actuate(this.m_tElapsed);
        
        this.ErrorDisplay.ComputeError();

        return false;
    }

    public void Display()
    {
        this.ErrorDisplay.Move();
        this.ErrorDisplay.Display();
    }
    
    // Compute simulation pause condition: return true to pause, false to continue
    private bool PauseTrialOnButtonCondition() { return BrunnerHandle.Instance.GetButton1(); }

    // Compute simulation end condition: return true to stop, false to continue
    // TODO: vérifier la condition d'itérations et la durée
    public bool EndTrialOnIterationCountCondition() { return this.m_currTrial > this.m_nbTrials - 1; }
    public bool EndTrialOnFinalTimeCondition() { return this.m_tElapsed - Time.fixedDeltaTime >= this.m_tFinal; }
    public bool EndTrialOnButtonCondition() { BrunnerHandle.Instance.ExecuteOneStep(); return BrunnerHandle.Instance.GetButton2(); }
    public bool EndTrialOnTaskEndCondition() { return this.GetDistanceToArrival() <= this.m_arrivalRadius; }
    public float GetDistanceToArrival() { return float.PositiveInfinity; /*Utilities.Distance(AeroFrame.ProjectOnHorizontalplane(AeroFrame.GetPosition(this.QuadController.ArrivalRb)), AeroFrame.ProjectOnHorizontalplane(AeroFrame.GetPosition(this.QuadController.Rb)))*/; }
    public int GetCurrTrial() { return this.m_currTrial; }
    // public void SetCurrTrial(int currTrial) { this.m_currTrial = currTrial; }
    public float GetElapsedTime() { return this.m_tElapsed; }
    public float GetCorrectedElapsedTime() { return this.m_tElapsed - Time.fixedDeltaTime; }
    public void SetElapsedTime(float tElapsed) { this.m_tElapsed = tElapsed; }
    public bool DuringTrial() { return (this.m_currTrial >= 0 /*&& this.m_currTrial < this.m_nbTrials*/); }
}


[Serializable]
public class Config
{
    public float tFinal;
    public int nbTrials;
    public float arrivalRadius;
    public ForcingFunctionConfig ForcingFunctionConfig = new ForcingFunctionConfig();
    public MappingConfig MappingConfig = new MappingConfig();
    public ControlConfig ControlConfig = new ControlConfig();
    public ActuationConfig ActuationConfig = new ActuationConfig();
    public HapticConfig HapticConfig = new HapticConfig();
    public ErrorDisplayConfig ErrorDisplayConfig = new ErrorDisplayConfig();
    public LoggerConfig LoggerConfig = new LoggerConfig();
    // public InitialConditions InitialConditions = new InitialConditions();
}

// [Serializable]
// public class InitialConditions
// {
//     public Vector3 Position0 = new Vector3();
//     public Vector3 Attitude0 = new Vector3();
//     public Vector3 Velocity0 = new Vector3();
//     public Vector3 AngularVelocity0 = new Vector3();

//     public System.Collections.Generic.List<string> loggers = new System.Collections.Generic.List<string>();
//     public InitialConditions() { }

//     public void SetInitialConditions(Rigidbody rb) {
//         AeroFrame.SetPosition(rb, this.Position0);
//         AeroFrame.SetAngles(rb, this.Attitude0);
//         AeroFrame.SetAbsoluteVelocity(rb, this.Velocity0);
//         AeroFrame.SetAngularVelocity(rb, this.AngularVelocity0);
//         AeroFrame.ApplyRelativeForce(rb, new Vector3());
//         AeroFrame.ApplyRelativeTorque(rb, new Vector3());
//     }

//     public void SetResetConditions(Rigidbody rb) {
//         AeroFrame.SetPosition(rb, this.Position0);
//         AeroFrame.SetAngles(rb, this.Attitude0);
//         AeroFrame.SetAbsoluteVelocity(rb, this.Velocity0);
//         AeroFrame.SetAngularVelocity(rb, this.AngularVelocity0);
//         rb.constraints = UnityEngine.RigidbodyConstraints.None;
//         AeroFrame.ApplyRelativeForce(rb, new Vector3());
//         AeroFrame.ApplyRelativeTorque(rb, new Vector3());
//     }
// }