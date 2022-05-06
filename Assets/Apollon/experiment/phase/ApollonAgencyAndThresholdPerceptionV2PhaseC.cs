using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Latency phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseC
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseC(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseC.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get a (Unity) bounded random amount of time to wait
            float bounded_random_timeout
                = UnityEngine.Random.Range(
                    this.FSM.CurrentSettings.phase_C_settings.inter_stim_timeout[0],
                    this.FSM.CurrentSettings.phase_C_settings.inter_stim_timeout[1]
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseC.OnEntry() : will wait " 
                + bounded_random_timeout
                + " ms"
            );

            // wait a certain amout of time between each bound
            await this.FSM.DoSleep(bounded_random_timeout);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseC.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseC */

} /* } Labsim.apollon.experiment.phase */
