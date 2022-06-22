using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseA
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseA(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // show grey(active)/green(passive) cross, green frame & counter if active
            if (this.FSM.CurrentSettings.bIsActive) 
            {
                Labsim.apollon.frontend.ApollonFrontendManager.Instance.setActive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            } 
            else
            {
                Labsim.apollon.frontend.ApollonFrontendManager.Instance.setActive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            }
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setActive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.duration / 2.0f);

            // hide green frame first
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setInactive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // fade in to black asynchronously for all scenario condition
            this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.duration / 2.0f);

            // then hide green cross
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setInactive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseA.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseA */

} /* } Labsim.experiment.tactile */