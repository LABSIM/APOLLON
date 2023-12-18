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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV4Phase0 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4Phase0(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // reset internal if required 
            this.FSM.CurrentResults.phase_A_results.step_is_valid = false;

            // extract settings 
            float[]
                initial_angular_acceleration_target           = this.FSM.CurrentSettings.phase_0_settings.angular_acceleration_target,
                initial_angular_velocity_saturation_threshold = this.FSM.CurrentSettings.phase_0_settings.angular_velocity_saturation_threshold,
                initial_angular_displacement_limiter          = this.FSM.CurrentSettings.phase_0_settings.angular_displacement_limiter,
                initial_linear_acceleration_target            = this.FSM.CurrentSettings.phase_0_settings.linear_acceleration_target,
                initial_linear_velocity_saturation_threshold  = this.FSM.CurrentSettings.phase_0_settings.linear_velocity_saturation_threshold,
                initial_linear_displacement_limiter           = this.FSM.CurrentSettings.phase_0_settings.linear_displacement_limiter;

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
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // synchronisation mechanism (TCS + local function)
            var sync_init_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var sync_virtual_motion_idle_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var sync_physical_motion_idle_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            // var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = (this.FSM.CurrentSettings.bIsActive ? "Condition manuelle" : "Condition automatique");

            // show grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // // if active condition 
            // if (this.FSM.CurrentSettings.bIsActive)
            // {

            //     // synchronisation mechanism (TCS + local function)
            //     System.EventHandler<
            //         gameplay.control.AgencyAndThresholdPerceptionV4ControlDispatcher.AgencyAndThresholdPerceptionV4ControlEventArgs
            //     > sync_local_function 
            //         = (sender, args) => sync_init_point?.TrySetResult(true);

            //     // void sync_local_function(object sender, gameplay.control.AgencyAndThresholdPerceptionV4ControlDispatcher.AgencyAndThresholdPerceptionV4ControlEventArgs e)
            //     //     => sync_init_point?.TrySetResult(true);

            //     // register our synchronisation function
            //     control_bridge.ConcreteDispatcher.ThrottleNegativeCommandTriggeredEvent += sync_local_function;

            //     // wait synchronisation point indefinitely & reset it once hit
            //     await sync_init_point.Task;

            //     // unregister our synchronisation function
            //     control_bridge.ConcreteDispatcher.ThrottleNegativeCommandTriggeredEvent -= sync_local_function;

            // } 
            // else 
            // {

                // wait a certain amout of time
                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            // } /* if() */

            // lambdas
            System.EventHandler<
                apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs
            > sync_end_virtual_motion_local_function 
                = (sender, args) => sync_virtual_motion_idle_point?.TrySetResult(true);
            System.EventHandler<
                apollon.gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs
            > sync_end_physical_motion_local_function 
                = (sender, args) => sync_physical_motion_idle_point?.TrySetResult(true);

            // void sync_end_virtual_motion_local_function(object sender, gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs e)
            //     => sync_virtual_motion_idle_point?.TrySetResult(true);
            // void sync_end_physical_motion_local_function(object sender, gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs e)
            //     => sync_physical_motion_idle_point?.TrySetResult(true);

            // register our synchronisation function
            virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent  += sync_end_virtual_motion_local_function;
            physical_motion_system_bridge.ConcreteDispatcher.IdleEvent += sync_end_physical_motion_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : initial  stim"
            );

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

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : raise virtual accel motion"
                        );

                        apollon.ApollonEngine.Schedule(() => {
                            virtual_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                                initial_angular_acceleration_target,
                                initial_angular_velocity_saturation_threshold,
                                initial_angular_displacement_limiter,
                                initial_linear_acceleration_target,
                                initial_linear_velocity_saturation_threshold,
                                initial_linear_displacement_limiter,
                                this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f,
                                (
                                    this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                                    ? true 
                                    : false
                                )
                            );
                        });

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for virtual motion idle state"
                        );

                        await sync_virtual_motion_idle_point.Task; 
                        
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : virtual motion idle state reached"
                        );

                    }
                ).Unwrap(),

                parallel_tasks_factory.StartNew(
                    async () => 
                    { 

                        // skip
                        if(!bHasRealMotion)
                            return;

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : raise physical accel motion"
                        );

                        apollon.ApollonEngine.Schedule(() => {
                            physical_motion_system_bridge.ConcreteDispatcher.RaiseAccelerate(
                                initial_angular_acceleration_target,
                                initial_angular_velocity_saturation_threshold,
                                initial_angular_displacement_limiter,
                                initial_linear_acceleration_target,
                                initial_linear_velocity_saturation_threshold,
                                initial_linear_displacement_limiter,
                                this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f,
                                (
                                    this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                                    ? true 
                                    : false
                                )
                            );
                        });

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : waiting for physical motion idle state"
                        );

                        await sync_physical_motion_idle_point.Task; 

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : physical motion idle state reached"
                        );

                    }
                ).Unwrap(),

                parallel_tasks_factory.StartNew(
                    async () => 
                    { 
                        
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : will wait " 
                            + this.FSM.CurrentSettings.phase_0_settings.duration / 4.0f
                            + "ms before hiding green frame"
                        );
                        
                        // hide green frame at mid duration
                        await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 4.0f);

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : hide green frame"
                        );

                        apollon.ApollonEngine.Schedule(
                            () => apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI)
                        );

                    }
                ).Unwrap(),

                parallel_tasks_factory.StartNew(
                    async () => 
                    { 

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : will wait " 
                            + this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f
                            + "ms before ending phase"
                        );

                        // end of phase timer
                        await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : timeout, phase will end"
                        );

                    }
                ).Unwrap()

            }; /* parallel_tasks(){} */
            
            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Mise en mouvement";

            // hide grey frame/cross & show green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // // check visual degradation
            // if(this.FSM.CurrentSettings.bWithVisualDegradation)
            // {
                
            //     // enable fog
            //     UnityEngine.RenderSettings.fog = true;

            //     // setup fog type
            //     switch(this.FSM.CurrentSettings.fog_type)
            //     {
            //         case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Linear:
            //         {
            //             UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Linear;
            //             break;
            //         }

            //         default:
            //         case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Exponential:
            //         {
            //             UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Exponential;
            //             break;
            //         }

            //         case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.ExponentialSquared:
            //         {
            //             UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.ExponentialSquared;
            //             break;
            //         }

            //     } /* switch() */

            //     // setup fog parameter
            //     UnityEngine.RenderSettings.fogColor   
            //         = new UnityEngine.Color(
            //             /* R */ this.FSM.CurrentSettings.fog_color[0], 
            //             /* G */ this.FSM.CurrentSettings.fog_color[1], 
            //             /* B */ this.FSM.CurrentSettings.fog_color[2]
            //         );
            //     UnityEngine.RenderSettings.fogDensity       = this.FSM.CurrentSettings.fog_density;
            //     UnityEngine.RenderSettings.fogStartDistance = this.FSM.CurrentSettings.fog_start_distance;
            //     UnityEngine.RenderSettings.fogEndDistance   = this.FSM.CurrentSettings.fog_end_distance;

            //     // solid background
            //     UnityEngine.Camera.main.backgroundColor = UnityEngine.RenderSettings.fogColor;
            //     UnityEngine.Camera.main.clearFlags      = UnityEngine.CameraClearFlags.SolidColor;

            // } /* if() */

            // wait for idle sync point
            await System.Threading.Tasks.Task.WhenAll(parallel_tasks);
            // System.Threading.Tasks.Task.WaitAll(parallel_tasks.ToArray());

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : reaching phase end"
            );

            // unregister our synchronisation function
            virtual_motion_system_bridge.ConcreteDispatcher.IdleEvent  -= sync_end_virtual_motion_local_function;
            physical_motion_system_bridge.ConcreteDispatcher.IdleEvent -= sync_end_physical_motion_local_function;

            // finally, hide green cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4Phase0.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines

        private System.Collections.IEnumerator doVirtualMotionInitialAcceleration
        (
            apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge _bridge,
            float[] _initial_angular_acceleration_target,
            float[] _initial_angular_velocity_saturation_threshold,
            float[] _initial_angular_displacement_limiter,
            float[] _initial_linear_acceleration_target,
            float[] _initial_linear_velocity_saturation_threshold,
            float[] _initial_linear_displacement_limiter
        )
        {

            _bridge.ConcreteDispatcher.RaiseAccelerate(
                _initial_angular_acceleration_target,
                _initial_angular_velocity_saturation_threshold,
                _initial_angular_displacement_limiter,
                _initial_linear_acceleration_target,
                _initial_linear_velocity_saturation_threshold,
                _initial_linear_displacement_limiter,
                this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f,
                (
                    this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                    ? true 
                    : false
                )
            );

            yield return null;

        } /* doVirtualMotionInitialAcceleration() */

        private System.Collections.IEnumerator doPhysicalMotionInitialAcceleration
        (
            apollon.gameplay.device.command.ApollonMotionSystemCommandBridge _bridge,
            float[] _initial_angular_acceleration_target,
            float[] _initial_angular_velocity_saturation_threshold,
            float[] _initial_angular_displacement_limiter,
            float[] _initial_linear_acceleration_target,
            float[] _initial_linear_velocity_saturation_threshold,
            float[] _initial_linear_displacement_limiter
        )
        {

            _bridge.ConcreteDispatcher.RaiseAccelerate(
                _initial_angular_acceleration_target,
                _initial_angular_velocity_saturation_threshold,
                _initial_angular_displacement_limiter,
                _initial_linear_acceleration_target,
                _initial_linear_velocity_saturation_threshold,
                _initial_linear_displacement_limiter,
                this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f,
                (
                    this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly 
                    ? true 
                    : false
                )
            );

            yield return null;

        } /* doPhysicalMotionInitialAcceleration() */

        #endregion

    } /* public sealed class AgencyAndThresholdPerceptionV4Phase0 */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
