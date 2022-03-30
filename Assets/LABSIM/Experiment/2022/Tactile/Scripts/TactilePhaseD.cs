using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseD
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseD(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseD.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // fade out asynchronously from black for all scenario
            this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            // show red cross
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setActive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_D_settings.duration / 2.0f);

            // show red frame
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setActive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_D_settings.duration / 2.0f);

            // hide red cross & frame
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setInactive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setInactive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseD.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseD */

} /* } Labsim.experiment.tactile */