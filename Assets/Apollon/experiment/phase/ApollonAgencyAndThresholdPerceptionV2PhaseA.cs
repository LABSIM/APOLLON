using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseA 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseA(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {
                
                // currrent timestamp
                System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();
                void sync_user_response_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlDispatcher.EventArgs e)
                    => sync_point?.TrySetResult((
                        /* user responded */ 
                        true, 
                        /* unity render timestamp */
                        UnityEngine.Time.time,
                        /* host timestamp */
                        UXF.ApplicationHandler.CurrentHighResolutionTime,
                        /* current timestamp */
                        current_stopwatch.ElapsedMilliseconds
                    ));
                System.EventHandler<
                    gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlDispatcher.EventArgs
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
                            frontend.ApollonFrontendManager.Instance.getBridge(
                                frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI
                            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                                = (args.Z + 1.0f) / 2.0f;
                            
                        }; /* lambda */

                // register our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).Dispatcher.AxisZValueChangedEvent += user_interaction_local_function;
                
                // wait synchronisation point indefinitely & reset it once hit
                bool bRequestEndPhaseALoop = false;
                do
                {

                    // restart current stopwatch
                    current_stopwatch.Restart();

                    // update instructions 
                    this.FSM.CurrentInstruction = "Intensité ?";

                    // show intensity slider                            
                    frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

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
                                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user has "
                                    + this.FSM.CurrentSettings.phase_A_settings.response_max_duration
                                    + " ms to respond"
                                );

                                // wait a certain amout of time between each bound if cancel not requested
                                if(!phase_running_task_ct.IsCancellationRequested)
                                {
                                    await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_settings.response_max_duration);
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
                                            "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user hasn't responded, injecting default result"
                                        );
                                        
                                        sync_point?.TrySetResult((false, -1.0f, "-1.0", -1));

                                    } else {
                                        
                                        UnityEngine.Debug.Log(
                                            "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : user has responded, keep result"
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
                            = frontend.ApollonFrontendManager.Instance.getBridge(
                                frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI
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
                                    = (int)profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong;
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
                                    = (int)profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak;
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
                        frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);
                        frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                        // wait a certain amout of time 
                        await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_settings.confirm_duration);

                        // hide red cross
                        frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                    
                        // re-tasking
                        sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();

                    } /* if() */

                } while (!bRequestEndPhaseALoop); /* while() */

                // unregister our synchronisation function
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
                (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
                ).Dispatcher.AxisZValueChangedEvent -= user_interaction_local_function;

                // fade in to black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

                } /* if() */

                // hide intensity slider
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

            } 
            else 
            {

                // fade in to black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

                } /* if() */

                // get a random timeout
                float random_latency = this.FSM.GetRandomLatencyFromBucket();

                // record it 
                this.FSM.CurrentResults.phase_A_results.user_randomized_stim1_latency = random_latency;

                // inject passive intensity
                switch(this.FSM.CurrentSettings.passive_intensity_type)
                {

                    case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Strong;
                        break;
                    }
                    case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command 
                            = (int)profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Weak;
                        break;
                    }

                    default:
                    case profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Undefined:
                    {
                        this.FSM.CurrentResults.phase_A_results.user_command
                            = (int)profile.ApollonAgencyAndThresholdPerceptionV2Profile.Settings.IntensityIDType.Undefined;
                        break;
                    }

                } /* switch() */
                
                // set other result to default values
                this.FSM.CurrentResults.phase_A_results.user_measured_latency = -1;
                this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = -1.0f;
                this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = "-1.0";

                // finally wait the requested random  
                await this.FSM.DoSleep(random_latency);

            } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : final result {"
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
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseA */

} /* } Labsim.apollon.experiment.phase */
