
namespace Labsim.apollon.gameplay.device.command
{

    public class ApollonVirtualMotionSystemCommandBridge 
        : gameplay.ApollonAbstractGameplayFiniteStateMachine<ApollonVirtualMotionSystemCommandBridge>
    {

        //ctor
        public ApollonVirtualMotionSystemCommandBridge()
            : base()
        { }

        public ApollonVirtualMotionSystemCommandDispatcher Dispatcher { private set; get; } = null;


        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonVirtualMotionSystemCommandBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonVirtualMotionSystemCommandBridge.WrapBehaviour() : could not find object of type behaviour.ApollonRealRobosoftEntityBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new ApollonVirtualMotionSystemCommandDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

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
                this.Dispatcher.InitEvent += this.OnInitRequested;
                this.Dispatcher.IdleEvent += this.OnIdleRequested;
                this.Dispatcher.AccelerateEvent += this.OnAccelerateRequested;
                this.Dispatcher.DecelerateEvent += this.OnDecelerateRequested;
                this.Dispatcher.SaturationEvent += this.OnSaturationRequested;
                this.Dispatcher.ResetEvent += this.OnResetRequested;

                // activate the motion system backend
                backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(
                    backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
                );

                // go init
                await this.SetState(new InitState(this));

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
                this.Dispatcher.InitEvent -= this.OnInitRequested;
                this.Dispatcher.IdleEvent -= this.OnIdleRequested;
                this.Dispatcher.AccelerateEvent -= this.OnAccelerateRequested;
                this.Dispatcher.DecelerateEvent -= this.OnDecelerateRequested;
                this.Dispatcher.SaturationEvent -= this.OnSaturationRequested;
                this.Dispatcher.ResetEvent -= this.OnResetRequested;

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

        private async void OnInitRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
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

        private async void OnIdleRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
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

        private async void OnAccelerateRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
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

        private async void OnDecelerateRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
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

        private async void OnSaturationRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
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

        private async void OnResetRequested(object sender, ApollonVirtualMotionSystemCommandDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonVirtualMotionSystemCommandBridge.OnResetRequested() : begin"
            );

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