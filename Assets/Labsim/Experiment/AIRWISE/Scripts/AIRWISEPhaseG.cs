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
namespace Labsim.experiment.AIRWISE
{

    //
    // Question : Temporal demand - FSM state
    //
    public sealed class AIRWISEPhaseG 
        : apollon.experiment.ApollonAbstractExperimentState<AIRWISEProfile>
    {
        public AIRWISEPhaseG(AIRWISEProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseG.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseG.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseG.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseG.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long, long)>();

            // lambdas
            System.EventHandler<
                AIRWISEControlDispatcher.AIRWISEControlEventArgs
            > sync_user_response_local_function 
                = (sender, args) => { 

                    // hit barrier
                    sync_point?.TrySetResult((
                        /* user responded */ 
                        apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.TemporalDemandSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value,
                        /* unity render timestamp */
                        UnityEngine.Time.time,
                        /* host timestamp */
                        apollon.ApollonHighResolutionTime.Now.ToString(),
                        /* host timestamp */
                        Varjo.XR.VarjoTime.GetVarjoTimestamp(),
                        /* current timestamp */
                        current_stopwatch.ElapsedMilliseconds
                    ));
                    
                }; /* lambda */
            System.EventHandler<
                AIRWISEControlDispatcher.AIRWISEControlEventArgs
            > user_interaction_local_function 
                = (sender, args) => {  
                        
                    // update UI cursor from normalized command value then to confidence range [0;100]
                    //
                    apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                        apollon.frontend.ApollonFrontendManager.FrontendIDType.TemporalDemandSliderGUI
                    ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                        = (args.JoystickHorizontal * 50.0f) + 50.0f;
                    
                }; /* user interaction lambda */

            // update instructions 
            this.FSM.CurrentInstruction = "Charge temporelle ?";

            // reset cursor value to default position
            apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                apollon.frontend.ApollonFrontendManager.FrontendIDType.TemporalDemandSliderGUI
            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 50.0f;

            // show confidence slider                            
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.TemporalDemandSliderGUI);
            
            // register our synchronisation function

            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEControlBridge>(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            ).ConcreteDispatcher.ResponseTriggeredEvent 
                += sync_user_response_local_function;

            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEControlBridge>(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent 
                += user_interaction_local_function;

            // wait until any result
            (float, float, string, long, long) result = await sync_point.Task;

            // unregister our synchronisation function

            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEControlBridge>(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            ).ConcreteDispatcher.ResponseTriggeredEvent 
                -= sync_user_response_local_function;

            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEControlBridge>(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            ).ConcreteDispatcher.JoystickHorizontalValueChangedEvent 
                -= user_interaction_local_function;

            // hide confidence slider                            
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.TemporalDemandSliderGUI);

            // record
            // 1 - user responded
            // 2 - unity render timestamp
            // 3 - host timestamp
            // 4 - host timestamp
            // 5 - current timestamp
            this.FSM.CurrentResults.PhaseG.user_response_value                  = result.Item1;
            this.FSM.CurrentResults.PhaseG.user_response_timing_unity_timestamp = result.Item2;
            this.FSM.CurrentResults.PhaseG.user_response_timing_host_timestamp  = result.Item3;
            this.FSM.CurrentResults.PhaseG.user_response_timing_varjo_timestamp = result.Item4;
            this.FSM.CurrentResults.PhaseG.user_elapsed_ms_since_entry          = result.Item5;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseG.OnEntry() : final [Temporal demand] result {"  
                + "user responded["          + this.FSM.CurrentResults.PhaseG.user_response_value                  + "]"
                + "/unity render timestamp[" + this.FSM.CurrentResults.PhaseG.user_response_timing_unity_timestamp + "]"
                + "/host timestamp["         + this.FSM.CurrentResults.PhaseG.user_response_timing_host_timestamp  + "]"
                + "/host timestamp["         + this.FSM.CurrentResults.PhaseG.user_response_timing_varjo_timestamp + "]"
                + "/current timestamp["      + this.FSM.CurrentResults.PhaseG.user_elapsed_ms_since_entry          + "]"
                + "}"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseG.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseG.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseG.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseG.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseG.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseG.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines


        #endregion

    } /* public sealed class AIRWISEPhaseG */

} /* } Labsim.experiment.AIRWISE */