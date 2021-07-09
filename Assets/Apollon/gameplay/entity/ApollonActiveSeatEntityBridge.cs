
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonActiveSeatEntityBridge 
        : ApollonAbstractGameplayFiniteStateMachine<ApollonActiveSeatEntityBridge>
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
                
                // subscribe
                this.Dispatcher.IdleEvent += this.OnIdleRequested;
                this.Dispatcher.VisualOnlyEvent += this.OnVisualOnlyRequested;
                this.Dispatcher.VestibularOnlyEvent += this.OnVestibularOnlyRequested;
                this.Dispatcher.VisuoVestibularEvent += this.OnVisuoVestibularRequested;

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
                this.Dispatcher.IdleEvent -= this.OnIdleRequested;
                this.Dispatcher.VisualOnlyEvent -= this.OnVisualOnlyRequested;
                this.Dispatcher.VestibularOnlyEvent -= this.OnVestibularOnlyRequested;
                this.Dispatcher.VisuoVestibularEvent -= this.OnVisuoVestibularRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

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

        internal sealed class VisualOnlyState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {

            public VisualOnlyState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VisualOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VisualOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisualOnlyState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class VisualOnlyState */

        internal sealed class VestibularOnlyState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {

            public VestibularOnlyState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VestibularOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VestibularOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VestibularOnlyState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class VestibularOnlyState */

        internal sealed class VisuoVestibularState : ApollonAbstractGameplayState<ApollonActiveSeatEntityBridge>
        {

            public VisuoVestibularState(ApollonActiveSeatEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VestibularOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<ApollonActiveSeatEntityBehaviour.VestibularOnlyController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.VisuoVestibularState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class VisuoVestibularState */

        #endregion

        #region FSM event delegate

        private async void OnIdleRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnVisualOnlyRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVisualOnlyRequested() : begin"
            );

            // activate state
            await this.SetState(new VisualOnlyState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVisualOnlyRequested() : end"
            );

        } /* OnVisualOnlyRequested() */

        private async void OnVestibularOnlyRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVestibularOnlyRequested() : begin"
            );

            // activate state
            await this.SetState(new VestibularOnlyState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVestibularOnlyRequested() : end"
            );

        } /* OnVestibularOnlyRequested() */

        private async void OnVisuoVestibularRequested(object sender, ApollonActiveSeatEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVisuoVestibularRequested() : begin"
            );

            // activate state
            await this.SetState(new VisuoVestibularState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatEntityBridge.OnVisuoVestibularRequested() : end"
            );

        } /* OnVisuoVestibularRequested() */

        #endregion

    }  /* class ApollonActiveSeatEntityBridge */

} /* } Labsim.apollon.gameplay.entity */