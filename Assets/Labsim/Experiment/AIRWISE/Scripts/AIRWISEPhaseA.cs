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
namespace Labsim.experiment.AIRWISE
{

    //
    // Internal init - FSM state
    //
    public sealed class AIRWISEPhaseA 
        : apollon.experiment.ApollonAbstractExperimentState<AIRWISEProfile>
    {
        public AIRWISEPhaseA(AIRWISEProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseA.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseA.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseA.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // refs
            var dynamic_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity
                ).ConcreteBehaviour;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseA.OnEntry() : activating control & AIRWISE Vecteur"
            );

            // activate subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            );

            dynamic_entity.References["EntityTag_Vecteur"].SetActive(true);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseA.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseA.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseA.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseA.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseA.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines


        #endregion

    } /* public sealed class AIRWISEPhaseA */

} /* } Labsim.experiment.AIRWISE */