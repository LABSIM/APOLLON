using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseD 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV3Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV3PhaseD(profile.ApollonAgencyAndThresholdPerceptionV3Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseD.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();
            void sync_user_response_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlDispatcher.AgencyAndThresholdPerceptionV3ControlEventArgs e)
                => sync_point?.TrySetResult((
                    /* user responded */ 
                    frontend.ApollonFrontendManager.Instance.getBridge(
                        frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                    ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value,
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */
                    ApollonHighResolutionTime.Now.ToString(),
                    /* current timestamp */
                    current_stopwatch.ElapsedMilliseconds
                ));

            // user interaction lambda
            System.EventHandler<
                gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlDispatcher.AgencyAndThresholdPerceptionV3ControlEventArgs
            > user_interaction_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // update UI cursor from raw command value
                        frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                            = args.Joystick;
                        
                    }; /* lambda */

            // register our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control
                )
            ).ConcreteDispatcher.ResponseTriggeredEvent += sync_user_response_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control
                )
            ).ConcreteDispatcher.JoystickValueChangedEvent += user_interaction_local_function;

            // wait synchronisation point indefinitely & reset it once hit
            bool bRequestEndPhaseDLoop = false;
            do
            {
                
                // update instructions 
                this.FSM.CurrentInstruction = "Synchrones ?";

                // reset cursor value to default position
                frontend.ApollonFrontendManager.Instance.getBridge(
                    frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 0.0f;

                // show response slider                            
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);
                
                // wait until any result
                (float, float, string, long) result = await sync_point.Task;

                // validity pass ?
                if(System.Math.Sign(result.Item1) < 0)
                {
                    
                    // record "OUI" value
                    this.FSM.CurrentResults.phase_D_results.user_response = true;

                    // request end phase
                    bRequestEndPhaseDLoop = true;

                }
                else if(System.Math.Sign(result.Item1) > 0)
                {
                    
                    // record "NON" value
                    this.FSM.CurrentResults.phase_D_results.user_response = false;

                    // request end phase
                    bRequestEndPhaseDLoop = true;

                }
                else
                {

                    // central zone == fail

                    // update instructions 
                    this.FSM.CurrentInstruction = "Réponse non valide";
                    
                    // hide intensity slider & show red cross
                    frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);
                    frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                    // wait a certain amout of time 
                    await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.confirm_duration);

                    // hide red cross
                    frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                
                    // re-tasking
                    sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();

                } /* if() */

            } while (!bRequestEndPhaseDLoop); /* while() */

            // unregister our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control
                )
            ).ConcreteDispatcher.ResponseTriggeredEvent -= sync_user_response_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control
                )
            ).ConcreteDispatcher.JoystickValueChangedEvent -= user_interaction_local_function;

            // hide response slider                            
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseD.OnEntry() : final result {"
                + "[user_response: " 
                    + this.FSM.CurrentResults.phase_D_results.user_response
                + "]}"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseD.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseD */

} /* } Labsim.apollon.experiment.phase */
