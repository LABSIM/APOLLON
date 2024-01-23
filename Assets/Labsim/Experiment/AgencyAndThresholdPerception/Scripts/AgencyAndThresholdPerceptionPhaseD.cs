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
namespace Labsim.experiment.AgencyAndThresholdPerception
{
    
    //
    // End phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionPhaseD
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionProfile>
    {
        public AgencyAndThresholdPerceptionPhaseD(AgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseD.OnEntry() : begin"
            );
                
            // get bridge
            var motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            // check
            if (motion_system_bridge == null || virtual_motion_system_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionPhaseD.OnEntry() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */
                
            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoLightFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // inactivate HOTAS
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl);

            // show red cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

            // show red frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

            // hide red cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset();
                    break;
                }
                case AgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    motion_system_bridge.ConcreteDispatcher.RaiseReset();
                    break;
                }

            } /* switch() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseD.OnExit() : begin"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionPhaseD */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
