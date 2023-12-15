using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class CAVIARPhaseD
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {
        
        public int PreviousID { private set; get; } = -1;
        public int NextID { private set; get; } = -1;
        

        public CAVIARPhaseD(CAVIARProfile fsm, int previousID, int nextID)
            : base(fsm)
        {
            this.PreviousID = previousID;
            this.NextID = nextID;
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseD["
                    + this.PreviousID + "," + this.NextID 
                + "].OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // get our entity bridge & settings
            var caviar_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as CAVIAREntityBridge
                );
            var fog_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    ) as apollon.gameplay.element.ApollonFogElementBridge
                );
            var control_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                    ) as CAVIARControlBridge
                );
            
            // instantiate vars & save our local origin to world coord depth point
            float 
                phase_acceleration = 0.0f, 
                phase_duration = 0.0f,
                current_phase_start_distance = caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z;

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();
            void sync_user_response_local_function(object sender, CAVIARControlDispatcher.CAVIARControlEventArgs e)
                => sync_point?.TrySetResult((
                    /* detection!  */ 
                    true, 
                    /* current relative phase depth */
                    (caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z),
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */
                    apollon.ApollonHighResolutionTime.Now.ToString()
                ));
            void sync_end_stim_local_function(object sender, CAVIAREntityDispatcher.CAVIAREntityEventArgs e)
                => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // blend function
            System.EventHandler<
                apollon.gameplay.device.sensor.ApollonRadioSondeSensorDispatcher.RadioSondeSensorEventArgs
            > blend_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // extract elevation above terrain value
                        float
                            value       = args.DistanceFromSensorToHit,
                            target      = apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_elevation_above_terrain_meter"),
                            gap         = UnityEngine.Mathf.Abs(value - target) * 100.0f / target,
                            lower_bound = 10.0f,
                            upper_bound = 30.0f;

                        // update cross properties
                        foreach(var child in apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.SimpleCrossGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {

                            UnityEngine.Color new_color = child.material.color;

                            // if modyfing color
                            if(gap <= lower_bound )
                            { 
                                // ok !
                                new_color = UnityEngine.Color.green;
                                new_color.a = 1.0f;
                            }
                            else if(gap <= upper_bound)
                            {

                                float ratio = (gap - lower_bound) / (upper_bound - lower_bound);
                                new_color = UnityEngine.Color.Lerp(
                                    UnityEngine.Color.red, 
                                    UnityEngine.Color.green, 
                                    ratio
                                );
                                new_color.a = 1.0f - ratio;

                            }
                            else 
                            {
                                // fail
                                new_color = UnityEngine.Color.red;
                                new_color.a = 0.0f;
                            }

                            // assign new color
                            child.material.color = new_color;

                        } /* foreach() */

                        // update frame properties
                        foreach(var child in apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.SimpleFrameGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {

                            UnityEngine.Color new_color = child.material.color;

                            // if modyfing color
                            if(gap <= lower_bound )
                            { 
                                // ok !
                                new_color = UnityEngine.Color.green;
                            }
                            else if(gap <= upper_bound)
                            {

                                float ratio = (gap - lower_bound) / (upper_bound - lower_bound);
                                new_color = UnityEngine.Color.Lerp(
                                    UnityEngine.Color.red, 
                                    UnityEngine.Color.green, 
                                    ratio
                                );

                            }
                            else 
                            {
                                // fail
                                new_color = UnityEngine.Color.red;
                            }

                            // assign new color
                            child.material.color = new_color;

                        } /* foreach()*/
                        
                    }; /* lambda */

            // register our synchronisation function
            control_bridge.ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            caviar_bridge.ConcreteDispatcher.WaypointReachedEvent += sync_end_stim_local_function;
            if(apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {

                // if practicing
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor
                    ) as apollon.gameplay.device.sensor.ApollonRadioSondeSensorBridge
                ).ConcreteDispatcher.HitChangedEvent += blend_local_function;
            
            } /* if() */

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
                    "<color=Blue>Info: </color>  CAVIARPhaseD["
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
                    caviar_bridge.ConcreteDispatcher.RaiseAccelerate(
                        UnityEngine.Mathf.Abs(phase_acceleration),
                        this.FSM.CurrentSettings.phase_C_settings[this.NextID].target_velocity
                    );

                }
                else
                {

                    // decelerate up to the next phase C settings
                    caviar_bridge.ConcreteDispatcher.RaiseDecelerate(
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
                "<color=Blue>Info: </color> CAVIARPhaseD["
                    + this.PreviousID + "," + this.NextID 
                + "].OnEntry() : begin smoothing, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // apply fog
            fog_bridge.ConcreteDispatcher.RaiseSmoothLinearFogRequested(
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
                        "<color=Blue>Info: </color> CAVIARPhaseD["
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
                            "<color=Blue>Info: </color> CAVIARPhaseD["
                                + this.PreviousID + "," + this.NextID 
                            + "].OnEntry() : it seems user doesn't detected anything... :("
                        );
                    
                    } /* if() */

                    // request end
                    bRequestEndWaitLoop = true;
                    
                } /* if() */

            } while (!bRequestEndWaitLoop); /* while() */

            // unregister our synchronisation function
            control_bridge.ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            caviar_bridge.ConcreteDispatcher.WaypointReachedEvent -= sync_end_stim_local_function;
            if(apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {

                // if practicing
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor
                    ) as apollon.gameplay.device.sensor.ApollonRadioSondeSensorBridge
                ).ConcreteDispatcher.HitChangedEvent -= blend_local_function;
            
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseD["
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
                "<color=Blue>Info: </color> CAVIARPhaseD["
                + this.PreviousID + "," + this.NextID 
                + "].OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results[this.PreviousID].timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseD["
                + this.PreviousID + "," + this.NextID 
                + "].OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseD */
    
} /* } Labsim.apollon.experiment.phase */