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

namespace Labsim.apollon.gameplay.device
{

    public class ApollonGeneric6DoFMotionSystemBridge 
        : ApollonGameplayBridge<ApollonGeneric6DoFMotionSystemBridge>
    {

        //ctor
        public ApollonGeneric6DoFMotionSystemBridge()
            : base()
        { }

        public ApollonGeneric6DoFMotionSystemBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonGeneric6DoFMotionSystemBehaviour;

        public ApollonGeneric6DoFMotionSystemDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonGeneric6DoFMotionSystemDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonGeneric6DoFMotionSystemBehaviour>(
                "ApollonGeneric6DoFMotionSystemBridge",
                "ApollonGeneric6DoFMotionSystemBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGeneric6DoFMotionSystemDispatcher>(
                "ApollonGeneric6DoFMotionSystemBridge",
                "ApollonGeneric6DoFMotionSystemDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.Generic6DoFMotionSystem;
        }

        protected override async void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);
                
                // subscribe
                this.ConcreteDispatcher.InitEvent       += this.OnInitRequested;
                this.ConcreteDispatcher.IdleEvent       += this.OnIdleRequested;
                this.ConcreteDispatcher.AccelerateEvent += this.OnAccelerateRequested;
                this.ConcreteDispatcher.DecelerateEvent += this.OnDecelerateRequested;
                this.ConcreteDispatcher.SaturationEvent += this.OnSaturationRequested;
                this.ConcreteDispatcher.ControlEvent    += this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent      += this.OnResetRequested;

                // activate the motion system backend
                backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(
                    backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
                );

                // nullify FSM
                await this.SetState(null);

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // inactivate the motion system backend
                backend.ApollonBackendManager.Instance.RaiseHandleDeactivationRequestedEvent(
                    backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
                );

