﻿//
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

namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATEntityBehaviour
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<LEXIKHUMOATEntityBridge>
    {

        #region properties/members

        public UnityEngine.Vector3 AngularAccelerationTarget { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 AngularVelocitySaturationThreshold { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 LinearAccelerationTarget { get; set; } = new UnityEngine.Vector3();
        public UnityEngine.Vector3 LinearVelocitySaturationThreshold { get; set; } = new UnityEngine.Vector3();
        
        public float Duration { get; set; } = 0.0f;
        public System.Diagnostics.Stopwatch Chrono { get; private set; } = new System.Diagnostics.Stopwatch();

        private bool m_bHasInitialized = false;

        #endregion

        #region controllers implemention

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private LEXIKHUMOATEntityBehaviour _parent = null;
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
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // Reset our rigidbody & gameObject
                this._parent.transform.SetPositionAndRotation(UnityEngine.Vector3.up * 0.5f, UnityEngine.Quaternion.identity);
                this._rigidbody.ResetCenterOfMass();
                this._rigidbody.ResetInertiaTensor();
                this._rigidbody.constraints = UnityEngine.RigidbodyConstraints.FreezePositionY | UnityEngine.RigidbodyConstraints.FreezeRotation;
                this._rigidbody.drag = 0.0f;
                this._rigidbody.angularDrag = 0.0f;
                this._rigidbody.useGravity = false;
                this._rigidbody.isKinematic = false;
                this._rigidbody.interpolation = UnityEngine.RigidbodyInterpolation.None;
                this._rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Continuous;
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                this._rigidbody.velocity = UnityEngine.Vector3.zero;
                this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                // restart 
                this._parent.Chrono.Restart();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
            private void FixedUpdate()
            {

                // check if saturation point is reached on each axis
                if( (UnityEngine.Mathf.Abs(UnityFrame.GetAngularVelocity(this._rigidbody).x)     >= UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.x))
                    && (UnityEngine.Mathf.Abs(UnityFrame.GetAngularVelocity(this._rigidbody).y)  >= UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.y))
                    && (UnityEngine.Mathf.Abs(UnityFrame.GetAngularVelocity(this._rigidbody).z)  >= UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.z))
                    && (UnityEngine.Mathf.Abs(UnityFrame.GetAbsoluteVelocity(this._rigidbody).x) >= UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.x))
                    && (UnityEngine.Mathf.Abs(UnityFrame.GetAbsoluteVelocity(this._rigidbody).y) >= UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.y))
                    && (UnityEngine.Mathf.Abs(UnityFrame.GetAbsoluteVelocity(this._rigidbody).z) >= UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.z))
                )
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.InitController.FixedUpdate() : saturation reached on all axis, raising Hold state"
                    );

                    // change state
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();

                }
                else if(this._parent.Chrono.ElapsedMilliseconds >= this._parent.Duration)
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.InitController.FixedUpdate() : elapsed time has reached duration, raising Hold state"
                    );

                    // change state
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();

                }
                else
                {

                    // initializing 
                    UnityEngine.Vector3 
                        torque_target = UnityEngine.Vector3.zero,
                        force_target  = UnityEngine.Vector3.zero;

                    // continuous perfect world acceleration

                    if ((this._parent.AngularAccelerationTarget.x != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.x) < UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.x))
                    ) { torque_target.x = this._parent.AngularAccelerationTarget.x; }
                    if ((this._parent.AngularAccelerationTarget.y != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.y) < UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.y))
                    ) { torque_target.y = this._parent.AngularAccelerationTarget.y;  }
                    if ((this._parent.AngularAccelerationTarget.z != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.z) < UnityEngine.Mathf.Abs(this._parent.AngularVelocitySaturationThreshold.z))
                    ) { torque_target.z = this._parent.AngularAccelerationTarget.z;  }    

                    if ((this._parent.LinearAccelerationTarget.x != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.x) < UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.x))
                    ) { force_target.x = this._parent.LinearAccelerationTarget.x; }
                    if ((this._parent.LinearAccelerationTarget.y != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.y) < UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.y))
                    ) { force_target.y = this._parent.LinearAccelerationTarget.y; }
                    if ((this._parent.LinearAccelerationTarget.z != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.z) < UnityEngine.Mathf.Abs(this._parent.LinearVelocitySaturationThreshold.z))
                    ) { force_target.z = this._parent.LinearAccelerationTarget.z; }

                    // finally
                    this._rigidbody.AddTorque(
                        torque_target, 
                        UnityEngine.ForceMode.Acceleration
                    );
                    this._rigidbody.AddForce(
                        force_target,
                        UnityEngine.ForceMode.Acceleration
                    );
                            
                } /* if() */

            } /* FixedUpdate */
            
        } /* internal class InitController */

        internal class HoldController
            : UnityEngine.MonoBehaviour
        {

            private LEXIKHUMOATEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;
                //this.name = "ApollonHoldController";

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnEnable() : failed to get parent reference ! Self disabling..."
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
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.HoldController.OnDisable() : end"
                );

            } /* OnDisable() */
            
        } /* class HoldController */

        internal class ControlController
            : UnityEngine.MonoBehaviour
        {

            private LEXIKHUMOATEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;
            private System.EventHandler<LEXIKHUMOATControlDispatcher.LEXIKHUMOATControlEventArgs> _interaction_lambda = null;
            private apollon.gameplay.device.impedence.IApollonImpedenceModel _impedence_ref = null;


            // private System 

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;
                //this.name = "ApollonControlController";

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // internal 
                this._interaction_lambda 
                    = new(
                        (sender, args) 
                        => {
                            this._rigidbody.MovePosition(
                                new(
                                    args.JoystickHorizontal * 5.0f,
                                    this._rigidbody.position.y,
                                    this._rigidbody.position.z
                                )
                            );
                        }
                    );

                this._impedence_ref
                    = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                        apollon.gameplay.device.ApollonGeneric3DoFHapticArmBridge
                    >(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                    ).ConcreteBehaviour.EffectorImpedence;


                // map joystick 
                if(this._interaction_lambda != null)
                {

                    apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<LEXIKHUMOATControlBridge>(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATControl
                    ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent += this._interaction_lambda;
                
                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // unmap joystick 
                
                if(this._interaction_lambda != null)
                {

                    apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<LEXIKHUMOATControlBridge>(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATControl
                    ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent -= this._interaction_lambda;
                
                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ControlController.OnDisable() : end"
                );

            } /* OnDisable() */

            private void FixedUpdate()
            {
                
                // skip
                if(this._impedence_ref == null || this._parent == null)
                {
                    return;
                }

                // downstream
                // inject everything
                this._impedence_ref.VirtualWorld.Sensor.transform.position   = this._parent.transform.position;
                this._impedence_ref.VirtualWorld.Sensor.transform.rotation   = this._parent.transform.rotation;
                this._impedence_ref.VirtualWorld.Sensor.transform.localScale = this._parent.transform.localScale;

                // upstream
                // => only use x (horizontal) axis (closed loop) 
                // this._parent.transform.position.Set(
                //     this._impedence_ref.VirtualWorld.Command.transform.position.x,
                //     this._parent.transform.position.y,
                //     this._parent.transform.position.z
                // );

                this._rigidbody.MovePosition(
                    new UnityEngine.Vector3(
                        this._impedence_ref.VirtualWorld.Command.transform.position.y,
                        this._parent.transform.position.y,
                        this._parent.transform.position.z
                    )                    
                );
                
            } /* FixedUpdate() */

            // private void Update()
            // {

            //     // skip
            //     if(this._rigidbody == null)
            //     {
            //         return;
            //     }

            //     // debug set from keyboard inputs 
                
            //     if(UnityEngine.InputSystem.Keyboard.current.leftArrowKey.isPressed)
            //     {
            //         this._rigidbody.MovePosition(this._rigidbody.position + (UnityEngine.Vector3.left * 0.2f));
            //     }

            //     if(UnityEngine.InputSystem.Keyboard.current.rightArrowKey.isPressed)
            //     {
            //         this._rigidbody.MovePosition(this._rigidbody.position + (UnityEngine.Vector3.right * 0.2f));
            //     }

            // } /* Update() */
            
        } /* class ControlController */
        
        internal class ResetController
            : UnityEngine.MonoBehaviour
        {

            private LEXIKHUMOATEntityBehaviour _parent = null;
            private UnityEngine.Rigidbody _rigidbody = null;

            private void Awake()
            {

                // disable this script by default
                this.enabled = false;
                //this.name = "ApollonResetController";

            } /* Awake()  */

            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnEnable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                 // restart 
                this._parent.Chrono.Restart();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnEnable() : end"
                );

            } /* OnEnable() */

            private void OnDisable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnDisable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>()) == null
                    || (this._rigidbody = this.gameObject.GetComponent<UnityEngine.Rigidbody>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnDisable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.OnDisable() : end"
                );

            } /* OnDisable() */

            private void FixedUpdate()
            {

                // check if saturation point is reached
                if ((this._rigidbody.angularVelocity.x <= 0.0001f)
                    && (this._rigidbody.angularVelocity.y <= 0.0001f)
                    && (this._rigidbody.angularVelocity.z <= 0.0001f)
                    && (this._rigidbody.velocity.x <= 0.0001f)
                    && (this._rigidbody.velocity.y <= 0.0001f)
                    && (this._rigidbody.velocity.z <= 0.0001f)
                )
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.FixedUpdate() : saturation point reached, zeroing all forces & torques then raise idle event"
                    );
                    
                    // iding - zero velocity, acceleration & enforce velocity
                    this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                    this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                    this._rigidbody.velocity = UnityEngine.Vector3.zero;
                    this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                    // notify Control event
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();                        

                }
                else if(this._parent.Chrono.ElapsedMilliseconds >= this._parent.Duration)
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.ResetController.FixedUpdate() : elapsed time has reached duration, raising Control state"
                    );

                    // iding - zero velocity, acceleration & enforce velocity
                    this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.VelocityChange);
                    this._rigidbody.AddForce(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                    this._rigidbody.AddTorque(UnityEngine.Vector3.zero, UnityEngine.ForceMode.Acceleration);
                    this._rigidbody.velocity = UnityEngine.Vector3.zero;
                    this._rigidbody.angularVelocity = UnityEngine.Vector3.zero;

                    // change state
                    this._parent.ConcreteBridge.ConcreteDispatcher.RaiseHold();

                }
                else
                {

                    // initializing 
                    UnityEngine.Vector3 
                        torque_target = UnityEngine.Vector3.zero,
                        force_target  = UnityEngine.Vector3.zero;

                    // continuous perfect world deceleration
                    if ((this._parent.AngularAccelerationTarget.x != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.x) > 0.0001f) 
                    ) { torque_target.x = this._parent.AngularAccelerationTarget.x; }
                    if ((this._parent.AngularAccelerationTarget.y != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.y) > 0.0001f) 
                    ) { torque_target.y =  this._parent.AngularAccelerationTarget.y; }
                    if ((this._parent.AngularAccelerationTarget.z != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.angularVelocity.z) > 0.0001f) 
                    ) { torque_target.z = this._parent.AngularAccelerationTarget.z; }

                    if ((this._parent.LinearAccelerationTarget.x != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.x) > 0.0001f) 
                    ) { force_target.x = this._parent.LinearAccelerationTarget.x; }
                    if ((this._parent.LinearAccelerationTarget.y != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.y) > 0.0001f) 
                    ) { force_target.y = this._parent.LinearAccelerationTarget.y; }
                    if ((this._parent.LinearAccelerationTarget.z != 0.0f) 
                        && (UnityEngine.Mathf.Abs(this._rigidbody.velocity.z) > 0.0001f)
                    ) { force_target.z = this._parent.LinearAccelerationTarget.z; }

                    // finally
                    this._rigidbody.AddTorque(
                        -1.0f * torque_target, 
                        UnityEngine.ForceMode.Acceleration
                    );
                    this._rigidbody.AddForce(
                        -1.0f * force_target, 
                        UnityEngine.ForceMode.Acceleration
                    );

                } /* if() */

            } /* FixedUpdate() */

        } /* class ResetController */

        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var init       = this.gameObject.AddComponent<InitController>();
            var hold       = this.gameObject.AddComponent<HoldController>();
            var control    = this.gameObject.AddComponent<ControlController>();
            var reset      = this.gameObject.AddComponent<ResetController>();
            
            UnityEngine.Debug.Log("<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.Initialize() : state controller added as gameObject's component, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.Initialize() : end");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> LEXIKHUMOATEntityBehaviour.OnEnable() : initialize recquired");

                // call 
                this.Initialize();

            } /* if() */

            // reset our gameObject inital transform
            this.transform.SetPositionAndRotation(UnityEngine.Vector3.up * 0.5f, UnityEngine.Quaternion.identity);
                
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

    } /* public class LEXIKHUMOATEntityBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
