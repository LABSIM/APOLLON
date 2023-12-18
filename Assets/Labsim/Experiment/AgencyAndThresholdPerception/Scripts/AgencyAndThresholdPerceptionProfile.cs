using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerception
{

    public class AgencyAndThresholdPerceptionProfile 
        : apollon.experiment.ApollonAbstractExperimentFiniteStateMachine< AgencyAndThresholdPerceptionProfile >
    {

        // Ctor
        public AgencyAndThresholdPerceptionProfile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.AgencyAndThresholdPerception;
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
                phase_C_stim_duration,
                phase_C_total_duration,
                phase_D_duration;

            public float[]
                phase_C_angular_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f },
                phase_C_angular_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f },
                phase_C_angular_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f },
                phase_C_linear_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f },
                phase_C_linear_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f },
                phase_C_linear_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };
            
            public bool[]
                phase_C_angular_mandatory_axis = new bool[3] { false, false, false },
                phase_C_linear_mandatory_axis = new bool[3] { false, false, false };

        } /* class Settings */

        public class Results
        {

            public bool
                user_response_B,
                user_response_C;

            public int
                user_command;

            public float
                user_stim_unity_timestamp,
                user_perception_B_unity_timestamp,
                user_perception_C_unity_timestamp;

            public string
                user_stim_host_timestamp,
                user_perception_B_host_timestamp,
                user_perception_C_host_timestamp;

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
        
        #region abstract implementation

        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" 
                + apollon.ApollonEngine.GetEnumDescription(this.ID) 
                + "]\n" 
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + " | "
                + UXF.Session.instance.CurrentTrial.settings.GetString("current_pattern")
                + " | "
                + this.positiveConditionCount
                + "(+)/"
                + this.negativeConditionCount
                + "(-)" 
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return (
                (
                    this.CurrentSettings.bIsActive 
                ) ? ( 
                    UXF.Session.instance.CurrentBlock.number
                    + "/"
                    + UXF.Session.instance.blocks.Count
                    + " | (+)" 
                    + (
                        (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 2) 
                        - this.positiveConditionCount
                    ).ToString("D2")
                    + "/(-)"
                    +(
                        (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 2) 
                        - this.negativeConditionCount
                    ).ToString("D2")
                ) : (
                    ""
                )
            );

        } /* getCurrentCounterStatusInfo() */

        public override void onUpdate(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.onUpdate(sender, arg);

        } /* onUpdate() */

        public async override void onExperimentSessionBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentSessionBegin() : begin"
            );

            // activate all motion system command/sensor
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );

            // fade in
            await this.DoFadeIn(2500.0f, false);

            // deactivate default DB & activate room setup
            var we_behaviour
                 = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as apollon.gameplay.element.ApollonWorldElementBehaviour;
            we_behaviour.References["DBTag_Default"].SetActive(false);
            we_behaviour.References["DBTag_Room"].SetActive(true);
            we_behaviour.References["DBTag_ExoFrontend"].SetActive(true);

            // base call
            base.onExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void onExperimentSessionEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.onExperimentSessionEnd(sender, arg);

            // deactivate all motion system command/sensor
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentSessionEnd() : end"
            );


        } /* onExperimentSessionEnd() */

        public override async void onExperimentTrialBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : begin"
            );

            // // send event to motion system backend
            // (
            //     backend.ApollonBackendManager.Instance.GetValidHandle(
            //         backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
            //     ) as backend.handle.ApollonMotionSystemPS6TM550Handle
            // ).BeginTrial();
        
            // local
            int currentIdx = apollon.experiment.ApollonExperimentManager.Instance.Session.currentTrialNum - 1;

            // activate the active seat entity
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity);

            // inactivate all visual cues through LINQ request
            var we_behaviour
                 = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as apollon.gameplay.element.ApollonWorldElementBehaviour;
            foreach (var vc_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("VCTag_")).Select(kvp => kvp.Value))
            {
                vc_ref.SetActive(false);
            }

            // current scenario
            switch (arg.Trial.settings.GetString("scenario_name"))
            {

                case "visual-only":
                {
                    we_behaviour.References["VCTag_Fan"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisualOnly;
                    // transit to corresponding entity state
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                    ).ConcreteDispatcher.RaiseVisualOnly();
                    break;
                }
                case "vestibular-only":
                {
                    we_behaviour.References["VCTag_Spot"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                    // transit to corresponding entity state
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                    ).ConcreteDispatcher.RaiseVestibularOnly();
                    break;
                }
                case "visuo-vestibular":
                {
                    we_behaviour.References["VCTag_Fan"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
                    // transit to corresponding entity state
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                    ).ConcreteDispatcher.RaiseVisuoVestibular();
                    break;
                }
                default:
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.Undefined;
                    break;
                }

            } /* switch() */

            // extract trial settings
            this.CurrentSettings.bIsTryCatch                                    = arg.Trial.settings.GetBool("is_catch_try_condition");
            this.CurrentSettings.bIsActive                                      = arg.Trial.settings.GetBool("is_active_condition");
            this.CurrentSettings.phase_A_duration                               = arg.Trial.settings.GetFloat("phase_A_duration_ms");
            this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound         = arg.Trial.settings.GetFloatList("phase_B_begin_stim_timeout_ms")[0];
            this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound         = arg.Trial.settings.GetFloatList("phase_B_begin_stim_timeout_ms")[1];
            this.CurrentSettings.phase_C_stim_duration                          = arg.Trial.settings.GetFloat("phase_C_stim_duration_ms");
            this.CurrentSettings.phase_C_total_duration                         = arg.Trial.settings.GetFloat("phase_C_total_duration_ms");
            this.CurrentSettings.phase_C_angular_displacement_limiter           = arg.Trial.settings.GetFloatList("phase_C_angular_displacement_limiter_deg").ToArray();
            this.CurrentSettings.phase_C_angular_acceleration_target            = arg.Trial.settings.GetFloatList("phase_C_angular_acceleration_target_deg_per_s2").ToArray();
            this.CurrentSettings.phase_C_angular_velocity_saturation_threshold  = arg.Trial.settings.GetFloatList("phase_C_angular_velocity_saturation_threshold_deg_per_s").ToArray();
            this.CurrentSettings.phase_C_angular_mandatory_axis                 = arg.Trial.settings.GetBoolList("phase_C_angular_mandatory_axis").ToArray();
            this.CurrentSettings.phase_C_linear_displacement_limiter            = arg.Trial.settings.GetFloatList("phase_C_linear_displacement_limiter_m").ToArray();
            this.CurrentSettings.phase_C_linear_acceleration_target             = arg.Trial.settings.GetFloatList("phase_C_linear_acceleration_target_m_per_s2").ToArray();
            this.CurrentSettings.phase_C_linear_velocity_saturation_threshold   = arg.Trial.settings.GetFloatList("phase_C_linear_velocity_saturation_threshold_m_per_s").ToArray();
            this.CurrentSettings.phase_C_linear_mandatory_axis                  = arg.Trial.settings.GetBoolList("phase_C_linear_mandatory_axis").ToArray();
            this.CurrentSettings.phase_D_duration                               = arg.Trial.settings.GetFloat("phase_D_duration_ms");

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : found current settings with pattern["
                + arg.Trial.settings.GetString("current_pattern")
                + "]"
                + "\n - bIsTryCatch : " + this.CurrentSettings.bIsTryCatch
                + "\n - bIsActive : " + this.CurrentSettings.bIsActive
                + "\n - scenario_name : " + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + "\n - phase_A_duration : " + this.CurrentSettings.phase_A_duration
                + "\n - phase_B_begin_stim_timeout_lower_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound
                + "\n - phase_B_begin_stim_timeout_upper_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound
                + "\n - phase_C_stim_duration : " + this.CurrentSettings.phase_C_stim_duration
                + "\n - phase_C_total_duration : " + this.CurrentSettings.phase_C_total_duration
                + "\n - phase_C_angular_displacement_limiter : [" + System.String.Join(",",this.CurrentSettings.phase_C_angular_displacement_limiter) + "]"
                + "\n - phase_C_angular_acceleration_target : [" + System.String.Join(",",this.CurrentSettings.phase_C_angular_acceleration_target) + "]"
                + "\n - phase_C_angular_velocity_saturation_threshold : [" + System.String.Join(",",this.CurrentSettings.phase_C_angular_velocity_saturation_threshold) + "]"
                + "\n - phase_C_angular_mandatory_axis : [" + System.String.Join(",",this.CurrentSettings.phase_C_angular_mandatory_axis) + "]"
                + "\n - phase_C_linear_displacement_limiter : [" + System.String.Join(",",this.CurrentSettings.phase_C_linear_displacement_limiter) + "]"
                + "\n - phase_C_linear_acceleration_target : [" + System.String.Join(",",this.CurrentSettings.phase_C_linear_acceleration_target) + "]"
                + "\n - phase_C_linear_velocity_saturation_threshold : [" + System.String.Join(",",this.CurrentSettings.phase_C_linear_velocity_saturation_threshold) + "]"
                + "\n - phase_C_linear_mandatory_axis : [" + System.String.Join(",",this.CurrentSettings.phase_C_linear_mandatory_axis) + "]"
                + "\n - phase_D_duration : " + this.CurrentSettings.phase_D_duration
            );
           
            // activate world element & contriol system
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl);

            // base call
            base.onExperimentTrialBegin(sender, arg);

            // fade out
            await this.DoFadeOut(this._trial_fade_out_duration, false);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialBegin() : end " + UnityEngine.Time.fixedTime
            );

            // build protocol
            await this.DoRunProtocol(
                async () => { await this.SetState( new AgencyAndThresholdPerceptionPhase0(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionPhaseA(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionPhaseB(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionPhaseC(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionPhaseD(this) ); },
                async () => { await this.SetState( null ); }
            );
            
        } /* onExperimentTrialBegin() */

        public override async void onExperimentTrialEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : begin"
            );
            
            // write the randomized scenario/pattern/conditions(s) as result for convenience
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["scenario"] = apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["pattern"] = apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetString("current_pattern");
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["active_condition"] = this.CurrentSettings.bIsActive.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] = this.CurrentSettings.bIsTryCatch.ToString();

            // write result
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_command"] = this.CurrentResults.user_command;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_stim_host_timestamp"] = this.CurrentResults.user_stim_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_stim_unity_timestamp"] = this.CurrentResults.user_stim_unity_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response_B"] = this.CurrentResults.user_response_B;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_perception_B_host_timestamp"] = this.CurrentResults.user_perception_B_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_perception_B_unity_timestamp"] = this.CurrentResults.user_perception_B_unity_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response_C"] = this.CurrentResults.user_response_C;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_perception_C_host_timestamp"] = this.CurrentResults.user_perception_C_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_perception_C_unity_timestamp"] = this.CurrentResults.user_perception_C_unity_timestamp;

            // fade in
            await this.DoFadeIn(this._trial_fade_in_duration, false);

            // // send event to motion system backend
            // (
            //     backend.ApollonBackendManager.Instance.GetValidHandle(
            //         backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
            //     ) as backend.handle.ApollonMotionSystemPS6TM550Handle
            // ).EndTrial();

            // get active seat bridge
            var seat_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                ) as apollon.gameplay.entity.ApollonActiveSeatEntityBridge;

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // base call
            base.onExperimentTrialEnd(sender, arg);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class AgencyAndTBWExperimentProfile */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
