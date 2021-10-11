
using UnityEngine.InputSystem;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBehaviour 
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject RawZAxisTracker = null; 
        [UnityEngine.SerializeField]
        public UnityEngine.GameObject FilteredZAxisTracker = null; 

        #region properties/members

        public UnityEngine.InputSystem.Users.InputUser User { private set; get; }
        public UnityEngine.InputSystem.InputDevice Device { private set; get; }
        public UnityEngine.InputSystem.InputControlScheme ControlScheme { private set; get; }
        public UnityEngine.InputSystem.InputActionMap ActionMap { private set; get; }

        public UnityEngine.InputSystem.Controls.AxisControl Z { private set; get; }
        public UnityEngine.InputSystem.Controls.ButtonControl Button15 { private set; get; }

        // Apollon bridge

        public ApollonHOTASWarthogThrottleSensorBridge Bridge { get; set; }

        // internal properties

        internal float Value { set; get; }
        internal float ValidationDuration { set; get; }
        internal float Threshold { set; get; }
        internal float UpperNeutralBound { set; get; }
        internal float LowerNeutralBound { set; get; }
        internal float PositiveBound { set; get; }
        internal float NegativeBound { set; get; }

        // private members

        private float
            m_settings_validation_duration,
            m_settings_validation_threshold_ratio,
            m_settings_neutral_value,
            m_settings_minimum_value,
            m_settings_maximum_value,
            m_settings_neutral_validation_point,
            m_settings_positive_validation_point,
            m_settings_negative_validation_point;

        private bool m_bHasInitialized = false;

        #endregion

        private void Initialize()
        {

            // get global session settings
            this.m_settings_validation_duration = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_validation_duration_ms");
            this.m_settings_validation_threshold_ratio = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_validation_threshold_ratio");
            this.m_settings_neutral_value = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_neutral_value");
            this.m_settings_minimum_value = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_push_stop_value");
            this.m_settings_maximum_value = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_pull_stop_value");
            this.m_settings_neutral_validation_point = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_neutral_validation_point");
            this.m_settings_positive_validation_point = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_positive_validation_point");
            this.m_settings_negative_validation_point = experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("axis_negative_validation_point");

            // init properties
            this.ValidationDuration = this.m_settings_validation_duration / 1000.0f;
            this.Threshold = System.Math.Abs((this.m_settings_maximum_value - this.m_settings_minimum_value) * this.m_settings_validation_threshold_ratio);
            this.UpperNeutralBound = this.m_settings_neutral_validation_point + this.Threshold / 2.0f;
            this.LowerNeutralBound = this.m_settings_neutral_validation_point - this.Threshold / 2.0f;
            this.PositiveBound = this.m_settings_positive_validation_point;
            this.NegativeBound = this.m_settings_negative_validation_point;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : initial settings :"
                + "\n - validation_duration_ms [" + this.m_settings_validation_duration + "]"
                + "\n - validation_threshold_ratio [" + this.m_settings_validation_threshold_ratio + "]"
                + "\n - neutral_value [" + this.m_settings_neutral_value + "]"
                + "\n - minimum_value [" + this.m_settings_minimum_value + "]"
                + "\n - maximum_value [" + this.m_settings_maximum_value + "]"
                + "\n - neutral_validation_point [" + this.m_settings_neutral_validation_point + "]"
                + "\n - positive_validation_point [" + this.m_settings_positive_validation_point + "]"
                + "\n - negative_validation_point [" + this.m_settings_negative_validation_point + "]"
                + "\n - ValidationDuration [" + this.ValidationDuration + "]"
                + "\n - Threshold [" + this.Threshold + "]"
                + "\n - UpperNeutralBound [" + this.UpperNeutralBound + "]"
                + "\n - LowerNeutralBound [" + this.LowerNeutralBound + "]"
                + "\n - PositiveBound [" + this.PositiveBound + "]"
                + "\n - NegativeBound [" + this.NegativeBound + "]"
            );

            // register impedance
            UnityEngine.InputSystem.InputSystem.RegisterInteraction<impedance.ApollonMaintainAtLeastInteraction>("MaintainAtLeast");
            UnityEngine.InputSystem.InputSystem.RegisterProcessor<impedance.ApollonDualNormalizeProcessor>("DualNormalize");

            // find HOTAS device
            foreach (var device in InputSystem.devices)
            {

                if (
                    device.name == "Thrustmaster Throttle - HOTAS Warthog"
                    && device.layout == "HID::Thrustmaster Throttle - HOTAS Warthog"
                )
                {

                    // configure device
                    this.Device = device;

                    // sync
                    if (!UnityEngine.InputSystem.InputSystem.TrySyncDevice(this.Device))
                    {

                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : fail to sync with HOTAS Throttle device."
                        );
                        return;

                    } /* if() */

                    // get control reference
                    this.Z = this.Device.TryGetChildControl("z") as UnityEngine.InputSystem.Controls.AxisControl;
                    this.Button15 = this.Device.TryGetChildControl("button15") as UnityEngine.InputSystem.Controls.ButtonControl;

                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : device & control found, sync OK"
                    );

                    // stop iteration
                    break;

                } /* if() */

            } /* foreach() */


            // instantiate action map & new control scheme
            this.ActionMap = new UnityEngine.InputSystem.InputActionMap("HOTASWarthog - Throttle");

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : action map instantiated"
            );

            // instantiate actions 

            this.ActionMap.AddAction(
               name: "ValueChanged",
               type: UnityEngine.InputSystem.InputActionType.Value
            ).AddBinding(
               this.Z
            ).WithProcessors(
                "DualNormalize(positive_min="
               + System.Math.Abs(this.m_settings_neutral_value)
               + ",positive_max="
               + this.m_settings_maximum_value
               + ",negative_min="
               + (-System.Math.Abs(this.m_settings_neutral_value))
               + ",negative_max="
               + this.m_settings_minimum_value
               + "),Invert()"
            );

            this.ActionMap.AddAction(
                name: "NeutralCommandTriggered",
                type: UnityEngine.InputSystem.InputActionType.Value
            ).AddBinding(
                this.Z
            ).WithInteraction(
                "MaintainAtLeast(duration="
                + this.ValidationDuration
                + ",threshold="
                + this.Threshold
                + ",min="
                + this.LowerNeutralBound
                + ",max="
                + this.UpperNeutralBound
                + ")"
            ).WithProcessors(
                "DualNormalize(positive_min="
                + System.Math.Abs(this.m_settings_neutral_value)
                + ",positive_max="
                + this.m_settings_maximum_value
                + ",negative_min="
                + (-System.Math.Abs(this.m_settings_neutral_value))
                + ",negative_max="
                + this.m_settings_minimum_value
                + "),AxisDeadzone(min="
                + (this.Threshold / 2.0f)
                + ",max="
                + (1.0f - this.Threshold)
                + "),Invert()"
            );

            this.ActionMap.AddAction(
                name: "PositiveCommandTriggered",
                type: UnityEngine.InputSystem.InputActionType.Value
            ).AddBinding(
                this.Z
            ).WithInteraction(
                "MaintainAtLeast(duration="
                + this.ValidationDuration
                + ",threshold="
                + this.Threshold
                + ",min="
                + this.PositiveBound
                + ",max="
                + 1.0f
                + ")"
            ).WithProcessors(
                "DualNormalize(positive_min="
                + System.Math.Abs(this.m_settings_neutral_value)
                + ",positive_max="
                + this.m_settings_maximum_value
                + ",negative_min="
                + (-System.Math.Abs(this.m_settings_neutral_value))
                + ",negative_max="
                + this.m_settings_minimum_value
                + "),AxisDeadzone(min="
                + (this.Threshold / 2.0f)
                + ",max="
                + (1.0f - this.Threshold)
                + "),Invert()"
            );

            this.ActionMap.AddAction(
                name: "NegativeCommandTriggered",
                type: UnityEngine.InputSystem.InputActionType.Value
            ).AddBinding(
                this.Z
            ).WithInteraction(
                "MaintainAtLeast(duration="
                + this.ValidationDuration
                + ",threshold="
                + this.Threshold
                + ",min="
                + -1.0f
                + ",max="
                + this.NegativeBound
                + ")"
            ).WithProcessors(
                "DualNormalize(positive_min="
                + System.Math.Abs(this.m_settings_neutral_value)
                + ",positive_max="
                + this.m_settings_maximum_value
                + ",negative_min="
                + (-System.Math.Abs(this.m_settings_neutral_value))
                + ",negative_max="
                + this.m_settings_minimum_value
                + "),AxisDeadzone(min="
                + (this.Threshold / 2.0f)
                + ",max="
                + (1.0f - this.Threshold)
                + "),Invert()"
            );

            this.ActionMap.AddAction(
                name: "UserResponse",
                type: UnityEngine.InputSystem.InputActionType.Button
            ).AddBinding(
                this.Button15
            ).WithInteraction(
                "press(behavior=0,pressPoint=0.5)"
            );

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : actions builded & added to map");
            
            // instantiate & pair with device
            this.User
                = UnityEngine.InputSystem.Users.InputUser.PerformPairingWithDevice(
                    user: UnityEngine.InputSystem.Users.InputUser.CreateUserWithoutPairedDevices(),
                    device: this.Device
                );

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : user created & paired with device");

            // associa
            this.User.AssociateActionsWithUser(this.ActionMap);

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : action map associated with user");
            
            //this.ActionMap.asset.AddControlScheme("HOTAS").WithRequiredDevice(this.Device.path);
            //var control_scheme = UnityEngine.InputSystem.InputControlScheme.FindControlSchemeForDevice(this.User.pairedDevices[0], this.User.actions.controlSchemes);
            //if (control_scheme == null)
            //{
            //    UnityEngine.Debug.LogError("<color=Red>Error: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Awake() : ControlScheme == null... Could not find control scheme for current device/actions");
            //    return;
            //}
            //this.User.ActivateControlScheme((UnityEngine.InputSystem.InputControlScheme)control_scheme);

            // switch state 
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.Initialize() : initialization OK, user configured & ready");

        } /* Initialize() */

        #region Unity Mono Behaviour implementation

        private void Start()
        {

        }

        private void FixedUpdate() 
        {

            if(this.RawZAxisTracker) 
            {
                this.RawZAxisTracker.transform.SetPositionAndRotation(
                    /* default */ 
                    this.RawZAxisTracker.transform.position,
                    /* invert Z axe */
                    UnityEngine.Quaternion.Euler( 
                        UnityEngine.Vector3.right * this.Z.ReadValue()
                    )
                );
            } /* if() */

        } /* FixedUpdate() */

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        private void OnDestroy()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnDestroy() : cleanup, unpair device & remove user"
            );

            // clear
            this.User.UnpairDevicesAndRemoveUser();

        } /* OnDestroy() */

        public void OnEnable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnEnable() : call");

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {
                
                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnEnable() : initialize recquired");

                // call
                this.Initialize();

            } /* if() */

            // check user validity
            if (this.User.valid)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnEnable() : activate actions");
                
                // enable actions
                this.User.actions.Enable();

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning("<color=Orange>Warn: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnEnable() : Current user isn't valid ! ");

            } /* if() */

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBehaviour.OnDisable() : call");

            // skip it hasn't been initialized 
            if (!this.m_bHasInitialized)
            {
                return;
            }
        
            // disable actions
            this.User.actions.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonActiveSeatEntityBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
