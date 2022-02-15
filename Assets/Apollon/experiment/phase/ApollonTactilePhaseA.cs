using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // FSM state
    //
    public sealed class ApollonTactilePhaseA
        : ApollonAbstractExperimentState<profile.ApollonTactileProfile>
    {
        public ApollonTactilePhaseA(profile.ApollonTactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // show grey(active)/green(passive) cross, green frame & counter if active
            if (this.FSM.CurrentSettings.bIsActive) 
            {
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            } 
            else
            {
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            }
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // hide green frame first
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // fade in to black asynchronously for all scenario condition
            this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // then hide green cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseA.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonTactilePhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonTactilePhaseA */

} /* } Labsim.apollon.experiment.phase */