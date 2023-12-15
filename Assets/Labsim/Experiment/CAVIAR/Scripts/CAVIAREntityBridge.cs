
namespace Labsim.experiment.CAVIAR
{

    public class CAVIAREntityBridge 
        : apollon.gameplay.ApollonGameplayBridge<CAVIAREntityBridge>
    {

        //ctor
        public CAVIAREntityBridge()
            : base()
        { }

        public CAVIAREntityBehaviour ConcreteBehaviour 
            => this.Behaviour as CAVIAREntityBehaviour;

        public CAVIAREntityDispatcher ConcreteDispatcher 
            => this.Dispatcher as CAVIAREntityDispatcher;

        #region Bridge abstract implementation 

        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<CAVIAREntityBehaviour>(
                "CAVIAREntityBridge",
                "CAVIAREntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<CAVIAREntityDispatcher>(
                "CAVIAREntityBridge",
                "CAVIAREntityDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity;
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

                // get bridge
                var caviar_control_bridge
                    = (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                        ) as CAVIARControlBridge
                    );
                
                // subscribe
                this.ConcreteDispatcher.AccelerateEvent                         += this.OnAccelerateRequested;
                this.ConcreteDispatcher.DecelerateEvent                         += this.OnDecelerateRequested;
                this.ConcreteDispatcher.HoldEvent                               += this.OnHoldRequested;
                this.ConcreteDispatcher.IdleEvent                               += this.OnIdleRequested;
                caviar_control_bridge.ConcreteDispatcher.AxisZValueChangedEvent += this.OnThrotthleAxisZValueChanged;

                // go init
                await this.SetState(new InitState(this));

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // get bridge
                var caviar_control_bridge
                    = (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                        ) as CAVIARControlBridge
                    );
                
                // unsubscribe
                this.ConcreteDispatcher.AccelerateEvent                         -= this.OnAccelerateRequested;
                this.ConcreteDispatcher.DecelerateEvent                         -= this.OnDecelerateRequested;
                this.ConcreteDispatcher.HoldEvent                               -= this.OnHoldRequested;
                this.ConcreteDispatcher.IdleEvent                               -= this.OnIdleRequested;
                caviar_control_bridge.ConcreteDispatcher.AxisZValueChangedEvent -= this.OnThrotthleAxisZValueChanged;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;
                
            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState 
            : apollon.gameplay.ApollonAbstractGameplayState<CAVIAREntityBridge>
        {

            public InitState(CAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState 
            : apollon.gameplay.ApollonAbstractGameplayState<CAVIAREntityBridge>
        {

            public IdleState(CAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState 
            : apollon.gameplay.ApollonAbstractGameplayState<CAVIAREntityBridge>
        {

            public AccelerateState(CAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState 
            : apollon.gameplay.ApollonAbstractGameplayState<CAVIAREntityBridge>
        {

            public DecelerateState(CAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.DecelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState 
            : apollon.gameplay.ApollonAbstractGameplayState<CAVIAREntityBridge>
        {
            public HoldState(CAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<CAVIAREntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIAREntityBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        #endregion

        #region FSM event delegate

        private void OnThrotthleAxisZValueChanged(object sender, CAVIARControlDispatcher.CAVIARControlEventArgs args) 
        {

            // extract event args, get behaviour & update altitude
            (this.Behaviour as CAVIAREntityBehaviour).SetUserThrottleAxisZValue(args.Z);

        } /* OnThrotthleAxisZValueChanged() */

        private async void OnAccelerateRequested(object sender, CAVIAREntityDispatcher.CAVIAREntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as CAVIAREntityBehaviour;

            // set internal settings
            behaviour.TargetLinearAcceleration = UnityEngine.Vector3.forward * args.LinearAcceleration;
            behaviour.TargetLinearVelocity = UnityEngine.Vector3.forward * args.LinearVelocity;

            // activate state
            await this.SetState(new AccelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, CAVIAREntityDispatcher.CAVIAREntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as CAVIAREntityBehaviour;

            // set internal settings
            behaviour.TargetLinearAcceleration = UnityEngine.Vector3.back * args.LinearAcceleration;
            behaviour.TargetLinearVelocity = UnityEngine.Vector3.forward * args.LinearVelocity;

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnDecelerateRequested() : end"
            );

        } /* OnDecelerateRequested() */

        private async void OnIdleRequested(object sender, CAVIAREntityDispatcher.CAVIAREntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnHoldRequested(object sender, CAVIAREntityDispatcher.CAVIAREntityEventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnHoldRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.OnHoldRequested() : end"
            );

        } /* OnHoldRequested() */

        #endregion

        #region Tasks

        public async System.Threading.Tasks.Task DoNotifyWhenWaypointReached(float distance_to_reach, bool bRaiseEventDispatcher = true)
        {
        
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.DoNotifyWhenWaypointReached() : begin, waiting distance["
                + distance_to_reach
                + "]"
                + (bRaiseEventDispatcher ? " with event dispatch" : " silently")
            );

            // wait until waypoint reached
            while ( (this.Behaviour as CAVIAREntityBehaviour).Reference.transform.position.z < distance_to_reach )
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

            if(bRaiseEventDispatcher)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> CAVIAREntityBridge.DoNotifyWhenWaypointReached() : waypoint reached, notifying"
                );

                // notifying
                this.ConcreteDispatcher.RaiseWaypointReached();
            }
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIAREntityBridge.DoNotifyWhenWaypointReached() : end"
            );

        } /* DoNotifyWhenWaypointReached() */

        #endregion

    }  /* class CAVIAREntityBridge */

} /* } Labsim.apollon.gameplay.entity */