using System;
using UnityEngine;

public class QuadController : MonoBehaviour
{
    private DateTime _timestamp = DateTime.Now;

    // Mechanical members
    private Rigidbody m_rb 
        = new();
    public Rigidbody Rb { get { return this.m_rb; } set { this.m_rb = value; } }

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initPos 
        = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitPos 
        => this.m_initPos;

    [UnityEngine.SerializeField]
    private UnityEngine.Vector3 m_initRot 
        = new(0.0f,0.0f,0.0f);
    public UnityEngine.Vector3 InitRot 
        => this.m_initRot;
    
    private Rotor[] Rotors { get; set; }
    private Vector3[] HitPts { get; set; }

    private float[] _distances;
    private const int _nbRaycast = 4;

    public Vector3[] RayDirections = new Vector3[_nbRaycast] { Constants.e1, Constants.e2, -Constants.e1, -Constants.e2 };
    public Color[] Colors = new Color[_nbRaycast] { Color.red, Color.yellow, Color.green, Color.blue };

    public int NbRaycast => _nbRaycast;

    [SerializeField]
    private Rigidbody m_arrivalRb = null;
    public Rigidbody ArrivalRb => this.m_arrivalRb;

    private bool m_bInhibit = false;
    public bool Inhibit { get { return this.m_bInhibit; } set{ this.m_bInhibit = value; } }

    // Unity workflow-based methods

    private void Awake()
    {
        this.Instantiate();
    }
    private void Start() { }

    private void FixedUpdate()
    {
        if(this.Inhibit)
        {
            return;
        }

        // UnityEditor.EditorApplication.pauseStateChanged += RestartChronoAfterPause;
        // Stop chrono to current time if Manager returned simulation stop condition
        Manager.Instance.Compute();
        Logger.Instance.FlushBuffer();
        Logger.Instance.FlushTrialConfigBuffer();

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

        // Pass current position to child rotor
        foreach (var rotor in this.Rotors) 
        {
            rotor.ComputeInitPosRot(this.InitPos, this.InitRot);
        }
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
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current translational velocity: " + AeroFrame.GetRelativeVelocity(this.Rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.Rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((PositionMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.Rb)[2]);

                    if (Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude increment: " + -((PositionMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.Rb)[2]);
                    }
                    else if (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired pitch: " + -((PositionMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current pitch: " + AeroFrame.GetAngles(this.Rb)[1]);
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
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current translational velocity: " + AeroFrame.GetRelativeVelocity(this.Rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.Rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.Rb)[2]);

                    if (Manager.Instance.Control.GetType() == typeof(DirectAltitudeSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude rate increment: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.Rb)[2]);
                    }
                    else if (Manager.Instance.Control.GetType() == typeof(DirectPitchSingleIntegratorVelocityDirectYaw))
                    {
                        GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired pitch rate: " + -((XYIncrementZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.z);
                        GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current pitch: " + AeroFrame.GetAngles(this.Rb)[1]);
                    }

                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            } 
            catch (InvalidCastException) { }
        }
        //MODIF_QUENTIN_19/02
         else if (Manager.Instance.Mapping.GetType() == typeof(IncrementXYZOtherAxisMapping)
            && ((Manager.Instance.Control.GetType() == typeof(DirectAltitudeDoubleIntegratorPositionDirectYaw))))
            { try {
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired position: " + ((IncrementXYZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.x + ", " +((IncrementXYZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.y);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current position increment: " + AeroFrame.GetRelativeVelocity(this.Rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.Rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((IncrementXYZOtherAxisMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.Rb)[2]);

                    
                    GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude rate: " + ((IncrementXYZOtherAxisMapping)Manager.Instance.Mapping).PositionDesired.z);
                    GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.Rb)[2]);
                    
                    
                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            } 
            catch (InvalidCastException) { }
        }
        
        else if(Manager.Instance.Mapping.GetType() == typeof(PositionMapping) 
        && ((Manager.Instance.Control.GetType() == typeof(PositionYawControl))))
        {
            try{
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired translational velocity: " + ((PositionMapping)Manager.Instance.Mapping).PositionDesired.x + ", " +((PositionMapping)Manager.Instance.Mapping).PositionDesired.y);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Current translational velocity: " + AeroFrame.GetRelativeVelocity(this.Rb)[0] + ", " + AeroFrame.GetRelativeVelocity(this.Rb)[1]);

                    GUI.Label(new Rect(0, 5 * d + 2 * dSpace, dWidth, d), "Desired heading: " + -((PositionMapping)Manager.Instance.Mapping).OtherAxisDesired);
                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.Rb)[2]);

                    
                    GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired altitude rate increment: " + -((PositionMapping)Manager.Instance.Mapping).PositionDesired.z);
                    GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.Rb)[2]);
                    
                    
                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);

            }
            catch (InvalidCastException) { }
        }


