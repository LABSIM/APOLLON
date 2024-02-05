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
    // Subject init - FSM state
    //
    public sealed class AIRWISEPhaseB 
        : apollon.experiment.ApollonAbstractExperimentState<AIRWISEProfile>
    {
        public AIRWISEPhaseB(AIRWISEProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseB.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseB.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseB.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // refs
            var checkpoint_manager
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour
                .References["EntityTag_Checkpoint"]
                .GetComponent<AIRWISECheckpointManagerBehaviour>();
            var airwise_quad_controller
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity
                ).ConcreteBehaviour.GetComponentInChildren<QuadController>();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : start moving"
            );

            // 
            // airwise_quad_controller.

            // await for end of phase 
            // DURATION OR GATE REACHED

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // actions
            System.Action sync_slalom_started_local_function = () => sync_point?.TrySetResult(true);

            // bind to checkpoint manager events
            checkpoint_manager.slalomStarted += sync_slalom_started_local_function;

            // running
            var phase_running_task_ct_src = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken phase_running_task_ct = phase_running_task_ct_src.Token;
            var phase_running_task
                // wait duration
                = System.Threading.Tasks.Task.Factory.StartNew(
                    async () => 
                    { 
                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur has "
                            + this.FSM.CurrentSettings.PhaseB.total_duration
                            + " ms to cross the start"
                        );

                        // wait a certain amout of time between each bound if cancel not requested
                        if(!phase_running_task_ct.IsCancellationRequested)
                        {
                            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.PhaseB.total_duration);
                        }

                    },
                    phase_running_task_ct_src.Token 
                ).Unwrap().ContinueWith(
                    antecedent => 
                    {
                        if(!phase_running_task_ct.IsCancellationRequested)
                        {

                            if(!sync_point.Task.IsCompleted) 
                            {
                                
                                UnityEngine.Debug.LogWarning(
                                    "<color=Orange>Warn: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur hasn't crossed the start line..."
                                );
                                
                                sync_point?.TrySetResult(false);

                            } else {
                                
                                UnityEngine.Debug.Log(
                                    "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur has crossed the start line ! Ignore this message ;)"
                                );
                            
                            } /* if() */

                        } /* if() */
                    },
                    phase_running_task_ct_src.Token
                );

            // wait until any result
            var result = await sync_point.Task;

            // cancel running task
            phase_running_task_ct_src.Cancel();

            // unbind from checkpoint manager events
            checkpoint_manager.slalomStarted -= sync_slalom_started_local_function;

            // log 
            if(result)
            {
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur cross the start line, PhaseC will begin"
                );
            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> AIRWISEPhaseB.OnEntry() : Timer has reached duration before AIRWISE Vecteur crossed the start line... You should check configuration file..."
                );
            }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseB.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseB.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseB.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines


        #endregion

    } /* public sealed class AIRWISEPhaseB */

} /* } Labsim.experiment.AIRWISE */