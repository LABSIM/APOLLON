using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    //
    // Latency phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV4PhaseC
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4PhaseC(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseC.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get bridges

            var physical_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                );

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                );

            // current scenario
            bool bHasRealMotion = false; 
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    bHasRealMotion = false;
                    break;
                }
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisuoVestibular:
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
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV4PhaseC.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */
            
            // decelerate
            virtual_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();
            physical_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();

            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration);

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Fin";

            // hide grey frame/cross & show green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait remaining time
            await apollon.ApollonHighResolutionTime.DoSleep(
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
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseC.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV4PhaseC */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
