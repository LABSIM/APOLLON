// using directives
using System;
using System.Text;
using System.Collections;
using System.Threading;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonActiveSeatEntityBehaviour
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        public UnityEngine.GameObject m_reference;
        public ref UnityEngine.GameObject Reference => ref this.m_reference;

        public UnityEngine.Vector3 AngularAcceleration { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 AngularVelocitySaturation { get; set; } = new UnityEngine.Vector3();
        public bool InhibitVestibularMotion { get; set; } = false;
        public float Duration { get; set; } = 0.0f;
        public float StopAngle { get; set; } = 0.0f;
        public System.Diagnostics.Stopwatch Chrono { get; private set; } = new System.Diagnostics.Stopwatch();
        public UnityEngine.Vector3 InitialPosition { get; private set; } = new UnityEngine.Vector3();
        public UnityEngine.Quaternion InitialRotation { get; private set; } = new UnityEngine.Quaternion();
        public ApollonActiveSeatEntityBridge Bridge { get; set; }

        private bool m_bHasInitialized = false;

        #endregion

        #region controllers implemantion

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private ApollonActiveSeatEntityBehaviour _parent = null;

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
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonActiveSeatEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonActiveSeatEntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
        } /* class IdleController */

        internal class AccelerateController
            : UnityEngine.MonoBehaviour
        {

            private ApollonActiveSeatEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonActiveSeatEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // start chrono
                this._parent.Chrono.Restart();

                // virtual world setup
                // - max angular velocity aka. saturation point
                // - CenterOf -Rotation/Mass offset --> chair settings
                // - perfect world == no dampening/drag & no gravity 
                float PoV_offset = 0.0f;
                if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_offset"].ToString(), out PoV_offset))
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Yellow>Warning: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.OnEnable() : failed to get current participant PoV_offset, setup PoV_offset to default value [ "
                        + PoV_offset
                        + " ]"
                    );

                } /* if() */
                
                this._rigidbody.maxAngularVelocity = this._parent.AngularVelocitySaturation.magnitude;
                this._rigidbody.centerOfMass 
                    = UnityEngine.Vector3.up 
                    * ( 
                        /* absolute center of view */ 2.0f 
                        /* offset PoV to meter     */- ( PoV_offset / 100.0f ) 
                    );
                this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = false;
                
                // check
                if (!this._parent.InhibitVestibularMotion)
                {

                    // send Acceleration signal to HW
                    // send event to active chair over CAN bus
                    (
                        backend.ApollonBackendManager.Instance.GetValidHandle(
                            backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                        ) as backend.handle.ApollonActiveSeatHandle
                    ).Start(
                        this._parent.AngularAcceleration.magnitude,
                        this._parent.AngularVelocitySaturation.magnitude,
                        this._parent.Duration
                    );

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // iff. we are inhibiting HW stim
                if (this._parent.InhibitVestibularMotion)
                {

                    // check if saturation point is reached
                    if (this._rigidbody.angularVelocity.magnitude >= this._parent.AngularVelocitySaturation.magnitude)
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.FixedUpdate() : angular saturation velocity reached, raise saturation event"
                        );

                        // fix saturatation speed (useless)
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
                            "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.FixedUpdate() : stimulation duration/angle reached, raise stop event"
                        );

                        // notify saturation event
                        this._parent.Bridge.Dispatcher.RaiseStop();
                    }
                    else
                    {
                        
                        // continuous accel
                        this._rigidbody.AddTorque(this._parent.AngularAcceleration, UnityEngine.ForceMode.Acceleration);

                    } /* if() */

                }
                else
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.AccelerateController.OnEnable() : received accelerate acquittal from Hardware"
                    );

                    // Hold State -> notify saturation event
                    this._parent.Bridge.Dispatcher.RaiseSaturation();

                } /* if() */

            } /* FixedUpdate() */

        } /* class AccelerateController */

        internal class HoldController
            : UnityEngine.MonoBehaviour
        {

            private ApollonActiveSeatEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.HoldController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonActiveSeatEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonActiveSeatEntityBehaviour.HoldController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.HoldController.OnEnable() : end"
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
                        "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.HoldController.FixedUpdate() : stimulation duration/angle reached, raise stop event"
                    );

                    // notify saturation event
                    this._parent.Bridge.Dispatcher.RaiseStop();
                }

            } /* FixedUpdate() */

        } /* class HoldController */

        internal class StopController
            : UnityEngine.MonoBehaviour
        {

            private ApollonActiveSeatEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;
                //this.name = "ApollonStopController";

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonActiveSeatEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // stop chrono
                this._parent.Chrono.Stop();

                // iff. we are inhibiting HW stim
                if (this._parent.InhibitVestibularMotion)
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : zero velocity, acceleration & enforce velocity"
                    );

                    // zero velocity, acceleration & enforce velocity
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                    this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                }
                else
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : send stop signal to Hardware"
                    );

                    // send Stop order to HW
                    // send event to active chair over CAN bus
                    (
                        backend.ApollonBackendManager.Instance.GetValidHandle(
                            backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                        ) as backend.handle.ApollonActiveSeatHandle
                    ).Stop();

                } /* if() */
               
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : end"
                );

            } /* OnEnable() */

            private void FixedUpdate()
            {

                // check
                if (this._parent.InhibitVestibularMotion)
                {
                    
                }
                else
                {

                    //// log
                    //UnityEngine.Debug.Log(
                    //    "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.StopController.OnEnable() : received stop acquittal from Hardware"
                    //);

                    // wait stop acquittal from HW
                    // TODO  

                } /* if() */

            } /* FixedUpdate() */

        } /* class StopController */

        internal class ResetController
            : UnityEngine.MonoBehaviour
        {

            private ApollonActiveSeatEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

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
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonActiveSeatEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // reset chrono
                this._parent.Chrono.Reset();

                // check
                if (this._parent.InhibitVestibularMotion)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : zero transform then reset center of mass & tensor"
                    );

                    // zero transform then reset center of mass & tensor

                    this._rigidbody.transform.SetPositionAndRotation(this._parent.InitialPosition, this._parent.InitialRotation);
                    this._rigidbody.ResetCenterOfMass();
                    this._rigidbody.ResetInertiaTensor();

                }
                else
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : send reset signal to Hardware"
                    );

                    // send reset signal to HW
                    // send event to active chair over CAN bus
                    (
                        backend.ApollonBackendManager.Instance.GetValidHandle(
                            backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                        ) as backend.handle.ApollonActiveSeatHandle
                    ).Reset();

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : end"
                );

            } /* OnEnable() */

            private void FixedUpdate()
            {

                // check
                if (this._parent.InhibitVestibularMotion)
                {

                }
                else
                {

                    //// log
                    //UnityEngine.Debug.Log(
                    //    "<color=blue>Info: </color> ApollonActiveSeatEntityBehaviour.ResetController.OnEnable() : received reset acquittal from Hardware"
                    //);

                    // wait reset acquittal from HW
                    // TODO  

                } /* if() */

            } /* FixedUpdate() */

        } /* class ResetController */

        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var idle = this.gameObject.AddComponent<IdleController>();
            var accelerate = this.gameObject.AddComponent<AccelerateController>();
            var hold = this.gameObject.AddComponent<HoldController>();
            var stop = this.gameObject.AddComponent<StopController>();
            var reset = this.gameObject.AddComponent<ResetController>();
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.Initialize() : state controller added as gameObject's component, mark as initialized");

            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.Initialize() : end");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.OnEnable() : initialize recquired");

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
                    "<color=Yellow>Warning: </color> ApollonActiveSeatEntityBehaviour.OnEnable() : failed to get current participant mass detail, setup mass to default value [ "
                    + user_mass
                    + " ]"
                );

            } /* if() */

            // add to settings 
            this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass += user_mass;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.OnEnable() : added participant mass detail to simulated setup, total mass value is [ "
                + this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass
                + " ]");

            // save initial orientation/position
            this.InitialPosition = this.Reference.transform.position;
            this.InitialRotation = this.Reference.transform.rotation;
                        
        } /* OnEnable()*/

        void OnDisable()
        {
            
            // skip it hasn't been initialized 
            if (!this.m_bHasInitialized)
            {
                return;
            }

            // get current user mass detail
            float user_mass = 60.0f;
            if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["mass"].ToString(), out user_mass))
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Yellow>Warning: </color> ApollonActiveSeatEntityBehaviour.OnDisable() : failed to get current participant mass detail, setup mass to default value [ "
                    + user_mass
                    + " ]"
                );

            } /* if() */

            // remove from settings 
            this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass -= user_mass;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBehaviour.OnDisable() : removed participant mass detail from simulated setup, now total mass value is [ "
                + this.gameObject.GetComponentInParent<UnityEngine.Rigidbody>().mass
                + " ]"
            );

        } /* OnDisable() */

        #endregion

    } /* public class ApollonActiveSeatEntityBehaviour */

} /* } Labsim.apollon.gameplay.entity */