                // unsubscribe
                this.ConcreteDispatcher.InitEvent       -= this.OnInitRequested;
                this.ConcreteDispatcher.IdleEvent       -= this.OnIdleRequested;
                this.ConcreteDispatcher.AccelerateEvent -= this.OnAccelerateRequested;
                this.ConcreteDispatcher.DecelerateEvent -= this.OnDecelerateRequested;
                this.ConcreteDispatcher.SaturationEvent -= this.OnSaturationRequested;
                this.ConcreteDispatcher.ControlEvent    -= this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent      -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {

            public InitState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {

            public IdleState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {

            public AccelerateState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {

            public DecelerateState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {
            public HoldState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        internal sealed class ControlState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {

            public ControlState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ControlState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class ControlState */

        internal sealed class ResetState 
            : ApollonAbstractGameplayState<ApollonGeneric6DoFMotionSystemBridge>
        {
            public ResetState(ApollonGeneric6DoFMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonGeneric6DoFMotionSystemBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnAccelerateRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonGeneric6DoFMotionSystemBehaviour;

            // set internal settings
            behaviour.AngularAccelerationTarget 
                = (
                    UnityEngine.Mathf.Deg2Rad 
                    * new UnityEngine.Vector3(
                        /* pitch - x axis */ args.AngularAccelerationTarget[0],
                        /* yaw   - y axis */ args.AngularAccelerationTarget[1],
                        /* roll  - z axis */ args.AngularAccelerationTarget[2]
                    )
                );
            behaviour.AngularVelocitySaturationThreshold
                = (
                    UnityEngine.Mathf.Deg2Rad 
                    * new UnityEngine.Vector3(
                        /* pitch - x axis */ args.AngularVelocitySaturationThreshold[0],
                        /* yaw   - y axis */ args.AngularVelocitySaturationThreshold[1],
                        /* roll  - z axis */ args.AngularVelocitySaturationThreshold[2]
                    )
                );
            behaviour.AngularDisplacementLimiter
                = (
                    new UnityEngine.Vector3(
                        /* pitch - x axis */ args.AngularDisplacementLimiter[0],
                        /* yaw   - y axis */ args.AngularDisplacementLimiter[1],
                        /* roll  - z axis */ args.AngularDisplacementLimiter[2]
                    )
                );
            behaviour.LinearAccelerationTarget 
                = ( 
                    new UnityEngine.Vector3(
                        /* sway  - x axis */ args.LinearAccelerationTarget[0],
                        /* heave - y axis */ args.LinearAccelerationTarget[1],
                        /* surge - z axis */ args.LinearAccelerationTarget[2]
                    )
                );
            behaviour.LinearVelocitySaturationThreshold
                = (
                    new UnityEngine.Vector3(
                        /* sway  - x axis */ args.LinearVelocitySaturationThreshold[0],
                        /* heave - y axis */ args.LinearVelocitySaturationThreshold[1],
                        /* surge - z axis */ args.LinearVelocitySaturationThreshold[2]
                    )
                );
            behaviour.LinearDisplacementLimiter
                = (
                    new UnityEngine.Vector3(
                        /* sway  - x axis */ args.LinearDisplacementLimiter[0],
                        /* heave - y axis */ args.LinearDisplacementLimiter[1],
                        /* surge - z axis */ args.LinearDisplacementLimiter[2]
                    )
                );
            behaviour.Duration = args.Duration;

            // activate state
            await this.SetState(new AccelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonGeneric6DoFMotionSystemBehaviour;

            // keep internal settings

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnDecelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnSaturationRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnSaturationRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnSaturationRequested() : end"
            );

        } /* OnSaturationRequested() */

        private async void OnControlRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnControlRequested() : begin"
            );

            // get behaviour
            // var behaviour = this.Behaviour as ApollonGeneric6DoFMotionSystemBehaviour;
            // set internal settings
            // behaviour.AngularAccelerationTarget 
            //     = (
            //         UnityEngine.Mathf.Deg2Rad 
            //         * new UnityEngine.Vector3(
            //             /* pitch - x axis */ args.AngularAccelerationTarget[0],
            //             /* yaw   - y axis */ args.AngularAccelerationTarget[1],
            //             /* roll  - z axis */ args.AngularAccelerationTarget[2]
            //         )
            //     );
            // behaviour.AngularVelocitySaturationThreshold
            //     = (
            //         UnityEngine.Mathf.Deg2Rad 
            //         * new UnityEngine.Vector3(
            //             /* pitch - x axis */ args.AngularVelocitySaturationThreshold[0],
            //             /* yaw   - y axis */ args.AngularVelocitySaturationThreshold[1],
            //             /* roll  - z axis */ args.AngularVelocitySaturationThreshold[2]
            //         )
            //     );
            // behaviour.AngularDisplacementLimiter
            //     = (
            //         new UnityEngine.Vector3(
            //             /* pitch - x axis */ args.AngularDisplacementLimiter[0],
            //             /* yaw   - y axis */ args.AngularDisplacementLimiter[1],
            //             /* roll  - z axis */ args.AngularDisplacementLimiter[2]
            //         )
            //     );
            // behaviour.LinearAccelerationTarget 
            //     = ( 
            //         new UnityEngine.Vector3(
            //             /* sway  - x axis */ args.LinearAccelerationTarget[0],
            //             /* heave - y axis */ args.LinearAccelerationTarget[1],
            //             /* surge - z axis */ args.LinearAccelerationTarget[2]
            //         )
            //     );
            // behaviour.LinearVelocitySaturationThreshold
            //     = (
            //         new UnityEngine.Vector3(
            //             /* sway  - x axis */ args.LinearVelocitySaturationThreshold[0],
            //             /* heave - y axis */ args.LinearVelocitySaturationThreshold[1],
            //             /* surge - z axis */ args.LinearVelocitySaturationThreshold[2]
            //         )
            //     );
            // behaviour.LinearDisplacementLimiter
            //     = (
            //         new UnityEngine.Vector3(
            //             /* sway  - x axis */ args.LinearDisplacementLimiter[0],
            //             /* heave - y axis */ args.LinearDisplacementLimiter[1],
            //             /* surge - z axis */ args.LinearDisplacementLimiter[2]
            //         )
            //     );
            // behaviour.Duration = args.Duration;

            // activate state
            await this.SetState(new ControlState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnControlRequested() : end"
            );

        } /* OnControlRequested() */

        private async void OnResetRequested(object sender, ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnResetRequested() : begin"
            );
            
            // inject duration
            (this.Behaviour as ApollonGeneric6DoFMotionSystemBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonGeneric6DoFMotionSystemBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class ApollonGeneric6DoFMotionSystemBridge */

} /* } Labsim.apollon.gameplay.device.command */