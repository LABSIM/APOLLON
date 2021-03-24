using UXF;
using System.Linq;
using System.Threading.Tasks;

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
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_host_timestamp = System.DateTime.Now.ToString("HH:mm:ss.ffffff");
            this.FSM.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
            
            // synchronisation mechanism (TCS + local function)
            TaskCompletionSource<bool> sync_point = new TaskCompletionSource<bool>();
            void sync_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                => sync_point?.TrySetResult(true);
            System.EventHandler<
                gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs
            > blend_alpha_local_function 
                = (sender, args) 
                    => 
                    {  
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {
                            UnityEngine.Color color = child.material.color;
                            color.a =  UnityEngine.Mathf.Clamp( (1.0f - UnityEngine.Mathf.Abs(args.Z)), 0.0f, 1.0f );
                            child.material.color = color;
                        }
                        foreach(var child in frontend.ApollonFrontendManager.Instance.getBridge(
                            frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI
                        ).Behaviour.GetComponentsInChildren<UnityEngine.MeshRenderer>() )
                        {
                            UnityEngine.Color color = child.material.color;
                            color.a =  UnityEngine.Mathf.Clamp( (1.0f - UnityEngine.Mathf.Abs(args.Z)), 0.0f, 1.0f );
                            child.material.color = color;
                        }
                        return;
                    };

            // register our synchronisation function
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
            ).Dispatcher.UserNeutralCommandTriggeredEvent += sync_local_function;
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
            ).Dispatcher.AxisZValueChangedEvent += blend_alpha_local_function;            

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
                    gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
            ).Dispatcher.AxisZValueChangedEvent -= blend_alpha_local_function;   
            (
                gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
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
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_host_timestamp = System.DateTime.Now.ToString("HH:mm:ss.ffffff");
            this.FSM.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseA */
    
} /* } Labsim.apollon.experiment.phase */