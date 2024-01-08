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

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class CAVIARPhaseE
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {
        public CAVIARPhaseE(CAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get our entity bridge & settings
            var caviar_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as CAVIAREntityBridge
                );

            // get our acceleration value & timestamp
            float 
                /* saturation speed {m.s-1} */
                entry_linear_velocity 
                    = this.FSM.CurrentSettings.phase_C_settings.Last().target_velocity,
                /* constant linear acceleration {m.s-2} */
                linear_absolute_deceleration 
                    = UnityEngine.Mathf.Abs(
                        - UnityEngine.Mathf.Pow(entry_linear_velocity, 2.0f) 
                        / (2.0f * this.FSM.CurrentSettings.phase_E_distance)
                    ),
                /* phase duration {ms} */
                phase_duration 
                    = ( entry_linear_velocity / linear_absolute_deceleration ) * 1000.0f;
                    
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnEntry() : calculated following parameter ["
                + "entry_linear_velocity:" 
                    + entry_linear_velocity 
                + ",linear_absolute_deceleration:" 
                    + linear_absolute_deceleration
                + ",phase_duration:" 
                    + phase_duration 
                + "], begin transition, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // if practicing
            if(apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {
            
                // hide guidance
                // frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.SimpleCrossGUI);
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SimpleFrameGUI);
            
            } /* if() */

            // show red cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // inactivate all visual cues through LINQ request
            var we_behaviour
                 = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).Behaviour as apollon.gameplay.element.ApollonStaticElementBehaviour;
            foreach (var vc_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("VCTag_")).Select(kvp => kvp.Value))
            {
                vc_ref.SetActive(false);
            }
            
            // decelerate up to stop
            caviar_bridge.ConcreteDispatcher.RaiseDecelerate(linear_absolute_deceleration,0.0f);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(phase_duration / 2.0f);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnEntry() : mid-phase, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // show red frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(phase_duration / 2.0f);

            // hide red cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnEntry() : end, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseE.OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseE */
    
} /* } Labsim.apollon.experiment.phase */