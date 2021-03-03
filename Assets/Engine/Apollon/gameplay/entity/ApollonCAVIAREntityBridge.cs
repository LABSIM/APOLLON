
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonCAVIAREntityBridge 
        : ApollonAbstractGameplayFiniteStateMachine<ApollonCAVIAREntityBridge>
    {

        //ctor
        public ApollonCAVIAREntityBridge()
            : base()
        { }

        public ApollonCAVIAREntityDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonCAVIAREntityBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.WrapBehaviour() : could not find object of type behaviour.ApollonRealRobosoftEntityBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new ApollonCAVIAREntityDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.CAVIAREntity;
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
                var hotas_throtthle_bridge
                    = (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    );
                
                // subscribe
                this.Dispatcher.AccelerateEvent += this.OnAccelerateRequested;
                this.Dispatcher.DecelerateEvent += this.OnDecelerateRequested;
                this.Dispatcher.HoldEvent += this.OnHoldRequested;
                this.Dispatcher.IdleEvent += this.OnIdleRequested;
                hotas_throtthle_bridge.Dispatcher.AxisZValueChangedEvent += this.OnThrotthleAxisZValueChanged;

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
                var hotas_throtthle_bridge
                    = (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    );
                    
                // unsubscribe
                hotas_throtthle_bridge.Dispatcher.AxisZValueChangedEvent -= this.OnThrotthleAxisZValueChanged;
                this.Dispatcher.AccelerateEvent -= this.OnAccelerateRequested;
                this.Dispatcher.DecelerateEvent -= this.OnDecelerateRequested;
                this.Dispatcher.HoldEvent -= this.OnHoldRequested;
                this.Dispatcher.IdleEvent -= this.OnIdleRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class InitState 
            : ApollonAbstractGameplayState<ApollonCAVIAREntityBridge>
        {

            public InitState(ApollonCAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.InitState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.InitState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.InitState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.InitState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.InitController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.InitState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.InitState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class InitState */

        internal sealed class IdleState 
            : ApollonAbstractGameplayState<ApollonCAVIAREntityBridge>
        {

            public IdleState(ApollonCAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState 
            : ApollonAbstractGameplayState<ApollonCAVIAREntityBridge>
        {

            public AccelerateState(ApollonCAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class DecelerateState 
            : ApollonAbstractGameplayState<ApollonCAVIAREntityBridge>
        {

            public DecelerateState(ApollonCAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DecelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.DecelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DecelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DecelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.DecelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.DecelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DecelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class DecelerateState */

        internal sealed class HoldState 
            : ApollonAbstractGameplayState<ApollonCAVIAREntityBridge>
        {
            public HoldState(ApollonCAVIAREntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonCAVIAREntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIAREntityBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        #endregion

        #region FSM event delegate

        private void OnThrotthleAxisZValueChanged(object sender, device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs args)
        {

            // get behaviour & update altitude
            (this.Behaviour as ApollonCAVIAREntityBehaviour).SetAltitudeFromThrottleAxisZValue(args.Z);

        } /* OnThrotthleAxisZValueChanged() */

        private async void OnAccelerateRequested(object sender, ApollonCAVIAREntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonCAVIAREntityBehaviour;

            // set internal settings
            behaviour.TargetLinearAcceleration = UnityEngine.Vector3.forward * args.LinearAcceleration;
            behaviour.TargetLinearVelocity = UnityEngine.Vector3.forward * args.LinearVelocity;

            // activate state
            await this.SetState(new AccelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnDecelerateRequested(object sender, ApollonCAVIAREntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnDecelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonCAVIAREntityBehaviour;

            // set internal settings
            behaviour.TargetLinearAcceleration = UnityEngine.Vector3.forward * args.LinearAcceleration;
            behaviour.TargetLinearVelocity = UnityEngine.Vector3.forward * args.LinearVelocity;

            // activate state
            await this.SetState(new DecelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnDecelerateRequested() : end"
            );

        } /* OnDecelerateRequested() */

        private async void OnIdleRequested(object sender, ApollonCAVIAREntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnHoldRequested(object sender, ApollonCAVIAREntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnHoldRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.OnHoldRequested() : end"
            );

        } /* OnHoldRequested() */

        #endregion

        #region Tasks

        public async System.Threading.Tasks.Task DoNotifyWhenWaypointReached(float distance_to_reach, bool bRaiseEventDispatcher = true)
        {
        
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DoNotifyWhenWaypointReached() : begin, waiting distance["
                + distance_to_reach
                + "]"
                + (bRaiseEventDispatcher ? " with event dispatch" : " silently")
            );

            // wait until waypoint reached
            while ( (this.Behaviour as ApollonCAVIAREntityBehaviour).Reference.transform.position.z < distance_to_reach )
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

            if(bRaiseEventDispatcher)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DoNotifyWhenWaypointReached() : waypoint reached, notifying"
                );

                // notifying
                this.Dispatcher.RaiseWaypointReached();
            }
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIAREntityBridge.DoNotifyWhenWaypointReached() : end"
            );

        } /* DoNotifyWhenWaypointReached() */

        #endregion

    }  /* class ApollonCAVIAREntityBridge */

} /* } Labsim.apollon.gameplay.entity */