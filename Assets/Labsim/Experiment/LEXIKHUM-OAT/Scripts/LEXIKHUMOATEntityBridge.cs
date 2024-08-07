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

    public class LEXIKHUMOATEntityBridge 
        : apollon.gameplay.ApollonGameplayBridge<LEXIKHUMOATEntityBridge>
    {

        //ctor
        public LEXIKHUMOATEntityBridge()
            : base()
        { }

        public LEXIKHUMOATEntityBehaviour ConcreteBehaviour 
            => this.Behaviour as LEXIKHUMOATEntityBehaviour;

        public LEXIKHUMOATEntityDispatcher ConcreteDispatcher 
            => this.Dispatcher as LEXIKHUMOATEntityDispatcher;

        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<LEXIKHUMOATEntityBehaviour>(
                "LEXIKHUMOATEntityBridge",
                "LEXIKHUMOATEntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<LEXIKHUMOATEntityDispatcher>(
                "LEXIKHUMOATEntityBridge",
                "LEXIKHUMOATEntityDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATEntity;
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
                this.ConcreteDispatcher.InitEvent    += this.OnInitRequested;
                this.ConcreteDispatcher.HoldEvent    += this.OnHoldRequested;
                this.ConcreteDispatcher.ControlEvent += this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent   += this.OnResetRequested;

                // nullify FSM
                await this.SetState(null);

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // unsubscribe
                this.ConcreteDispatcher.InitEvent    -= this.OnInitRequested;
                this.ConcreteDispatcher.HoldEvent    -= this.OnHoldRequested;
                this.ConcreteDispatcher.ControlEvent -= this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent   -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState 
            : apollon.gameplay.ApollonAbstractGameplayState<LEXIKHUMOATEntityBridge>
        {

            public InitState(LEXIKHUMOATEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class HoldState 
            : apollon.gameplay.ApollonAbstractGameplayState<LEXIKHUMOATEntityBridge>
        {

            public HoldState(LEXIKHUMOATEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class HoldState */

        internal sealed class ControlState 
            : apollon.gameplay.ApollonAbstractGameplayState<LEXIKHUMOATEntityBridge>
        {

            public ControlState(LEXIKHUMOATEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ControlState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.ControlState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ControlState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ControlState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.ControlState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ControlState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class ControlState */

        internal sealed class ResetState 
            : apollon.gameplay.ApollonAbstractGameplayState<LEXIKHUMOATEntityBridge>
        {
            public ResetState(LEXIKHUMOATEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> LEXIKHUMOATEntityBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, LEXIKHUMOATEntityDispatcher.LEXIKHUMOATEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnInitRequested() : begin"
            );

            // get behaviour
            var behaviour = this.ConcreteBehaviour;

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
            behaviour.Duration = args.Duration;

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnHoldRequested(object sender, LEXIKHUMOATEntityDispatcher.LEXIKHUMOATEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnHoldRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnHoldRequested() : end"
            );

        } /* OnHoldRequested() */

        private async void OnControlRequested(object sender, LEXIKHUMOATEntityDispatcher.LEXIKHUMOATEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnControlRequested() : begin"
            );

            // activate state
            await this.SetState(new ControlState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnControlRequested() : end"
            );

        } /* OnControlRequested() */

        private async void OnResetRequested(object sender, LEXIKHUMOATEntityDispatcher.LEXIKHUMOATEntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnResetRequested() : begin"
            );

            // get behaviour
            var behaviour = this.ConcreteBehaviour;

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
            behaviour.Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATEntityBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class LEXIKHUMOATEntityBridge */

} /* } Labsim.experiment.LEXIKHUM_OAT */