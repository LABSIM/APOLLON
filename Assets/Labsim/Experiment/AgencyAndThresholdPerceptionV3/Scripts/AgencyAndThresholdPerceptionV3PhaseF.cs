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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV3
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV3PhaseF 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV3Profile>
    {
        public AgencyAndThresholdPerceptionV3PhaseF(AgencyAndThresholdPerceptionV3Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseF.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // fade in 
            await this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration, false);

            // get bridge
            var motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                );

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                );

            var seat_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                );

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV3Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }

            } /* switch() */

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // wait
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_settings.duration);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseF.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseF.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3PhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV3PhaseF */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV3 */
