﻿using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseE 
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionV3Profile>
    {
        public ApollonAgencyAndThresholdPerceptionV3PhaseE(profile.ApollonAgencyAndThresholdPerceptionV3Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseE.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();
            void sync_user_response_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionV3ControlDispatcher.AgencyAndThresholdPerceptionV3ControlEventArgs e)
                => sync_point?.TrySetResult((
                    /* user responded */ 
                    frontend.ApollonFrontendManager.Instance.getBridge(
                        frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
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
                        
                        // update UI cursor from normalized command value then to confidence range [1;5]
                        //
                        // NORMALISATON [0;+1]
                        // -------------
                        // x_norm = ( x_raw - x_min[-1] ) / amplitude[[+2] => xmax[+1.0] - x_min[-1.0]]
                        //
                        frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                            = (((args.Joystick + 1.0f) / 2.0f) * 4.0f) + 1.0f;
                        
                    }; /* lambda */

            // update instructions 
            this.FSM.CurrentInstruction = "Niveau de confiance ?";

            // reset cursor value to default position
            frontend.ApollonFrontendManager.Instance.getBridge(
                frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 3.0f;

            // show confidence slider                            
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI);
            
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

            // wait until any result
            (float, float, string, long) result = await sync_point.Task;

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

            // hide confidence slider                            
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI);

            // record
            this.FSM.CurrentResults.phase_E_results.user_confidence = result.Item1;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseE.OnEntry() : final result {"
                + "[user_confidence: " 
                    + this.FSM.CurrentResults.phase_E_results.user_confidence
                + "]}"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseE.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseE.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3PhaseE.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionV3PhaseE */

} /* } Labsim.apollon.experiment.phase */