// using directives
using System;
using System.Text;
using System.Collections;
using System.Threading;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonCAVIAREntityBehaviour
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        public UnityEngine.GameObject m_reference;
        public ref UnityEngine.GameObject Reference => ref this.m_reference;

        public UnityEngine.Vector3 TargetLinearAcceleration { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 TargetLinearVelocity { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 InitialPosition { get; private set; } = new UnityEngine.Vector3();
        public UnityEngine.Quaternion InitialRotation { get; private set; } = new UnityEngine.Quaternion();
        public ApollonCAVIAREntityBridge Bridge { get; set; }

        private bool m_bHasInitialized = false;

        private float 
            // altitude de l'appareil {meter}
            m_settings_pilotable_target_elevation_value = 0.0f,
            // permet la definition du range de pilotage en altitude {meter}
            // [
            //     (session_user_base_elevation - abs(session_user_pilotable_elevation));
            //     (session_user_base_elevation + abs(session_user_pilotable_elevation))
            // ]
            m_settings_pilotable_half_range_value = 0.0f,
            // current user command
            m_user_throttle_axis_z_command = 0.0f;


        #endregion

        #region Controllers 

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private ApollonCAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonCAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
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
                        UnityEngine.RigidbodyConstraints.FreezePositionX 
                        | UnityEngine.RigidbodyConstraints.FreezeRotation
                    );
                this._rigidbody.drag = this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = this._rigidbody.isKinematic = false;
                this._rigidbody.interpolation = UnityEngine.RigidbodyInterpolation.None;
                this._rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Discrete;
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : rigidbody initialized"
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
                        "<color=Yellow>Warning: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : failed to get current participant PoV_offset, setup PoV_offset (height,depth) to default value [ "
                        + PoV_height_offset 
                        + ","
                        + PoV_depth_offset
                        + " ]"
                    );

                } /* if() */
                this._rigidbody.centerOfMass 
                    = (
                        UnityEngine.Vector3.up 
                        * ( 
                            /* absolute center of view */ 2.0f 
                            /* offset PoV to meter     */- ( PoV_height_offset / 100.0f ) 
                        )
                    ) + (
                        UnityEngine.Vector3.forward * (PoV_depth_offset / 100.0f)
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : rigidbody configured with current user settings, going idle state"
                );

                // change state
                this._parent.Bridge.Dispatcher.RaiseIdle();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
        } /* internal class InitController */

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private ApollonCAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonCAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonCAVIAREntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */
                
                // zero velocity, acceleration & enforce velocity
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = UnityEngine.Vector3.zero;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
        } /* class IdleController */

        internal class AccelerateController
            : UnityEngine.MonoBehaviour
        {

            private ApollonCAVIAREntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonCAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonCAVIAREntityBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // check if target velocity is reached
                if (this._rigidbody.velocity.magnitude >= this._parent.TargetLinearVelocity.magnitude)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.AccelerateController.FixedUpdate() : linear saturation velocity reached, raise hold event"
                    );

                    // notify hold event
                    this._parent.Bridge.Dispatcher.RaiseHold();

                }
                else
                {
                    
                    // continuous perfect world acceleration
                    this._rigidbody.AddForce(
                        this._parent.TargetLinearAcceleration, 
                        UnityEngine.ForceMode.Acceleration
                    );

                } /* if() */

            } /* FixedUpdate() */

        } /* class AccelerateController */

        internal class DecelerateController
            : UnityEngine.MonoBehaviour
        {

            private ApollonCAVIAREntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.DecelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonCAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonCAVIAREntityBehaviour.DecelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.DecelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {
                
                // check if saturation point is reached or if == 0.0
                if ((this._rigidbody.velocity.magnitude <= this._parent.TargetLinearVelocity.magnitude)
                    || (this._rigidbody.velocity.z <= 0.0f)
                ) {
                    
                    // check if empty speed
                    if (this._parent.TargetLinearVelocity.magnitude < 0.1f)
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.DecelerateController.FixedUpdate() : linear saturation velocity reached with no speed detected, raise idle event"
                        );
                        

                        // notify Idle event
                        this._parent.Bridge.Dispatcher.RaiseIdle();

                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.DecelerateController.FixedUpdate() : linear saturation velocity reached, raise hold event"
                        );

                        // notify saturation event
                        this._parent.Bridge.Dispatcher.RaiseHold();

                    } /* if() */

                }
                else
                {
                    
                    // continuous perfect world acceleration
                    this._rigidbody.AddForce(
                        -1.0f * this._parent.TargetLinearAcceleration, 
                        UnityEngine.ForceMode.Acceleration
                    );

                } /* if() */

            } /* FixedUpdate() */

        } /* class DecelerateController */

        internal class HoldController
            : UnityEngine.MonoBehaviour
        {

            private ApollonCAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.HoldController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonCAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonCAVIAREntityBehaviour.HoldController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.HoldController.OnEnable() : end"
                );

            } /* OnEnable()*/

        } /* class HoldController */

        #endregion

        #region MonoBehaviours 

        private void Awake()
        {

            // behaviour inactive by default & gameobject inactive
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.OnEnable() : initialize required");

                // call
                this.Initialize();

            } /* if() */
                        
        } /* OnEnable()*/

        private void OnDisable()
        {
            
            // skip if it hasn't been initialized 
            if (this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        private void FixedUpdate()
        {

            // update altitude only
            var updated = this.Reference.transform.position;
            updated.y 
                = this.m_settings_pilotable_target_elevation_value + (
                    this.m_user_throttle_axis_z_command * this.m_settings_pilotable_half_range_value
                );
            this.Reference.transform.position = updated;

        } /* FixedUpdate() */

        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.Initialize() : begin");
            
            // skip if no experimental setup is found necessary
            if (experiment.ApollonExperimentManager.Instance.Trial == null) return;

            // get global session settings
            this.m_settings_pilotable_target_elevation_value
                = experiment.ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_target_elevation_meter");
            this.m_settings_pilotable_half_range_value
                = experiment.ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_half_range_meter");

            // instantiate state controller components
            var idle = this.gameObject.AddComponent<IdleController>();
            var accelerate = this.gameObject.AddComponent<AccelerateController>();
            var decelerate = this.gameObject.AddComponent<DecelerateController>();
            var hold = this.gameObject.AddComponent<HoldController>();
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.Initialize() : state controller added as gameObject's component, mark as initialized");

            // set & save initial orientation/position
            this.Reference.transform.SetPositionAndRotation(
                UnityEngine.Vector3.up * this.m_settings_pilotable_target_elevation_value,
                UnityEngine.Quaternion.identity
            );
            this.InitialPosition = this.Reference.transform.position;
            this.InitialRotation = this.Reference.transform.rotation;

            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.Close() : begin");

            // restore initial orientation/position
            this.Reference.transform.position = this.InitialPosition;
            this.Reference.transform.rotation = this.InitialRotation;
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIAREntityBehaviour.Close() : end");

        } /* Close() */

        public void SetAltitudeFromThrottleAxisZValue(float value)
        {

            // save user command
            this.m_user_throttle_axis_z_command = value;

        } /* SetAltitudeFromThrottleAxisZValue() */

    } /* public class ApollonCAVIAREntityBehaviour */

} /* } Labsim.apollon.gameplay.entity */
