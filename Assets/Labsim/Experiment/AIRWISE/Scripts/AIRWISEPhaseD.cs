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
    // End subject + reset internal - FSM state
    //
    public sealed class AIRWISEPhaseD 
        : apollon.experiment.ApollonAbstractExperimentState<AIRWISEProfile>
    {
        public AIRWISEPhaseD(AIRWISEProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseD.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseD.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseD.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // refs
            var airwise_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEEntityBridge>(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
                );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : stop moving"
            );

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // running
            var parallel_tasks_ct_src = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken parallel_tasks_ct = parallel_tasks_ct_src.Token;
            var parallel_tasks_factory
                = new System.Threading.Tasks.TaskFactory(
                    parallel_tasks_ct,
                    System.Threading.Tasks.TaskCreationOptions.DenyChildAttach,
                    System.Threading.Tasks.TaskContinuationOptions.DenyChildAttach,
                    System.Threading.Tasks.TaskScheduler.Default
                );
            var parallel_tasks 
                = new System.Collections.Generic.List<System.Threading.Tasks.Task>() 
                {
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : AIRWISE Vecteur has "
                                + this.FSM.CurrentSettings.PhaseD.total_duration
                                + " ms to cross the stop"
                            );

                            // wait a certain amout of time between each bound if cancel not requested
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.PhaseD.total_duration);
                            }

                        },
                        parallel_tasks_ct_src.Token 
                    ).Unwrap().ContinueWith(
                        antecedent => 
                        {
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {

                                if(!sync_point.Task.IsCompleted) 
                                {
                                    
                                    UnityEngine.Debug.LogWarning(
                                        "<color=Orange>Warn: </color> AIRWISEPhaseD.OnEntry() : AIRWISE Vecteur hasn't stopped..."
                                    );
                                    
                                    sync_point?.TrySetResult(false);

                                } else {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : AIRWISE Vecteur has stopped ! Ignore this message ;)"
                                    );
                                
                                } /* if() */

                            } /* if() */
                        },
                        parallel_tasks_ct_src.Token
                    )
                };
            
            // inject decceleration settings 
            airwise_entity.ConcreteDispatcher.RaiseReset(
                this.FSM.CurrentSettings.PhaseD.angular_decceleration_target,
                this.FSM.CurrentSettings.PhaseD.angular_velocity_saturation_threshold,
                this.FSM.CurrentSettings.PhaseD.linear_decceleration_target,
                this.FSM.CurrentSettings.PhaseD.linear_velocity_saturation_threshold,
                this.FSM.CurrentSettings.PhaseD.decceleration_duration,
                this.FSM.CurrentSettings.Trial.bIsTryCatch
            );

            // wait until any result
            var result = await sync_point.Task;

            // cancel running task
            parallel_tasks_ct_src.Cancel();

            // log 
            if(result)
            {
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : AIRWISE Vecteur has stopped, will check performance criteria"
                );
            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> AIRWISEPhaseD.OnEntry() : Timer has reached duration before AIRWISE Vecteur stopped... You should check configuration file..."
                );
            }
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : deactivating AIRWISE Vecteur"
            );

            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseD.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseD.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseD.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseD.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseD.OnExit() : end"
            );

        } /* OnExit() */
        
        #region Coroutines


        #endregion

    } /* public sealed class AIRWISEPhaseD */

} /* } Labsim.experiment.AIRWISE */