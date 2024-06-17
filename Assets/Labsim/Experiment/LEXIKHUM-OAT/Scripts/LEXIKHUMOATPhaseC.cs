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
namespace Labsim.experiment.LEXIKHUM_OAT
{

    //
    // Slalom - FSM state
    //
    public sealed class LEXIKHUMOATPhaseC 
        : apollon.experiment.ApollonAbstractExperimentState<LEXIKHUMOATProfile>
    {
        public LEXIKHUMOATPhaseC(LEXIKHUMOATProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseC.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseC.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseC.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            var checkpoint_manager
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour
                .References["EntityTag_Checkpoints"]
                .GetComponent<LEXIKHUMOATCheckpointManagerBehaviour>();
            var lexikhum_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<LEXIKHUMOATEntityBridge>(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATEntity
                );
            var haptic_arm
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.ApollonGeneric3DoFHapticArmBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                );

             // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnEntry() : starting slalom task, raise control event"
            );
            
            lexikhum_entity.ConcreteDispatcher.RaiseControl();
            haptic_arm.ConcreteDispatcher.RaiseControl();

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
            lexikhum_entity.ConcreteDispatcher.RaiseHold();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnEntry() : ending slalom task, saving performance results["
                + result
                + "]"
            );

            // backup
            (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                .CurrentResults
                .Trial
                .user_performance_value
                = result;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseC.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseC.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseC.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class LEXIKHUMOATPhaseC */

} /* } Labsim.experiment.LEXIKHUM_OAT */