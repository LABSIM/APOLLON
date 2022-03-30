using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseB
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseB(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get a (Unity) bounded random amount of time to wait
            float bounded_random_timeout
                = UnityEngine.Random.Range(
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_lower_bound,
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseB.OnEntry() : will wait " 
                + bounded_random_timeout
                + " ms"
            );

            // wait a certain amout of time between each bound
            await this.FSM.DoSleep(bounded_random_timeout);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseB.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseB */

} /* } Labsim.experiment.tactile */