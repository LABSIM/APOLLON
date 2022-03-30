using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileProfile 
        : Labsim.apollon.experiment.ApollonAbstractExperimentFiniteStateMachine< TactileProfile >
    {    

        // Ctor
        public TactileProfile()
            : base()
        {
            // default profile
            this.m_profileID = Labsim.apollon.experiment.ApollonExperimentManager.ProfileIDType.Tactile;
        }

        #region settings/result
        
        public class Settings
        {
            public enum ScenarioIDType
            {

                [System.ComponentModel.Description("Undefined")]
                Undefined = 0,

                [System.ComponentModel.Description("Temporal")]
                TemporalOnly,

                [System.ComponentModel.Description("Spatial")]
                SpatialOnly,

                [System.ComponentModel.Description("Spatio-Temporal")]
                SpatioTemporal

            } /* enum */

            public enum PatternIDType
            {

                [System.ComponentModel.Description("Undefined")]
                Undefined = 0,

                [System.ComponentModel.Description("CC")]
                CC,

                [System.ComponentModel.Description("CV")]
                CV,

                [System.ComponentModel.Description("VV")]
                VV,

                [System.ComponentModel.Description("VC")]
                VC

            } /* enum */

            public bool bIsActive;
            public bool bIsTryCatch;

            public ScenarioIDType scenario_type;

            public PatternIDType phase_C_stim_pattern = PatternIDType.Undefined;

            public float
                phase_A_duration,
                phase_B_begin_stim_timeout_lower_bound,
                phase_B_begin_stim_timeout_upper_bound,
                phase_C_duration,
                phase_D_duration;
            

        } /* class Settings */

        public class Results
        {

            public class DefaultPhaseTimingResults
            {

                #region timing_*

                public float 
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;
                
                #endregion

            } /* class DefaultPhaseTimingResults */

            public class PhaseEResult 
                : DefaultPhaseTimingResults
            {

                #region user_*

                public struct Touchpoint 
                {
                    float x, y, unity_timestamp;
                    string host_timestamp;
                }

                public System.Collections.Generic.List<Touchpoint> user_response = new System.Collections.Generic.List<Touchpoint>();

                #endregion

            } /* class PhaseEResult */

            public DefaultPhaseTimingResults
                phase_A_results = new DefaultPhaseTimingResults(),
                phase_B_results = new DefaultPhaseTimingResults(),
                phase_C_results = new DefaultPhaseTimingResults(),
                phase_D_results = new DefaultPhaseTimingResults();
            
            public PhaseEResult phase_E_results = new PhaseEResult();


        } /* class Results */

        // properties
        public Settings CurrentSettings { get; } = new Settings();
        public Results CurrentResults { get; set; } = new Results();

        #endregion

        #region abstract implementation

        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" 
                + Labsim.apollon.ApollonEngine.GetEnumDescription(this.ID) 
                + "]\n" 
                + Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + " | "
                + Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.phase_C_stim_pattern)
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return "";

        } /* getCurrentCounterStatusInfo() */

        public async override void onExperimentSessionBegin(object sender, Labsim.apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentSessionBegin() : begin"
            );

            // fade in
            await this.DoFadeIn(2500.0f, false);

            // deactivate default DB & activate room setup
            var we_behaviour    
                 = Labsim.apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    Labsim.apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as Labsim.apollon.gameplay.element.ApollonWorldElementBehaviour;
            we_behaviour.References["DBTag_Default"].SetActive(false);
            we_behaviour.References["DBTag_Room"].SetActive(true);
            we_behaviour.References["DBTag_ExoFrontend"].SetActive(true);

            // base call
            base.onExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void onExperimentSessionEnd(object sender, Labsim.apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.onExperimentSessionEnd(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentSessionEnd() : end"
            );

        } /* onExperimentSessionEnd() */

        public override async void onExperimentTrialBegin(object sender, Labsim.apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
             // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentTrialBegin() : begin"
            );

            // temporary string
            string log = "";

            // extract current duration settings
            this.CurrentSettings.bIsTryCatch                            = arg.Trial.settings.GetBool("is_catch_try_condition");
            this.CurrentSettings.bIsActive                              = arg.Trial.settings.GetBool("is_active_condition");
            this.CurrentSettings.phase_A_duration                       = arg.Trial.settings.GetFloat("phase_A_duration_ms");
            this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound = arg.Trial.settings.GetFloatList("phase_B_begin_stim_timeout_ms")[0];
            this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound = arg.Trial.settings.GetFloatList("phase_B_begin_stim_timeout_ms")[1];
            this.CurrentSettings.phase_C_duration                       = arg.Trial.settings.GetFloat("phase_C_duration_ms");
            this.CurrentSettings.phase_D_duration                       = arg.Trial.settings.GetFloat("phase_D_duration_ms");

            // scenario
            switch (arg.Trial.settings.GetString("scenario_name"))
            {

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.TemporalOnly),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.TemporalOnly;
                    break;
                }

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.SpatialOnly),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.SpatialOnly;
                    break;
                }

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.SpatioTemporal),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.SpatioTemporal;
                    break;
                }
                
                default:
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.Undefined;
                    break;
                }

            } /* switch() */

            // current pattern
            switch (arg.Trial.settings.GetString("phase_C_stim_pattern_name"))
            {

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.PatternIDType.VV),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.phase_C_stim_pattern = Settings.PatternIDType.VV;
                    break;
                }

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.PatternIDType.VC),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.phase_C_stim_pattern = Settings.PatternIDType.VC;
                    break;
                }

                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.PatternIDType.CC),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.phase_C_stim_pattern = Settings.PatternIDType.CC;
                    break;
                }
                
                case string param when param.Equals(
                    Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.PatternIDType.CV),
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    this.CurrentSettings.phase_C_stim_pattern = Settings.PatternIDType.CV;
                    break;
                }

                default:
                {
                    this.CurrentSettings.phase_C_stim_pattern = Settings.PatternIDType.Undefined;
                    break;
                }

            } /* switch() */

            // log settings
            log +="\n - scenario_name : " + Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + "\n - phase_A_duration : " + this.CurrentSettings.phase_A_duration 
                + "\n - phase_B_begin_stim_timeout_lower_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_lower_bound 
                + "\n - phase_B_begin_stim_timeout_upper_bound : " + this.CurrentSettings.phase_B_begin_stim_timeout_upper_bound 
                + "\n - phase_C_duration : " + this.CurrentSettings.phase_C_duration 
                + "\n - phase_C_stim_pattern : " + Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.phase_C_stim_pattern)
                + "\n - phase_D_duration : " + this.CurrentSettings.phase_D_duration;

            // clean response arrays
            this.CurrentResults.phase_E_results.user_response.Clear();
            
            // log the final result
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentTrialBegin() : found current settings "
                + log
            );

            // activate gameplay element
            Labsim.apollon.gameplay.ApollonGameplayManager.Instance.setActive(Labsim.apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);

            // base call
            base.onExperimentTrialBegin(sender, arg);

            // fade out
            await this.DoFadeOut(this._trial_fade_out_duration, false);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentTrialBegin() : end"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => { await this.SetState( new TactilePhaseA(this) ); },
                async () => { await this.SetState( new TactilePhaseB(this) ); },
                async () => { await this.SetState( new TactilePhaseC(this) ); },
                async () => { await this.SetState( new TactilePhaseD(this) ); },
                async () => { await this.SetState( new TactilePhaseE(this) ); },
                async () => { await this.SetState( null ); }
            );
            
        } /* onExperimentTrialBegin() */

        public override async void onExperimentTrialEnd(object sender, Labsim.apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentTrialEnd() : begin"
            );

            // write result
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["scenario"] 
                = Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["pattern"] 
                = Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.phase_C_stim_pattern);
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["active_condition"] 
                = this.CurrentSettings.bIsActive.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] 
                = this.CurrentSettings.bIsTryCatch.ToString();

            // phase A
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_host_timestamp;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_host_timestamp;

            // phase B
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_host_timestamp;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_host_timestamp;

            // phase C
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_host_timestamp;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_host_timestamp;

            // phase D
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_host_timestamp;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_host_timestamp;

            // phase E
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_host_timestamp;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_host_timestamp;
                
            // fade in
            await this.DoFadeIn(this._trial_fade_in_duration, false);

            // inactivate gameplay & frontend
            Labsim.apollon.gameplay.ApollonGameplayManager.Instance.setInactive(Labsim.apollon.gameplay.ApollonGameplayManager.GameplayIDType.All);
            Labsim.apollon.frontend.ApollonFrontendManager.Instance.setInactive(Labsim.apollon.frontend.ApollonFrontendManager.FrontendIDType.All);
           
            // base call
            base.onExperimentTrialEnd(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class TactileProfile */

} /* } Labsim.experiment.tactile */