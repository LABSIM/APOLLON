using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Latency phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseB
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseB(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : begin"
            );

            // get a (Unity) bounded random amount of time to wait
            float bounded_random_timeout
                = UnityEngine.Random.Range(
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_lower_bound,
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : will wait " 
                + bounded_random_timeout
                + " ms"
            );

            // wait a certain amout of time between each bound
            await this.FSM.DoSleep(bounded_random_timeout);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseB */

} /* } Labsim.apollon.experiment.phase */
