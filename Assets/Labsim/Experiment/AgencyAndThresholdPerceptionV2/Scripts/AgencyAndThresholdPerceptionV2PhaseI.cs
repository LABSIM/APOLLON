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
    public sealed class AgencyAndThresholdPerceptionV2PhaseI 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseI(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseI.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_I_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_I_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = new System.Diagnostics.Stopwatch();

            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<(float, float, string, long)>();
            void sync_user_response_local_function(object sender, AgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs e)
                => sync_point?.TrySetResult((
                    /* user responded */ 
                    apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                        apollon.frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
                    ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value,
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */
                    apollon.ApollonHighResolutionTime.Now.ToString(),
                    /* current timestamp */
                    current_stopwatch.ElapsedMilliseconds
                ));

            // user interaction lambda
            System.EventHandler<
                AgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs
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
                        apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
                        ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value 
                            = (((args.Z + 1.0f) / 2.0f) * 4.0f) + 1.0f;
                        
                    }; /* lambda */

            // update instructions 
            this.FSM.CurrentInstruction = "Niveau de confiance ?";

            // reset cursor value to default position
            apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                apollon.frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI
            ).Behaviour.GetComponent<UnityEngine.UI.Slider>().value = 3.0f;

            // show confidence slider                            
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI);
            
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

            // wait until any result
            (float, float, string, long) result = await sync_point.Task;

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

            // hide confidence slider                            
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ConfidenceSliderGUI);

            // record
            this.FSM.CurrentResults.phase_I_results.user_confidence = result.Item1;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseI.OnEntry() : final result {"
                + "[user_confidence: " 
                    + this.FSM.CurrentResults.phase_I_results.user_confidence
                + "]}"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseI.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseI.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_I_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_I_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseI.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseI */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
