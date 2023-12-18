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
    // Latency phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV2PhaseG
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseG(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseG.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_G_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_G_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
            
            // fade out from black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionV2Profile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeOut(this.FSM._trial_fade_out_duration);

            } /* if() */

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Fin";

            // hide grey frame/cross & show green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_G_settings.duration);

            // finally, hide green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseG.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseG.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_G_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_G_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseG.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseG */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
