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
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionPhase0 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionProfile>
    {
        public AgencyAndThresholdPerceptionPhase0(AgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhase0.OnEntry() : begin"
            );

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                // synchronisation mechanism (TCS + local function)
                var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
                void sync_local_function(object sender, AgencyAndThresholdPerceptionControlDispatcher.AgencyAndThresholdPerceptionControlEventArgs e)
                    => sync_point?.TrySetResult(true);

                // register our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as AgencyAndThresholdPerceptionControlBridge
                ).ConcreteDispatcher.UserNeutralCommandTriggeredEvent += sync_local_function;

                // show grey cross & frame
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

                // wait synchronisation point indefinitely & reset it once hit
                await sync_point.Task;

                // hide grey cross & frame
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);

                // unregister our synchronisation function
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                    ) as AgencyAndThresholdPerceptionControlBridge
                ).ConcreteDispatcher.UserNeutralCommandTriggeredEvent -= sync_local_function;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhase0.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhase0.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhase0.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionPhase0 */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
