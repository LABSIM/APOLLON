using UXF;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseB
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {
        public ApollonCAVIARPhaseB(profile.ApollonCAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_host_timestamp = (
                // from ref point
                new System.DateTime(UXF.FileIOManager._hr_refpoint.Ticks).AddMilliseconds(
                    // then add elapsed ticks to ns to ms
                    UXF.FileIOManager._hr_timer.ElapsedTicks
                    * ((1000L * 1000L * 1000L) / System.Diagnostics.Stopwatch.Frequency)
                    / 1000000.0
                )
            ).ToString("HH:mm:ss.fffffff");
            this.FSM.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;
           
            // show green cross & frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // get our entity bridge & settings
            var caviar_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as gameplay.entity.ApollonCAVIAREntityBridge
                );
            var fog_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    ) as gameplay.element.ApollonFogElementBridge
                );

            // get our acceleration value & timestamp
            float 
                /* saturation speed {m.s-1} */
                linear_velocity 
                    = this.FSM.CurrentSettings.phase_C_settings.First().target_velocity,
                /* constant linear acceleration {m.s-2} */
                linear_acceleration 
                    = (
                        UnityEngine.Mathf.Pow(linear_velocity, 2.0f) 
                        / (2.0f * this.FSM.CurrentSettings.phase_B_distance)
                    ),
                /* phase duration {ms} */
                phase_duration 
                    = ( linear_velocity / linear_acceleration ) * 1000.0f;
                    
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnEntry() : calculated following parameter ["
                + "linear_velocity:" 
                    + linear_velocity 
                + ",linear_acceleration:" 
                    + linear_acceleration
                + ",phase_duration:" 
                    + phase_duration 
                + "], begin transition, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // accelerate up to the first phase C settings & smooth fog
            caviar_bridge.Dispatcher.RaiseAccelerate(linear_acceleration,linear_velocity);
            fog_bridge.Dispatcher.RaiseSmoothLinearFogRequested(
                this.FSM.CurrentSettings.phase_C_settings.First().fog_start_distance,
                this.FSM.CurrentSettings.phase_C_settings.First().fog_end_distance,
                UnityEngine.Color.white,
                phase_duration
            );

            // wait a certain amout of time
            await this.FSM.DoSleep(phase_duration / 2.0f);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnEntry() : mid-phase, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

            // hide green frame first
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(phase_duration / 2.0f);

            // then hide cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnEntry() : end, current distance["
                + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_host_timestamp = (
                // from ref point
                new System.DateTime(UXF.FileIOManager._hr_refpoint.Ticks).AddMilliseconds(
                    // then add elapsed ticks to ns to ms
                    UXF.FileIOManager._hr_timer.ElapsedTicks
                    * ((1000L * 1000L * 1000L) / System.Diagnostics.Stopwatch.Frequency)
                    / 1000000.0
                )
            ).ToString("HH:mm:ss.fffffff");
            this.FSM.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseB */
    
} /* } Labsim.apollon.experiment.phase */