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
    // End subject + reset internal - FSM state
    //
    public sealed class LEXIKHUMOATPhaseD 
        : apollon.experiment.ApollonAbstractExperimentState<LEXIKHUMOATProfile>
    {
        public LEXIKHUMOATPhaseD(LEXIKHUMOATProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseD.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            // this.FSM.CurrentResults.PhaseD.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseD.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // refs
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

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Arret";

            // show red cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : stop moving"
            );

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // result
            bool result = false;

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

                    // end of phase
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : LEXIKHUMOAT Vecteur has "
                                + this.FSM.CurrentSettings.PhaseD.total_duration
                                + " ms to stop"
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
                                        "<color=Orange>Warn: </color> LEXIKHUMOATPhaseD.OnEntry() : LEXIKHUMOAT Vecteur hasn't stopped..."
                                    );
                                    
                                    sync_point?.TrySetResult(false);

                                } else {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : LEXIKHUMOAT Vecteur has stopped ! Ignore this message ;)"
                                    );
                                
                                } /* if() */

                            } /* if() */
                        },
                        parallel_tasks_ct_src.Token
                    ),

                    // stop motion
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : raise final decceleration motion"
                            );

                            // inject decceleration settings 
                            apollon.ApollonEngine.Schedule(() => { 
                                lexikhum_entity.ConcreteDispatcher.RaiseReset(
                                    this.FSM.CurrentSettings.PhaseD.angular_decceleration_target,
                                    this.FSM.CurrentSettings.PhaseD.angular_velocity_saturation_threshold,
                                    this.FSM.CurrentSettings.PhaseD.linear_decceleration_target,
                                    this.FSM.CurrentSettings.PhaseD.linear_velocity_saturation_threshold,
                                    this.FSM.CurrentSettings.PhaseD.decceleration_duration,
                                    this.FSM.CurrentSettings.Trial.bIsTryCatch
                                );
                            });
                            apollon.ApollonEngine.Schedule(() => { 
                                haptic_arm.ConcreteDispatcher.RaiseReset();
                            });

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : waiting for motion idle state"
                            );

                        },
                        parallel_tasks_ct_src.Token 
                    ).Unwrap().ContinueWith(
                        async(antecedent) => 
                        {
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {

                                if(!sync_point.Task.IsCompleted) 
                                {
                                    
                                    // log
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : wait for even start lane is crossed or end of phase"
                                    );

                                    // wait until any result
                                    result = await sync_point.Task;

                                } else {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : LEXIKHUMOAT Vecteur has crossed the start line or end of phase already reached !"
                                    );

                                } /* if() */

                                // // log
                                // UnityEngine.Debug.Log(
                                //     "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : motion idle state reached"
                                // );

                                // // stop acceleration settings 
                                // lexikhum_entity.ConcreteDispatcher.RaiseHold();

                            } /* if() */
                        },
                        parallel_tasks_ct_src.Token
                    ),

                    // hide green frame 
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 
                            
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : will wait " 
                                + this.FSM.CurrentSettings.PhaseD.decceleration_duration
                                + "ms before showing red frame"
                            );
                            
                            // wait a certain amout of time between each bound if cancel not requested
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {    
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.PhaseD.decceleration_duration);
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
                                    
                                    // log
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : show red frame"
                                    );
                                    
                                    // show red frame at decceleration duration
                                    apollon.ApollonEngine.Schedule(
                                        () => apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI)
                                    );

                                }

                            } /* if() */
                        },
                        parallel_tasks_ct_src.Token
                    ),

                }; /* parrallel tasks*/
            
            // wait for sync point + end of phase timer
            await System.Threading.Tasks.Task.WhenAll(parallel_tasks);    

            // cancel running task
            parallel_tasks_ct_src.Cancel();
            
            // finally, hide both red frame & cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // log 
            if(result)
            {
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : LEXIKHUMOAT Vecteur has stopped, will check performance criteria"
                );
            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> LEXIKHUMOATPhaseD.OnEntry() : Timer has reached duration before LEXIKHUMOAT Vecteur stopped... You should check configuration file..."
                );
            }
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : deactivating LEXIKHUMOAT Vecteur"
            );

            // inactivate entity
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATEntity
            );

            // update backend status
            var backend 
                = apollon.backend.ApollonBackendManager.Instance.GetValidHandle(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
                ) as apollon.backend.handle.ApollonISIRForceDimensionOmega3Handle;

            // end trial
            backend.NextGateKind          = "End";
            backend.NextGateSide          = "Trial";
            backend.SharedIntentionMode   = "";
            backend.NextGateWorldPosition = new(0.0f, 0.0f, 0.0f);
            backend.NextGateWidth         = 0.0f;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseD.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            // this.FSM.CurrentResults.PhaseD.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseD.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseD.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class LEXIKHUMOATPhaseD */

} /* } Labsim.experiment.LEXIKHUM_OAT */