using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseA
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {
        public ApollonCAVIARPhaseA(profile.ApollonCAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
            
            // synchronisation mechanism (TCS + local function)
            var sync_point = new System.Threading.Tasks.TaskCompletionSource<bool>();
            void sync_local_function(object sender, gameplay.control.ApollonCAVIARControlDispatcher.EventArgs e)
                => sync_point?.TrySetResult(true);
            System.EventHandler<
                gameplay.control.ApollonCAVIARControlDispatcher.EventArgs
            > blend_local_function 
                = (sender, args) 
                    => 
                    {  
                        
                        // extract clamped value [-1.0 < x < 1.0]
                        var value = UnityEngine.Mathf.Clamp( (1.0f - UnityEngine.Mathf.Abs(args.Z)), 0.0f, 1.0f );

                        // update cross properties
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {

                            // alpha blend on value
                            UnityEngine.Color color = child.material.color;
                            color.a = value;
                            child.material.color = color;

                        } /* foreach() */

                        // update frame properties
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI
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
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as gameplay.control.ApollonCAVIARControlBridge
            ).Dispatcher.UserNeutralCommandTriggeredEvent += sync_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as gameplay.control.ApollonCAVIARControlBridge
            ).Dispatcher.AxisZValueChangedEvent += blend_local_function;

            // show grey cross & frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // wait synchronisation point indefinitely & reset it once hit
            await sync_point.Task;

            // hide grey cross & frame
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            
            // unregister our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as gameplay.control.ApollonCAVIARControlBridge
            ).Dispatcher.AxisZValueChangedEvent -= blend_local_function;   
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl
                ) as gameplay.control.ApollonCAVIARControlBridge
            ).Dispatcher.UserNeutralCommandTriggeredEvent -= sync_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseA.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseA */
    
} /* } Labsim.apollon.experiment.phase */