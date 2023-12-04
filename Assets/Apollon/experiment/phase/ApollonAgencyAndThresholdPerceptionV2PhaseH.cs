using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseH 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV2Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV2PhaseH(profile.ApollonAgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseH.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_H_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_H_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();
            void sync_user_response_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs e)
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
                gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs
            > user_interaction_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // update UI cursor from raw command value
                        frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                            = args.Z;
                        
                    }; /* lambda */

            // register our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
            ).ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
            ).ConcreteDispatcher.AxisZValueChangedEvent += user_interaction_local_function;

            // wait synchronisation point indefinitely & reset it once hit
            bool bRequestEndPhaseHLoop = false;
            do
            {
                
                // update instructions 
                this.FSM.CurrentInstruction = "Plus fort ?";

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
                    
                    // record "Deplacement 1" value
                    this.FSM.CurrentResults.phase_H_results.user_response = 0;

                    // request end phase
                    bRequestEndPhaseHLoop = true;

                }
                else if(System.Math.Sign(result.Item1) > 0)
                {
                    
                    // record "Deplacement 2" value
                    this.FSM.CurrentResults.phase_H_results.user_response = 1;

                    // request end phase
                    bRequestEndPhaseHLoop = true;

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

            } while (!bRequestEndPhaseHLoop); /* while() */

            // unregister our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
            ).ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionV2ControlBridge
            ).ConcreteDispatcher.AxisZValueChangedEvent -= user_interaction_local_function;

            // hide response slider                            
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.ResponseSliderGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseH.OnEntry() : final result {"
                + "[user_response: " 
                    + this.FSM.CurrentResults.phase_H_results.user_response
                + "]}"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseH.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseH.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_H_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_H_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2PhaseH.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV2PhaseH */

} /* } Labsim.apollon.experiment.phase */
