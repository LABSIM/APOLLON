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
    public sealed class AgencyAndThresholdPerceptionV4PhaseA 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4PhaseA(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // // check visual degradation
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
            //             dof.enabled.value = true;
            //         }

                    
            //         // dirty
            //         UnityEngine.Rendering.PostProcessing.Grain grain = null;
            //         if(postProcessingVolume.TryGetSettings(out grain))
            //         {
            //             grain.enabled.value = true;
            //         }

            //     } /* if() */

            // }

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {
                
                // currrent timestamp
                System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

                // synchronisation mechanism (TCS + lambda event handler)
                var sync_point 
                    = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();
                System.EventHandler<
                    AgencyAndThresholdPerceptionV4ControlDispatcher.AgencyAndThresholdPerceptionV4ControlEventArgs
                > sync_user_response_local_function 
                    = (sender, args) => {

                        // hit barrier
                        sync_point?.TrySetResult((
                            /* user responded */ 
                            true, 
                            /* unity render timestamp */
                            UnityEngine.Time.time,
                            /* host timestamp */
                            apollon.ApollonHighResolutionTime.Now.ToString(),
                            /* current timestamp */
                            current_stopwatch.ElapsedMilliseconds
                        ));

                    }; /* lambda */
                System.EventHandler<
                    AgencyAndThresholdPerceptionV4ControlDispatcher.AgencyAndThresholdPerceptionV4ControlEventArgs
                > user_interaction_local_function 
                    = (sender, args) => {  
                            
                        // update UI cursor from raw command value
                        apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value  
                            = -1.0f * args.JoystickVertical;
                        
                    }; /* lambda */

                // get bridge
                var control_bridge
                    = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                    ) as AgencyAndThresholdPerceptionV4ControlBridge;

                // register our synchronisation function
                control_bridge.ConcreteDispatcher.ResponseTriggeredEvent            += sync_user_response_local_function;
                control_bridge.ConcreteDispatcher.JoystickVerticalValueChangedEvent += user_interaction_local_function;
                
                // restart current stopwatch
                current_stopwatch.Restart();

                // update instructions 
                this.FSM.CurrentInstruction = "Direction ?";

                // reset cursor value to default position
                apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                    apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 0.0f;

                // show intensity slider                            
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI);

                // running
                var phase_running_task_ct_src = new System.Threading.CancellationTokenSource();
                System.Threading.CancellationToken phase_running_task_ct = phase_running_task_ct_src.Token;
                var phase_running_task
                    // wait response
                    = System.Threading.Tasks.Task.Factory.StartNew(
                        async () => 
                        { 
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : user has "
                                + this.FSM.CurrentSettings.phase_A_settings.response_max_duration
                                + " ms to respond"
                            );

                            // wait a certain amout of time between each bound if cancel not requested
                            if(!phase_running_task_ct.IsCancellationRequested)
                            {
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.response_max_duration);
                            }

                        },
                        phase_running_task_ct_src.Token 
                    ).Unwrap().ContinueWith(
                        antecedent => 
                        {
                            if(!phase_running_task_ct.IsCancellationRequested)
                            {

                                if(!sync_point.Task.IsCompleted) 
                                {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : user hasn't responded, injecting default result"
                                    );
                                    
                                    sync_point?.TrySetResult((false, -1.0f, "-1.0", -1));

                                } else {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : user has responded, keep result"
                                    );
                                
                                } /* if() */

                            } /* if() */
                        },
                        phase_running_task_ct_src.Token
                    );
                
                // wait until any result
                (bool, float, string, long) result = await sync_point.Task;

                // unregister our synchronisation function
                control_bridge.ConcreteDispatcher.ResponseTriggeredEvent            -= sync_user_response_local_function;
                control_bridge.ConcreteDispatcher.JoystickVerticalValueChangedEvent -= user_interaction_local_function;

                // hide intensity slider
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI);

                // validity barrier
                bool bValid = false;

                // check result
                if(result.Item1)
                {

                    // cancel running task
                    phase_running_task_ct_src.Cancel();

                    // it's a hit then check intensity from slider value
                    float slider_value 
                        = apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                           apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value;

                    // validity pass ?
                    if(System.Math.Sign(slider_value) < 0)
                    {
                        
                        // down stim == check
                        if(
                            (
                                (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                                - this.FSM.downConditionCount
                            ) > 0
                        ) 
                        {

                            // check settings
                            if(this.FSM.CurrentSettings.passive_side_type != AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Down)
                            {
                                
                                // switch parameters with next available trial

                                for(
                                    var current_index = apollon.experiment.ApollonExperimentManager.Instance.Session.NextTrial.numberInBlock -1;
                                    current_index <= apollon.experiment.ApollonExperimentManager.Instance.Session.LastTrial.numberInBlock - 1;
                                    ++current_index
                                ) {
                                    
                                    // next settings
                                    var next_UXF 
                                        = new UXF.Settings(
                                            apollon.experiment.ApollonExperimentManager.Instance.Session.Trials.ToList()[current_index].settings.baseDict
                                        );
                                    var next_Apollon
                                        = new AgencyAndThresholdPerceptionV4Profile.Settings();
                        
                                    next_Apollon.ImportUXFSettings(next_UXF);
                                
                                    // check
                                    if((next_Apollon.passive_side_type != AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Down)
                                        || (next_Apollon.bIsActive != true)
                                    ) {
                                        continue;
                                    }

                                    // it's a match ! switch dict

                                    UnityEngine.Debug.LogWarning(
                                        "<color=Orange>Warning: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : found a match (Down condition) in next trial parameter, switch from ["
                                        + this.FSM.CurrentSettings.pattern_type
                                        + "] to ["
                                        + next_Apollon.pattern_type
                                        + "]"
                                    );

                                    // real current -- to -> real next
                                    apollon.experiment.ApollonExperimentManager.Instance.Session.Trials.ToList()[current_index].settings.UpdateWithDict(
                                        apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentTrial.settings.baseDict
                                    );

                                    // temp next -- to -> real current
                                    apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentTrial.settings.UpdateWithDict(
                                        next_UXF.baseDict
                                    );

                                    // import current & log
                                    this.FSM.CurrentSettings = next_Apollon;
                                    this.FSM.CurrentSettings.LogUXFSettings();

                                    // banco ! stop iterating
                                    bValid = true;
                                    break;

                                } /* for() */

                            }
                            else
                            {
                                
                                // banco !
                                bValid = true;
                                
                            } /* if() */
                            
                            if(bValid)
                            {

                                // finally OK, save command, increment counter & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command
                                    = (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Down;
                                this.FSM.downConditionCount++;
                                
                            }
                            else
                            {
                                
                                // no more down settings available == fail

                                // update instructions 
                                this.FSM.CurrentInstruction = "Parametrages direction bas epuisé";

                            } /* if() */

                        }
                        else
                        {
                            
                            // no more down count available == fail

                            // update instructions 
                            this.FSM.CurrentInstruction = "Quota direction bas epuisé";

                        } /* if() */

                    }
                    else if(System.Math.Sign(slider_value) > 0)
                    {
                        
                        // up stim == check
                        if(
                            (
                                (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                                - this.FSM.upConditionCount
                            ) > 0
                        ) 
                        {

                            // check settings
                            if(this.FSM.CurrentSettings.passive_side_type != AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Up)
                            {
                                
                                // switch parameters with next available trial

                                for(
                                    var current_index = apollon.experiment.ApollonExperimentManager.Instance.Session.NextTrial.numberInBlock -1;
                                    current_index <= apollon.experiment.ApollonExperimentManager.Instance.Session.LastTrial.numberInBlock - 1;
                                    ++current_index
                                ) {
                                    
                                    // next settings
                                    var next_UXF 
                                        = new UXF.Settings(
                                            apollon.experiment.ApollonExperimentManager.Instance.Session.Trials.ToList()[current_index].settings.baseDict
                                        );
                                    var next_Apollon
                                        = new AgencyAndThresholdPerceptionV4Profile.Settings();
                        
                                    next_Apollon.ImportUXFSettings(next_UXF);
                                
                                    // check
                                    if((next_Apollon.passive_side_type != AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Up)
                                        || (next_Apollon.bIsActive != true)
                                    ) {
                                        continue;
                                    }

                                    // it's a match ! switch dict

                                    UnityEngine.Debug.LogWarning(
                                        "<color=Orange>Warning: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : found a match (Up condition) in next trial parameter, switch from ["
                                        + this.FSM.CurrentSettings.pattern_type
                                        + "] to ["
                                        + next_Apollon.pattern_type
                                        + "]"
                                    );

                                    // real current -- to -> real next
                                    apollon.experiment.ApollonExperimentManager.Instance.Session.Trials.ToList()[current_index].settings.UpdateWithDict(
                                        apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentTrial.settings.baseDict
                                    );

                                    // temp next -- to -> real current
                                    apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentTrial.settings.UpdateWithDict(
                                        next_UXF.baseDict
                                    );

                                    // import current & log
                                    this.FSM.CurrentSettings = next_Apollon;
                                    this.FSM.CurrentSettings.LogUXFSettings();

                                    // banco ! stop iterating
                                    bValid = true;
                                    break;

                                } /* for() */

                            }
                            else
                            {
                                
                                // banco !
                                bValid = true;
                                
                            } /* if() */
                            
                            if(bValid)
                            {

                                // finally OK, save command, increment counter & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command
                                    = (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Up;
                                this.FSM.upConditionCount++;
                                
                            }
                            else
                            {
                                
                                // no more up settings available == fail

                                // update instructions 
                                this.FSM.CurrentInstruction = "Parametrages direction haut epuisé";

                            } /* if() */

                        }
                        else
                        {
                            
                            // no more strong available == fail

                            // update instructions 
                            this.FSM.CurrentInstruction = "Quota direction haut epuisé";

                        } /* if() */

                    }
                    else
                    {

                        // neutral zone == fail

                        // update instructions 
                        this.FSM.CurrentInstruction = "Neutre non valide";

                    } /* if() */

                // timeout
                } else {

                    // Too long == fail

                    // update instructions 
                    this.FSM.CurrentInstruction = "Trop lent";
                    
                } /* if() */

                // validity pass ?
                if(bValid)
                {
                    
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : response OK !"
                    );
                    
                    // fade in to black for vestibular-only scenario
                    if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly)
                    {

                        // run it asynchronously
                        this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration);

                    } /* if() */

                    // get a random timeout
                    float random_latency = this.FSM.GetRandomLatencyFromBucket();

                    // inject current user latency
                    this.FSM.AddUserLatencyToBucket(result.Item4);
                    
                    // record user_*
                    this.FSM.CurrentResults.phase_A_results.user_measured_latency = result.Item4;
                    this.FSM.CurrentResults.phase_A_results.user_randomized_stim_latency = random_latency;        
                    this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = result.Item2;
                    this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = result.Item3;
                    
                    // request end phase
                    this.FSM.CurrentResults.phase_A_results.step_is_valid = true;

                }
                else
                {
                    
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : fail.. reseting"
                    );

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

                    // get bridges
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

                    // check
                    if (physical_motion_system_bridge == null || virtual_motion_system_bridge == null)
                    {

                        // log
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : Could not find corresponding gameplay bridge !"
                        );

                        // fail
                        return;

                    } /* if() */

                    // synchronisation mechanism (TCS + local function)
                    var sync_virtual_motion_init_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
                    var sync_physical_motion_init_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

                    // callback
                    void sync_end_virtual_motion_local_function(object sender, apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandDispatcher.VirtualMotionSystemCommandEventArgs e)
                        => sync_virtual_motion_init_point?.TrySetResult(true);
                    void sync_end_physical_motion_local_function(object sender, apollon.gameplay.device.command.ApollonMotionSystemCommandDispatcher.MotionSystemCommandEventArgs e)
                        => sync_physical_motion_init_point?.TrySetResult(true);

                    // register our synchronisation function
                    virtual_motion_system_bridge.ConcreteDispatcher.InitEvent  += sync_end_virtual_motion_local_function;
                    physical_motion_system_bridge.ConcreteDispatcher.InitEvent += sync_end_physical_motion_local_function;

                    // parallel task
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

                        // UI
                        parallel_tasks_factory.StartNew(
                            async () => 
                            { 

                                // hide intensity slider & show red cross
                                apollon.ApollonEngine.Schedule(() => {
                                    apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                                });

                                // wait confirm_duration ms
                                await apollon.ApollonHighResolutionTime.DoSleep(
                                    this.FSM.CurrentSettings.phase_A_settings.confirm_duration
                                );

                                // hide red cross
                                apollon.ApollonEngine.Schedule(() => {
                                    apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                                });

                            }
                        ).Unwrap(),

                        // virtual motion
                        System.Threading.Tasks.Task.Factory.StartNew(
                            async () => 
                            { 

                                // stop movement
                                apollon.ApollonEngine.Schedule(() => {
                                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();
                                });

                                // wait a certain amout of time 
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

                                // reset movement
                                apollon.ApollonEngine.Schedule(() => {
                                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_A_settings.reset_duration);
                                });

                                UnityEngine.Debug.Log(
                                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : waiting for virtual motion init state"
                                );

                                await sync_virtual_motion_init_point.Task; 
                                
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.reset_duration);

                            }
                        ).Unwrap(),

                        // physical motion
                        System.Threading.Tasks.Task.Factory.StartNew(
                            async () => 
                            { 

                                // check
                                if(!bHasRealMotion)
                                {
                                    return;
                                }

                                // stop movement
                                apollon.ApollonEngine.Schedule(() => {
                                    physical_motion_system_bridge.ConcreteDispatcher.RaiseDecelerate();
                                });

                                // wait a certain amout of time 
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

                                // reset movement
                                apollon.ApollonEngine.Schedule(() => {
                                    physical_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_A_settings.reset_duration);
                                });
                                
                                UnityEngine.Debug.Log(
                                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : waiting for physical motion init state"
                                );

                                await sync_physical_motion_init_point.Task; 

                            }
                        ).Unwrap(),

                        // end timer
                        System.Threading.Tasks.Task.Factory.StartNew(
                            async () => 
                            { 

                                // decelerate + reset motion
                                await apollon.ApollonHighResolutionTime.DoSleep(
                                    (this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f)
                                    + this.FSM.CurrentSettings.phase_A_settings.reset_duration
                                );

                            }
                        ).Unwrap()

                    }; 

                    // wait all parallel tasks
                    await System.Threading.Tasks.Task.WhenAll(parallel_tasks);

                    // unregister our synchronisation function
                    virtual_motion_system_bridge.ConcreteDispatcher.InitEvent  -= sync_end_virtual_motion_local_function;
                    physical_motion_system_bridge.ConcreteDispatcher.InitEvent -= sync_end_physical_motion_local_function;
                    
                } /* if() */

            } 
            else 
            {

                // fade in to black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration);

                } /* if() */

                // get a random timeout
                float random_latency = this.FSM.GetRandomLatencyFromBucket();

                // record it 
                this.FSM.CurrentResults.phase_A_results.user_randomized_stim_latency = random_latency;

                // inject passive side
                switch(this.FSM.CurrentSettings.passive_side_type)
                {

                    case AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Left:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Left;
                        break;
                    }
                    case AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Right:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Right;
                        break;
                    }
                    case AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Reference:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Reference;
                        break;
                    }

                    default:
                    case AgencyAndThresholdPerceptionV4Profile.Settings.SideIDType.Undefined:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command
                            = (int)AgencyAndThresholdPerceptionV4Profile.Settings.IntensityIDType.Undefined;
                        break;
                    }

                } /* switch() */
                
                // set other result to default values
                this.FSM.CurrentResults.phase_A_results.user_measured_latency = -1;
                this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = -1.0f;
                this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = "-1.0";

                // always request end phase
                this.FSM.CurrentResults.phase_A_results.step_is_valid = true;

                // finally wait the requested random  
                await apollon.ApollonHighResolutionTime.DoSleep(random_latency);

            } /* if() */

            // check visual degradation
            if(this.FSM.CurrentSettings.bWithVisualDegradation)
            {
                
                // enable fog
                UnityEngine.RenderSettings.fog = true;

                // setup fog type
                switch(this.FSM.CurrentSettings.fog_type)
                {
                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Linear:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Linear;
                        break;
                    }

                    default:
                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Exponential:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Exponential;
                        break;
                    }

                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.ExponentialSquared:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.ExponentialSquared;
                        break;
                    }

                } /* switch() */

                // setup fog parameter
                UnityEngine.RenderSettings.fogColor   
                    = new UnityEngine.Color(
                        /* R */ this.FSM.CurrentSettings.fog_color[0], 
                        /* G */ this.FSM.CurrentSettings.fog_color[1], 
                        /* B */ this.FSM.CurrentSettings.fog_color[2]
                    );
                UnityEngine.RenderSettings.fogDensity       = this.FSM.CurrentSettings.fog_density;
                UnityEngine.RenderSettings.fogStartDistance = this.FSM.CurrentSettings.fog_start_distance;
                UnityEngine.RenderSettings.fogEndDistance   = this.FSM.CurrentSettings.fog_end_distance;

                // solid background
                UnityEngine.Camera.main.backgroundColor = UnityEngine.RenderSettings.fogColor;
                UnityEngine.Camera.main.clearFlags      = UnityEngine.CameraClearFlags.SolidColor;

            } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : final result {"
                + "[user_randomized_stim_latency: " 
                    + this.FSM.CurrentResults.phase_A_results.user_randomized_stim_latency 
                + "][user_command: "
                    + this.FSM.CurrentResults.phase_A_results.user_command
                + "][user_measured_latency: "
                    + this.FSM.CurrentResults.phase_A_results.user_measured_latency
                + "][user_latency_unity_timestamp: "
                    + this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp
                + "][user_latency_host_timestamp: "
                    + this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp
                + "]}"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV4PhaseA */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
