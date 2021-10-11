
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonActiveSeatEntityBridge : ApollonAbstractGameplayFiniteStateMachine<ApollonActiveSeatEntityBridge>
    {

        //ctor
        public ApollonActiveSeatEntityBridge()
            : base()
        { }

        public ApollonActiveSeatEntityDispatcher Dispatcher { private set; get; } = null;


        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonActiveSeatEntityBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.WrapBehaviour() : could not find object of type behaviour.ApollonRealRobosoftEntityBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new ApollonActiveSeatEntityDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.ActiveSeatEntity;
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
                //await System.Threading.Tasks.Task.Run(
                //    async () =>
                //    {
                //        while (!this.Behaviour.isActiveAndEnabled) await System.Threading.Tasks.Task.Delay(10);
                //    }
                //);
                
                // subscribe
                this.Dispatcher.AccelerateEvent += this.OnAccelerateRequested;
                this.Dispatcher.InteruptEvent += this.OnInteruptRequested;
                this.Dispatcher.SaturationEvent += this.OnSaturationRequested;
                this.Dispatcher.StopEvent += this.OnStopRequested;
                this.Dispatcher.ResetEvent += this.OnResetRequested;

                // go idle
                await this.SetState(new IdleState(this));

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // unsubscribe
                this.Dispatcher.AccelerateEvent -= this.OnAccelerateRequested;
                this.Dispatcher.InteruptEvent -= this.OnInteruptRequested;
                this.Dispatcher.SaturationEvent -= this.OnSaturationRequested;
                this.Dispatcher.StopEvent -= this.OnStopRequested;
                this.Dispatcher.ResetEvent -= this.OnResetRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;
                //await System.Threading.Tasks.Task.Run(
                //     async () =>
                //     {
                //         while (this.Behaviour.isActiveAndEnabled) await System.Threading.Tasks.Task.Delay(10);
                //     }
                // );

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class IdleState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {

            public IdleState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class AccelerateState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {

            public AccelerateState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.AccelerateController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.AccelerateState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class AccelerateState */

        internal sealed class HoldState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {
            public HoldState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.HoldState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.HoldState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.HoldState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.HoldState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.HoldController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.HoldState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.HoldState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Hold */

        internal sealed class StopState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {
            public StopState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.StopState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.StopController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.StopState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.StopState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.StopState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.StopController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.StopState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.StopState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Stop */

        internal sealed class ResetState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {
            public ResetState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.ResetState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.ResetState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.ResetState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.ResetState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.ResetController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.ResetState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.ResetState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Reset */

        #endregion

        #region FSM event delegate

        private async void OnAccelerateRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnAccelerateRequested() : begin"
            );

            // get behaviour
            var behaviour = this.Behaviour as ApollonActiveSeatEntityBehaviour;

            // set internal settings
            behaviour.AngularVelocitySaturation = UnityEngine.Vector3.right * UnityEngine.Mathf.Deg2Rad * args.AngularVelocitySaturation;
            behaviour.AngularAcceleration = UnityEngine.Vector3.right * UnityEngine.Mathf.Deg2Rad * args.AngularAcceleration;
            behaviour.Duration = args.Duration;
            behaviour.StopAngle = args.StopAngle;
            behaviour.InhibitVestibularMotion = args.InhibitVestibularMotion;

            // activate state
            await this.SetState(new AccelerateState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnAccelerateRequested() : end"
            );

        } /* OnAccelerateRequested() */

        private async void OnSaturationRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnSaturationRequested() : begin"
            );

            // activate state
            await this.SetState(new HoldState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnSaturationRequested() : end"
            );

        } /* OnSaturationRequested() */

        private async void OnInteruptRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnInteruptRequested() : begin"
            );
            
            // activate state
            await this.SetState(new StopState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnInteruptRequested() : end"
            );

        } /* OnInteruptRequested() */

        private async void OnStopRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnStopRequested() : begin"
            );

            // activate state
            await this.SetState(new StopState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnStopRequested() : end"
            );

        } /* OnStopRequested() */

        private async void OnResetRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnResetRequested() : begin"
            );

            // activate state
            await this.SetState(new ResetState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnResetRequested() : end"
            );

        } /* OnStopRequested() */

        #endregion

    }  /* class ApollonActiveSeatEntityBridge */

} /* } Labsim.apollon.gameplay.entity */