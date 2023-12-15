using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Latency phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseB
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseB(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : begin"
            );

            // get a (Unity) bounded random amount of time to wait
            float bounded_random_timeout
                = UnityEngine.Random.Range(
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_lower_bound,
                    this.FSM.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                );

            // get bridges
            var control_bridge
                = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge;

            // synchronisation mechanism (TCS + local function)
            var sync_detection_point = new System.Threading.Tasks.TaskCompletionSource<(bool, float, string)>();  
            void sync_user_response_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionControlDispatcher.AgencyAndThresholdPerceptionControlEventArgs e)
                    => sync_detection_point?.TrySetResult((true, UnityEngine.Time.time, System.DateTime.Now.ToString("HH:mm:ss.ffffff")));

            // register our synchronisation function
            control_bridge.ConcreteDispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;

            var phase_running_task 
                // wait for random wait
                = System.Threading.Tasks.Task.Factory.StartNew(
                    async () => 
                    { 
                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : will wait " 
                            + bounded_random_timeout
                            + " ms"
                        );

                        // wait a certain amout of time between each bound
                        await ApollonHighResolutionTime.DoSleep(bounded_random_timeout);
                    } 
                ).Unwrap().ContinueWith(
                    antecedent => 
                    {
                        if(!sync_detection_point.Task.IsCompleted) 
                        {
                            
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : user hasn't responded, injecting default result"
                            );
                            
                            sync_detection_point?.TrySetResult((false, -1.0f, "-1.0"));

                        } else {
                            
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : user has responded, keep result"
                            );
                        
                        } /* if() */
                    }
                );

            // wait for detection synchronisation point indefinitely & reset it once hit
            (
                this.FSM.CurrentResults.user_response_B, 
                this.FSM.CurrentResults.user_perception_B_unity_timestamp,
                this.FSM.CurrentResults.user_perception_B_host_timestamp
            ) = await sync_detection_point.Task;

            // unregister our control synchronisation function
            control_bridge.ConcreteDispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnEntry() : end phase, result [user_response_B:"
                + this.FSM.CurrentResults.user_response_B
                + ",user_perception_B_unity_timestamp:"
                + this.FSM.CurrentResults.user_perception_B_unity_timestamp
                + ",user_perception_B_host_timestamp:"
                + this.FSM.CurrentResults.user_perception_B_host_timestamp
                + "]"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseB.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseB */

} /* } Labsim.apollon.experiment.phase */
