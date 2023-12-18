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
namespace Labsim.experiment.CAVIAR
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class CAVIARPhaseA
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {
        public CAVIARPhaseA(CAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
            
            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            void sync_local_function(object sender, CAVIARControlDispatcher.CAVIARControlEventArgs e)
                => sync_point?.TrySetResult(true);
            System.EventHandler<
                CAVIARControlDispatcher.CAVIARControlEventArgs
            > blend_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // extract clamped value [-1.0 < x < 1.0]
                        var value = UnityEngine.Mathf.Clamp( (1.0f - UnityEngine.Mathf.Abs(args.Z)), 0.0f, 1.0f );

                        // update cross properties
                        foreach(var child in apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {

                            // alpha blend on value
                            UnityEngine.Color color = child.material.color;
                            color.a = value;
                            child.material.color = color;

                        } /* foreach() */

                        // update frame properties
                        foreach(var child in apollon.frontend.ApollonFrontendManager.Instance.getBridge(
                            apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {

                            // alpha blend on value
                            UnityEngine.Color color = child.material.color;
                            color.a = value;
                            child.material.color = color;

                        } /* foreach()*/
                        
                    }; /* lambda */

            // register our synchronisation function
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as CAVIARControlBridge
            ).ConcreteDispatcher.UserNeutralCommandTriggeredEvent += sync_local_function;
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as CAVIARControlBridge
            ).ConcreteDispatcher.AxisZValueChangedEvent += blend_local_function;

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
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as CAVIARControlBridge
            ).ConcreteDispatcher.AxisZValueChangedEvent -= blend_local_function;   
            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as CAVIARControlBridge
            ).ConcreteDispatcher.UserNeutralCommandTriggeredEvent -= sync_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseA.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseA */
    
} /* } Labsim.apollon.experiment.phase */