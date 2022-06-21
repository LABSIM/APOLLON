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