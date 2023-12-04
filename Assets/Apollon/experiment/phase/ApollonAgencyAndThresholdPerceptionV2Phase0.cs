using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2Phase0 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2Phase0(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Phase0.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = (this.FSM.CurrentSettings.bIsActive ? "Condition manuelle" : "Condition automatique");

            // show grey cross & frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
                void sync_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs e)
                    => sync_point?.TrySetResult(true);

                // register our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent += sync_local_function;

                // wait synchronisation point indefinitely & reset it once hit
                await sync_point.Task;

                // unregister our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent -= sync_local_function;

            } 
            else 
            {

                // wait a certain amout of time
                await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            } /* if() */

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Déplacement 1";

            // hide grey frame/cross & show green frame/cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            // finally, hide green frame/cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Phase0.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Phase0.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Phase0.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2Phase0 */

} /* } Labsim.apollon.experiment.phase */
