using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Latency phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV4PhaseC
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV4Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV4PhaseC(profile.ApollonAgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4PhaseC.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get bridges

            var physical_motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.device.command.ApollonMotionSystemCommandBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                );

            var virtual_motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.device.command.ApollonVirtualMotionSystemCommandBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                );

            // current scenario
            bool bHasRealMotion = false; 
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case profile.ApollonAgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    bHasRealMotion = false;
                    break;
                }
                case profile.ApollonAgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly:
                case profile.ApollonAgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    bHasRealMotion = true;
                    break;
                }

            } /* switch() */

            // check
            if (physical_motion_system_bridge == null || virtual_motion_system_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionV4PhaseC.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */
            
            // decelerate
            virtual_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();
            physical_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();

            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration);

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Fin";

            // hide grey frame/cross & show green frame/cross
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait remaining time
            await ApollonHighResolutionTime.DoSleep(
                UnityEngine.Mathf.Clamp(
                    (
                        this.FSM.CurrentSettings.phase_C_settings.duration
                        - this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration
                    ),
                    0.0f,
                    float.MaxValue
                )
            );

            // finally, hide green frame/cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // inhibit visual degradation
            if(this.FSM.CurrentSettings.bWithVisualDegradation)
            {

                // reset fog
                UnityEngine.RenderSettings.fog              = true;
                UnityEngine.RenderSettings.fogMode          = UnityEngine.FogMode.ExponentialSquared;
                UnityEngine.RenderSettings.fogColor         = UnityEngine.Color.white;
                UnityEngine.RenderSettings.fogDensity       = 0.005f;
                UnityEngine.RenderSettings.fogStartDistance = 0.0f;
                UnityEngine.RenderSettings.fogEndDistance   = 10000.0f;

                // skybox background
                UnityEngine.Camera.main.backgroundColor = UnityEngine.Color.white;
                UnityEngine.Camera.main.clearFlags      = UnityEngine.CameraClearFlags.Skybox;

            } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4PhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4PhaseC.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4PhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV4PhaseC */

} /* } Labsim.apollon.experiment.phase */
