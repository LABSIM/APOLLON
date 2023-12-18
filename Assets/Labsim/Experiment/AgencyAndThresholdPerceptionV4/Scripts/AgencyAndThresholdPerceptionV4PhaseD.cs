using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV4PhaseD 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4PhaseD(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseD.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();
            
            // lambdas
            System.EventHandler<
                AgencyAndThresholdPerceptionV4ControlDispatcher.AgencyAndThresholdPerceptionV4ControlEventArgs
            > sync_user_response_local_function 
                = (sender, args) => {  
                    
                    // hit barrier
                    sync_point?.TrySetResult((
                        /* user responded */ 
                        apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value,
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
                        apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                    ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                        = args.JoystickHorizontal;
                    
                }; /* user interaction lambda */

            // register our synchronisation function
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV4ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                )
            ).ConcreteDispatcher.ResponseTriggeredEvent += sync_user_response_local_function;
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV4ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                )
            ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent += user_interaction_local_function;

            // wait synchronisation point indefinitely & reset it once hit
            bool bRequestEndPhaseDLoop = false;
            do
            {
                
                // update instructions 
                this.FSM.CurrentInstruction = "Cohérence spatiale ?";

                // reset cursor value to default position
                apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                    apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 0.0f;

                // show response slider                            
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);
                
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
                    apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);
                    apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                    // wait a certain amout of time 
                    await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_settings.confirm_duration);

                    // hide red cross
                    apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                
                    // re-tasking
                    sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();

                } /* if() */

            } while (!bRequestEndPhaseDLoop); /* while() */

            // unregister our synchronisation function
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV4ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                )
            ).ConcreteDispatcher.ResponseTriggeredEvent -= sync_user_response_local_function;
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    AgencyAndThresholdPerceptionV4ControlBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control
                )
            ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent -= user_interaction_local_function;

            // hide response slider                            
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseD.OnEntry() : final result {"
                + "[user_response: " 
                    + this.FSM.CurrentResults.phase_D_results.user_response
                + "]}"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseD.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV4PhaseD */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
