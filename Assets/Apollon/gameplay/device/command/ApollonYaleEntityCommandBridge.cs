
namespace Labsim.apollon.gameplay.device.command
{

    public class ApollonYaleEntityCommandBridge 
        : ApollonGameplayBridge<ApollonYaleEntityCommandBridge>
    {

        //ctor
        public ApollonYaleEntityCommandBridge()
            : base()
        { }

        public ApollonYaleEntityCommandBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonYaleEntityCommandBehaviour;

        public ApollonYaleEntityCommandDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonYaleEntityCommandDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonYaleEntityCommandBehaviour>(
                "ApollonYaleEntityCommandBridge",
                "ApollonYaleEntityCommandBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonYaleEntityCommandDispatcher>(
                "ApollonYaleEntityCommandBridge",
                "ApollonYaleEntityCommandDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.YaleEntityCommand;
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
                this.ConcreteDispatcher.ControlEvent    -= this.OnControlRequested;
                this.ConcreteDispatcher.ResetEvent      -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState : ApollonAbstractGameplayState<ApollonYaleEntityCommandBridge>
        {

            public InitState(ApollonYaleEntityCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState : ApollonAbstractGameplayState<ApollonYaleEntityCommandBridge>
        {

            public IdleState(ApollonYaleEntityCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class ControlState : ApollonAbstractGameplayState<ApollonYaleEntityCommandBridge>
        {

            public ControlState(ApollonYaleEntityCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ControlState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.ControlState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ControlState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ControlState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.ControlController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.ControlState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ControlState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class ResetState : ApollonAbstractGameplayState<ApollonYaleEntityCommandBridge>
        {
            public ResetState(ApollonYaleEntityCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonYaleEntityCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonYaleEntityCommandBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, ApollonYaleEntityCommandDispatcher.YaleEntityCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, ApollonYaleEntityCommandDispatcher.YaleEntityCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnControlRequested(object sender, ApollonYaleEntityCommandDispatcher.YaleEntityCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnControlRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonYaleEntityCommandBehaviour;

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
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnControlRequested() : end"
            );

        } /* OnControlRequested() */
        private async void OnResetRequested(object sender, ApollonYaleEntityCommandDispatcher.YaleEntityCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnResetRequested() : begin"
            );
            
            // inject duration
            (this.Behaviour as ApollonYaleEntityCommandBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonYaleEntityCommandBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class ApollonYaleEntityCommandBridge */

} /* } Labsim.apollon.gameplay.device.command */