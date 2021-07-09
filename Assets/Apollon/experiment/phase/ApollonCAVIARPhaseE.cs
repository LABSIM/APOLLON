using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseE
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {
        public ApollonCAVIARPhaseE(profile.ApollonCAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // get our entity bridge & settings
            var caviar_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as gameplay.entity.ApollonCAVIAREntityBridge
                );

            // get our acceleration value & timestamp
            float 
                /* saturation speed {m.s-1} */
                entry_linear_velocity 
                    = this.FSM.CurrentSettings.phase_C_settings.Last().target_velocity,
                /* constant linear acceleration {m.s-2} */
                linear_absolute_deceleration 
                    = UnityEngine.Mathf.Abs(
                        - UnityEngine.Mathf.Pow(entry_linear_velocity, 2.0f) 
                        / (2.0f * this.FSM.CurrentSettings.phase_E_distance)
                    ),
                /* phase duration {ms} */
                phase_duration 
                    = ( entry_linear_velocity / linear_absolute_deceleration ) * 1000.0f;
                    
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnEntry() : calculated following parameter ["
                + "entry_linear_velocity:" 
                    + entry_linear_velocity 
                + ",linear_absolute_deceleration:" 
                    + linear_absolute_deceleration
                + ",phase_duration:" 
                    + phase_duration 
                + "], begin transition, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // if practicing
            if(ApollonExperimentManager.Instance.Trial.settings.GetBool("is_practice_condition"))
            {
            
                // hide guidance
                // frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.SimpleCrossGUI);
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.SimpleFrameGUI);
            
            } /* if() */

            // show red cross
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // inactivate all visual cues through LINQ request
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;
            foreach (var vc_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("VCTag_")).Select(kvp => kvp.Value))
            {
                vc_ref.SetActive(false);
            }
            
            // decelerate up to stop
            caviar_bridge.Dispatcher.RaiseDecelerate(linear_absolute_deceleration,0.0f);

            // wait a certain amout of time
            await this.FSM.DoSleep(phase_duration / 2.0f);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnEntry() : mid-phase, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // show red frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(phase_duration / 2.0f);

            // hide red cross & frame
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnEntry() : end, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_host_timestamp = UXF.ApplicationHandler.CurrentHighResolutionTime;
            this.FSM.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseE.OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseE */
    
} /* } Labsim.apollon.experiment.phase */