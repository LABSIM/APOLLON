//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEEntityBehaviour
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<AIRWISEEntityBridge>
    {

        #region properties/members

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_CommandReference = null;        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_SensorReference = null;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_initialPosition = new UnityEngine.Vector3();
        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_initialRotation = new UnityEngine.Vector3();

        public ref UnityEngine.GameObject CommandReference => ref this.m_CommandReference;
        public ref UnityEngine.GameObject SensorReference => ref this.m_SensorReference;
        public ref UnityEngine.Vector3 InitialPosition => ref this.m_initialPosition;
        public ref UnityEngine.Vector3 InitialRotation => ref this.m_initialRotation;

        public float Duration { get; set; } = 0.0f;
        public System.Diagnostics.Stopwatch Chrono { get; private set; } = new System.Diagnostics.Stopwatch();

        private bool m_bHasInitialized = false;

        #endregion

        #region controllers implemantion

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private AIRWISEEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<AIRWISEEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
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
                this._rigidbody.transform.SetPositionAndRotation(this._parent.InitialPosition, UnityEngine.Quaternion.Euler(this._parent.InitialRotation));
                this._rigidbody.constraints = UnityEngine.RigidbodyConstraints.None;
                this._rigidbody.drag = 0.0f;
                this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = false;
                this._rigidbody.isKinematic = false;
                this._rigidbody.interpolation = UnityEngine.RigidbodyInterpolation.Interpolate;
                this._rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Discrete;
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = UnityEngine.Vector3.zero;
                this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : rigidbody initialized"
                );

                // virtual world setup
                // - user dependant PoV offset : height + depth
                // - max angular velocity aka. saturation point
                // - CenterOf -Rotation/Mass offset --> chair settings
                // - perfect world == no dampening/drag & no gravity 
                float PoV_height_offset = 0.0f, PoV_depth_offset = 0.0f;
                //if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_height_offset"].ToString(), out PoV_height_offset)
                //    || !float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_depth_offset"].ToString(), out PoV_depth_offset)
                //) {

                //    // log
                //    UnityEngine.Debug.LogWarning(
                //        "<color=Yellow>Warning: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : failed to get current participant PoV_offset, setup PoV_offset (height,depth) to default value [ "
                //        + PoV_height_offset 
                //        + ","
                //        + PoV_depth_offset
                //        + " ]"
                //    );

                //} /* if() */
                this._rigidbody.centerOfMass 
                    = (
                        UnityEngine.Vector3.up * ( PoV_height_offset / 100.0f )
                    ) + (
                        UnityEngine.Vector3.back * (PoV_depth_offset / 100.0f)
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : rigidbody configured with current user settings, going idle state"
                );

                // change state
                this._parent.ConcreteBridge.ConcreteDispatcher.RaiseIdle();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
        } /* internal class InitController */

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private AIRWISEEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<AIRWISEEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEEntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // TODO
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<AIRWISEEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEEntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */
            
        } /* class IdleController */

        internal class ControlController
            : UnityEngine.MonoBehaviour
        {

            private AIRWISEEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;
            private UnityEngine.Vector3 _torque_dampener = UnityEngine.Vector3.zero;
            private UnityEngine.Vector3 _force_dampener = UnityEngine.Vector3.zero;
            private readonly float _smooth_time = 0.01f;

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
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<AIRWISEEntityBehaviour>()) == null
                    || (this._rigidbody = this._parent.CommandReference.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEEntityBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
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
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            // private void FixedUpdate()
            // {

            // } /* FixedUpdate() */

        } /* class ControlController */

        internal class ResetController
            : UnityEngine.MonoBehaviour
        {

            private AIRWISEEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.ResetController.OnEnable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.GetComponentInParent<AIRWISEEntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEEntityBehaviour.ResetController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
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
                this._total_time = this._parent.Duration / 1000.0f;
                this._bEnd = false;
                this._rigidbody.angularDrag = 1.0f;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBehaviour.ResetController.OnEnable() : end"
                );

            } /* OnEnable() */

            private void FixedUpdate()
            {

                // check end cond
                if(this._bEnd)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> AIRWISEEntityBehaviour.ResetController.FixedUpdate() : reset reached, doing re-init procedure"
                    );

                    // notify Init event
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseInit();

                    return;

                } /* if() */

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
                                UnityEngine.Quaternion.Euler(this._parent.InitialRotation),
                                this._time_count / this._total_time
                            )
                        ).eulerAngles,
                    linear_delta 
                        = (
                            /* objective */ 
                            UnityEngine.Vector3.Lerp(
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
                    angular_forward_state
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
                    angular_forward_state,
                    UnityEngine.ForceMode.Acceleration
                );
                this._rigidbody.AddForce(
                    linear_forward_state,
                    UnityEngine.ForceMode.Acceleration
                );
                
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
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AIRWISEEntityBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var init       = this.gameObject.AddComponent<InitController>();
            var idle       = this.gameObject.AddComponent<IdleController>();
            var control    = this.gameObject.AddComponent<ControlController>();
            var reset      = this.gameObject.AddComponent<ResetController>();
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AIRWISEEntityBehaviour.Initialize() : state controller added as gameObject's component, mark as initialized");

            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AIRWISEEntityBehaviour.Initialize() : end");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> AIRWISEEntityBehaviour.OnEnable() : initialize recquired");

                // call 
                this.Initialize();

            } /* if() */

            // skip if no experimental setup is found necessary
            //if (experiment.ApollonExperimentManager.Instance.Session == null) return;

            // save initial orientation/position
            // this.InitialPosition = this.Anchor.transform.position;
            // this.InitialRotation = this.Anchor.transform.rotation;
                        
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

    } /* public class AIRWISEEntityBehaviour */

} /* } Labsim.experiment.AIRWISE */
