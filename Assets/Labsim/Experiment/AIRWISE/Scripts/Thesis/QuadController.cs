using System;
using UnityEngine;

public class QuadController : MonoBehaviour
{
    private System.Diagnostics.Stopwatch _chrono = System.Diagnostics.Stopwatch.StartNew();
    private DateTime _timestamp = DateTime.Now;

    // Mechanical members
    private Rigidbody m_rb;
    public Rigidbody Rb {
        get { return m_rb; }
    }
    private Rotor[] Rotors { get; set; }
    private Vector3[] HitPts { get; set; }

    private float[] _distances;
    private const int _nbRaycast = 4;

    public Vector3[] RayDirections = new Vector3[_nbRaycast] { Constants.e1, Constants.e2, -Constants.e1, -Constants.e2 };
    public Color[] Colors = new Color[_nbRaycast] { Color.red, Color.yellow, Color.green, Color.blue };

    public int NbRaycast => _nbRaycast;

    [SerializeField]
    private Rigidbody m_arrivalRb;
    public Rigidbody ArrivalRb {
        get { return m_arrivalRb; }
    }

    // Unity workflow-based methods

    private void Awake()
    {
        this.Instantiate();
    }
    private void Start() { 
        Logger.Instance.FlushTopLevelBuffer();

    }


    private void FixedUpdate()
    {        
        UnityEditor.EditorApplication.pauseStateChanged += RestartChronoAfterPause;
        // Stop chrono to current time if Manager returned simulation stop condition
        if (Manager.Instance.Compute(this._chrono.Elapsed)) {
            this._chrono.Stop();
        }
        Logger.Instance.FlushBuffer();
        Logger.Instance.FlushTrialConfigBuffer();
    }

    private void RestartChronoAfterPause(UnityEditor.PauseState state){
        if (state == UnityEditor.PauseState.Unpaused) {
            this._chrono.Restart();
        }
    }

    // TODO: FlushBuffer in LateUpdate to avoid flooding writing routine
    private void LateUpdate() { }

    private void Update()
    {
        Manager.Instance.Display();
    }

    // Event-based methods

    private void OnEnable()
    {
        // Initialize simulation state
        this.Initialize();
    }

    private void OnDisable()
    {
        // Reset simulation state (in preparation for new block)
        this.Reset();
    }

    private void OnGUI()
    {
        //TODO: horizon artificiel
        //TODO: cap numérique (cc HUD)
        //TODO: vitesse en compteur + vecteur vitesse
        GUI.color = Color.red;
        int d = 20;
        int dSpace = 20;
        int dWidth = 500;
        GUI.Label(new Rect(0, 0, dWidth, d), "Mapping mode: " + Manager.Instance.Mapping.GetType());
        GUI.Label(new Rect(0, d, dWidth, d), "Control mode: " + Manager.Instance.Control.GetType());
        GUI.Label(new Rect(0, 2 * d, dWidth, d), "Actuation mode: " + Manager.Instance.Actuation.GetType());

        if (Manager.Instance.Mapping.GetType() == typeof(PositionMapping)
            && ((Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw)) 
            || (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))))
        {
            try {
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired translational velocity: " + ((PositionMapping)Manager.Instance.Mapping).PositionDesired.x + ", " +((PositionMapping)Manager.Instance.Mapping).PositionDesired.y);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current translational velocity: " + AeroFrame.GetRelativeVelocity(this.m_rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.m_rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((PositionMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.m_rb)[2]);

                    if (Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude increment: " + -((PositionMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.m_rb)[2]);
                    }
                    else if (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired pitch: " + -((PositionMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current pitch: " + AeroFrame.GetAngles(this.m_rb)[1]);
                    }

                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            } 
            catch (InvalidCastException) { }
        }
        else if (Manager.Instance.Mapping.GetType() == typeof(XYIncrementZOtherAxisMapping)
            && ((Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw)) 
            || (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))))
        {
            try {
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired translational velocity: " + ((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.x + ", " +((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.y);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current translational velocity: " + AeroFrame.GetRelativeVelocity(this.m_rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.m_rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.m_rb)[2]);

                    if (Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude rate increment: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.m_rb)[2]);
                    }
                    else if (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired pitch rate: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current pitch: " + AeroFrame.GetAngles(this.m_rb)[1]);
                    }

                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            } 
            catch (InvalidCastException) { }
        }
        else if (Manager.Instance.Mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
        {
            try {
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired unfiltered velocity: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).VelocityDesiredUnfiltered);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Desired filtered velocity: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).VelocityDesired);
                    GUI.Label(new Rect(0, 5 * d + dSpace, dWidth, d), "Current velocity: " + AeroFrame.GetAbsoluteVelocity(this.m_rb));

                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Desired unfiltered altitude: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).AltitudeDesiredUnfiltered);
                    GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired filtered altitude: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).AltitudeDesired);
                    GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.m_rb)[2]);

                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Desired unfiltered heading: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).YawDesiredUnfiltered);
                    GUI.Label(new Rect(0, 10 * d + 4 * dSpace, dWidth, d), "Desired filtered heading: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).YawDesired);
                    GUI.Label(new Rect(0, 11 * d + 4 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.m_rb)[2]);

                    GUI.Label(new Rect(0, 12 * d + 5 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            }
            catch (InvalidCastException) { }
        }
    }

    private void OnApplicationQuit()
    {
        BrunnerHandle.Instance.Dispose();
        Logger.Instance.Dispose();
    }

    // Home-made methods

    private void Instantiate() 
    {
        // Instantiate mechanical control elements

        // find all loaded tags gate
        foreach(var obj in UnityEngine.GameObject.FindGameObjectsWithTag("AIRWISETag_Arrival"))
        {
            
            // Only center
            if(obj.name != "Center")
            {
                continue;
            }

            // extract Arrival Rb
            this.m_arrivalRb = obj.GetComponent<Rigidbody>();

            // resume flow
            break;

        }/* foreach() */

        this.m_rb = this.gameObject.GetComponent<Rigidbody>();
        this.HitPts = new Vector3[_nbRaycast] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };
        this._distances = new float[_nbRaycast] { Parameters.DistMax, Parameters.DistMax, Parameters.DistMax, Parameters.DistMax };
        this.Rotors = this.transform.parent.GetComponentsInChildren<Rotor>();

        // Reset chrono
        this._chrono.Reset();
        this._chrono.Restart();
        
        // Re-build manager
        Manager.Instance.Instantiate(this, this._chrono.Elapsed, this._timestamp, this.m_rb, this.Rotors);
    }

    private void Initialize() 
    {
        // Reset chrono
        this._chrono.Reset();
        this._chrono.Restart();

        // Initialize manager
        Manager.Instance.Initialize(this.m_rb, this._chrono.Elapsed, this._timestamp, this.Rotors);
    }

    private void Reset()
    {
        // Reset chrono
        this._chrono.Reset();
        this._chrono.Restart();

        // Reset initial conditions
        Manager.Instance.Reset(this.m_rb, this._timestamp);
    }
}