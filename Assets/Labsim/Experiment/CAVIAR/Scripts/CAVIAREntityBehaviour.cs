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

// using directives
using System;
using System.Text;
using System.Collections;
using System.Threading;

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    public class CAVIAREntityBehaviour
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<CAVIAREntityBridge>
    {

        #region properties/members

        public UnityEngine.GameObject m_reference;
        public ref UnityEngine.GameObject Reference => ref this.m_reference;

        public UnityEngine.Vector3 TargetLinearAcceleration { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 TargetLinearVelocity { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 InitialPosition { get; private set; } = new UnityEngine.Vector3();
        public UnityEngine.Quaternion InitialRotation { get; private set; } = new UnityEngine.Quaternion();

        private bool m_bHasInitialized = false;
        private float
            // altitude de l'appareil {meter}
            m_settings_pilotable_elevation_above_terrain = 0.0f,
            // taux de montee {m.s}
            m_settings_pilotable_climb_rate = 0.0f,
            // current user command
            m_user_throttle_axis_z_command = 0.0f;

        #endregion

        #region Controllers 

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private CAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
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
                this._rigidbody.drag = 0.0f;
                this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = false;
                this._rigidbody.isKinematic = false;
                this._rigidbody.interpolation = UnityEngine.RigidbodyInterpolation.Interpolate;
                this._rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousDynamic;
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.InitController.OnEnable() : rigidbody initialized"
                );

                // virtual world setup
                // - user dependant PoV offset : height + depth
                // - max angular velocity aka. saturation point
                // - CenterOf -Rotation/Mass offset --> chair settings
                // - perfect world == no dampening/drag & no gravity 
                float PoV_height_offset = 0.0f, PoV_depth_offset = 0.0f;
                if (!float.TryParse(apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_height_offset"].ToString(), out PoV_height_offset)
                    || !float.TryParse(apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["PoV_depth_offset"].ToString(), out PoV_depth_offset)
                ) {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Yellow>Warning: </color> CAVIAREntityBehaviour.InitController.OnEnable() : failed to get current participant PoV_offset, setup PoV_offset (height,depth) to default value [ "
                        + PoV_height_offset 
                        + ","
                        + PoV_depth_offset
                        + " ]"
                    );

                } /* if() */
                this._rigidbody.centerOfMass 
                    = (
                        UnityEngine.Vector3.down * ( PoV_height_offset / 100.0f )
                    ) + (
                        UnityEngine.Vector3.forward * (PoV_depth_offset / 100.0f)
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.InitController.OnEnable() : rigidbody configured with current user settings, going idle state"
                );

                // change state
                this._parent.ConcreteBridge.ConcreteDispatcher.RaiseIdle();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
        } /* internal class InitController */

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private CAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
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

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class IdleController */

        internal class AccelerateController
            : UnityEngine.MonoBehaviour
        {

            private CAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // check if linear target velocity is reached
                if (this._rigidbody.velocity.z >= this._parent.TargetLinearVelocity.z)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> CAVIAREntityBehaviour.AccelerateController.FixedUpdate() : linear saturation velocity reached, raise hold event"
                    );

                    // notify hold event
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();

                }
                else
                {

                    // calculate user command acceleration vector
                    var user_command_acc
                        = UnityEngine.Vector3.up
                            * (
                                (
                                    (
                                        this._parent.m_settings_pilotable_climb_rate 
                                        * this._parent.m_user_throttle_axis_z_command
                                    ) 
                                    - this._rigidbody.velocity.y
                                )
                                / UnityEngine.Time.fixedDeltaTime
                            );

                    // sum up = continuous perfect world acceleration + user command
                    var final_acc = this._parent.TargetLinearAcceleration + user_command_acc;

                    // assign
                    this._rigidbody.AddForce(final_acc, UnityEngine.ForceMode.Acceleration);

                } /* if() */

            } /* FixedUpdate() */

        } /* class AccelerateController */

        internal class DecelerateController
            : UnityEngine.MonoBehaviour
        {

            private CAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.DecelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.DecelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.DecelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {
                
                // check if saturation point is reached or if == 0.0
                if ((this._rigidbody.velocity.z <= this._parent.TargetLinearVelocity.z)
                    || (this._rigidbody.velocity.z <= 0.0f)
                ) {
                    
                    // check if empty speed
                    if (this._parent.TargetLinearVelocity.magnitude < 0.1f)
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> CAVIAREntityBehaviour.DecelerateController.FixedUpdate() : linear saturation velocity reached with no speed detected, raise idle event"
                        );
                        

                        // notify Idle event
                        this._parent.ConcreteBridge.ConcreteDispatcher.RaiseIdle();

                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> CAVIAREntityBehaviour.DecelerateController.FixedUpdate() : linear saturation velocity reached, raise hold event"
                        );

                        // notify saturation event
                        this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();

                    } /* if() */

                }
                else
                {

                    // calculate user command acceleration vector
                    var user_command_acc
                        = UnityEngine.Vector3.up
                            * (
                                (
                                    (
                                        this._parent.m_settings_pilotable_climb_rate
                                        * this._parent.m_user_throttle_axis_z_command
                                    )
                                    - this._rigidbody.velocity.y
                                )
                                / UnityEngine.Time.fixedDeltaTime
                            );

                    // sum up = continuous perfect world deceleration + user command
                    var final_acc = (this._parent.TargetLinearAcceleration) + user_command_acc;

                    // assign
                    this._rigidbody.AddForce(final_acc, UnityEngine.ForceMode.Acceleration);
                    
                } /* if() */

            } /* FixedUpdate() */

        } /* class DecelerateController */

        internal class HoldController
            : UnityEngine.MonoBehaviour
        {

            private CAVIAREntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.HoldController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<CAVIAREntityBehaviour>()) == null
                    || (this._rigidbody = this.GetComponentInParent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> CAVIAREntityBehaviour.HoldController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBehaviour.HoldController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void FixedUpdate()
            {

                // calculate user command acceleration vector
                var user_command_acc
                    = UnityEngine.Vector3.up
                        * (
                            (
                                (
                                    this._parent.m_settings_pilotable_climb_rate
                                    * this._parent.m_user_throttle_axis_z_command
                                )
                                - this._rigidbody.velocity.y
                            )
                            / UnityEngine.Time.fixedDeltaTime
                        );

                // assign
                this._rigidbody.AddForce(user_command_acc, UnityEngine.ForceMode.Acceleration);

            } /* FixedUpdate() */

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */
        
        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Initialize() : begin");
            
            // skip if no experimental setup is found necessary
            if (apollon.experiment.ApollonExperimentManager.Instance.Trial == null) return;

            // get global session settings
            this.m_settings_pilotable_elevation_above_terrain
                = apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_elevation_above_terrain_meter");
            this.m_settings_pilotable_climb_rate
                = apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_climb_rate_meter_per_second");

            // instantiate state controller components
            this.gameObject.AddComponent<IdleController>();
            this.gameObject.AddComponent<AccelerateController>();
            this.gameObject.AddComponent<DecelerateController>();
            this.gameObject.AddComponent<HoldController>();
            this.gameObject.AddComponent<InitController>();

            // intersect DB
            UnityEngine.RaycastHit hit;
            if (
                !(
                    UnityEngine.Physics.Raycast(
                        origin:
                            this.Reference.transform.position + (UnityEngine.Vector3.up * 200.0f),
                        direction:
                            UnityEngine.Vector3.down,
                        maxDistance:
                            UnityEngine.Mathf.Infinity,
                        hitInfo:
                            out hit
                    )
                )
            )
            {

                // log
                UnityEngine.Debug.LogError("<color=Red>Error: </color> CAVIAREntityBehaviour.Initialize() : failed to raycast elevation above terrain... exiting.");

                // exit
                return;

            } /* if() */

            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Initialize() : get hit point for elevation above terrain: " + hit.point);

            // set & save initial orientation/position
            this.Reference.transform.SetPositionAndRotation(
                new UnityEngine.Vector3(
                    this.Reference.transform.position.x,
                    (hit.point.y + this.m_settings_pilotable_elevation_above_terrain),
                    this.Reference.transform.position.z
                ),
                UnityEngine.Quaternion.identity
            );
            this.InitialPosition = this.Reference.transform.position;
            this.InitialRotation = this.Reference.transform.rotation;
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Initialize() : state controller added as gameObject's component and position initialized, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Close() : begin");

            // restore initial orientation/position
            this.Reference.transform.position = this.InitialPosition;
            this.Reference.transform.rotation = this.InitialRotation;
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIAREntityBehaviour.Close() : end");

        } /* Close() */

        public void SetUserThrottleAxisZValue(float value)
        {

            // save user command
            this.m_user_throttle_axis_z_command = -1.0f * value;

        } /* SetAltitudeFromThrottleAxisZValue() */

    } /* public class CAVIAREntityBehaviour */

} /* } Labsim.apollon.gameplay.entity */
