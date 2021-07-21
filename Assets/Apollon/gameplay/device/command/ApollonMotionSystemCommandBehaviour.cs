// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.command
{

    public class ApollonMotionSystemCommandBehaviour
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        public UnityEngine.GameObject m_anchor;
        public ref UnityEngine.GameObject Anchor => ref this.m_anchor;

        public UnityEngine.Vector3 AngularAcceleration { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 AngularVelocitySaturation { get; set; } = new UnityEngine.Vector3();
        public bool InhibitVestibularMotion { get; set; } = false;
        public float Duration { get; set; } = 0.0f;
        public float StopAngle { get; set; } = 0.0f;
        public System.Diagnostics.Stopwatch Chrono { get; private set; } = new System.Diagnostics.Stopwatch();
        public UnityEngine.Vector3 InitialPosition { get; private set; } = new UnityEngine.Vector3();
        public UnityEngine.Quaternion InitialRotation { get; private set; } = new UnityEngine.Quaternion();
        public ApollonMotionSystemCommandBridge Bridge { get; set; }

        private bool m_bHasInitialized = false;

        #endregion

        #region controllers implemantion

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */

            private void OnEnable() 
            {
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // initialize our rigidbody
                this._rigidbody.ResetCenterOfMass();
                this._rigidbody.ResetInertiaTensor();
                this._rigidbody.transform.SetPositionAndRotation(this._parent.InitialPosition, this._parent.InitialRotation);
                this._rigidbody.constraints
                    = (
                        UnityEngine.RigidbodyConstraints.FreezeRotationY
                        | UnityEngine.RigidbodyConstraints.FreezeRotationZ
                    );
                this._rigidbody.drag = 0.0f;
                this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = false;
                this._rigidbody.isKinematic = false;
                this._rigidbody.interpolation = UnityEngine.RigidbodyInterpolation.None;
                this._rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousDynamic;
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = UnityEngine.Vector3.zero;
                this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : rigidbody initialized"
                );

                // virtual world setup
                // - user dependant PoV offset : height + depth
                // - max angular velocity aka. saturation point
                // - CenterOf -Rotation/Mass offset --> chair settings
                // - perfect world == no dampening/drag & no gravity 
                float PoV_height_offset = 0.0f, PoV_depth_offset = 0.0f;
                if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_height_offset"].ToString(), out PoV_height_offset)
                    || !float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_depth_offset"].ToString(), out PoV_depth_offset)
                ) {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Yellow>Warning: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : failed to get current participant PoV_offset, setup PoV_offset (height,depth) to default value [ "
                        + PoV_height_offset 
                        + ","
                        + PoV_depth_offset
                        + " ]"
                    );

                } /* if() */
                this._rigidbody.centerOfMass 
                    = (
                        UnityEngine.Vector3.up * ( PoV_height_offset / 100.0f )
                    ) + (
                        UnityEngine.Vector3.back * (PoV_depth_offset / 100.0f)
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : rigidbody configured with current user settings, going idle state"
                );

                // change state
                this._parent.Bridge.Dispatcher.RaiseIdle();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
        } /* internal class InitController */

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;
                //this.name = "ApollonIdleController";

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // zero velocity, acceleration & enforce velocity
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = UnityEngine.Vector3.zero;
                this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */
            
        } /* class IdleController */

        internal class AccelerateController
            : UnityEngine.MonoBehaviour
        {

            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;
                //this.name = "ApollonAccelerateController";

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // start chrono
                this._parent.Chrono.Restart();
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // check if saturation point is reached
                if (this._rigidbody.angularVelocity.magnitude >= this._parent.AngularVelocitySaturation.magnitude)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.AccelerateController.FixedUpdate() : angular saturation velocity reached, raise saturation event"
                    );

                    // fix saturatation speed (useless ?)
                    //this._rigidbody.AddTorque(this._parent.AngularVelocitySaturation, UnityEngine.ForceMode.VelocityChange);

                    // notify saturation event
                    this._parent.Bridge.Dispatcher.RaiseSaturation();

                }
                // or if max angle nor elapsed time is reached
                else if (
                    (this._parent.Chrono.ElapsedMilliseconds >= this._parent.Duration)
                    || (System.Math.Abs(UnityEngine.Quaternion.Angle(this._parent.InitialRotation, this._rigidbody.rotation)) >= this._parent.StopAngle)
                ) {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.AccelerateController.FixedUpdate() : stimulation duration/angle reached, raise deceleration event"
                    );

                    // notify stop event
                    this._parent.Bridge.Dispatcher.RaiseDecelerate();            

                }
                else
                {

                    // continuous perfect world acceleration
                    this._rigidbody.AddTorque(this._parent.AngularAcceleration, UnityEngine.ForceMode.Acceleration);

                } /* if() */

            } /* FixedUpdate() */

        } /* class AccelerateController */

        internal class DecelerateController
            : UnityEngine.MonoBehaviour
        {

            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;
                //this.name = "DeccelerateController";

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.DecelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.DecelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.DecelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // check if saturation point is reached
                if (this._rigidbody.angularVelocity.magnitude <= 0.0001f)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.DecelerateController.FixedUpdate() : movement stopped, raise iddle event"
                    );

                    // notify reset event
                    this._parent.Bridge.Dispatcher.RaiseReset();

                }
                else
                {

                    // continuous perfect world deceleration
                    this._rigidbody.AddTorque( (-1.0f * this._parent.AngularAcceleration), UnityEngine.ForceMode.Acceleration);

                } /* if() */

            } /* FixedUpdate() */

        } /* class DecelerateController */

        internal class HoldController
            : UnityEngine.MonoBehaviour
        {

            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {
                
                // disable by default
                this.enabled = false;
                //this.name = "ApollonHoldController";

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.HoldController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.HoldController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.HoldController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // Hold until Duration/StopAngle is reached
                if (
                    (this._parent.Chrono.ElapsedMilliseconds >= this._parent.Duration)
                    || (UnityEngine.Quaternion.Angle(this._parent.InitialRotation, this._rigidbody.rotation) >= this._parent.StopAngle)
                ) {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.HoldController.FixedUpdate() : stimulation duration/angle reached, raise stop event"
                    );

                    // notify saturation event
                    this._parent.Bridge.Dispatcher.RaiseDecelerate();

                } /* if() */

            } /* FixedUpdate() */

        } /* class HoldController */
        
        internal class ResetController
            : UnityEngine.MonoBehaviour
        {

            private ApollonMotionSystemCommandBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;
            private UnityEngine.Quaternion _lerp_rotation_from;
            private UnityEngine.Vector3 _lerp_position_from;
            private UnityEngine.Vector3 _angular_filter_state;
            private UnityEngine.Vector3 _linear_filter_state;
            private float _time_count = 0.0f;
            private float _total_time = 0.0f;
            private bool _bEnd = false;


            private void Awake()
            {

                // disable this script by default
                this.enabled = false;
                //this.name = "ApollonResetController";

            } /* Awake() */

            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.ResetController.OnEnable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonMotionSystemCommandBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonMotionSystemCommandBehaviour.ResetController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // save initial local rotation, intialize filter state & curent timepoint
                this._lerp_rotation_from = this._rigidbody.transform.localRotation;
                this._lerp_position_from = this._rigidbody.transform.position;
                this._angular_filter_state = this._linear_filter_state = UnityEngine.Vector3.zero;
                this._time_count = 0.0f;
                this._total_time 
                    = ( 
                        experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("trial_inter_sleep_duration_ms")
                        - 250.0f
                    )
                    / 1000.0f;
                this._bEnd = false;
                this._rigidbody.angularDrag = 1.0f;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.ResetController.OnEnable() : end"
                );

            } /* OnEnable() */

            private void FixedUpdate()
            {

                // check end cond
                if(this._bEnd)
                {
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.ResetController.FixedUpdate() : reset reached, doing re-init procedure"
                    );


                    // notify Init event
                    this._parent.Bridge.Dispatcher.RaiseInit();

                    return;

                } /* if() */

                // // skip initial condition
                // if(this._time_count != 0.0f)
                // {

                    //
                    // Phase forward corrector
                    //
                    // Variables
                    // xcons : consigne en position 
                    // xpos  : position actuelle
                    // accel : accélération commandée au rigidbody
                    // dt    : pas de temps (s)
                    // X     : état interne du correcteur (dimension 1)// Initialisation
                    //
                    // I/O
                    // -> l'erreur en position est l'entrée du correcteur
                    // <- l'accélération commandée est la sortie du correcteur
                    //
                    // Pseudo
                    // if init
                    // {
                    //     X = 0.0;
                    // }
                    // else
                    // { 
                    //     while true
                    //     {
                    //         err     = xcons - xpos;         // équation d'état du correcteur
                    //         dXsurdt = -7.849*X +  1.0*err;
                    //         Y       = -242.6*X + 37.23*err; // calcul de l'état au pas de temps suivant
                    //         Xp      = X + dXsurdt*dt;       // mise a jour de l'état
                    //         X       = Xp;    
                    //         accel   = Y;
                    //     }
                    // }

                    // get delta from objectives
                    UnityEngine.Vector3 
                        angular_delta
                            = (
                                /* current orientation*/
                                UnityEngine.Quaternion.Inverse(
                                    this._rigidbody.transform.localRotation
                                )
                                /* objective */
                                * UnityEngine.Quaternion.Slerp(
                                    this._lerp_rotation_from, 
                                    this._parent.InitialRotation,
                                    this._time_count / this._total_time
                                )
                            ).eulerAngles,
                        linear_delta 
                            = (
                                /* objective */ 
                                UnityEngine.Vector3.Slerp(
                                    this._lerp_position_from, 
                                    this._parent.InitialPosition,
                                    this._time_count / this._total_time
                                )
                                /* actual position */ 
                                - this._rigidbody.transform.position
                            );

                    // apply modulo 2pi
                    if(angular_delta.x > 180.0f) { angular_delta.x -= 360.0f; }
                    if(angular_delta.y > 180.0f) { angular_delta.y -= 360.0f; }
                    if(angular_delta.z > 180.0f) { angular_delta.z -= 360.0f; }
                    
                    // forward state
                    UnityEngine.Vector3 
                        angular_dXsurdt
                            = (
                                -7.849f * this._angular_filter_state +  1.0f * angular_delta
                            ),
                        linear_dXsurdt
                            = (
                                -7.849f * this._linear_filter_state +  1.0f * linear_delta
                            ),
                        anglar_forward_state
                            = (
                                -242.6f * this._angular_filter_state + 37.23f * angular_delta
                            ),
                        linear_forward_state
                            = (
                                -242.6f * this._linear_filter_state + 37.23f * linear_delta
                            );
                    
                    // update internal
                    this._angular_filter_state 
                        += (
                            angular_dXsurdt * UnityEngine.Time.fixedDeltaTime
                        );
                    this._linear_filter_state 
                        += (
                            linear_dXsurdt * UnityEngine.Time.fixedDeltaTime
                        );

                    // apply instructions
                    this._rigidbody.AddTorque(
                        anglar_forward_state,
                        UnityEngine.ForceMode.Acceleration
                    );
                    this._rigidbody.AddForce(
                        linear_forward_state,
                        UnityEngine.ForceMode.Acceleration
                    );

                // } /* if() */
                
                // check final condition
                if((this._time_count / this._total_time) > 1.0f)
                {

                    // rise end sig
                    this._bEnd = true;

                } /* if() */

                // whatever, advance timeline
                this._time_count += UnityEngine.Time.fixedDeltaTime;

            } /* FixedUpdate() */

        } /* class ResetController */

        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var init = this.gameObject.AddComponent<InitController>();
            var idle = this.gameObject.AddComponent<IdleController>();
            var accelerate = this.gameObject.AddComponent<AccelerateController>();
            var decelarate = this.gameObject.AddComponent<DecelerateController>();
            var hold = this.gameObject.AddComponent<HoldController>();
            var reset = this.gameObject.AddComponent<ResetController>();
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.Initialize() : state controller added as gameObject's component, mark as initialized");

            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.Initialize() : end");

        } /* Initialize() */

        #region MonoBehaviour implementation

        void Awake()
        {

            // behaviour inactive by default & gameobject inactive
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.OnEnable() : initialize recquired");

                // call
                this.Initialize();

            } /* if() */

            // skip if no experimental setup is found necessary
            if (experiment.ApollonExperimentManager.Instance.Session == null) return;

            // get current user mass detail
            float user_mass = 60.0f;
            if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["mass"].ToString(), out user_mass))
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Yellow>Warning: </color> ApollonMotionSystemCommandBehaviour.OnEnable() : failed to get current participant mass detail, setup mass to default value [ "
                    + user_mass
                    + " ]"
                );

            } /* if() */

            // add to settings 
            this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass += user_mass;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBehaviour.OnEnable() : added participant mass detail to simulated setup, total mass value is [ "
                + this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass
                + " ]");

            // save initial orientation/position
            this.InitialPosition = this.Anchor.transform.position;
            this.InitialRotation = this.Anchor.transform.rotation;
                        
        } /* OnEnable()*/

        void OnDisable()
        {
            
            // skip it hasn't been initialized 
            if (!this.m_bHasInitialized)
            {
                return;
            }

        } /* OnDisable() */

        #endregion

    } /* public class ApollonMotionSystemCommandBehaviour */

} /* } Labsim.apollon.gameplay.device.command */
