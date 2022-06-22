using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseE
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseE(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseE.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            void sync_local_function(object sender, TactileValidateButtonDispatcher.EventArgs e)
                => sync_point?.TrySetResult(true);

            // register our synchronisation function
            (
                TactileManager.Instance.getBridge(
                    TactileManager.IDType.TactileValidateButton
                ) as TactileValidateButtonBridge
            ).Dispatcher.PressedEvent += sync_local_function;
            
            // show tactile surface + reponse area + buttons
            TactileManager.Instance.setActive(TactileManager.IDType.TactileSurfaceEntity);
            TactileManager.Instance.setActive(TactileManager.IDType.TactileResponseArea);
            TactileManager.Instance.setActive(TactileManager.IDType.TactileValidateButton);
            TactileManager.Instance.setActive(TactileManager.IDType.TactileRevertButton);

            // wait for user response validation
            await sync_point.Task;

            // hide response area & button
            TactileManager.Instance.setInactive(TactileManager.IDType.TactileResponseArea);
            TactileManager.Instance.setInactive(TactileManager.IDType.TactileValidateButton);
            TactileManager.Instance.setInactive(TactileManager.IDType.TactileRevertButton);

            // unregister our synchronisation function
            (
                TactileManager.Instance.getBridge(
                    TactileManager.IDType.TactileValidateButton
                ) as TactileValidateButtonBridge
            ).Dispatcher.PressedEvent += sync_local_function;
            
            // get results
            var behaviour = TactileManager.Instance.getBridge(TactileManager.IDType.TactileResponseArea).Behaviour as TactileResponseAreaBehaviour;
            foreach(var touchpoint in behaviour.TouchpointList)
            {

                // inject results
                this.FSM.CurrentResults.phase_E_results.user_response.Add(
                    new TactileProfile.Results.PhaseEResult.Touchpoint {
                        host_timestamp = touchpoint.HostTimestamp,
                        unity_timestamp = touchpoint.UnityTimestamp,
                        x = touchpoint.X,
                        y = touchpoint.Y
                    }
                );

            } /* foreach() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseE.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseE.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseE.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseE */

} /* } Labsim.experiment.tactile */