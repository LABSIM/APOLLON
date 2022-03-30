using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseC
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseC(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : begin"
            );
 
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseC */

} /* } Labsim.experiment.tactile */