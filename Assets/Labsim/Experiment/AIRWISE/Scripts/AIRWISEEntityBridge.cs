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

namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEEntityBridge 
        : apollon.gameplay.ApollonGameplayBridge<AIRWISEEntityBridge>
    {

        //ctor
        public AIRWISEEntityBridge()
            : base()
        { }

        public AIRWISEEntityBehaviour ConcreteBehaviour 
            => this.Behaviour as AIRWISEEntityBehaviour;

        public AIRWISEEntityDispatcher ConcreteDispatcher 
            => this.Dispatcher as AIRWISEEntityDispatcher;

        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<AIRWISEEntityBehaviour>(
                "AIRWISEEntityBridge",
                "AIRWISEEntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<AIRWISEEntityDispatcher>(
                "AIRWISEEntityBridge",
                "AIRWISEEntityDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity;
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
                this.ConcreteDispatcher.ControlEvent    += this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent      += this.OnResetRequested;

                // activate the motion system backend
                apollon.backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
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
                apollon.backend.ApollonBackendManager.Instance.RaiseHandleDeactivationRequestedEvent(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
                );

                // unsubscribe
                this.ConcreteDispatcher.InitEvent       -= this.OnInitRequested;
                this.ConcreteDispatcher.IdleEvent       -= this.OnIdleRequested;
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
            : apollon.gameplay.ApollonAbstractGameplayState<AIRWISEEntityBridge>
        {

            public InitState(AIRWISEEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState 
            : apollon.gameplay.ApollonAbstractGameplayState<AIRWISEEntityBridge>
        {

            public IdleState(AIRWISEEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class ControlState 
            : apollon.gameplay.ApollonAbstractGameplayState<AIRWISEEntityBridge>
        {

            public ControlState(AIRWISEEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ControlState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.ControlState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ControlState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ControlState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.ControlState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ControlState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class ResetState 
            : apollon.gameplay.ApollonAbstractGameplayState<AIRWISEEntityBridge>
        {
            public ResetState(AIRWISEEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<AIRWISEEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> AIRWISEEntityBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEEntityBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, AIRWISEEntityDispatcher.AIRWISEEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, AIRWISEEntityDispatcher.AIRWISEEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnControlRequested(object sender, AIRWISEEntityDispatcher.AIRWISEEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnControlRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as AIRWISEEntityBehaviour;

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
        private async void OnResetRequested(object sender, AIRWISEEntityDispatcher.AIRWISEEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnResetRequested() : begin"
            );
            
            // inject duration
            (this.Behaviour as AIRWISEEntityBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEEntityBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class AIRWISEEntityBridge */

} /* } Labsim.experiment.AIRWISE */