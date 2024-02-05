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

    public class AppollonGenericMotionSystemBridge 
        : ApollonGameplayBridge<AppollonGenericMotionSystemBridge>
    {

        //ctor
        public AppollonGenericMotionSystemBridge()
            : base()
        { }

        public AppollonGenericMotionSystemBehaviour ConcreteBehaviour 
            => this.Behaviour as AppollonGenericMotionSystemBehaviour;

        public AppollonGenericMotionSystemDispatcher ConcreteDispatcher 
            => this.Dispatcher as AppollonGenericMotionSystemDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<AppollonGenericMotionSystemBehaviour>(
                "AppollonGenericMotionSystemBridge",
                "AppollonGenericMotionSystemBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<AppollonGenericMotionSystemDispatcher>(
                "AppollonGenericMotionSystemBridge",
                "AppollonGenericMotionSystemDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.GenericMotionSystem;
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
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {

            public InitState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {

            public IdleState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {

            public AccelerateState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {

            public DecelerateState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {
            public HoldState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        internal sealed class ControlState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {

            public ControlState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ControlState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.ControlState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ControlState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ControlState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.ControlState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ControlState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class ControlState */

        internal sealed class ResetState 
            : ApollonAbstractGameplayState<AppollonGenericMotionSystemBridge>
        {
            public ResetState(AppollonGenericMotionSystemBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AppollonGenericMotionSystemBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AppollonGenericMotionSystemBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnAccelerateRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as AppollonGenericMotionSystemBehaviour;

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
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as AppollonGenericMotionSystemBehaviour;

            // keep internal settings

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnDecelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnSaturationRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnSaturationRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnSaturationRequested() : end"
            );

        } /* OnSaturationRequested() */

        private async void OnControlRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnControlRequested() : begin"
            );

            // get behaviour
            // var behaviour = this.Behaviour as AppollonGenericMotionSystemBehaviour;
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

        private async void OnResetRequested(object sender, AppollonGenericMotionSystemDispatcher.MotionSystemEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnResetRequested() : begin"
            );
            
            // inject duration
            (this.Behaviour as AppollonGenericMotionSystemBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AppollonGenericMotionSystemBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class AppollonGenericMotionSystemBridge */

} /* } Labsim.apollon.gameplay.device.command */