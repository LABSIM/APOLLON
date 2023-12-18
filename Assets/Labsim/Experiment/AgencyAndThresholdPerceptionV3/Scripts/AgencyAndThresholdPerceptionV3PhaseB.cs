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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV3
{

    //
    // Acceleration phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV3PhaseB
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV3Profile>
    {
        public AgencyAndThresholdPerceptionV3PhaseB(AgencyAndThresholdPerceptionV3Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = System.Diagnostics.Stopwatch.StartNew();

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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : user reference {"
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

            // get bridges
            var control_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV3ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control
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
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    bHasRealMotion = false;
                    break;
                }
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisuoVestibular:
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
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // synchronisation mechanism (TCS + local function)
            var sync_virtual_motion_idle_point  = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var sync_physical_motion_idle_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // callback
            void sync_end_virtual_motion_local_function(object sender, apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs e)
                => sync_virtual_motion_idle_point?.TrySetResult(true);
            void sync_end_physical_motion_local_function(object sender, apollon.gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs e)
                => sync_physical_motion_idle_point?.TrySetResult(true);

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
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : waiting [" 
                                + remaining 
                                + "ms] for end of phase"
                            );

                        }
                        else
                        {
                            
                            // log
                            UnityEngine.Debug.LogWarning(
                                "<color=Orange>Warn: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : strange... no remaing time to wait..."
                            );

                        } /* if() */

                        // wait end of phase 
                        await apollon.ApollonHighResolutionTime.DoSleep(remaining);

                    }
                ).Unwrap()
            };

            #region primary stim

            if(
                this.FSM.CurrentResults.phase_A_results.user_command 
                ==  (int)AgencyAndThresholdPerceptionV3Profile.Settings.IntensityIDType.Weak
            ) {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : primary weak settings"
                );

                // weak stim settings
                current_angular_acceleration_target
                    = user_angular_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.primary_angular_weak_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_B_settings.primary_linear_weak_displacement_limiter;

            }
            else if(
                this.FSM.CurrentResults.phase_A_results.user_command 
                    ==  (int)AgencyAndThresholdPerceptionV3Profile.Settings.IntensityIDType.Strong
            ) {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : primary strong settings"
                );

                // strong stim settings
                current_angular_acceleration_target
                    = user_angular_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.primary_angular_strong_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_B_settings.primary_linear_strong_displacement_limiter;

            }
            else
            {

                // log 
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : unknown user_command... failed."
                );
                
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : primary target {"
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : primary stim"
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : waiting for virtual motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_virtual_motion_idle_point.Task; 

                        }
                    ).Unwrap()
                );
            
            } /* if() */

            // physical if SOA >= 0
            if(this.FSM.CurrentSettings.phase_B_settings.SOA >= 0.0f)
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : waiting for physical motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_physical_motion_idle_point.Task;

                        }
                    ).Unwrap()
                );
            
            } /* if() */

            #endregion

            // only if three is a SOA, else ignore
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

            if(
                this.FSM.CurrentResults.phase_A_results.user_command 
                ==  (int)AgencyAndThresholdPerceptionV3Profile.Settings.IntensityIDType.Weak
            ) {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : secondary weak settings"
                );

                // weak stim settings
                current_angular_acceleration_target
                    = user_angular_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_weak_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_weak_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_weak_displacement_limiter;

            }
            else if(
                this.FSM.CurrentResults.phase_A_results.user_command 
                    ==  (int)AgencyAndThresholdPerceptionV3Profile.Settings.IntensityIDType.Strong
            ) {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : secondary strong settings"
                );

                // strong stim settings
                current_angular_acceleration_target
                    = user_angular_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_angular_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_velocity_saturation_threshold;
                current_angular_displacement_limiter           = this.FSM.CurrentSettings.phase_B_settings.secondary_angular_strong_displacement_limiter;
                
                current_linear_acceleration_target
                    = user_linear_strong_acceleration_threshold.Zip(
                        this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_acceleration_offset_from_reference,
                        (user_value, offset_value) 
                            => user_value + offset_value
                    ).ToArray();
                current_linear_velocity_saturation_threshold   = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_velocity_saturation_threshold;
                current_linear_displacement_limiter            = this.FSM.CurrentSettings.phase_B_settings.secondary_linear_strong_displacement_limiter;

            }
            else
            {

                // log 
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : unknown user_command... failed."
                );
                
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : secondary target {"
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : secondary stim"
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : waiting for virtual motion idle state"
                            );

                            // wait idling state to hit barrier
                            await sync_virtual_motion_idle_point.Task; 

                        }
                    ).Unwrap()
                );
            
            } /* if() */

            // physical if SOA < 0
            if(this.FSM.CurrentSettings.phase_B_settings.SOA < 0.0f)
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                            this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly 
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
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : waiting for physical motion idle state"
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Phase0.OnEntry() : reaching phase end"
            );

            // unregister our synchronisation function
            virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent  -= sync_end_virtual_motion_local_function;
            physical_motion_system_bridge.ConcreteDispatcher.IdleEvent -= sync_end_physical_motion_local_function;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV3PhaseB */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV3 */
