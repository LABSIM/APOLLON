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
    // Slalom - FSM state
    //
    public sealed class AIRWISEPhaseC 
        : apollon.experiment.ApollonAbstractExperimentState<AIRWISEProfile>
    {
        public AIRWISEPhaseC(AIRWISEProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseC.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseC.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseC.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // refs
            var checkpoint_manager
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour
                .References["EntityTag_Checkpoint"]
                .GetComponent<AIRWISECheckpointManagerBehaviour>();
            var airwise_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEEntityBridge>(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
                );
            var airwise_quad_controller
                = airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>();
            var motion_platform
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.AppollonGenericMotionSystemBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.GenericMotionSystem
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnEntry() : switching Motion platform impedence system from init->idle to control state"
            );

            // raise control state motion event
            motion_platform.ConcreteDispatcher.RaiseControl();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnEntry() : starting slalom task"
            );
            
            if(!this.FSM.CurrentSettings.Trial.bIsActive)
            {
            
                // airwise_quad_controller.
                airwise_entity.ConcreteDispatcher.RaiseControl();
                airwise_quad_controller.Inhibit = false;
            
            } /* if() */
            
            // await for end of phase 
            // END REACHED

            float result = -1.0f;

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_motion_point = new System.Threading.Tasks.TaskCompletionSource<float>();

            // lambdas
            System.EventHandler<float> sync_slalom_ended_local_function 
                = (sender, args) 
                    => sync_motion_point?.TrySetResult(args);

            // bind to checkpoint manager events
            checkpoint_manager.slalomEnded += sync_slalom_ended_local_function;

            // wait end of slalom
            result = await sync_motion_point.Task;

            // unbind from checkpoint manager events
            checkpoint_manager.slalomEnded -= sync_slalom_ended_local_function;

            // inhibit
            airwise_entity.ConcreteDispatcher.RaiseHold();
            airwise_quad_controller.Inhibit = true;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnEntry() : ending slalom task, saving performance results["
                + result 
                + "]"
            );

            // backup
            (apollon.experiment.ApollonExperimentManager.Instance.Profile as AIRWISEProfile)
                .CurrentResults
                .Trial
                .user_performance_value 
                = result;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseC.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseC.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseC.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseC.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines


        #endregion

    } /* public sealed class AIRWISEPhaseC */

} /* } Labsim.experiment.AIRWISE */