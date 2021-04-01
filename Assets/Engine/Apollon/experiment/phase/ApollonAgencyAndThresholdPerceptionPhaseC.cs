using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Acceleration phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseC
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseC(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : begin"
            );
                

            // get bridges
            gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge hotas_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge;

            gameplay.entity.ApollonActiveSeatEntityBridge seat_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                ) as gameplay.entity.ApollonActiveSeatEntityBridge;

            // check
            if (hotas_bridge == null || seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string)>();
            void sync_user_response_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                => sync_point?.TrySetResult((true, UnityEngine.Time.time, System.DateTime.Now.ToString("HH:mm:ss.ffffff")));
            void sync_end_stim_local_function(object sender, gameplay.entity.ApollonActiveSeatEntityDispatcher.EventArgs e)
                => sync_point?.TrySetResult((false, -1.0f, "-1"));

            // register our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            seat_bridge.Dispatcher.StopEvent += sync_end_stim_local_function;

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : begin stim"
            );

            // log stim begin timestamp
            this.FSM.CurrentResults.user_stim_unity_timestamp = UnityEngine.Time.time;
            this.FSM.CurrentResults.user_stim_host_timestamp = System.DateTime.Now.ToString("HH:mm:ss.ffffff");
                
            // begin stim
            seat_bridge.Dispatcher.RaiseAccelerate(
                /* if not a try/catch condition ? */ (
                    this.FSM.CurrentSettings.bIsTryCatch
                        ? 0.0f
                        : (this.FSM.CurrentResults.user_command * this.FSM.CurrentSettings.phase_C_angular_acceleration)
                ),
                /* if not a try/catch condition ? */ (
                    this.FSM.CurrentSettings.bIsTryCatch
                        ? 0.0f
                        : (this.FSM.CurrentResults.user_command * this.FSM.CurrentSettings.phase_C_angular_saturation_speed)
                ),
                this.FSM.CurrentSettings.phase_C_max_stim_duration,
                this.FSM.CurrentSettings.phase_C_max_stim_angle,
                (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VisualOnly ? true : false)
            );
                
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : begin "
                + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                + " stim, result [user_stim_unity_timestamp:"
                + this.FSM.CurrentResults.user_stim_unity_timestamp
                + ",user_stim_host_timestamp:"
                + this.FSM.CurrentResults.user_stim_host_timestamp
                + "]"
            );

            // wait synchronisation point indefinitely & reset it once hit
            (
                this.FSM.CurrentResults.user_response, 
                this.FSM.CurrentResults.user_perception_unity_timestamp,
                this.FSM.CurrentResults.user_perception_host_timestamp
            ) = await sync_point.Task;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : end stim, result [user_response:"
                + this.FSM.CurrentResults.user_response
                + ",user_perception_unity_timestamp:"
                + this.FSM.CurrentResults.user_perception_unity_timestamp
                + ",user_perception_host_timestamp:"
                + this.FSM.CurrentResults.user_perception_host_timestamp
                + "]"
            );

            // unregister our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            seat_bridge.Dispatcher.StopEvent -= sync_end_stim_local_function;


            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseC */

} /* } Labsim.apollon.experiment.phase */
