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
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV2PhaseA 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseA(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {
                
                // currrent timestamp
                System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();
                void sync_user_response_local_function(object sender, AgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs e)
                    => sync_point?.TrySetResult((
                        /* user responded */ 
                        true, 
                        /* unity render timestamp */
                        UnityEngine.Time.time,
                        /* host timestamp */
                        apollon.ApollonHighResolutionTime.Now.ToString(),
                        /* current timestamp */
                        current_stopwatch.ElapsedMilliseconds
                    ));
                System.EventHandler<
                    AgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs
                > user_interaction_local_function 
                    = (sender, args) 
                        => 
                        {  
                            
                            // update UI cursor from normalized command value 
                            //
                            // NORMALISATON [0;+1]
                            // -------------
                            // x_norm = ( x_raw - x_min[-1] ) / amplitude[[+2] => xmax[+1.0] - x_min[-1.0]]
                            //
                            apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                                apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI
                            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                                = (args.Z + 1.0f) / 2.0f;
                            
                        }; /* lambda */

                // register our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.AxisZValueChangedEvent += user_interaction_local_function;
                
                // wait synchronisation point indefinitely & reset it once hit
                bool bRequestEndPhaseALoop = false;
                do
                {

                    // restart current stopwatch
                    current_stopwatch.Restart();

                    // update instructions 
                    this.FSM.CurrentInstruction = "Intensité ?";

                    // reset cursor value to default position
                    apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                        apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI
                    ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 0.0f;

                    // show intensity slider                            
                    apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

                    // running
                    var phase_running_task_ct_src = new System.Threading.CancellationTokenSource();
                    System.Threading.CancellationToken phase_running_task_ct = phase_running_task_ct_src.Token;
                    var phase_running_task
                        // wait for random wait
                        = System.Threading.Tasks.Task.Factory.StartNew(
                            async () => 
                            { 
                                // log
                                UnityEngine.Debug.Log(
                                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user has "
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
                                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user hasn't responded, injecting default result"
                                        );
                                        
                                        sync_point?.TrySetResult((false, -1.0f, "-1.0", -1));

                                    } else {
                                        
                                        UnityEngine.Debug.Log(
                                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user has responded, keep result"
                                        );
                                    
                                    } /* if() */

                                } /* if() */
                            },
                            phase_running_task_ct_src.Token
                        );
                    
                    // wait until any result
                    (bool, float, string, long) result = await sync_point.Task;

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
                                apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI
                            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value;

                        // [ 0.0 % ; 20.0 % [ 
                        if(slider_value < 0.20f)
                        {

                            // neutral zone == fail

                            // update instructions 
                            this.FSM.CurrentInstruction = "Neutre non valide";

                        }
                        // [ 60.0 % ; 100.0 % ]
                        else if(slider_value >= 0.60f)
                        {

                            // strong stim == check
                            if(
                                (
                                    (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                                    - this.FSM.strongConditionCount
                                ) > 0
                            ) 
                            {

                                // OK, save command & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command
                                    = (int)AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong;
                                this.FSM.strongConditionCount++;
                                bValid = true;

                            }
                            else
                            {
                                
                                // no more strong available == fail

                                // update instructions 
                                this.FSM.CurrentInstruction = "Quota intensité forte epuisé";

                            } /* if() */

                        }
                         // [ 20.0 % ; 60.0 % [
                        else
                        {

                            // weak stim == check
                            if(
                                (
                                    (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                                    - this.FSM.weakConditionCount
                                ) > 0
                            ) 
                            {

                                // OK, save command, increment counter & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command
                                    = (int)AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak;
                                this.FSM.weakConditionCount++;
                                bValid = true;

                            }
                            else
                            {
                               
                                // no more weak available == fail

                                // update instructions 
                                this.FSM.CurrentInstruction = "Quota intensité faible epuisé";

                            } /* if() */

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
                        
                        // inject current user latency
                        this.FSM.AddUserLatencyToBucket(result.Item4);

                        // get a random timeout
                        float random_latency = this.FSM.GetRandomLatencyFromBucket();

                        // record user_*
                        this.FSM.CurrentResults.phase_A_results.user_measured_latency = result.Item4;
                        this.FSM.CurrentResults.phase_A_results.user_randomized_stim1_latency = random_latency;        
                        this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = result.Item2;
                        this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = result.Item3;

                        // request end phase
                        bRequestEndPhaseALoop = true;

                    }
                    else
                    {
                        
                        // hide intensity slider & show red cross
                        apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);
                        apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                        // wait a certain amout of time 
                        await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.confirm_duration);

                        // hide red cross
                        apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                    
                        // re-tasking
                        sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();

                    } /* if() */

                } while (!bRequestEndPhaseALoop); /* while() */

                // unregister our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.AxisZValueChangedEvent -= user_interaction_local_function;

                // fade in to black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration);

                } /* if() */

                // hide intensity slider
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

            } 
            else 
            {

                // fade in to black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration);

                } /* if() */

                // get a random timeout
                float random_latency = this.FSM.GetRandomLatencyFromBucket();

                // record it 
                this.FSM.CurrentResults.phase_A_results.user_randomized_stim1_latency = random_latency;

                // inject passive intensity
                switch(this.FSM.CurrentSettings.passive_intensity_type)
                {

                    case AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong;
                        break;
                    }
                    case AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak;
                        break;
                    }

                    default:
                    case AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Undefined:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command
                            = (int)AgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Undefined;
                        break;
                    }

                } /* switch() */
                
                // set other result to default values
                this.FSM.CurrentResults.phase_A_results.user_measured_latency = -1;
                this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = -1.0f;
                this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = "-1.0";

                // finally wait the requested random  
                await apollon.ApollonHighResolutionTime.DoSleep(random_latency);

            } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : final result {"
                + "[user_randomized_stim1_latency: " 
                    + this.FSM.CurrentResults.phase_A_results.user_randomized_stim1_latency 
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseA */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
