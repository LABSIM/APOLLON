using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    //
    // Acceleration phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV4PhaseB
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4PhaseB(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // get angular/linear acceleration threshold raw string from user settings
            var angular_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["angular_acceleration_threshold"]
                .ToString();
            // var angular_strong_acceleration_threshold_raw_string 
            //     = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["angular_strong_acceleration_threshold"]
            //     .ToString();
            var linear_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["linear_acceleration_threshold"]
                .ToString();
            // var linear_strong_acceleration_threshold_raw_string 
            //     = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["linear_strong_acceleration_threshold"]
            //     .ToString();
            
            // then pop first & last element & split from "," separator & convert to a float array
            float[] user_angular_acceleration_threshold
                = System.Array.ConvertAll(
                    angular_acceleration_threshold_raw_string.Substring(1, (angular_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );
            // float[] user_angular_strong_acceleration_threshold
            //     = System.Array.ConvertAll(
            //         angular_strong_acceleration_threshold_raw_string.Substring(1, (angular_strong_acceleration_threshold_raw_string.Length - 2)).Split(','),
            //         float.Parse
            //     ); 
            float[] user_linear_acceleration_threshold
                = System.Array.ConvertAll(
                    linear_acceleration_threshold_raw_string.Substring(1, (linear_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );
            // float[] user_linear_strong_acceleration_threshold
            //     = System.Array.ConvertAll(
            //         linear_strong_acceleration_threshold_raw_string.Substring(1, (linear_strong_acceleration_threshold_raw_string.Length - 2)).Split(','),
            //         float.Parse
            //     );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : user reference {"
                + "[user_angular_acceleration_threshold:" 
                    + System.String.Join(",",user_angular_acceleration_threshold) 
                // + "][user_angular_strong_acceleration_threshold:" 
                //     + System.String.Join(",",user_angular_strong_acceleration_threshold) 
                + "][user_linear_acceleration_threshold:" 
                    + System.String.Join(",",user_linear_acceleration_threshold) 
                // + "][user_linear_strong_acceleration_threshold:" 
                //     + System.String.Join(",",user_linear_strong_acceleration_threshold) 
                + "]}"
            );

            // extract settings from current user_command
            float[]
                current_angular_acceleration_target = null,
                current_angular_velocity_saturation_threshold = null,
                current_angular_displacement_limiter = null,
                current_linear_acceleration_target = null,
                current_linear_velocity_saturation_threshold = null,
                current_linear_displacement_limiter = null;

            // get bridges
            var control_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV4ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                );

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
            if (control_bridge == null || physical_motion_system_bridge == null || virtual_motion_system_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_virtual_motion_idle_point  = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var sync_physical_motion_idle_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // lambdas
            System.EventHandler<
                apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs
            > sync_end_virtual_motion_local_function 
                = (sender, args) => sync_virtual_motion_idle_point?.TrySetResult(true);
            System.EventHandler<
               apollon. gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs
            > sync_end_physical_motion_local_function 
                = (sender, args) => sync_physical_motion_idle_point?.TrySetResult(true);

            // register our synchronisation function
            virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent  += sync_end_virtual_motion_local_function;
            physical_motion_system_bridge.ConcreteDispatcher.IdleEvent += sync_end_physical_motion_local_function;

            // parallel                    
            var parallel_tasks_ct_src = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken parallel_tasks_ct = parallel_tasks_ct_src.Token;
            var parallel_tasks_factory
                        = new System.Threading.Tasks.TaskFactory(
                            parallel_tasks_ct,
                            System.Threading.Tasks.TaskCreationOptions.DenyChildAttach,
                            System.Threading.Tasks.TaskContinuationOptions.DenyChildAttach,
                            System.Threading.Tasks.TaskScheduler.Default
                        );
            var parallel_tasks = new System.Collections.Generic.List<
                System.Threading.Tasks.Task
            >() {
                parallel_tasks_factory.StartNew(
                    async () => 
                    { 

                        // get elapsed 
                        var remaining = this.FSM.CurrentSettings.phase_B_settings.duration - current_stopwatch.ElapsedMilliseconds;
                        if(remaining > 0.0f)
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : waiting [" 
                                + remaining 
                                + "ms] for end of phase"
                            );

                        }
                        else
                        {
                            
                            // log
                            UnityEngine.Debug.LogWarning(
                                "<color=Orange>Warn: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : strange... no remaing time to wait..."
                            );

                        } /* if() */

                        // wait end of phase 
                        await apollon.ApollonHighResolutionTime.DoSleep(remaining);

                    }
                ).Unwrap()
            };

            #region primary stim

            // find the offset matrix 
            var primary_offset_angular_matrix = UnityEngine.Matrix4x4.identity;
            var primary_offset_linear_matrix = UnityEngine.Matrix4x4.identity;

            // nested switch... i know
            switch(this.FSM.CurrentResults.phase_A_results.user_command)
            {

                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Down:
                {
                    
                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary down strong settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary down weak settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary down reference settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */

                    break;

                } /* down */

                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Up:
                {
                    
                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary up strong settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary up weak settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary up reference settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */
                    
                    break;

                } /* up */
                
                default:
                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Reference:
                {

                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary reference strong settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary reference weak settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary reference reference settings"
                            );

                            primary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    UnityEngine.Vector3.one
                                );

                            primary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */

                    break;

                } /* reference */

            } /* switch() */

            // apply offset

            var primary_angular_acceleration 
                = primary_offset_angular_matrix.MultiplyVector(
                    new UnityEngine.Vector3(
                        user_angular_acceleration_threshold[0],
                        user_angular_acceleration_threshold[1],
                        user_angular_acceleration_threshold[2]
                    )
                );

            var primary_linear_acceleration
                = primary_offset_linear_matrix.MultiplyVector(
                    new UnityEngine.Vector3(
                        user_linear_acceleration_threshold[0],
                        user_linear_acceleration_threshold[1],
                        user_linear_acceleration_threshold[2]
                    )
                );

            // primary stim settings

            current_angular_acceleration_target           = new float[]{ primary_angular_acceleration.x, primary_angular_acceleration.y, primary_angular_acceleration.z };
            current_angular_velocity_saturation_threshold = this.FSM.CurrentSettings.phase_B_settings.primary_angular_velocity_saturation_threshold;
            current_angular_displacement_limiter          = this.FSM.CurrentSettings.phase_B_settings.primary_angular_displacement_limiter;
            
            current_linear_acceleration_target            = new float[]{ primary_linear_acceleration.x, primary_linear_acceleration.y, primary_linear_acceleration.z };
            current_linear_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.primary_linear_velocity_saturation_threshold;
            current_linear_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.primary_linear_displacement_limiter;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : primary target {"
                + "[current_angular_acceleration_target:" 
                    + System.String.Join(",",current_angular_acceleration_target) 
                + "][current_angular_velocity_saturation_threshold:" 
                    + System.String.Join(",",current_angular_velocity_saturation_threshold) 
                + "][current_linear_acceleration_target:" 
                    + System.String.Join(",",current_linear_acceleration_target) 
                + "][current_linear_velocity_saturation_threshold:" 
                    + System.String.Join(",",current_linear_velocity_saturation_threshold) 
                + "]}"
            );

            // filtering
            foreach(var (saturation_item, index) in current_angular_velocity_saturation_threshold.Select((e,idx) => (e,idx)))
            {
                if(saturation_item == 0.0f)
                {
                    current_angular_velocity_saturation_threshold[index] 
                        = (
                            current_angular_acceleration_target[index] 
                            * ( this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration / 1000.0f )
                        );
                }
            }
            foreach(var (saturation_item, index) in current_linear_velocity_saturation_threshold.Select((e,idx) => (e,idx)))
            {
                if(saturation_item == 0.0f )
                {
                    current_linear_velocity_saturation_threshold[index] 
                        = (
                            current_linear_acceleration_target[index] 
                            * ( this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration / 1000.0f )
                        );
                }
            }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : primary stim"
            );
                
            // virtual if SOA < 0
            if(this.FSM.CurrentSettings.phase_B_settings.SOA < 0.0f)
            {
            
                // raise virtual accel motion
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        current_angular_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_angular_velocity_saturation_threshold.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_angular_displacement_limiter.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_velocity_saturation_threshold.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_displacement_limiter.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        current_angular_acceleration_target,
                        current_angular_velocity_saturation_threshold,
                        current_angular_displacement_limiter,
                        current_linear_acceleration_target,
                        current_linear_velocity_saturation_threshold,
                        current_linear_displacement_limiter,
                        this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // append to running task
                parallel_tasks.Add(
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for virtual motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_virtual_motion_idle_point.Task; 

                        }
                    ).Unwrap()
                );
            
            } 
            // physical if SOA >= 0
            else
            {
            
                // raise physical accel motion
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    physical_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_acceleration_target.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_velocity_saturation_threshold.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_displacement_limiter.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_acceleration_target.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_velocity_saturation_threshold.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_displacement_limiter.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.primary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    physical_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        (bHasRealMotion ? current_angular_acceleration_target           : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_angular_velocity_saturation_threshold : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_angular_displacement_limiter          : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_acceleration_target            : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_velocity_saturation_threshold  : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_displacement_limiter           : new float[]{ 0.0f, 0.0f, 0.0f }),
                        this.FSM.CurrentSettings.phase_B_settings.primary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // append to running task
                parallel_tasks.Add(
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for physical motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_physical_motion_idle_point.Task;

                        }
                    ).Unwrap()
                );
            
            } /* if() */

            #endregion

            // only if there is a SOA, else ignore
            if(this.FSM.CurrentSettings.phase_B_settings.SOA != 0.0f)
            {

                // SOA
                await apollon.ApollonHighResolutionTime.DoSleep(
                    UnityEngine.Mathf.Abs(
                        this.FSM.CurrentSettings.phase_B_settings.SOA
                    )   
                );

            } /* if()*/
        
            #region secondary stim
            
            // find the offset matrix 
            
            // find the offset matrix 
            var secondary_offset_angular_matrix = UnityEngine.Matrix4x4.identity;
            var secondary_offset_linear_matrix = UnityEngine.Matrix4x4.identity;

            // nested switch... i know
            switch(this.FSM.CurrentResults.phase_A_results.user_command)
            {

                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Down:
                {
                    
                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary down strong settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary down weak settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary down reference settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_down_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_down_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */

                    break;

                } /* down */

                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Up:
                {
                    
                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary up strong settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary up weak settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary up reference settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_up_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.Euler(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_up_acceleration_rotation_from_reference[2]
                                    ),
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */
                    
                    break;

                } /* up */
                
                default:
                case (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Reference:
                {

                    switch(this.FSM.CurrentSettings.passive_intensity_type)
                    {

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Strong:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary reference strong settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* strong */

                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Weak:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary reference weak settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    new UnityEngine.Vector3(
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[0],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[1],
                                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_scale_from_reference[2]
                                    )
                                );

                            break;

                        } /* weak */

                        default:
                        case AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Reference:
                        {

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary reference reference settings"
                            );

                            secondary_offset_angular_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    UnityEngine.Vector3.one
                                );

                            secondary_offset_linear_matrix 
                                = UnityEngine.Matrix4x4.TRS(
                                    UnityEngine.Vector3.zero,
                                    UnityEngine.Quaternion.identity,
                                    UnityEngine.Vector3.one
                                );

                            break;

                        } /* reference */

                    } /* switch() */

                    break;

                } /* reference */

            } /* switch() */

            // apply offset

            var secondary_angular_acceleration 
                = secondary_offset_angular_matrix.MultiplyVector(
                    new UnityEngine.Vector3(
                        user_angular_acceleration_threshold[0],
                        user_angular_acceleration_threshold[1],
                        user_angular_acceleration_threshold[2]
                    )
                );

            var secondary_linear_acceleration
                = secondary_offset_linear_matrix.MultiplyVector(
                    new UnityEngine.Vector3(
                        user_linear_acceleration_threshold[0],
                        user_linear_acceleration_threshold[1],
                        user_linear_acceleration_threshold[2]
                    )
                );

            // secondary stim settings

            current_angular_acceleration_target           = new float[]{ secondary_angular_acceleration.x, secondary_angular_acceleration.y, secondary_angular_acceleration.z };
            current_angular_velocity_saturation_threshold = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_velocity_saturation_threshold;
            current_angular_displacement_limiter          = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_displacement_limiter;
            
            current_linear_acceleration_target            = new float[]{ secondary_linear_acceleration.x, secondary_linear_acceleration.y, secondary_linear_acceleration.z };
            current_linear_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_velocity_saturation_threshold;
            current_linear_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_displacement_limiter;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : secondary target {"
                + "[current_angular_acceleration_target:" 
                    + System.String.Join(",",current_angular_acceleration_target) 
                + "][current_angular_velocity_saturation_threshold:" 
                    + System.String.Join(",",current_angular_velocity_saturation_threshold) 
                + "][current_linear_acceleration_target:" 
                    + System.String.Join(",",current_linear_acceleration_target) 
                + "][current_linear_velocity_saturation_threshold:" 
                    + System.String.Join(",",current_linear_velocity_saturation_threshold) 
                + "]}"
            );

            // filtering
            foreach(var (saturation_item, index) in current_angular_velocity_saturation_threshold.Select((e,idx) => (e,idx)))
            {
                if(saturation_item == 0.0f)
                {
                    current_angular_velocity_saturation_threshold[index] 
                        = (
                            current_angular_acceleration_target[index] 
                            * ( this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration / 1000.0f )
                        );
                }
            }
            foreach(var (saturation_item, index) in current_linear_velocity_saturation_threshold.Select((e,idx) => (e,idx)))
            {
                if(saturation_item == 0.0f )
                {
                    current_linear_velocity_saturation_threshold[index] 
                        = (
                            current_linear_acceleration_target[index] 
                            * ( this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration / 1000.0f )
                        );
                }
            }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : secondary stim"
            );
                
            // virtual if SOA >= 0
            if(this.FSM.CurrentSettings.phase_B_settings.SOA >= 0.0f)
            {
            
                // raise virtual accel motion
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        current_angular_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_angular_velocity_saturation_threshold.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_angular_displacement_limiter.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_velocity_saturation_threshold.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        current_linear_displacement_limiter.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        current_angular_acceleration_target,
                        current_angular_velocity_saturation_threshold,
                        current_angular_displacement_limiter,
                        current_linear_acceleration_target,
                        current_linear_velocity_saturation_threshold,
                        current_linear_displacement_limiter,
                        this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // append to running task
                parallel_tasks.Add(
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for virtual motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_virtual_motion_idle_point.Task; 

                        }
                    ).Unwrap()
                );
            
            }
            // physical if SOA < 0
            else
            {
            
                // raise virtual accel motion
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    physical_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_acceleration_target.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_velocity_saturation_threshold.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_angular_displacement_limiter.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_acceleration_target.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_velocity_saturation_threshold.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        (
                            bHasRealMotion
                                ? (
                                    current_linear_displacement_limiter.Select(
                                        (e, idx) 
                                            => new { 
                                                Value = e, 
                                                IsMandatory = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_mandatory_axis[idx] 
                                            }
                                    ).Select(
                                        x => (
                                            x.IsMandatory ? x.Value : 0.0f
                                        )
                                    ).ToArray()
                                )
                                : new float[]{ 0.0f, 0.0f, 0.0f }
                        ),
                        this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    physical_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        (bHasRealMotion ? current_angular_acceleration_target           : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_angular_velocity_saturation_threshold : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_angular_displacement_limiter          : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_acceleration_target            : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_velocity_saturation_threshold  : new float[]{ 0.0f, 0.0f, 0.0f }),
                        (bHasRealMotion ? current_linear_displacement_limiter           : new float[]{ 0.0f, 0.0f, 0.0f }),
                        this.FSM.CurrentSettings.phase_B_settings.secondary_stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // append to running task
                parallel_tasks.Add(
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for physical motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_physical_motion_idle_point.Task;

                        }
                    ).Unwrap()
                );
            
            } /* if() */

            #endregion

            // wait for both physical & virtual idle sync point + end of phase timer
            await System.Threading.Tasks.Task.WhenAll(parallel_tasks);            

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : reaching phase end"
            );

            // unregister our synchronisation function
            virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent  -= sync_end_virtual_motion_local_function;
            physical_motion_system_bridge.ConcreteDispatcher.IdleEvent -= sync_end_physical_motion_local_function;
            
            // // inhibit visual degradation
            // if(this.FSM.CurrentSettings.bWithVisualDegradation)
            // {

            //     // post processing
            //     var postProcessingVolume = UnityEngine.Component.FindObjectsOfType<UnityEngine.Rendering.PostProcessing.PostProcessVolume>()[0];
            //     if(postProcessingVolume != null)
            //     {

            //         // blur
            //         UnityEngine.Rendering.PostProcessing.DepthOfField dof = null;
            //         if(postProcessingVolume.TryGetSettings(out dof))
            //         {
            //             dof.enabled.Override(false);
            //         }
                    
            //         // dirty
            //         UnityEngine.Rendering.PostProcessing.Grain grain = null;
            //         if(postProcessingVolume.TryGetSettings(out grain))
            //         {
            //             grain.enabled.Override(false);
            //         }

            //     } /* if() */

            // } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV4PhaseB */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
