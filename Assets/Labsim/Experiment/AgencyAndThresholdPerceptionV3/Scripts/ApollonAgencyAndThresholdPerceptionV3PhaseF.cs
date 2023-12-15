using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseF 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV3Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV3PhaseF(profile.ApollonAgencyAndThresholdPerceptionV3Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseF.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // fade in 
            await this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration, false);

            // get bridge
            var motion_system_bridge
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

            var seat_bridge
                = gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.entity.ApollonActiveSeatEntityBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                );

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case profile.ApollonAgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }
                case profile.ApollonAgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VestibularOnly:
                case profile.ApollonAgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }

            } /* switch() */

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionV3Profile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // wait
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_settings.duration);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseF.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseF.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseF */

} /* } Labsim.apollon.experiment.phase */
