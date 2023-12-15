using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseJ 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseJ(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseJ.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_J_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_J_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // fade in 
            await this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration, false);

            // get bridge
            var motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            var seat_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                ) as gameplay.entity.ApollonActiveSeatEntityBridge;

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_J_settings.duration);
                    break;
                }
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly:
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_J_settings.duration);
                    break;
                }

            } /* switch() */

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // wait
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_J_settings.duration);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseJ.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseJ.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_J_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_J_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseJ.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseJ */

} /* } Labsim.apollon.experiment.phase */
