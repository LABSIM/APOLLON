
namespace Labsim.apollon.gameplay.device.command
{

    public class ApollonMotionSystemCommandBridge 
        : ApollonGameplayBridge<ApollonMotionSystemCommandBridge>
    {

        //ctor
        public ApollonMotionSystemCommandBridge()
            : base()
        { }

        public ApollonMotionSystemCommandBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonMotionSystemCommandBehaviour;

        public ApollonMotionSystemCommandDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonMotionSystemCommandDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonMotionSystemCommandBehaviour>(
                "ApollonMotionSystemCommandBridge",
                "ApollonMotionSystemCommandBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonMotionSystemCommandDispatcher>(
                "ApollonMotionSystemCommandBridge",
                "ApollonMotionSystemCommandDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.MotionSystemCommand;
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
                this.ConcreteDispatcher.ResetEvent      -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {

            public InitState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {

            public IdleState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {

            public AccelerateState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {

            public DecelerateState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {
            public HoldState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        internal sealed class ResetState : ApollonAbstractGameplayState<ApollonMotionSystemCommandBridge>
        {
            public ResetState(ApollonMotionSystemCommandBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonMotionSystemCommandBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonMotionSystemCommandBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnInitRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnInitRequested() : begin"
            );

            // activate state
            await this.SetState(new InitState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnInitRequested() : end"
            );

        } /* OnInitRequested() */

        private async void OnIdleRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnAccelerateRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonMotionSystemCommandBehaviour;

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
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonMotionSystemCommandBehaviour;

            // keep internal settings

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnDecelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnSaturationRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnSaturationRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnSaturationRequested() : end"
            );

        } /* OnSaturationRequested() */

        private async void OnResetRequested(object sender, ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnResetRequested() : begin"
            );
            
            // inject duration
            (this.Behaviour as ApollonMotionSystemCommandBehaviour).Duration = args.Duration;

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonMotionSystemCommandBridge.OnResetRequested() : end"
            );

        } /* OnResetRequested() */

        #endregion

    }  /* class ApollonMotionSystemCommandBridge */

} /* } Labsim.apollon.gameplay.device.command */