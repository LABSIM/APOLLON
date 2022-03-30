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
        
            [System.AttributeUsage(System.AttributeTargets.Field)]  
            private class JSONSettingsAttribute 
                : System.Attribute  
            {

                public string name;
            
                public JSONSettingsAttribute(string settings, string phase = "", string unit = "", string separator = "_")  
                {  
                    if(!string.IsNullOrEmpty(phase)) {
                        this.name += phase + separator;
                    }
                    this.name += settings;
                    if(!string.IsNullOrEmpty(unit)) {
                        this.name += separator + unit;
                    }
                }  
            
            } 

            private string GetJSONSettingsAttributeName<T>(string field_name)
            {
                try 
                {
                    return ((JSONSettingsAttribute)System.Attribute.GetCustomAttribute(typeof(T).GetField(field_name), typeof(JSONSettingsAttribute))).name;
                }
                catch(System.Exception ex)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Info: </color> TactileProfile.Settings.GetJSONSettingsAttributeName() : failed to extract ("
                        + field_name
                        + ") with error ["
                        + ex.Message
                        + "]"
                    );

                    return "Error";

                } /* try */
            }    

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

            [JSONSettingsAttribute("current_pattern")]
            public string pattern_type;

            [JSONSettingsAttribute("is_active_condition")]
            public bool bIsActive;

            [JSONSettingsAttribute("is_catch_try_condition")]
            public bool bIsTryCatch;

            [JSONSettingsAttribute("scenario_name")]
            public ScenarioIDType scenario_type;

            public class PhaseASettings
            {

                [JSONSettingsAttribute(phase:"phase_A", settings:"duration", unit:"ms")]
                public float duration;

            } /* class PhaseASettings */

            public class PhaseBSettings
            {

                [JSONSettingsAttribute(phase:"phase_B", settings:"begin_stim_timeout", unit:"ms")]
                public float 
                    begin_stim_timeout_lower_bound,
                    begin_stim_timeout_upper_bound;

            } /* PhaseBSettings */ 

            public class PhaseCSettings
            {

                [JSONSettingsAttribute(phase:"phase_C", settings:"stim_pattern_name")]
                public PatternIDType stim_pattern = PatternIDType.Undefined;

                [JSONSettingsAttribute(phase:"phase_C", settings:"total_duration", unit:"ms")]
                public float total_duration;

            } /* PhaseCSettings */ 

            public class PhaseDSettings
            {

                [JSONSettingsAttribute(phase:"phase_D", settings:"duration", unit:"ms")]
                public float duration;

            } /* PhaseDSettings */ 

            public PhaseASettings phase_A_settings = new PhaseASettings(); 
            public PhaseBSettings phase_B_settings = new PhaseBSettings();
            public PhaseCSettings phase_C_settings = new PhaseCSettings();
            public PhaseDSettings phase_D_settings = new PhaseDSettings();

            public bool ImportUXFSettings(UXF.Settings settings)
            {

                // encapsulate
                try
                {

                    // extract general trial settings
                    this.pattern_type 
                        = settings.GetString(
                            this.GetJSONSettingsAttributeName<Settings>("pattern_type")
                        );
                    this.bIsTryCatch 
                        = settings.GetBool(
                            this.GetJSONSettingsAttributeName<Settings>("bIsTryCatch")
                        );
                    this.bIsActive 
                        = settings.GetBool(
                            this.GetJSONSettingsAttributeName<Settings>("bIsActive")
                        );
                    
                    // current scenario
                    switch(
                        settings.GetString(
                            this.GetJSONSettingsAttributeName<Settings>("scenario_type")
                        )
                    ) {
                        
                        // spatial only
                        case string param when param.Equals(
                            Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.SpatialOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.SpatialOnly;
                            break;
                        }

                        // temporal only
                        case string param when param.Equals(
                            Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.TemporalOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.TemporalOnly;
                            break;
                        }

                        // spatio-temporal
                        case string param when param.Equals(
                            Labsim.apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.SpatioTemporal),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.SpatioTemporal;
                            break;
                        }

                        // default
                        default: 
                        {
                            this.scenario_type = Settings.ScenarioIDType.Undefined;
                            break;
                        }

                    } /* switch() */

                    // phase A
                    this.phase_A_settings.duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("duration")
                        );

                    // phase B
                    this.phase_B_settings.begin_stim_timeout_lower_bound
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("begin_stim_timeout")
                        )[0];
                    this.phase_B_settings.begin_stim_timeout_upper_bound
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("begin_stim_timeout")
                        )[1];

                    // phase C
                    this.phase_C_settings.total_duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("total_duration")
                        );
                    
                    // phase D
                    this.phase_D_settings.duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("duration")
                        );

                } 
                catch(System.Exception ex)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Info: </color> TactileProfile.Settings.ImportUXFSettings() : failed to import settings with error ["
                        + ex.Message
                        + "]"
                    );

                    // failure
                    return false;

                } /* try */

                // success
                return true;

            } /* ImportUXFSettings */

            public void LogUXFSettings()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileProfile.Settings.LogUXFSettings() : imported current settings with pattern["
                        + this.pattern_type
                    + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings>("bIsTryCatch") 
                        + " : " 
                        + this.bIsTryCatch
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings>("bIsActive") 
                        + " : " 
                        + this.bIsActive
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings>("scenario_type") 
                        + " : " 
                        + Labsim.apollon.ApollonEngine.GetEnumDescription(this.scenario_type)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("duration") 
                        + " : " 
                        + this.phase_A_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("begin_stim_timeout") 
                        + " : [" 
                            + this.phase_B_settings.begin_stim_timeout_lower_bound
                            + ","
                            + this.phase_B_settings.begin_stim_timeout_upper_bound
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("stim_pattern") 
                        + " : " 
                        + Labsim.apollon.ApollonEngine.GetEnumDescription(this.phase_C_settings.stim_pattern)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("total_duration") 
                        + " : " 
                        + this.phase_C_settings.total_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseDSettings>("duration") 
                        + " : " 
                        + this.phase_D_settings.duration
                );

            } /* LogUXFSettings() */

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
                    public float x, y, unity_timestamp;
                    public string host_timestamp;
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
                + Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.phase_C_settings.stim_pattern)
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

            // import current trial settings & log on completion
            if(this.CurrentSettings.ImportUXFSettings(arg.Trial.settings))
            {
                this.CurrentSettings.LogUXFSettings();
            }

            // clean response arrays
            this.CurrentResults.phase_E_results.user_response.Clear();

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
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["pattern"] 
                = this.CurrentSettings.pattern_type;
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["active_condition"] 
                = this.CurrentSettings.bIsActive.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] 
                = this.CurrentSettings.bIsTryCatch.ToString();
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["scenario"] 
                = Labsim.apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);

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
            Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response"] 
                = (
                    "[" 
                    + System.String.Join(
                        ";", 
                        this.CurrentResults.phase_E_results.user_response.Select(
                            touchpoint 
                                => (
                                    "[" 
                                    + touchpoint.x 
                                    + ";"
                                    + touchpoint.y 
                                    + ";" 
                                    + touchpoint.unity_timestamp
                                    + ";"
                                    + touchpoint.host_timestamp
                                    + "]"
                                )
                        ) 
                    ) 
                    + "]"
                );
                
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