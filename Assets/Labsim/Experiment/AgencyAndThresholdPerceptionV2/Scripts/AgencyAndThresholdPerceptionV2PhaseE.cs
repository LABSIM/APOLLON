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
    public sealed class AgencyAndThresholdPerceptionV2PhaseE
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2PhaseE(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseE.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get a random timeout
            float random_latency = this.FSM.GetRandomLatencyFromBucket();

            // record user_*
            this.FSM.CurrentResults.phase_E_results.user_randomized_stim2_latency = random_latency; 

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseE.OnEntry() : will wait " 
                + random_latency
                + " ms"
            );

            // wait a certain amout of time between each bound
            await apollon.ApollonHighResolutionTime.DoSleep(random_latency);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseE.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseE.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2PhaseE.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2PhaseE */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
