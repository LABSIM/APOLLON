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
    public sealed class AgencyAndThresholdPerceptionV2PhaseJ 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseJ(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseJ.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_J_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_J_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // fade in 
            await this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration, false);

            // get bridge
            var motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonMotionSystemCommandBridge;

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                ) as apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge;

            var seat_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                ) as apollon.gameplay.entity.ApollonActiveSeatEntityBridge;

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_J_settings.duration);
                    break;
                }
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_J_settings.duration);
                    break;
                }

            } /* switch() */

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV2Profile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // wait
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_J_settings.duration);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseJ.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseJ.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_J_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_J_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseJ.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseJ */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
