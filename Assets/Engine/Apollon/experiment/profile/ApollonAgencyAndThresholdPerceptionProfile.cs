﻿using UXF;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// avoid namespace pollution
namespace Labsim.apollon.experiment.profile
{

    public class ApollonAgencyAndThresholdPerceptionProfile 
        : ApollonAbstractExperimentFiniteStateMachine< ApollonAgencyAndThresholdPerceptionProfile >
    {

        // Ctor
        public ApollonAgencyAndThresholdPerceptionProfile()
            : base()
        {
            // default profile
            this.m_profileID = ApollonExperimentManager.ProfileIDType.AgencyAndThresholdPerception;
        }

        #region settings/result
        
        public class Settings
        {
            public enum ScenarioIDType
            {

                [System.ComponentModel.Description("Undefined")]
                Undefined = 0,

                [System.ComponentModel.Description("Visual-Only")]
                VisualOnly,

                [System.ComponentModel.Description("Vestibular-Only")]
                VestibularOnly,

                [System.ComponentModel.Description("Visuo-Vestibular")]
                VisuoVestibular

            } /* enum */

            public bool bIsActive;
            public bool bIsTryCatch;

            public ScenarioIDType scenario_type;

            public float
                phase_A_duration,
                phase_B_begin_stim_timeout_lower_bound,
                phase_B_begin_stim_timeout_upper_bound,
                phase_C_max_stim_duration,
                phase_C_max_stim_angle,
                phase_C_angular_acceleration,
                phase_C_angular_saturation_speed,
                phase_D_duration;

        } /* class Settings */

        public class Results
        {

            public bool
                user_response;

            public int
                user_command;

            public float
                user_stim_unity_timestamp,
                user_perception_unity_timestamp;

            public string
                user_stim_host_timestamp,
                user_perception_host_timestamp;

            public UnityEngine.AudioClip
                user_clip;

        } /* class Results */

        // properties
        public Settings CurrentSettings { get; } = new Settings();
        public Results CurrentResults { get; set; } = new Results();

        // fast hack
        public uint
            positiveConditionCount = 0,
            negativeConditionCount = 0;

        #endregion

        #region FSM phase states implementation


        //
        // Reset/init condition phase - FSM state
        //
        internal sealed class Phase0 : ApollonAbstractExperimentState<ApollonAgencyAndThresholdPerceptionProfile>
        {
            public Phase0(ApollonAgencyAndThresholdPerceptionProfile fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.Phase0.OnEntry() : begin"
                );

                // if active condition 
                if (this.FSM.CurrentSettings.bIsActive)
                {

                    // synchronisation mechanism (TCS + local function)
                    TaskCompletionSource<bool> sync_point = new TaskCompletionSource<bool>();
                    void sync_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                        => sync_point?.TrySetResult(true);

                    // register our synchronisation function
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    ).Dispatcher.UserNeutralCommandTriggeredEvent += sync_local_function;

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
                    ).Dispatcher.UserNeutralCommandTriggeredEvent -= sync_local_function;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.Phase0.OnEntry() : end"
                );

            } /* OnEntry() */

            public async override System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.Phase0.OnExit() : begin"
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.Phase0.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class Phase0 */

        //
        // User command input phase - FSM state
        //
        internal sealed class PhaseA : ApollonAbstractExperimentState<ApollonAgencyAndThresholdPerceptionProfile>
        {
            public PhaseA(ApollonAgencyAndThresholdPerceptionProfile fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnEntry() : begin"
                );
            
                // fade in to black for vestibular-only scenario
                if(this.FSM.CurrentSettings.scenario_type == Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    Task.Run(() => this.FSM.DoFadeIn(this.FSM.CurrentSettings.phase_A_duration));

                } /* if() */

                // show green cross & frame
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

                // wait a certain amout of time
                await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

                // hide green frame first
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

                // wait a certain amout of time
                await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

                // then hide cross
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

                // if active condition 
                if (this.FSM.CurrentSettings.bIsActive)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnEntry() : active condition"
                    );

                    // synchronisation mechanism (TCS + local function)
                    TaskCompletionSource<int> sync_point = new TaskCompletionSource<int>();
                    void sync_positive_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                        => sync_point?.TrySetResult(1);
                    void sync_negative_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                        => sync_point?.TrySetResult(-1);

                    // register our synchronisation function
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    ).Dispatcher.UserPositiveCommandTriggeredEvent += sync_positive_local_function;
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    ).Dispatcher.UserNegativeCommandTriggeredEvent += sync_negative_local_function;

                    // wait synchronisation point indefinitely & reset it once hit
                    this.FSM.CurrentResults.user_command = await sync_point.Task;

                    // hack
                    if (this.FSM.CurrentResults.user_command > 0)
                    {
                        this.FSM.positiveConditionCount++;
                    }
                    else
                    {
                        this.FSM.negativeConditionCount++;
                    }

                    // unregister our synchronisation function
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    ).Dispatcher.UserPositiveCommandTriggeredEvent -= sync_positive_local_function;
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                        ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge
                    ).Dispatcher.UserNegativeCommandTriggeredEvent -= sync_negative_local_function;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnEntry() : user command request ["
                        + this.FSM.CurrentResults.user_command
                        + "]"
                    );

                }
                else
                {

                    // null command 
                    this.FSM.CurrentResults.user_command = 1;
                    
                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnEntry() : end"
                );

            } /* OnEntry() */

            public async override System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnExit() : begin"
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseA.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class PhaseA */

        //
        // Latency phase - FSM state
        //
        internal sealed class PhaseB : ApollonAbstractExperimentState<ApollonAgencyAndThresholdPerceptionProfile>
        {
            public PhaseB(ApollonAgencyAndThresholdPerceptionProfile fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseB.OnEntry() : begin"
                );

                // get a (Unity) bounded random amount of time to wait
                float bounded_random_timeout
                    = UnityEngine.Random.Range(
                        this.FSM.CurrentSettings.phase_B_begin_stim_timeout_lower_bound,
                        this.FSM.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                    );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseB.OnEntry() : will wait " 
                    + bounded_random_timeout
                    + " ms"
                );

                // wait a certain amout of time between each bound
                await this.FSM.DoSleep(bounded_random_timeout);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseB.OnEntry() : end"
                );

            } /* OnEntry() */

            public async override System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseB.OnExit() : begin"
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseB.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class PhaseB */

        //
        // Acceleration phase - FSM state
        //
        internal sealed class PhaseC : ApollonAbstractExperimentState<ApollonAgencyAndThresholdPerceptionProfile>
        {
            public PhaseC(ApollonAgencyAndThresholdPerceptionProfile fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : begin"
                );
                

                // get bridges
                gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge hotas_bridge
                    = gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor
                    ) as gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorBridge;

                gameplay.entity.ApollonActiveSeatEntityBridge seat_bridge
                    = gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                    ) as gameplay.entity.ApollonActiveSeatEntityBridge;

                // check
                if (hotas_bridge == null || seat_bridge == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : Could not find corresponding gameplay bridge !"
                    );

                    // fail
                    return;

                } /* if() */

                // synchronisation mechanism (TCS + local function)
                TaskCompletionSource<(bool, float, string)> sync_point = new TaskCompletionSource<(bool, float, string)>();
                void sync_user_response_local_function(object sender, gameplay.device.sensor.ApollonHOTASWarthogThrottleSensorDispatcher.EventArgs e)
                    => sync_point?.TrySetResult((true, UnityEngine.Time.time, System.DateTime.Now.ToString("HH:mm:ss.ffffff")));
                void sync_end_stim_local_function(object sender, gameplay.entity.ApollonActiveSeatEntityDispatcher.EventArgs e)
                    => sync_point?.TrySetResult((false, -1.0f, "-1"));

                // register our synchronisation function
                hotas_bridge.Dispatcher.UserResponseTriggeredEvent += sync_user_response_local_function;
                seat_bridge.Dispatcher.StopEvent += sync_end_stim_local_function;

                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : begin stim"
                );

                // log stim begin timestamp
                this.FSM.CurrentResults.user_stim_unity_timestamp = UnityEngine.Time.time;
                this.FSM.CurrentResults.user_stim_host_timestamp = System.DateTime.Now.ToString("HH:mm:ss.ffffff");
                
                // begin stim
                seat_bridge.Dispatcher.RaiseAccelerate(
                    /* if not a try/catch condition ? */ (
                        this.FSM.CurrentSettings.bIsTryCatch
                            ? 0.0f
                            : (this.FSM.CurrentResults.user_command * this.FSM.CurrentSettings.phase_C_angular_acceleration)
                    ),
                    /* if not a try/catch condition ? */ (
                        this.FSM.CurrentSettings.bIsTryCatch
                            ? 0.0f
                            : (this.FSM.CurrentResults.user_command * this.FSM.CurrentSettings.phase_C_angular_saturation_speed)
                    ),
                    this.FSM.CurrentSettings.phase_C_max_stim_duration,
                    this.FSM.CurrentSettings.phase_C_max_stim_angle,
                    (this.FSM.CurrentSettings.scenario_type == Settings.ScenarioIDType.VisualOnly ? true : false)
                );
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : begin "
                    + (this.FSM.CurrentSettings.bIsTryCatch ? "fake" : "real")
                    + " stim, result [user_stim_unity_timestamp:"
                    + this.FSM.CurrentResults.user_stim_unity_timestamp
                    + ",user_stim_host_timestamp:"
                    + this.FSM.CurrentResults.user_stim_host_timestamp
                    + "]"
                );

                // wait synchronisation point indefinitely & reset it once hit
                (
                    this.FSM.CurrentResults.user_response, 
                    this.FSM.CurrentResults.user_perception_unity_timestamp,
                    this.FSM.CurrentResults.user_perception_host_timestamp
                ) = await sync_point.Task;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : end stim, result [user_response:"
                    + this.FSM.CurrentResults.user_response
                    + ",user_perception_unity_timestamp:"
                    + this.FSM.CurrentResults.user_perception_unity_timestamp
                    + ",user_perception_host_timestamp:"
                    + this.FSM.CurrentResults.user_perception_host_timestamp
                    + "]"
                );

                // unregister our synchronisation function
                hotas_bridge.Dispatcher.UserResponseTriggeredEvent -= sync_user_response_local_function;
                seat_bridge.Dispatcher.StopEvent -= sync_end_stim_local_function;


                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnEntry() : end"
                );

            } /* OnEntry() */

            public async override System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnExit() : begin"
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseC.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class PhaseC */

        //
        // End phase - FSM state
        //
        internal sealed class PhaseD : ApollonAbstractExperimentState<ApollonAgencyAndThresholdPerceptionProfile>
        {
            public PhaseD(ApollonAgencyAndThresholdPerceptionProfile fsm)
                : base(fsm)
            {
            }

            public async override System.Threading.Tasks.Task OnEntry()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnEntry() : begin"
                );
                
                // get bridge
                gameplay.entity.ApollonActiveSeatEntityBridge seat_bridge
                    = gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                    ) as gameplay.entity.ApollonActiveSeatEntityBridge;

                // check
                if (seat_bridge == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnEntry() : Could not find corresponding gameplay bridge !"
                    );

                    // fail
                    return;

                } /* if() */

                // stop movement
                seat_bridge.Dispatcher.RaiseStop();
                
                // fade out from black for vestibular-only scenario
                if (this.FSM.CurrentSettings.scenario_type == Settings.ScenarioIDType.VestibularOnly)
                {

                    // run it asynchronously
                    Task.Run(() => this.FSM.DoFadeOut(this.FSM.CurrentSettings.phase_D_duration));

                } /* if() */

                // inactivate HOTAS
                gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor);

                // show red cross
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                // wait a certain amout of time
                await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

                // show red frame
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

                // wait a certain amout of time
                await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_D_duration / 2.0f);

                // hide red cross & frame
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedFrameGUI);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnEntry() : end"
                );

            } /* OnEntry() */

            public async override System.Threading.Tasks.Task OnExit()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnExit() : begin"
                );

                // get bridge
                gameplay.entity.ApollonActiveSeatEntityBridge seat_bridge
                    = gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                    ) as gameplay.entity.ApollonActiveSeatEntityBridge;

                // check
                if (seat_bridge == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnExit() : Could not find corresponding gameplay bridge !"
                    );

                    // fail
                    return;

                } /* if() */

                // finally reset
                seat_bridge.Dispatcher.RaiseReset();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.PhaseD.OnExit() : end"
                );

            } /* OnExit() */

        } /* internal sealed class PhaseD */
        
        #endregion

        #region abstract implementation

        public override void onUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.onUpdate(sender, arg);

        } /* onUpdate() */

        public override void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // activate the active chair backend
            backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(
                backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
            );

            // send event to active chair over CAN bus
            (
                backend.ApollonBackendManager.Instance.GetValidHandle(
                    backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                ) as backend.handle.ApollonActiveSeatHandle
            ).BeginSession();

            // base call
            base.onExperimentSessionBegin(sender, arg);

        } /* onExperimentSessionBegin() */

        public override void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // base call
            base.onExperimentSessionEnd(sender, arg);

            // send event to active chair over CAN bus
            (
                backend.ApollonBackendManager.Instance.GetValidHandle(
                    backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                ) as backend.handle.ApollonActiveSeatHandle
            ).EndSession();

            // deactivate the active chair backend
            backend.ApollonBackendManager.Instance.RaiseHandleDeactivationRequestedEvent(
                backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
            );

        } /* onExperimentSessionEnd() */
        
        public override async void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : begin"
            );

            // send event to active chair over CAN bus
            (
                backend.ApollonBackendManager.Instance.GetValidHandle(
                    backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                ) as backend.handle.ApollonActiveSeatHandle
            ).BeginTrial();
            
            //// activate audio recording if any available
            //if (UnityEngine.Microphone.devices.Length != 0)
            //{
            //    // use default device
            //    this.CurrentResults.user_clip
            //    = UnityEngine.Microphone.Start(
            //        null,
            //        true,
            //        45,
            //        44100
            //    );
            //}

            //// log
            //UnityEngine.Debug.Log(
            //    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : audio recording started"
            //);

            // local
            int currentIdx = ApollonExperimentManager.Instance.Session.currentTrialNum - 1;

            // current scenario
            switch (arg.Trial.settings.GetString("scenario_name"))
            {

                case "visual-only":
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisualOnly;
                    break;
                }
                case "vestibular-only":
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                    break;
                }
                case "visuo-vestibular":
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
                    break;
                }
                default:
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.Undefined;
                    break;
                }

            } /* switch() */

            // extract trial settings
            this.CurrentSettings.bIsTryCatch                            = arg.Trial.settings.GetBool("is_catch_try_condition");
            this.CurrentSettings.bIsActive                              = arg.Trial.settings.GetBool("is_active_condition");
            this.CurrentSettings.phase_A_duration                       = arg.Trial.settings.GetFloat("phase_A_duration_ms");
            this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound = arg.Trial.settings.GetFloat("phase_B_begin_stim_timeout_lower_bound_ms");
            this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound = arg.Trial.settings.GetFloat("phase_B_begin_stim_timeout_upper_bound_ms");
            this.CurrentSettings.phase_C_max_stim_duration              = arg.Trial.settings.GetFloat("phase_C_max_stim_duration_ms");
            this.CurrentSettings.phase_C_max_stim_angle                 = arg.Trial.settings.GetFloat("phase_C_max_stim_angle_deg");
            this.CurrentSettings.phase_C_angular_acceleration           = arg.Trial.settings.GetFloat("phase_C_angular_acceleration_deg_per_s2");
            this.CurrentSettings.phase_C_angular_saturation_speed       = arg.Trial.settings.GetFloat("phase_C_angular_saturation_speed_deg_per_s");
            this.CurrentSettings.phase_D_duration                       = arg.Trial.settings.GetFloat("phase_D_duration_ms");
            
            // log the
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : found current settings with pattern["
                + arg.Trial.settings.GetString("current_pattern")
                + "]"
                + "\n - bIsTryCatch : " + this.CurrentSettings.bIsTryCatch
                + "\n - bIsActive : " + this.CurrentSettings.bIsActive
                + "\n - scenario_name : " + ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + "\n - phase_A_duration : " + this.CurrentSettings.phase_A_duration
                + "\n - phase_B_begin_stim_timeout_lower_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound
                + "\n - phase_B_begin_stim_timeout_upper_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                + "\n - phase_C_max_stim_duration : " + this.CurrentSettings.phase_C_max_stim_duration
                + "\n - phase_C_max_stim_angle : " + this.CurrentSettings.phase_C_max_stim_angle
                + "\n - phase_C_angular_acceleration : " + this.CurrentSettings.phase_C_angular_acceleration
                + "\n - phase_C_angular_saturation_speed : " + this.CurrentSettings.phase_C_angular_saturation_speed
                + "\n - phase_D_duration : " + this.CurrentSettings.phase_D_duration
            );

            // write the randomized scenario/pattern as result for convenience
            arg.Trial.result["scenario"] = ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            arg.Trial.result["pattern"] = arg.Trial.settings.GetString("current_pattern");
           

            // activate world, Active seat, HOTAS
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor);
            
            // base call
            base.onExperimentTrialBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : end"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => { await this.SetState( new Phase0(this) ); },
                async () => { await this.SetState( new PhaseA(this) ); },
                async () => { await this.SetState( new PhaseB(this) ); },
                async () => { await this.SetState( new PhaseC(this) ); },
                async () => { await this.SetState( new PhaseD(this) ); }
            );
            
        } /* onExperimentTrialBegin() */

        public override void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : begin"
            );

            // send event to active chair over CAN bus
            (
                backend.ApollonBackendManager.Instance.GetValidHandle(
                    backend.ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle
                ) as backend.handle.ApollonActiveSeatHandle
            ).EndTrial();

            //// stop audio recording & save it, if any available...
            //if (UnityEngine.Microphone.devices.Length != 0)
            //{

            //    UnityEngine.Microphone.End(null);
            //    common.ApollonWavRecorder recorder = new common.ApollonWavRecorder();
            //    recorder.Save(
            //        ApollonExperimentManager.Instance.Session.FullPath
            //        + string.Format(
            //            "/{0}_{1}_T{2:000}.wav",
            //            "audioClip",
            //            "DefaultMicrophone",
            //            ApollonExperimentManager.Instance.Session.currentTrialNum
            //        ),
            //        this.CurrentResults.user_clip
            //    );

            //} /* if() */

            //// log
            //UnityEngine.Debug.Log(
            //    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : audio recording stopped, writing result"
            //);

            // write result
            ApollonExperimentManager.Instance.Trial.result["user_stim_host_timestamp"] = this.CurrentResults.user_stim_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_stim_unity_timestamp"] = this.CurrentResults.user_stim_unity_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_response"] = this.CurrentResults.user_response;
            ApollonExperimentManager.Instance.Trial.result["user_perception_host_timestamp"] = this.CurrentResults.user_perception_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_perception_unity_timestamp"] = this.CurrentResults.user_perception_unity_timestamp;

            // base call
            base.onExperimentTrialEnd(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class ApollonAgencyAndTBWExperimentProfile */

} /* } Labsim.apollon.experiment.profile */
