using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // FSM state
    //
    public sealed class ApollonTactilePhaseB
        : ApollonAbstractExperimentState<profile.ApollonTactileProfile>
    {
        public ApollonTactilePhaseB(profile.ApollonTactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseB.OnEntry() : begin"
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
                "<color=Blue>Info: </color> ApollonTactilePhaseB.OnEntry() : will wait " 
                + bounded_random_timeout
                + " ms"
            );

            // wait a certain amout of time between each bound
            await this.FSM.DoSleep(bounded_random_timeout);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseB.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonTactilePhaseB */

} /* } Labsim.apollon.experiment.phase */