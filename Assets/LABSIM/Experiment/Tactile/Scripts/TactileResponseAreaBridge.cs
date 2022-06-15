using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileResponseAreaBridge
        : TactileAbstractFiniteStateMachine<TactileResponseAreaBridge>
    {

        //ctor
        public TactileResponseAreaBridge()
            : base()
        { }

        public TactileResponseAreaDispatcher Dispatcher { private set; get; } = null;
        
        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileResponseAreaBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileResponseAreaBridge.WrapBehaviour() : could not find object of type behaviour.TactileResponseAreaBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileResponseAreaDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileManager.IDType WrapID()
        {
            return TactileManager.IDType.TactileResponseArea;
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

                // subscribe to dispatch event
                this.Dispatcher.IdleEvent += this.OnIdleRequested;
                this.Dispatcher.SpatialConditionEvent += this.OnSpatialConditionRequested;
                this.Dispatcher.TemporalConditionEvent += this.OnTemporalConditionRequested;
                this.Dispatcher.SpatioTemporalConditionEvent += this.OnSpatioTemporalConditionRequested;
                
                // go init
                await this.SetState(new IdleState(this));

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // nullify FSM
                await this.SetState(null);

                // unsubscribe from dispatch event
                this.Dispatcher.IdleEvent -= this.OnIdleRequested;
                this.Dispatcher.SpatialConditionEvent -= this.OnSpatialConditionRequested;
                this.Dispatcher.TemporalConditionEvent -= this.OnTemporalConditionRequested;
                this.Dispatcher.SpatioTemporalConditionEvent -= this.OnSpatioTemporalConditionRequested;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region FSM state implementation

        internal sealed class IdleState 
            : TactileAbstractFiniteStateMachineState<TactileResponseAreaBridge>
        {

            public IdleState(TactileResponseAreaBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.IdleState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.IdleState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.IdleState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.IdleState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.IdleController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.IdleState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                } /* if() */

                // inactivate 
                controller.enabled = false;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.IdleState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class IdleState */

        internal sealed class SpatialConditionState 
            : TactileAbstractFiniteStateMachineState<TactileResponseAreaBridge>
        {

            public SpatialConditionState(TactileResponseAreaBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatialConditionState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.SpatialConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.SpatialConditionState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatialConditionState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatialConditionState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.SpatialConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.SpatialConditionState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatialConditionState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class SpatialConditionState */

        internal sealed class TemporalConditionState 
            : TactileAbstractFiniteStateMachineState<TactileResponseAreaBridge>
        {

            public TemporalConditionState(TactileResponseAreaBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.TemporalConditionState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.TemporalConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.TemporalConditionState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.TemporalConditionState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.TemporalConditionState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.TemporalConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.TemporalConditionState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.TemporalConditionState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class TemporalConditionState */

        internal sealed class SpatioTemporalConditionState 
            : TactileAbstractFiniteStateMachineState<TactileResponseAreaBridge>
        {
            public SpatioTemporalConditionState(TactileResponseAreaBridge fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnEntry() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.SpatioTemporalConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnEntry() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // activate 
                controller.enabled = true;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnEntry() : end"
                );

            } /* OnEntry() */

            public override async System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnExit() : begin"
                );

                // find component behaviour
                var controller = this.FSM.Behaviour.gameObject.GetComponent<TactileResponseAreaBehaviour.SpatioTemporalConditionController>();
                if (!controller)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnExit() : could not find controller component behaviour..."
                    );

                    // fail
                    return;

                }

                // inactivate 
                controller.enabled = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBridge.SpatioTemporalConditionState.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class SpatioTemporalConditionState */

        #endregion

        #region FSM event delegate

        private async void OnIdleRequested(object sender, TactileResponseAreaDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnIdleRequested() : begin"
            );

            // activate state
            await this.SetState(new IdleState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnIdleRequested() : end"
            );

        } /* OnIdleRequested() */

        private async void OnSpatialConditionRequested(object sender, TactileResponseAreaDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnSpatialConditionRequested() : begin"
            );

            // activate state
            await this.SetState(new SpatialConditionState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnSpatialConditionRequested() : end"
            );

        } /* OnSpatialConditionRequested() */

        private async void OnTemporalConditionRequested(object sender, TactileResponseAreaDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnTemporalConditionRequested() : begin"
            );

            // activate state
            await this.SetState(new TemporalConditionState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnTemporalConditionRequested() : end"
            );

        } /* OnTemporalConditionRequested() */

        private async void OnSpatioTemporalConditionRequested(object sender, TactileResponseAreaDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnSpatioTemporalConditionRequested() : begin"
            );

            // activate state
            await this.SetState(new SpatioTemporalConditionState(this));

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBridge.OnSpatioTemporalConditionRequested() : end"
            );

        } /* OnSpatioTemporalConditionRequested() */

        #endregion

    } /* class TactileResponseAreaBridge */

} /* } Labsim.experiment.tactile */