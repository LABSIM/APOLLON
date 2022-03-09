using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Acceleration phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseF
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseF(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get angular/linear acceleration threshold raw string from user settings
            var angular_weak_acceleration_threshold_raw_string 
                = ApollonExperimentManager.Instance.Session.participantDetails["angular_weak_acceleration_threshold"]
                .ToString();
            var angular_strong_acceleration_threshold_raw_string 
                = ApollonExperimentManager.Instance.Session.participantDetails["angular_strong_acceleration_threshold"]
                .ToString();
            var linear_weak_acceleration_threshold_raw_string 
                = ApollonExperimentManager.Instance.Session.participantDetails["linear_weak_acceleration_threshold"]
                .ToString();
            var linear_strong_acceleration_threshold_raw_string 
                = ApollonExperimentManager.Instance.Session.participantDetails["linear_strong_acceleration_threshold"]
                .ToString();
            
            // then pop first & last element & split from "," separator & convert to a float array
            float[] user_angular_weak_acceleration_threshold
                = System.Array.ConvertAll(
                    angular_weak_acceleration_threshold_raw_string.Substring(1, (angular_weak_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );
            float[] user_angular_strong_acceleration_threshold
                = System.Array.ConvertAll(
                    angular_strong_acceleration_threshold_raw_string.Substring(1, (angular_strong_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );
            float[] user_linear_weak_acceleration_threshold
                = System.Array.ConvertAll(
                    linear_weak_acceleration_threshold_raw_string.Substring(1, (linear_weak_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );
            float[] user_linear_strong_acceleration_threshold
                = System.Array.ConvertAll(
                    linear_strong_acceleration_threshold_raw_string.Substring(1, (linear_strong_acceleration_threshold_raw_string.Length - 2)).Split(','),
                    float.Parse
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : user reference {"
                + "[user_angular_weak_acceleration_threshold:" 
                    + System.String.Join(",",user_angular_weak_acceleration_threshold) 
                + "][user_angular_strong_acceleration_threshold:" 
                    + System.String.Join(",",user_angular_strong_acceleration_threshold) 
                + "][user_linear_weak_acceleration_threshold:" 
                    + System.String.Join(",",user_linear_weak_acceleration_threshold) 
                + "][user_linear_strong_acceleration_threshold:" 
                    + System.String.Join(",",user_linear_strong_acceleration_threshold) 
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
            if(this.FSM.CurrentResults.phase_A_results.user_command == 1)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : weak settings"
                );

                // weak stim settings
                current_angular_acceleration_target
                    = user_angular_weak_acceleration_threshold.Select(
                        value => this.FSM.CurrentSettings.phase_F_settings.angular_weak_acceleration_ratio_from_reference * value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_F_settings.angular_weak_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_F_settings.angular_weak_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_weak_acceleration_threshold.Select(
                        value => this.FSM.CurrentSettings.phase_F_settings.linear_weak_acceleration_ratio_from_reference * value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_F_settings.linear_weak_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_F_settings.linear_weak_displacement_limiter;

            }
            else if(this.FSM.CurrentResults.phase_A_results.user_command == 2)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : strong settings"
                );

                // strong stim settings
                current_angular_acceleration_target
                    = user_angular_strong_acceleration_threshold.Select(
                        value => this.FSM.CurrentSettings.phase_F_settings.angular_strong_acceleration_ratio_from_reference * value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_F_settings.angular_strong_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_F_settings.angular_strong_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_strong_acceleration_threshold.Select(
                        value => this.FSM.CurrentSettings.phase_F_settings.linear_strong_acceleration_ratio_from_reference * value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_F_settings.linear_strong_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_F_settings.linear_strong_displacement_limiter;

            }
            else
            {

                // log 
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : unknown user_command... failed."
                );
                
            } /* if() */

            // get bridges
            var control_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge;

            var motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            // current scenario
            bool bHasRealMotion = false; 
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    bHasRealMotion = false;
                    break;
                }
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly:
                case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    bHasRealMotion = true;
                    break;
                }

            } /* switch() */

            // check
            if (control_bridge == null || motion_system_bridge == null || virtual_motion_system_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // filtering
            foreach(var (saturation_item, index) in current_angular_velocity_saturation_threshold.Select((e,idx) => (e,idx)))
            {
                if(saturation_item == 0.0f)
                {
                    current_angular_velocity_saturation_threshold[index] 
                        = (
                            current_angular_acceleration_target[index] 
                            * ( this.FSM.CurrentSettings.phase_F_settings.stim_duration / 1000.0f )
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
                            * ( this.FSM.CurrentSettings.phase_F_settings.stim_duration / 1000.0f )
                        );
                }
            }

            // synchronisation mechanism (TCS + local function)
            var sync_idle_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            if(!bHasRealMotion) 
            {            

                void sync_end_stim_local_function(object sender, gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.EventArgs e)
                    => sync_idle_point?.TrySetResult(true);

                // register our synchronisation function
                virtual_motion_system_bridge.Dispatcher.IdleEvent += sync_end_stim_local_function;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : beginning "
                    + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                    + " stim"
                );
                    
                // check if it's a try/catch condition & begin stim
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    virtual_motion_system_bridge.Dispatcher.RaiseAccelerate(
                        current_angular_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    virtual_motion_system_bridge.Dispatcher.RaiseAccelerate(
                        current_angular_acceleration_target,
                        current_angular_velocity_saturation_threshold,
                        current_angular_displacement_limiter,
                        current_linear_acceleration_target,
                        current_linear_velocity_saturation_threshold,
                        current_linear_displacement_limiter,
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // wait for idle state
                await System.Threading.Tasks.Task.Factory.StartNew(
                        async () => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting for idle state"
                            );
                            await sync_idle_point.Task; 
                        } 
                    // then sleep remaining idle time & raise end
                    ).Unwrap().ContinueWith( 
                        async antecedant => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting [" 
                                + (this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ))
                                + " ms] for remaining phase total time"
                            );
                            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ));
                        
                        }
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : reaching phase end"
                );

                // unregister our motion synchronisation function
                virtual_motion_system_bridge.Dispatcher.IdleEvent -= sync_end_stim_local_function;

            }
            else
            {

                void sync_end_stim_local_function(object sender, gameplay.device.command.ApollonMotionSystemCommandDispatcher.EventArgs e)
                    => sync_idle_point?.TrySetResult(true);

                // register our synchronisation function
                motion_system_bridge.Dispatcher.IdleEvent += sync_end_stim_local_function;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : beginning "
                    + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                    + " stim"
                );

                // check if it's a try/catch condition & begin stim
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    motion_system_bridge.Dispatcher.RaiseAccelerate(
                        current_angular_acceleration_target.Select(
                            (e, idx) 
                                => new { 
                                    Value = e, 
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.angular_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
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
                                    IsMandatory = this.FSM.CurrentSettings.phase_F_settings.linear_mandatory_axis[idx] 
                                }
                        ).Select(
                            x => (
                                x.IsMandatory ? x.Value : 0.0f
                            )
                        ).ToArray(),
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    motion_system_bridge.Dispatcher.RaiseAccelerate(
                        current_angular_acceleration_target,
                        current_angular_velocity_saturation_threshold,
                        current_angular_displacement_limiter,
                        current_linear_acceleration_target,
                        current_linear_velocity_saturation_threshold,
                        current_linear_displacement_limiter,
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // wait for idle state
                await System.Threading.Tasks.Task.Factory.StartNew(
                        async () => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting for idle state"
                            );
                            await sync_idle_point.Task; 
                        } 
                    // then sleep remaining idle time & raise end
                    ).Unwrap().ContinueWith( 
                        async antecedant => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting [" 
                                + (this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ))
                                + " ms] for remaining phase total time"
                            );
                            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ));
                        
                        }
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : reaching phase end"
                );

                // unregister our motion synchronisation function
                motion_system_bridge.Dispatcher.IdleEvent -= sync_end_stim_local_function;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseF */

} /* } Labsim.apollon.experiment.phase */
