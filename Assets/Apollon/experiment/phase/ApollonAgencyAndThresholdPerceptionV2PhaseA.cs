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
                            
                            // update UI cursor value
                            frontend.ApollonFrontendManager.Instance.getBridge(
                                frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI
                            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = args.Z;
                            
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
                bool bRequestPhaseALoop = false;
                do
                {

                    // restart current stopwatch
                    current_stopwatch.Restart();

                    // update instructions 
                    this.FSM.CurrentInstruction = "Puissance ?";

                    // show intensity slider                            
                    frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

                    // running 
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

                                // wait a certain amout of time between each bound
                                await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_settings.response_max_duration);
                            } 
                        ).Unwrap().ContinueWith(
                            antecedent => 
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
                            }
                        );
                    
                    // wait until any result
                    (bool, float, string, long) result = await sync_point.Task;

                    // check result
                    if(result.Item1)
                    {

                        // validity barrier
                        bool bValid = false;

                        // it's a hit then check intensity from slider value
                        float slider_value 
                            = frontend.ApollonFrontendManager.Instance.getBridge(
                                frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI
                            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value;

                        // [ 0.0 % ; 20.0 % [ 
                        if(slider_value < 0.20f)
                        {

                            // neutral zone == fail

                            // update instructions 
                            this.FSM.CurrentInstruction = "Neutre non valide";

                            // hide intensity slider & show red cross
                            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);
                            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                            // wait a certain amout of time 
                            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_settings.confirm_duration);

                            // hide red cross
                            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                        }
                        // [ 60.0 % ; 100.0 % ]
                        else if(slider_value >= 0.60f)
                        {

                            // strong stim == check
                            if(
                                (
                                    (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 2) 
                                    - this.FSM.strongConditionCount
                                ) > 0
                            ) 
                            {

                                // OK, save command & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command = 2;
                                bValid = true;

                            }

                        }
                         // [ 20.0 % ; 60.0 % [
                        else
                        {

                            // weak stim == check
                            if(
                                (
                                    (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 2) 
                                    - this.FSM.weakConditionCount
                                ) > 0
                            ) 
                            {

                                // OK, save command & mark as valid
                                this.FSM.CurrentResults.phase_A_results.user_command = 1;      
                                bValid = true;

                            }

                        } /* if() */

                        // validity ?
                        if(bValid)
                        {
                            
                            // inject current user latency
                            this.FSM.AddUserLatencyToBucket(result.Item4);

                            // get a random timeout
                            float random_latency = this.FSM.GetRandomLatencyFromBucket();

                            // record it 
                            this.FSM.CurrentResults.phase_A_results.user_elected_latency = random_latency;        
                            
                            // set other result to default values
                            this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = result.Item2;
                            this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = result.Item3;

                            // request end phase
                            bRequestPhaseALoop = true;

                        } /* if() */

                    // timeout
                    } else {

                        // re-tasking
                        sync_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string, long)>();
                        
                    } /* if() */

                } while (!bRequestPhaseALoop); /* while() */

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

                // hide intensity slider
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.IntensitySliderGUI);

            } 
            else 
            {

                // get a random timeout
                float random_latency = this.FSM.GetRandomLatencyFromBucket();

                // record it 
                this.FSM.CurrentResults.phase_A_results.user_elected_latency = random_latency;        
                
                // set other result to default values
                this.FSM.CurrentResults.phase_A_results.user_command = -1;
                this.FSM.CurrentResults.phase_A_results.user_latency_unity_timestamp = -1.0f;
                this.FSM.CurrentResults.phase_A_results.user_latency_host_timestamp = "-1.0";

                // finally wait the requested random  
                await this.FSM.DoSleep(random_latency);

            } /* if() */
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseA.OnEntry() : final result {"
                + "[user_elected_latency: " 
                    + this.FSM.CurrentResults.phase_A_results.user_elected_latency 
                + "][user_command: "
                    + this.FSM.CurrentResults.phase_A_results.user_command
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
