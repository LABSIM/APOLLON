using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseD
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {
        
        public int PreviousID { private set; get; } = -1;
        public int NextID { private set; get; } = -1;
        

        public ApollonCAVIARPhaseD(profile.ApollonCAVIARProfile fsm, int previousID, int nextID)
            : base(fsm)
        {
            this.PreviousID = previousID;
            this.NextID = nextID;
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                    + this.PreviousID + "," + this.NextID 
                + "].OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_entry_host_timestamp = UXF.FileIOManager.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // get our entity bridge & settings
            var caviar_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as gameplay.entity.ApollonCAVIAREntityBridge
                );
            var fog_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    ) as gameplay.element.ApollonFogElementBridge
                );
            var hotas_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                    ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                );
            
            // instantiate vars & save our local origin to world coord depth point
            float 
                phase_acceleration = 0.0f, 
                phase_duration = 0.0f,
                current_phase_start_distance = caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z;

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();
            void sync_user_response_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                => sync_point?.TrySetResult((
                    /* detection!  */ 
                    true, 
                    /* current relative phase depth */
                    (caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z),
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */
                    UXF.FileIOManager.CurrentHighResolutionTime
                ));
            void sync_end_stim_local_function(object sender, gameplay.entity.ApollonCAVIAREntityDispatcher.EventArgs e)
                => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // register our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;

            // if we have a difference, calculate transition othewise just wait
            if(
                ( 
                    this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity 
                    - ( 
                        (this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity != -1.0f) 
                        ? this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity
                        : this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].target_velocity
                    ) 
                ) != 0.0f
            ) {

                // get our acceleration value & timestamp

                /* kinematic equation */
                phase_acceleration 
                    = (
                        (
                            UnityEngine.Mathf.Pow(this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity, 2.0f)
                            - UnityEngine.Mathf.Pow(
                                (
                                    (this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity != -1.0f)
                                    ? this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity
                                    : this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].target_velocity
                                ), 
                                2.0f
                            )
                        ) / ( 2.0f *  this.FSM.CurrentSettings.phase_D_distance )
                    );

                /* phase duration {ms} */
                phase_duration 
                    = ( 
                        (
                            this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity 
                            - (
                                (this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity != -1.0f)
                                ? this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].stim_velocity
                                : this.FSM.CurrentSettings.phase_C_settings[this.PreviousID].target_velocity
                            )
                        ) / phase_acceleration 
                    ) * 1000.0f;
                        
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color>  ApollonCAVIARPhaseD["
                        + this.PreviousID + "," + this.NextID
                    + "].OnEntry() : calculated following parameter [phase_acceleration:" 
                        + phase_acceleration 
                    + ",phase_duration:" 
                        + phase_duration
                    + "], begin transition."
                );

                // switch on sign
                if(phase_acceleration > 0.0f)
                {

                    // accelerate up to the next phase C settings
                    caviar_bridge.Dispatcher.RaiseAccelerate(
                        UnityEngine.Mathf.Abs(phase_acceleration),
                        this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity
                    );

                }
                else
                {

                    // decelerate up to the next phase C settings
                    caviar_bridge.Dispatcher.RaiseDecelerate(
                        UnityEngine.Mathf.Abs(phase_acceleration),
                        this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity
                    );
                    
                } /* if() */

            } 
            else
            {
                
                /* phase duration {ms} */
                phase_duration 
                    = ( 
                        this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity 
                        / this.FSM.CurrentSettings.phase_D_distance 
                    ) * 1000.0f;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                    + this.PreviousID + "," + this.NextID 
                + "].OnEntry() : begin smoothing, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // apply fog
            fog_bridge.Dispatcher.RaiseSmoothLinearFogRequested(
                this.FSM.CurrentSettings.phase_C_settings[this.NextID].fog_start_distance,
                this.FSM.CurrentSettings.phase_C_settings[this.NextID].fog_end_distance,
                UnityEngine.Color.white,
                phase_duration
            );

            // request async notification
            caviar_bridge.DoNotifyWhenWaypointReached(
                current_phase_start_distance + this.FSM.CurrentSettings.phase_D_distance 
            );

            // then, phase D [begin; end] we should wait this completion in all cases
            bool bRequestEndWaitLoop = false;
            do
            {

                // wait result
                (bool, float, float, string) result = await sync_point.Task;

                // check result boolean value
                if(result.Item1) 
                {

                    // it's a hit then
                    this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_response = result.Item1;
                    this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_distance.Add(result.Item2);
                    this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_unity_timestamp.Add(result.Item3);
                    this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_host_timestamp.Add(result.Item4);
                
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                            + this.PreviousID + "," + this.NextID 
                        + "].OnEntry() : it seems user detected something["
                            + result.Item2
                        + ","
                            + result.Item3
                        + ","
                            + result.Item4
                        + "], wait for waypoint reached or another detection."
                    );

                    // re-tasking
                    sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

                // end
                } else { 
                    
                    // if there is already a response == end, otherwise
                    if(!this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_response) {

                        // it's a miss, save failed result
                        this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_response = result.Item1;
                        this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_distance.Add(result.Item2);
                        this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_unity_timestamp.Add(result.Item3);
                        this.FSM.CurrentResults.phase_D_results[this.PreviousID].user_perception_host_timestamp.Add(result.Item4);

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                                + this.PreviousID + "," + this.NextID 
                            + "].OnEntry() : it seems user doesn't detected anything... :("
                        );
                    
                    } /* if() */

                    // request end
                    bRequestEndWaitLoop = true;
                    
                } /* if() */

            } while (!bRequestEndWaitLoop); /* while() */

            // unregister our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                    + this.PreviousID + "," + this.NextID 
                + "].OnEntry() : end, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                + this.PreviousID + "," + this.NextID 
                + "].OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_exit_host_timestamp = UXF.FileIOManager.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseD["
                + this.PreviousID + "," + this.NextID 
                + "].OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseD */
    
} /* } Labsim.apollon.experiment.phase */