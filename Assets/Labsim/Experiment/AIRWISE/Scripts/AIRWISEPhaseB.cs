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
            var airwise_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<AIRWISEEntityBridge>(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
                );
                
            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Mise en mouvement";

            // show green cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : start moving"
            );

            // synchronisation mechanism (TCS + lambda event handler)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();

            // lambdas
            System.EventHandler sync_slalom_started_local_function 
                = (sender, args) 
                    => sync_point?.TrySetResult(true);

            // bind to checkpoint manager events
            checkpoint_manager.slalomStarted += sync_slalom_started_local_function;

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

            // UI update lambda function
            System.EventHandler<
                apollon.ApollonEngine.EngineEventArgs
            > UI_lambda_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // extract values
                        float
                            current
                                = UnityFrame.GetAbsoluteVelocity(
                                    airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Rb
                                ).z,
                            target      
                                = /* Z accel */ this.FSM.CurrentSettings.PhaseB.linear_acceleration_target[2] 
                                * /* delta T */ (this.FSM.CurrentSettings.PhaseB.acceleration_duration / 1000.0f),
                            lower_bound 
                                = /* 0x  */ 0.0f * target,
                            upper_bound 
                                = /* 2x  */ 2.0f * target,
                            // margin_value 
                            //     = /* 10% */ 0.1f,
                            cursor_value         
                                = UnityEngine.Mathf.Clamp(
                                    current - lower_bound,
                                    lower_bound,
                                    upper_bound
                                ) 
                                / (upper_bound - lower_bound);

                        // update cursor
                        apollon.frontend.ApollonFrontendManager.Instance
                            .getConcreteBridge<apollon.frontend.gui.ApollonSideSliderGUIBridge>
                            (
                                apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                            )
                            .Behaviour
                            .GetComponentInParent<UnityEngine.UI.Slider>()
                            .value 
                            = cursor_value;

                        // log 
                        // WTF... demander a Yale... ? 
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : FUCK "
                            + "unity absolute frame:"  + UnityFrame.GetAbsoluteVelocity(airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Rb) 
                            + "/unity relative  frame:" + UnityFrame.GetRelativeVelocity(airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Rb) 
                            + "/aero absolute frame:"  + AeroFrame.GetAbsoluteVelocity(airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Rb) 
                            + "/aero relative  frame:" + AeroFrame.GetRelativeVelocity(airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Rb) 
                            + "/current:" + current
                            + "/target:" + target
                            + "/lower_bound:" + lower_bound
                            + "/upper_bound:" + upper_bound
                            + "/cursor_value:" + cursor_value
                        );
                        
                    }; /* lambda */

            // show UI if active
            if(this.FSM.CurrentSettings.Trial.bIsActive)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : active condition, initializing UI"
                );

                // activate UI
                apollon.frontend.ApollonFrontendManager.Instance.setActive(
                    apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                );

                // mapping UI update  
                apollon.ApollonEngine.Instance.EngineUpdateEvent += UI_lambda_function; 
                
            } /* if() */

            var parallel_tasks 
                = new System.Collections.Generic.List<System.Threading.Tasks.Task>() 
                {

                    // end of phase timer 
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur has "
                                + this.FSM.CurrentSettings.PhaseB.total_duration
                                + " ms to cross the start"
                            );

                            // wait a certain amout of time between each bound if cancel not requested
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.PhaseB.total_duration);
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
                        parallel_tasks_ct_src.Token
                    ),

                    // init motion
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 

                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : activating AIRWISE Entity & Yale's controller"
                            );

                            // initializing
                            apollon.ApollonEngine.Schedule(() => {

                                airwise_entity.ConcreteBehaviour.GetComponentInChildren<QuadController>().Inhibit 
                                    = !this.FSM.CurrentSettings.Trial.bIsActive;
                                
                                apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
                                );
                                
                            });

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : raise initial accel motion"
                            );

                            // inject acceleration settings 
                            apollon.ApollonEngine.Schedule(() => {
                                            
                                if(this.FSM.CurrentSettings.Trial.bIsActive)
                                {

                                    airwise_entity.ConcreteDispatcher.RaiseControl();
                                    
                                }
                                else
                                {

                                    airwise_entity.ConcreteDispatcher.RaiseInit(
                                        this.FSM.CurrentSettings.PhaseB.angular_acceleration_target,
                                        this.FSM.CurrentSettings.PhaseB.angular_velocity_saturation_threshold,
                                        this.FSM.CurrentSettings.PhaseB.linear_acceleration_target,
                                        this.FSM.CurrentSettings.PhaseB.linear_velocity_saturation_threshold,
                                        this.FSM.CurrentSettings.PhaseB.acceleration_duration,
                                        this.FSM.CurrentSettings.Trial.bIsTryCatch
                                    );
                                
                                } /* if() */

                            });

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : waiting for motion idle state"
                            );

                        },
                        parallel_tasks_ct_src.Token 
                    ).Unwrap(),

                    // hide green frame 
                    parallel_tasks_factory.StartNew(
                        async () => 
                        { 
                            
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : will wait " 
                                + this.FSM.CurrentSettings.PhaseB.acceleration_duration
                                + "ms before hiding green frame"
                            );
                            
                            // wait a certain amout of time between each bound if cancel not requested
                            if(!parallel_tasks_ct.IsCancellationRequested)
                            {
                                await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.PhaseB.acceleration_duration);
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
                                        "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : hide green frame"
                                    );
                                    
                                    // hide green frame at acceleration duration
                                    apollon.ApollonEngine.Schedule(
                                        () => apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI)
                                    );

                                } else {
                                    
                                    UnityEngine.Debug.Log(
                                        "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : AIRWISE Vecteur has crossed the start line ! green frame is already deactivated"
                                    );

                                } /* if() */

                            } /* if() */
                        },
                        parallel_tasks_ct_src.Token
                    ),

                }; /* parrallel tasks */
            
            // wait for sync point + end of phase timer
            result = await sync_point.Task;
            // await System.Threading.Tasks.Task.WhenAll(parallel_tasks);    

            // cancel running task
            parallel_tasks_ct_src.Cancel();

            // hide UI if active
            if(this.FSM.CurrentSettings.Trial.bIsActive)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEPhaseB.OnEntry() : active condition, closing UI"
                );

                // inactivate UI
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(
                    apollon.frontend.ApollonFrontendManager.FrontendIDType.SideSliderGUI
                );

                // deleting UI update  
                apollon.ApollonEngine.Instance.EngineUpdateEvent -= UI_lambda_function; 
                
            } /* if() */

            // finally, hide all green
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

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