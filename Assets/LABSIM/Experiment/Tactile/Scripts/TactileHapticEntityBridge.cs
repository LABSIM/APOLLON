using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileHapticEntityBridge
        : TactileAbstractFiniteStateMachine<TactileHapticEntityBridge>
    {

        //ctor
        public TactileHapticEntityBridge()
            : base()
        { }

        public TactileHapticEntityDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileHapticEntityBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileHapticEntityBridge.WrapBehaviour() : could not find object of type behaviour.TactileHapticEntityBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileHapticEntityDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileManager.IDType WrapID()
        {
            return TactileManager.IDType.TactileHapticEntity;
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

                // bind
                this.Dispatcher.RequestStimCCEvent += this.OnStimCCRequested;
                this.Dispatcher.RequestStimCVEvent += this.OnStimCVRequested;
                this.Dispatcher.RequestStimVCEvent += this.OnStimVCRequested;
                this.Dispatcher.RequestStimVVEvent += this.OnStimVVRequested;
                
                // go idle
                await this.SetState(new IdleState(this));

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // go idle
                await this.SetState(new IdleState(this));

                // unbind
                this.Dispatcher.RequestStimCCEvent -= this.OnStimCCRequested;
                this.Dispatcher.RequestStimCVEvent -= this.OnStimCVRequested;
                this.Dispatcher.RequestStimVCEvent -= this.OnStimVCRequested;
                this.Dispatcher.RequestStimVVEvent -= this.OnStimVVRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class IdleState 
            : TactileAbstractFiniteStateMachineState<TactileHapticEntityBridge>
        {

            public IdleState(TactileHapticEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class StimCCState 
            : TactileAbstractFiniteStateMachineState<TactileHapticEntityBridge>
        {

            public StimCCState(TactileHapticEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCCState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimCCController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimCCState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCCState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCCState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimCCController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimCCState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCCState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class StimCCState */

        internal sealed class StimCVState 
            : TactileAbstractFiniteStateMachineState<TactileHapticEntityBridge>
        {

            public StimCVState(TactileHapticEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCVState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimCVController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimCVState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCVState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCVState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimCVController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimCVState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimCVState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class StimCVState */

        internal sealed class StimVCState 
            : TactileAbstractFiniteStateMachineState<TactileHapticEntityBridge>
        {

            public StimVCState(TactileHapticEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVCState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimVCController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimVCState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVCState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVCState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimVCController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimVCState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVCState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class StimVCState */

        internal sealed class StimVVState 
            : TactileAbstractFiniteStateMachineState<TactileHapticEntityBridge>
        {

            public StimVVState(TactileHapticEntityBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVVState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimVVController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimVVState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVVState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVVState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileHapticEntityBehaviour.StimVVController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileHapticEntityBridge.StimVVState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBridge.StimVVState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class StimVVState */

        #endregion

        #region FSM event delegate

        private async void OnStimCCRequested(object sender, TactileHapticEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimCCRequested() : begin"
            );

            // activate state
            await this.SetState(new StimCCState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimCCRequested() : end"
            );

        } /* OnStimCCRequested() */

        private async void OnStimCVRequested(object sender, TactileHapticEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimCVRequested() : begin"
            );

            // activate state
            await this.SetState(new StimCVState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimCVRequested() : end"
            );

        } /* OnStimCVRequested() */

        private async void OnStimVCRequested(object sender, TactileHapticEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimVCRequested() : begin"
            );

            // activate state
            await this.SetState(new StimVCState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimVCRequested() : end"
            );

        } /* OnStimVCRequested() */

        private async void OnStimVVRequested(object sender, TactileHapticEntityDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimVVRequested() : begin"
            );

            // activate state
            await this.SetState(new StimVVState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHapticEntityBridge.OnStimVVRequested() : end"
            );

        } /* OnStimVVRequested() */

        #endregion

    } /* class TactileHapticEntityBridge */

} /* } Labsim.experiment.tactile */