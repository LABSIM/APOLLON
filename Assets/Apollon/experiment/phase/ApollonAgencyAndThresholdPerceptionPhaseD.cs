using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{
    
    //
    // End phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseD
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseD(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseD.OnEntry() : begin"
            );
                
            // get bridge
            var motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            // check
            if (motion_system_bridge == null || virtual_motion_system_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionPhaseD.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */
                
            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // inactivate HOTAS
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl);

            // show red cross
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

            // show red frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

            // hide red cross & frame
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.Dispatcher.RaiseReset();
                    break;
                }
                case profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly:
                case profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    motion_system_bridge.Dispatcher.RaiseReset();
                    break;
                }

            } /* switch() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseD.OnExit() : begin"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseD */

} /* } Labsim.apollon.experiment.phase */
