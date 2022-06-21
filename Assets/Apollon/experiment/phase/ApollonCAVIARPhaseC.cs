using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseC
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {

        public int CurrentID { private set; get; } = -1;

        public ApollonCAVIARPhaseC(profile.ApollonCAVIARProfile fsm, int currentID)
            : base(fsm)
        {
            this.CurrentID = currentID;
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // get our entity bridge & our settings
            var caviar_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as gameplay.entity.ApollonCAVIAREntityBridge
                );
            var control_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                    ) as gameplay.control.ApollonCAVIARControlBridge
                );
            var phase_settings
                = this.FSM.CurrentSettings.phase_C_settings[this.CurrentID];
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;

            // inactivate all visual cues through LINQ request
            foreach (var vc_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("VCTag_")).Select(kvp => kvp.Value))
            {
                vc_ref.SetActive(false);
            }

            // setup visual cue
            foreach (var cue in phase_settings.visual_cue_type)
            {

                switch (cue)
                {

                    // 3D object - tetrahedre / default
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC3D:
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC3DTetrahedre:
                    {
                        we_behaviour.References["VCTag_3DTetrahedre"].SetActive(true);
                        break;
                    }

                    // 3D object - cube
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC3DCube:
                    {
                        we_behaviour.References["VCTag_3DCube"].SetActive(true);
                        break;
                    }
                        
                    // 2D Object - all
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC2D:
                    {
                        we_behaviour.References["VCTag_2DCombined"].SetActive(true);
                        break;
                    }

                    // 2D Object - grid [default]
                    default:
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC2DGrid:
                    {
                        we_behaviour.References["VCTag_2DGrid"].SetActive(true);
                        break;
                    }

                    // 2D object - square
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC2DSquare:
                    {
                        we_behaviour.References["VCTag_2DSquare"].SetActive(true);
                        break;
                    }

                    // 2D object - circle
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VC2DCircle:
                    {
                        we_behaviour.References["VCTag_2DCircle"].SetActive(true);
                        break;
                    }

                    // Controle
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Control:
                    {
                        break;
                    }

                    // HUD - Radiosonde
                    case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.VCHUDRadiosonde:
                    {
                        we_behaviour.References["VCTag_HUDRadiosonde"].SetActive(true);
                        break;
                    }

                } /* switch() */

            } /* foreach() */
            
            // save our local origin to world coord depth point
            float current_phase_start_distance
                = caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z;

            // determine if there is a stim
            bool bHasStim 
                = (
                    !(
                        (phase_settings.stim_begin_distance == -1.0f)
                        && (phase_settings.stim_velocity == -1.0f)
                        && (phase_settings.stim_acceleration == -1.0f)
                    )
                ) ? true : false;

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();
            void sync_user_response_local_function(object sender, gameplay.control.ApollonCAVIARControlDispatcher.EventArgs e)
                => sync_point?.TrySetResult((
                    /* detection!  */ 
                    true, 
                    /* current relative phase depth */
                    (caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z),
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */
                    ApollonHighResolutionTime.Now.ToString()
                ));
            void sync_end_stim_local_function(object sender, gameplay.entity.ApollonCAVIAREntityDispatcher.EventArgs e)
                => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // blend function
            System.EventHandler<
                gameplay.device.sensor.ApollonRadioSondeSensorDispatcher.EventArgs
            > blend_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // extract elevation above terrain value
                        float
                            value       = args.DistanceFromSensorToHit,
                            target      = ApollonExperimentManager.Instance.Trial.settings.GetFloat("pilotable_elevation_above_terrain_meter"),
                            gap         = UnityEngine.Mathf.Abs(value - target) * 100.0f / target,
                            lower_bound = 10.0f,
                            upper_bound = 90.0f;

                        // update cross properties
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.SimpleCrossGUI
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
                                    UnityEngine.Color.green, 
                                    UnityEngine.Color.red, 
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
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.SimpleFrameGUI
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
                                    UnityEngine.Color.green, 
                                    UnityEngine.Color.red, 
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
            control_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;
            if(ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {

                // if practicing
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor
                    ) as gameplay.device.sensor.ApollonRadioSondeSensorBridge
                ).Dispatcher.HitChangedEvent += blend_local_function;
            
            } /* if() */

            // if stim, phase C [begin; stim]
            if(bHasStim)
            {

                // request async notification
                caviar_bridge.DoNotifyWhenWaypointReached(
                    current_phase_start_distance + phase_settings.stim_begin_distance
                );
            
                bool bRequestEndWaitStimLoop = false;
                do
                {

                    // wait result
                    (bool, float, float, string) result = await sync_point.Task;

                    // check result boolean value
                    if(result.Item1) 
                    {

                        // it's a hit then
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_response = result.Item1;
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_distance.Add(result.Item2);
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_unity_timestamp.Add(result.Item3);
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_host_timestamp.Add(result.Item4);
                    
                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                                + this.CurrentID
                            + "].OnEntry() : it seems user detected something["
                                + result.Item2
                            + ","
                                + result.Item3
                            + ","
                                + result.Item4
                            + "], wait for stim begin waypoint reached or another detection."
                        );

                        // re-tasking
                        sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

                    // end
                    } else {
                        
                        // // if there is already a response == end, otherwise
                        // if(!this.FSM.CurrentResults.phase_C_results[CurrentID].user_response) {

                        //     // it's a miss, save failed result
                        //     this.FSM.CurrentResults.phase_C_results[CurrentID].user_response = result.Item1;
                        //     this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance.Add(result.Item2);
                        //     this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp.Add(result.Item3);
                        //     this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp.Add(result.Item4);

                        //     // log
                        //     UnityEngine.Debug.Log(
                        //         "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                        //             + this.CurrentID
                        //         + "].OnEntry() : it seems user doesn't detected anything before stim... continuing !"
                        //     );
                        
                        // } /* if() */

                        // request end
                        bRequestEndWaitStimLoop = true;
                        
                    } /* if() */

                } while (!bRequestEndWaitStimLoop); /* while() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                        + this.CurrentID
                    + "].OnEntry() : stim will begin, current distance["
                        + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                    + "]"
                );
                
                // save stim results
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_distance = caviar_bridge.Behaviour.transform.TransformPoint(0.0f, 0.0f, 0.0f).z;
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_host_timestamp = ApollonHighResolutionTime.Now.ToString();
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_unity_timestamp = UnityEngine.Time.time;

                // accelerate/decelerate up to the stim settings or nothing :)
                if (phase_settings.stim_velocity > phase_settings.target_velocity) 
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise acceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // accel.
                    caviar_bridge.Dispatcher.RaiseAccelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else if(phase_settings.stim_velocity < phase_settings.target_velocity) 
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise deceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // decel.
                    caviar_bridge.Dispatcher.RaiseDecelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, requested stim velocity is identical to target valocity, skip."
                    );


                } /* if() */

            } /* if() */

            // request async notification
            caviar_bridge.DoNotifyWhenWaypointReached(
                current_phase_start_distance + phase_settings.total_distance
            );

            // re task 
            sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

            // then, phase C [stim or begin; end] we should wait this completion in all cases
            bool bRequestEndWaitLoop = false;
            do
            {

                // wait result
                (bool, float, float, string) result = await sync_point.Task;

                // check result boolean value
                if(result.Item1) 
                {

                    // it's a hit then
                    this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_response = result.Item1;
                    this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_distance.Add(result.Item2);
                    this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_unity_timestamp.Add(result.Item3);
                    this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_host_timestamp.Add(result.Item4);
                
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : it seems user detected something["
                            + result.Item2
                        + ","
                            + result.Item3
                        + ","
                            + result.Item4
                        + "], wait for waypoint reached or another detection."
                    );

                    // // unregister our synchronisation function
                    // hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
                    // caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

                    // re-tasking
                    sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

                    // // register our synchronisation function
                    // hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
                    // caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;

                // end
                } else { 
                    
                    // if there is already a response == end, otherwise
                    if(!this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_response) {

                        // it's a miss, save failed result
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_response = result.Item1;
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_distance.Add(result.Item2);
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_unity_timestamp.Add(result.Item3);
                        this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_perception_host_timestamp.Add(result.Item4);

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                                + this.CurrentID
                            + "].OnEntry() : it seems user doesn't detected anything... :("
                        );
                    
                    } /* if() */

                    // request end
                    bRequestEndWaitLoop = true;
                    
                } /* if() */

            } while (!bRequestEndWaitLoop); /* while() */

            // unregister our synchronisation function
            control_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;
            if(ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {

                // if practicing
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor
                    ) as gameplay.device.sensor.ApollonRadioSondeSensorBridge
                ).Dispatcher.HitChangedEvent -= blend_local_function;
            
            } /* if() */
            
            // // synchronisation mechanism (TCS + local function)
            // var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();
            // void sync_user_response_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
            //     => sync_point?.TrySetResult((
            //         /* detection!  */ 
            //         true, 
            //         /* current relative phase depth */
            //         (caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z - current_phase_start_distance),
            //         /* unity render timestamp */
            //         UnityEngine.Time.time,
            //         /* host timestamp */
            //         UXF.FileIOManager.CurrentHighResolutionTime
            //     ));
            // void sync_end_stim_local_function(object sender, gameplay.entity.ApollonCAVIAREntityDispatcher.EventArgs e)
            //     => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // // register our synchronisation function
            // hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            // caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;

            // // async end point task
            // System.Threading.Tasks.Task
            //     waypoint_reached_task = caviar_bridge.DoNotifyWhenWaypointReached(
            //         current_phase_start_distance + phase_settings.total_distance
            //     ),
            //     stim_reached_task = null;



            // // check if there is a stim
            // if( 
            //     !(
            //         (phase_settings.stim_begin_distance == -1.0f)
            //         && (phase_settings.stim_velocity == -1.0f)
            //         && (phase_settings.stim_acceleration == -1.0f)
            //     )
            // ) 
            // {
            
            //     // get our stim begin timestamp from actual target velocity
            //     float stim_begin_timestamp = (phase_settings.stim_begin_distance / phase_settings.target_velocity) * 1000.0f;

            //     // log
            //     UnityEngine.Debug.Log(
            //         "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //             + this.CurrentID
            //         + "].OnEntry() : stim detected, will be notified when reached"
            //     );

            //     // wait a certain amout of time
            //     await ApollonHighResolutionTime.DoSleep(stim_begin_timestamp);
                
            //     // log
            //     UnityEngine.Debug.Log(
            //         "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //             + this.CurrentID
            //         + "].OnEntry() : stim will begin, current distance["
            //             + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
            //         + "]"
            //     );
                
            //     // save stim results
            //     this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_distance = caviar_bridge.Behaviour.transform.TransformPoint(0.0f, 0.0f, 0.0f).z;
            //     this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_host_timestamp = UXF.FileIOManager.CurrentHighResolutionTime;
            //     this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_unity_timestamp = UnityEngine.Time.time;

            //     // accelerate/decelerate up to the stim settings or nothing :)
            //     if (phase_settings.stim_velocity > phase_settings.target_velocity) 
            //     {

            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : begin stim, raise acceleration["
            //             + "stim_acceleration:"
            //                 + phase_settings.stim_acceleration
            //             + ",stim_velocity:"
            //                 + phase_settings.stim_velocity
            //             + "]."
            //         );

            //         // accel.
            //         caviar_bridge.Dispatcher.RaiseAccelerate(
            //             phase_settings.stim_acceleration,
            //             phase_settings.stim_velocity
            //         );

            //     }
            //     else if(phase_settings.stim_velocity < phase_settings.target_velocity) 
            //     {
                    
            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : begin stim, raise deceleration["
            //             + "stim_acceleration:"
            //                 + phase_settings.stim_acceleration
            //             + ",stim_velocity:"
            //                 + phase_settings.stim_velocity
            //             + "]."
            //         );

            //         // decel.
            //         caviar_bridge.Dispatcher.RaiseDecelerate(
            //             phase_settings.stim_acceleration,
            //             phase_settings.stim_velocity
            //         );

            //     }
            //     else
            //     {

            //         // log
            //         UnityEngine.Debug.LogWarning(
            //             "<color=Orange>Warning: </color> ApollonCAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : begin stim, requested stim velocity is identical to target valocity, skip."
            //         );


            //     } /* if() */

            // } /* if() */

            // // async task
            // var waypoint_reached_task = caviar_bridge.DoNotifyWhenWaypointReached(
            //     current_phase_start_distance + phase_settings.total_distance
            // );
            
            // // whatever, we should wait this completion in all cases
            // do
            // {

            //     // get result
            //     (bool, float, float, string) result = await sync_point.Task;

            //     // check result boolean value
            //     if(result.Item1) 
            //     {

            //         // it's a hit then
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_response = result.Item1;
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance.Add(result.Item2);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp.Add(result.Item3);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp.Add(result.Item4);
                
            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : it seems user detected something["
            //                 + result.Item2
            //             + ","
            //                 + result.Item3
            //             + ","
            //                 + result.Item4
            //             + "], wait for waypoint reached or another detection."
            //         );

            //         // unregister our synchronisation function
            //         hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            //         caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

            //         // re-tasking
            //         sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

            //         // register our synchronisation function
            //         hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            //         caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;

            //     // if there is already a response == end, then
            //     } else if(!this.FSM.CurrentResults.phase_C_results[CurrentID].user_response) {

            //         // it's a miss, save failed result
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_response = result.Item1;
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance.Add(result.Item2);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp.Add(result.Item3);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp.Add(result.Item4);

            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : it seems user doesn't detected anything... :("
            //         );
                    
            //     } /* if() */

            // } while (!waypoint_reached_task.IsCompleted); /* while() */
                    
            // UnityEngine.Debug.Log(
            //     "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
            //         + this.CurrentID
            //     + "].OnEntry() : it seems user doesn't detected anything... :("
            // );
                    
            // // unregister our synchronisation function
            // hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            // caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnEntry() : end, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "], phase results ["
                    + this.FSM.CurrentResults.phase_C_results[CurrentID].user_response
                + ","
                    + string.Join(
                        "/",  
                        this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance
                    )
                + ","
                    + string.Join(
                        "/",  
                        this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp
                    )
                + ","
                    + string.Join(
                        "/",  
                        this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp
                    )
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseC */
    
} /* } Labsim.apollon.experiment.phase */