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
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV2
{

    //
    // Acceleration phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV2PhaseF
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseF(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get angular/linear acceleration threshold raw string from user settings
            var angular_weak_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["angular_weak_acceleration_threshold"]
                .ToString();
            var angular_strong_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["angular_strong_acceleration_threshold"]
                .ToString();
            var linear_weak_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["linear_weak_acceleration_threshold"]
                .ToString();
            var linear_strong_acceleration_threshold_raw_string 
                = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["linear_strong_acceleration_threshold"]
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : user reference {"
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : weak settings"
                );

                // weak stim settings
                current_angular_acceleration_target
                    = user_angular_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_F_settings.angular_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_F_settings.angular_weak_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_F_settings.angular_weak_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_F_settings.linear_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_F_settings.linear_weak_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_F_settings.linear_weak_displacement_limiter;

            }
            else if(this.FSM.CurrentResults.phase_A_results.user_command == 2)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : strong settings"
                );

                // strong stim settings
                current_angular_acceleration_target
                    = user_angular_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_F_settings.angular_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_F_settings.angular_strong_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_F_settings.angular_strong_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_F_settings.linear_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_F_settings.linear_strong_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_F_settings.linear_strong_displacement_limiter;

            }
            else
            {

                // log 
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : unknown user_command... failed."
                );
                
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : target {"
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

            // get bridges
            var control_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as AgencyAndThresholdPerceptionV2ControlBridge;

            var motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            // current scenario
            bool bHasRealMotion = false; 
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    bHasRealMotion = false;
                    break;
                }
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisuoVestibular:
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
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : Could not find corresponding gameplay bridge !"
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
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            if(!bHasRealMotion) 
            {            

                void sync_end_stim_local_function(object sender, apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs e)
                    => sync_idle_point?.TrySetResult(true);

                // register our synchronisation function
                virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent += sync_end_stim_local_function;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : beginning "
                    + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                    + " stim"
                );
                    
                // check if it's a try/catch condition & begin stim
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
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
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // wait for idle state
                var phase_running_task
                    = System.Threading.Tasks.Task.Factory.StartNew(
                        async () => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting for idle state"
                            );
                            await sync_idle_point.Task; 
                        } 
                    // then sleep remaining idle time & raise end
                    ).Unwrap().ContinueWith( 
                        async antecedant => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting [" 
                                + (this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ))
                                + " ms] for remaining phase total time"
                            );
                            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ));
                        
                            // hit barrier 
                            sync_point.TrySetResult(true);

                        }
                    );
                await sync_point.Task;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : reaching phase end"
                );

                // unregister our motion synchronisation function
                virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent -= sync_end_stim_local_function;

            }
            else
            {

                void sync_end_stim_local_function(object sender, apollon.gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs e)
                    => sync_idle_point?.TrySetResult(true);

                // register our synchronisation function
                motion_system_bridge.ConcreteDispatcher.IdleEvent += sync_end_stim_local_function;
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : beginning "
                    + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                    + " stim"
                );

                // check if it's a try/catch condition & begin stim
                if(this.FSM.CurrentSettings.bIsTryCatch)
                {

                    motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                }
                else
                {

                    motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                        current_angular_acceleration_target,
                        current_angular_velocity_saturation_threshold,
                        current_angular_displacement_limiter,
                        current_linear_acceleration_target,
                        current_linear_velocity_saturation_threshold,
                        current_linear_displacement_limiter,
                        this.FSM.CurrentSettings.phase_F_settings.stim_duration,
                        (
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly 
                            ? true 
                            : false
                        )
                    );

                } /* if() */

                // wait for idle state
                var phase_running_task
                    = System.Threading.Tasks.Task.Factory.StartNew(
                        async () => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting for idle state"
                            );
                            await sync_idle_point.Task; 
                        } 
                    // then sleep remaining idle time & raise end
                    ).Unwrap().ContinueWith( 
                        async antecedant => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : waiting [" 
                                + (this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ))
                                + " ms] for remaining phase total time"
                            );
                            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_settings.total_duration - ( 2.0f * this.FSM.CurrentSettings.phase_F_settings.stim_duration ));
                        
                            // hit barrier 
                            sync_point.TrySetResult(true);

                        }
                    );
                await sync_point.Task;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : reaching phase end"
                );

                // unregister our motion synchronisation function
                motion_system_bridge.ConcreteDispatcher.IdleEvent -= sync_end_stim_local_function;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseF */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
