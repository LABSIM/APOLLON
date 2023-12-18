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

namespace Labsim.apollon.gameplay.device.command
{

    public class ApollonVirtualMotionSystemCommandBridge 
        : ApollonGameplayBridge<ApollonVirtualMotionSystemCommandBridge>
    {

        //ctor
        public ApollonVirtualMotionSystemCommandBridge()
            : base()
        { }

        public ApollonVirtualMotionSystemCommandBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonVirtualMotionSystemCommandBehaviour;

        public ApollonVirtualMotionSystemCommandDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonVirtualMotionSystemCommandDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonVirtualMotionSystemCommandBehaviour>(
                "ApollonVirtualMotionSystemCommandBridge",
                "ApollonVirtualMotionSystemCommandBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonVirtualMotionSystemCommandDispatcher>(
                "ApollonVirtualMotionSystemCommandBridge",
                "ApollonVirtualMotionSystemCommandDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand;
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
                this.ConcreteDispatcher.ResetEvent      += this.OnResetRequested;

                // entry state FSM
                await this.SetState(new InitState(this));

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // unsubscribe
                this.ConcreteDispatcher.InitEvent       -= this.OnInitRequested;
                this.ConcreteDispatcher.IdleEvent       -= this.OnIdleRequested;
                this.ConcreteDispatcher.AccelerateEvent -= this.OnAccelerateRequested;
                this.ConcreteDispatcher.DecelerateEvent -= this.OnDecelerateRequested;
                this.ConcreteDispatcher.SaturationEvent -= this.OnSaturationRequested;
                this.ConcreteDispatcher.ResetEvent      -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {

            public InitState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {

            public IdleState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {

            public AccelerateState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {

            public DecelerateState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {
            public HoldState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        internal sealed class ResetState : ApollonAbstractGameplayState<ApollonVirtualMotionSystemCommandBridge>
        {
            public ResetState(ApollonVirtualMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonVirtualMotionSystemCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnAccelerateRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonVirtualMotionSystemCommandBehaviour;

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
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonVirtualMotionSystemCommandBehaviour;

            // keep internal settings

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnDecelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnSaturationRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnSaturationRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnSaturationRequested() : end"
            );

        } /* OnSaturationRequested() */

        private async void OnResetRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnResetRequested() : begin"
            );

            // inject duration
            (this.Behaviour as ApollonVirtualMotionSystemCommandBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class ApollonVirtualMotionSystemCommandBridge */

} /* } Labsim.apollon.gameplay.device.command */