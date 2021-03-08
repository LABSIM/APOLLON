using UXF;
using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseC
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {

        public int CurrentID { private set; get; } = -1;

        public ApollonCAVIARPhaseC(profile.ApollonCAVIARProfile fsm, int currentID)
            : base(fsm)
        {
            this.CurrentID = currentID;
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnEntry() : begin"
            );

            // get our entity bridge & our settings
            var caviar_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
                    ) as gameplay.entity.ApollonCAVIAREntityBridge
                );
            var hotas_bridge
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                    ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                );
            var phase_settings
                = this.FSM.CurrentSettings.phase_C_settings[this.CurrentID];
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;

            // setup visual cue
            switch(phase_settings.visual_cue_type)
            {

                // projected grid
                default:
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Grille:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(true);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(false);
                    we_behaviour.References["VCTag_3DCube"].SetActive(false);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(false);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(false);
                    break;
                }

                // 3D object - tetrahedre [default]
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet3D_Tetrahedre:
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet3D:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(false);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(true);
                    we_behaviour.References["VCTag_3DCube"].SetActive(false);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(false);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(false);
                    break;
                }

                // 3D object - cube
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet3D_Cube:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(false);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(false);
                    we_behaviour.References["VCTag_3DCube"].SetActive(true);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(false);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(false);
                    break;
                }

                // 2D object - square [default]
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet2D_Square:
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet2D:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(false);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(false);
                    we_behaviour.References["VCTag_3DCube"].SetActive(false);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(true);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(false);
                    break;
                }

                // 2D object - circle
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Objet2D_Circle:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(false);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(false);
                    we_behaviour.References["VCTag_3DCube"].SetActive(false);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(false);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(true);
                    break;
                }

                // Controle
                case profile.ApollonCAVIARProfile.Settings.VisualCueIDType.Controle:
                {
                    we_behaviour.References["VCTag_Grid"].SetActive(false);
                    we_behaviour.References["VCTag_3DTetrahedre"].SetActive(false);
                    we_behaviour.References["VCTag_3DCube"].SetActive(false);
                    we_behaviour.References["VCTag_2DSquare"].SetActive(false);
                    we_behaviour.References["VCTag_2DCircle"].SetActive(false);
                    break;
                }

            } /* switch() */
            
            // save our local origin to world coord depth point
            float current_phase_start_distance
                = caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z;

            // synchronisation mechanism (TCS + local function)
            TaskCompletionSource<(bool, float,float, string)> sync_point = new TaskCompletionSource<(bool, float, float, string)>();
            void sync_user_response_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                => sync_point?.TrySetResult((
                    /* detection!  */ 
                    true, 
                    /* current relative phase depth */
                    (caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z - current_phase_start_distance),
                    /* unity render timestamp */
                    UnityEngine.Time.time,
                    /* host timestamp */ 
                    System.DateTime.Now.ToString("HH:mm:ss.ffffff")
                ));
            void sync_end_stim_local_function(object sender, gameplay.entity.ApollonCAVIAREntityDispatcher.EventArgs e)
                => sync_point?.TrySetResult((false, -1.0f, -1.0f, "-1"));

            // register our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent += sync_end_stim_local_function;

            // check if there is a stim
            if( 
                !(
                    (phase_settings.stim_begin_distance == -1.0f)
                    && (phase_settings.stim_velocity == -1.0f)
                    && (phase_settings.stim_acceleration == -1.0f)
                )
            ) 
            {
            
                // get our stim begin timestamp from actual target velocity
                float stim_begin_timestamp = (phase_settings.stim_begin_distance / phase_settings.target_velocity) * 1000.0f;
                        
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                        + this.CurrentID
                    + "].OnEntry() : stim detected, calculated following parameter ["
                    + "stim_begin_timestamp:" 
                        + stim_begin_timestamp 
                    + "], will wait"
                );

                // wait a certain amout of time
                await this.FSM.DoSleep(stim_begin_timestamp);
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                        + this.CurrentID
                    + "].OnEntry() : stim will begin, current distance["
                        + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                    + "]"
                );

                // accelerate/decelerate up to the stim settings or nothing :)
                if(phase_settings.stim_velocity > phase_settings.target_velocity) 
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise acceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // accel.
                    caviar_bridge.Dispatcher.RaiseAccelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else if(phase_settings.stim_velocity < phase_settings.target_velocity) 
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, raise deceleration["
                        + "stim_acceleration:"
                            + phase_settings.stim_acceleration
                        + ",stim_velocity:"
                            + phase_settings.stim_velocity
                        + "]."
                    );

                    // decel.
                    caviar_bridge.Dispatcher.RaiseDecelerate(
                        phase_settings.stim_acceleration,
                        phase_settings.stim_velocity
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonCAVIARPhaseC["
                            + this.CurrentID
                        + "].OnEntry() : begin stim, requested stim velocity is identical to target valocity, skip."
                    );


                } /* if() */

            } /* if() */

            // async request notification until
            await caviar_bridge.DoNotifyWhenWaypointReached(
                current_phase_start_distance + phase_settings.total_distance
            );

            // wait synchronisation point & reset it once hit
            (
                this.FSM.CurrentResults.phase_C_results[CurrentID].user_response, 
                this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_distance, 
                this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_unity_timestamp,
                this.FSM.CurrentResults.phase_C_results[CurrentID].user_perception_host_timestamp
            ) = await sync_point.Task;

            // unregister our synchronisation function
            hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
            caviar_bridge.Dispatcher.WaypointReachedEvent -= sync_end_stim_local_function;
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnEntry() : end, current distance["
                    + caviar_bridge.Behaviour.transform.TransformPoint(0.0f,0.0f,0.0f).z
                + "]"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : begin"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseC["
                    + this.CurrentID
                + "].OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseC */
    
} /* } Labsim.apollon.experiment.phase */