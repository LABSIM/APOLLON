//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class CAVIARPhaseC
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {

        public int CurrentID { private set; get; } = -1;

        public CAVIARPhaseC(CAVIARProfile fsm, int currentID)
            : base(fsm)
        {
            this.CurrentID = currentID;
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseC["
                    + this.CurrentID
                + "].OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // get our entity bridge & our settings
            var caviar_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as CAVIAREntityBridge
                );
            var control_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                    ) as CAVIARControlBridge
                );
            var phase_settings
                = this.FSM.CurrentSettings.phase_C_settings[this.CurrentID];
            var we_behaviour
                 = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as apollon.gameplay.element.ApollonWorldElementBehaviour;

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
                    case CAVIARProfile.Settings.VisualCueIDType.VC3D:
                    case CAVIARProfile.Settings.VisualCueIDType.VC3DTetrahedre:
                    {
                        we_behaviour.References["VCTag_3DTetrahedre"].SetActive(true);
                        break;
                    }

                    // 3D object - cube
                    case CAVIARProfile.Settings.VisualCueIDType.VC3DCube:
                    {
                        we_behaviour.References["VCTag_3DCube"].SetActive(true);
                        break;
                    }
                        
                    // 2D Object - all
                    case CAVIARProfile.Settings.VisualCueIDType.VC2D:
                    {
                        we_behaviour.References["VCTag_2DCombined"].SetActive(true);
                        break;
                    }

                    // 2D Object - grid [default]
                    default:
                    case CAVIARProfile.Settings.VisualCueIDType.VC2DGrid:
                    {
                        we_behaviour.References["VCTag_2DGrid"].SetActive(true);
                        break;
                    }

                    // 2D object - square
                    case CAVIARProfile.Settings.VisualCueIDType.VC2DSquare:
                    {
                        we_behaviour.References["VCTag_2DSquare"].SetActive(true);
                        break;
                    }

                    // 2D object - circle
                    case CAVIARProfile.Settings.VisualCueIDType.VC2DCircle:
                    {
                        we_behaviour.References["VCTag_2DCircle"].SetActive(true);
                        break;
                    }

                    // Controle
                    case CAVIARProfile.Settings.VisualCueIDType.Control:
                    {
                        break;
                    }

                    // HUD - Radiosonde
                    case CAVIARProfile.Settings.VisualCueIDType.VCHUDRadiosonde:
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
                            upper_bound = 90.0f;

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
                            "<color=Blue>Info: </color> CAVIARPhaseC["
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
                        //         "<color=Blue>Info: </color> CAVIARPhaseC["
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
                    "<color=Blue>Info: </color> CAVIARPhaseC["
                        + this.CurrentID
                    + "].OnEntry() : stim will begin, current distance["
                        + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                    + "]"
                );
                
                // save stim results
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_distance = caviar_bridge.Behaviour.transform.TransformPoint(0.0f, 0.0f, 0.0f).z;
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
                this.FSM.CurrentResults.phase_C_results[this.CurrentID].user_stim_unity_timestamp = UnityEngine.Time.time;

                // accelerate/decelerate up to the stim settings or nothing :)
                if (phase_settings.stim_velocity > phase_settings.target_velocity) 
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> CAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise acceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // accel.
                    caviar_bridge.ConcreteDispatcher.RaiseAccelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else if(phase_settings.stim_velocity < phase_settings.target_velocity) 
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> CAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise deceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // decel.
                    caviar_bridge.ConcreteDispatcher.RaiseDecelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> CAVIARPhaseC["
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
                        "<color=Blue>Info: </color> CAVIARPhaseC["
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
                    // hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
                    // caviar_bridge.ConcreteDispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

                    // re-tasking
                    sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

                    // // register our synchronisation function
                    // hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
                    // caviar_bridge.ConcreteDispatcher.WaypointReachedEvent += sync_end_stim_local_function;

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
                            "<color=Blue>Info: </color> CAVIARPhaseC["
                                + this.CurrentID
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
            // void sync_end_stim_local_function(object sender, gameplay.entity.CAVIAREntityDispatcher.EventArgs e)
            //     => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // // register our synchronisation function
            // hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            // caviar_bridge.ConcreteDispatcher.WaypointReachedEvent += sync_end_stim_local_function;

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
            //         "<color=Blue>Info: </color> CAVIARPhaseC["
            //             + this.CurrentID
            //         + "].OnEntry() : stim detected, will be notified when reached"
            //     );

            //     // wait a certain amout of time
            //     await ApollonHighResolutionTime.DoSleep(stim_begin_timestamp);
                
            //     // log
            //     UnityEngine.Debug.Log(
            //         "<color=Blue>Info: </color> CAVIARPhaseC["
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
            //             "<color=Blue>Info: </color> CAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : begin stim, raise acceleration["
            //             + "stim_acceleration:"
            //                 + phase_settings.stim_acceleration
            //             + ",stim_velocity:"
            //                 + phase_settings.stim_velocity
            //             + "]."
            //         );

            //         // accel.
            //         caviar_bridge.ConcreteDispatcher.RaiseAccelerate(
            //             phase_settings.stim_acceleration,
            //             phase_settings.stim_velocity
            //         );

            //     }
            //     else if(phase_settings.stim_velocity < phase_settings.target_velocity) 
            //     {
                    
            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> CAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : begin stim, raise deceleration["
            //             + "stim_acceleration:"
            //                 + phase_settings.stim_acceleration
            //             + ",stim_velocity:"
            //                 + phase_settings.stim_velocity
            //             + "]."
            //         );

            //         // decel.
            //         caviar_bridge.ConcreteDispatcher.RaiseDecelerate(
            //             phase_settings.stim_acceleration,
            //             phase_settings.stim_velocity
            //         );

            //     }
            //     else
            //     {

            //         // log
            //         UnityEngine.Debug.LogWarning(
            //             "<color=Orange>Warning: </color> CAVIARPhaseC["
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
            //             "<color=Blue>Info: </color> CAVIARPhaseC["
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
            //         hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            //         caviar_bridge.ConcreteDispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

            //         // re-tasking
            //         sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, float, string)>();

            //         // register our synchronisation function
            //         hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            //         caviar_bridge.ConcreteDispatcher.WaypointReachedEvent += sync_end_stim_local_function;

            //     // if there is already a response == end, then
            //     } else if(!this.FSM.CurrentResults.phase_C_results[CurrentID].user_response) {

            //         // it's a miss, save failed result
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_response = result.Item1;
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance.Add(result.Item2);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp.Add(result.Item3);
            //         this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp.Add(result.Item4);

            //         // log
            //         UnityEngine.Debug.Log(
            //             "<color=Blue>Info: </color> CAVIARPhaseC["
            //                 + this.CurrentID
            //             + "].OnEntry() : it seems user doesn't detected anything... :("
            //         );
                    
            //     } /* if() */

            // } while (!waypoint_reached_task.IsCompleted); /* while() */
                    
            // UnityEngine.Debug.Log(
            //     "<color=Blue>Info: </color> CAVIARPhaseC["
            //         + this.CurrentID
            //     + "].OnEntry() : it seems user doesn't detected anything... :("
            // );
                    
            // // unregister our synchronisation function
            // hotas_bridge.ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            // caviar_bridge.ConcreteDispatcher.WaypointReachedEvent -= sync_end_stim_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseC["
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
                "<color=Blue>Info: </color> CAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results[this.CurrentID].timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseC */
    
} /* } Labsim.apollon.experiment.phase */