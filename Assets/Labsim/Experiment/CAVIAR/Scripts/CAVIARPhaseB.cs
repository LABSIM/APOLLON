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
    public sealed class CAVIARPhaseB
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {
        public CAVIARPhaseB(CAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // show green cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // get our entity bridge & settings
            var caviar_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as CAVIAREntityBridge
                );
            var fog_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    ) as apollon.gameplay.element.ApollonFogElementBridge
                );

            // get our acceleration value & timestamp
            float 
                /* saturation speed {m.s-1} */
                linear_velocity 
                    = this.FSM.CurrentSettings.phase_C_settings.First().target_velocity,
                /* constant linear acceleration {m.s-2} */
                linear_acceleration 
                    = (
                        UnityEngine.Mathf.Pow(linear_velocity, 2.0f) 
                        / (2.0f * this.FSM.CurrentSettings.phase_B_distance)
                    ),
                /* phase duration {ms} */
                phase_duration 
                    = ( linear_velocity / linear_acceleration ) * 1000.0f;
                    
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnEntry() : calculated following parameter ["
                + "linear_velocity:" 
                    + linear_velocity 
                + ",linear_acceleration:" 
                    + linear_acceleration
                + ",phase_duration:" 
                    + phase_duration 
                + "], begin transition, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // accelerate up to the first phase C settings & smooth fog
            caviar_bridge.ConcreteDispatcher.RaiseAccelerate(linear_acceleration,linear_velocity);
            fog_bridge.ConcreteDispatcher.RaiseSmoothLinearFogRequested(
                this.FSM.CurrentSettings.phase_C_settings.First().fog_start_distance,
                this.FSM.CurrentSettings.phase_C_settings.First().fog_end_distance,
                UnityEngine.Color.white,
                phase_duration
            );

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(phase_duration / 2.0f);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnEntry() : mid-phase, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // hide green frame first
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(phase_duration / 2.0f);

            // then hide cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // if practicing
            if(apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {
            
                // show guidance
                // frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.SimpleCrossGUI);
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SimpleFrameGUI);
            
            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnEntry() : end, current distance["
                + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseB */
    
} /* } Labsim.apollon.experiment.phase */