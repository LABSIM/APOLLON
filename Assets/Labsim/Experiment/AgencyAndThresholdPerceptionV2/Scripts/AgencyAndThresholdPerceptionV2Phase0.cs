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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV2
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV2Phase0 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV2Profile>
    {
        public AgencyAndThresholdPerceptionV2Phase0(AgencyAndThresholdPerceptionV2Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2Phase0.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = (this.FSM.CurrentSettings.bIsActive ? "Condition manuelle" : "Condition automatique");

            // show grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
                void sync_local_function(object sender, AgencyAndThresholdPerceptionV2ControlDispatcher.AgencyAndThresholdPerceptionV2ControlEventArgs e)
                    => sync_point?.TrySetResult(true);

                // register our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent += sync_local_function;

                // wait synchronisation point indefinitely & reset it once hit
                await sync_point.Task;

                // unregister our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control
                    ) as AgencyAndThresholdPerceptionV2ControlBridge
                ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent -= sync_local_function;

            } 
            else 
            {

                // wait a certain amout of time
                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            } /* if() */

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Déplacement 1";

            // hide grey frame/cross & show green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_0_settings.duration / 2.0f);

            // finally, hide green frame/cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2Phase0.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2Phase0.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2Phase0.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV2Phase0 */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
