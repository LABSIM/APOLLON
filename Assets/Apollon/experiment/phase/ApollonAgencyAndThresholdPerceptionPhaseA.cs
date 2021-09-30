using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // User command input phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseA
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseA(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : begin"
            );
            
            // fade in to black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            } /* if() */
           
            // show green cross & frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // hide green frame first
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // then hide cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : active condition"
                );

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<int>();
                void sync_positive_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionControlDispatcher.EventArgs e)
                    => sync_point?.TrySetResult(1);
                void sync_negative_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionControlDispatcher.EventArgs e)
                    => sync_point?.TrySetResult(-1);

                // register our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                ).Dispatcher.UserPositiveCommandTriggeredEvent += sync_positive_local_function;
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                ).Dispatcher.UserNegativeCommandTriggeredEvent += sync_negative_local_function;

                // wait synchronisation point indefinitely & reset it once hit
                this.FSM.CurrentResults.user_command = await sync_point.Task;

                // hack
                if (this.FSM.CurrentResults.user_command > 0)
                {
                    this.FSM.positiveConditionCount++;
                }
                else
                {
                    this.FSM.negativeConditionCount++;
                }

                // unregister our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                ).Dispatcher.UserPositiveCommandTriggeredEvent -= sync_positive_local_function;
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                ).Dispatcher.UserNegativeCommandTriggeredEvent -= sync_negative_local_function;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : user command request ["
                    + this.FSM.CurrentResults.user_command
                    + "]"
                );

            }
            else
            {

                // null command 
                this.FSM.CurrentResults.user_command = 1;
                    
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseA */

} /* } Labsim.apollon.experiment.phase */