//FIN MODIF

        else if (Manager.Instance.Mapping.GetType() == typeof(AltitudeTranslationalVelocityMapping))
        {
            try {
                    GUI.Label(new Rect(0, 3 * d + dSpace, dWidth, d), "Desired unfiltered velocity: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).VelocityDesiredUnfiltered);
                    GUI.Label(new Rect(0, 4 * d + dSpace, dWidth, d), "Desired filtered velocity: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).VelocityDesired);
                    GUI.Label(new Rect(0, 5 * d + dSpace, dWidth, d), "Current velocity: " + AeroFrame.GetAbsoluteVelocity(this.Rb));

                    GUI.Label(new Rect(0, 6 * d + 2 * dSpace, dWidth, d), "Desired unfiltered altitude: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).AltitudeDesiredUnfiltered);
                    GUI.Label(new Rect(0, 7 * d + 2 * dSpace, dWidth, d), "Desired filtered altitude: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).AltitudeDesired);
                    GUI.Label(new Rect(0, 8 * d + 2 * dSpace, dWidth, d), "Current altitude: " + AeroFrame.GetPosition(this.Rb)[2]);

                    GUI.Label(new Rect(0, 9 * d + 4 * dSpace, dWidth, d), "Desired unfiltered heading: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).YawDesiredUnfiltered);
                    GUI.Label(new Rect(0, 10 * d + 4 * dSpace, dWidth, d), "Desired filtered heading: " + ((AltitudeTranslationalVelocityMapping)Manager.Instance.Mapping).YawDesired);
                    GUI.Label(new Rect(0, 11 * d + 4 * dSpace, dWidth, d), "Current heading: " + AeroFrame.GetAngles(this.Rb)[2]);

                    GUI.Label(new Rect(0, 12 * d + 5 * dSpace, dWidth, d), "Force: " + Manager.Instance.Control.Order);
            }
            catch (InvalidCastException) { }
        }
    }

    // Home-made methods

    private void Instantiate() 
    {
        // Instantiate mechanical control elements

        this.Rb = this.gameObject.GetComponent<Rigidbody>();
        this.HitPts = new Vector3[_nbRaycast] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };
        this._distances = new float[_nbRaycast] { Parameters.DistMax, Parameters.DistMax, Parameters.DistMax, Parameters.DistMax };
        this.Rotors = this.transform.parent.GetComponentsInChildren<Rotor>();

        
        // Build manager
        Manager.Instance.Instantiate(this);
    }

    private void Initialize() 
    {
        
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

        // Initialize manager (hence, logger)
        Manager.Instance.Initialize(this.Rb, this._timestamp, this.Rotors);
    }

    private void Reset()
    {

        // Reset manager (hence, logger)
        Manager.Instance.Reset();
    }

    public void ResetRigidBody() 
    {
        if (this.Rb == null) { return; }
        AeroFrame.SetPosition(this.Rb, this.InitPos);
        AeroFrame.SetAngles(this.Rb, this.InitRot);
        AeroFrame.SetAbsoluteVelocity(this.Rb, Vector3.zero);
        AeroFrame.SetAngularVelocity(this.Rb, Vector3.zero);
        AeroFrame.ApplyRelativeForce(this.Rb, Vector3.zero);
        AeroFrame.ApplyRelativeTorque(this.Rb, Vector3.zero);
        
        foreach (var rotor in this.Rotors) {
            rotor.ResetRigidBody();
        }
    }
}