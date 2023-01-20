using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Latency phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseD
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseD(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseD.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
            
            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Déplacement 2";

            // hide grey frame/cross & show green frame/cross
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            // finally, hide green frame/cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseD.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseD */

} /* } Labsim.apollon.experiment.phase */
