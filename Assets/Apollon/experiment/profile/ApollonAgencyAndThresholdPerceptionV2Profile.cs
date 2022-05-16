using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.profile
{

    public class ApollonAgencyAndThresholdPerceptionV2Profile 
        : ApollonAbstractExperimentFiniteStateMachine< ApollonAgencyAndThresholdPerceptionV2Profile >
    {

        // Ctor
        public ApollonAgencyAndThresholdPerceptionV2Profile()
            : base()
        {
            // default profile
            this.m_profileID = ApollonExperimentManager.ProfileIDType.AgencyAndThresholdPerceptionV2;
        }

        // fast hack
        public uint
            strongConditionCount = 0,
            weakConditionCount = 0;

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
                        "<color=Red>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.Settings.GetJSONSettingsAttributeName() : failed to extract ("
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

                [System.ComponentModel.Description("Visual-Only")]
                VisualOnly,

                [System.ComponentModel.Description("Vestibular-Only")]
                VestibularOnly,

                [System.ComponentModel.Description("Visuo-Vestibular")]
                VisuoVestibular

            } /* enum */
            
            public enum IntensityIDType
            {

                [System.ComponentModel.Description("Undefined")]
                Undefined = 0,

                [System.ComponentModel.Description("Weak")]
                Weak = 1,

                [System.ComponentModel.Description("Strong")]
                Strong = 2

            } /* enum */

            [JSONSettingsAttribute("current_pattern")]
            public string pattern_type;

            [JSONSettingsAttribute("is_active_condition")]
            public bool bIsActive;

            [JSONSettingsAttribute("is_catch_try_condition")]
            public bool bIsTryCatch;
        
            [JSONSettingsAttribute("scenario_name")]
            public ScenarioIDType scenario_type;
            
            [JSONSettingsAttribute("passive_intensity_name")]
            public IntensityIDType passive_intensity_type;

            public class Phase0Settings
            {

                [JSONSettingsAttribute(phase:"phase_0", settings:"duration", unit:"ms")]
                public float duration;

            } /* class Phase0Settings */

            public class PhaseASettings
            {

                [JSONSettingsAttribute(phase:"phase_A", settings:"confirm_duration", unit:"ms")]
                public float confirm_duration;

                [JSONSettingsAttribute(phase:"phase_A", settings:"response_max_duration", unit:"ms")]
                public float response_max_duration;

            } /* class PhaseASettings */

            public class PhaseBSettings
            {

                [JSONSettingsAttribute(phase:"phase_B", settings:"stim_duration", unit:"ms")]
                public float stim_duration;

                [JSONSettingsAttribute(phase:"phase_B", settings:"total_duration", unit:"ms")]
                public float total_duration;


                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_strong_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] angular_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_strong_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] angular_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_strong_displacement_limiter", unit:"deg")]
                public float[] angular_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_weak_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] angular_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_weak_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] angular_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };
                
                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_weak_displacement_limiter", unit:"deg")]
                public float[] angular_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_strong_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] linear_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_strong_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] linear_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_strong_displacement_limiter", unit:"m")]
                public float[] linear_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_weak_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] linear_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_weak_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] linear_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_weak_displacement_limiter", unit:"m")]
                public float[] linear_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"angular_mandatory_axis")]
                public bool[] angular_mandatory_axis = new bool[3] { false, false, false };

                [JSONSettingsAttribute(phase:"phase_B", settings:"linear_mandatory_axis")]
                public bool[] linear_mandatory_axis = new bool[3] { false, false, false };

            } /* PhaseBSettings */ 
            
            public class PhaseCSettings
            {

                [JSONSettingsAttribute(phase:"phase_C", settings:"inter_stim_timeout", unit:"ms")]
                public float[] inter_stim_timeout = new float [2];

            } /* PhaseCSettings */ 

            public class PhaseFSettings
            {

                [JSONSettingsAttribute(phase:"phase_F", settings:"stim_duration", unit:"ms")]
                public float stim_duration;

                [JSONSettingsAttribute(phase:"phase_F", settings:"total_duration", unit:"ms")]
                public float total_duration;


                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_strong_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] angular_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_strong_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] angular_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_strong_displacement_limiter", unit:"deg")]
                public float[] angular_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_weak_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] angular_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_weak_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] angular_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };
                
                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_weak_displacement_limiter", unit:"deg")]
                public float[] angular_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_strong_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] linear_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_strong_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] linear_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_strong_displacement_limiter", unit:"m")]
                public float[] linear_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_weak_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] linear_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_weak_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] linear_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_weak_displacement_limiter", unit:"m")]
                public float[] linear_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_F", settings:"angular_mandatory_axis")]
                public bool[] angular_mandatory_axis = new bool[3] { false, false, false };

                [JSONSettingsAttribute(phase:"phase_F", settings:"linear_mandatory_axis")]
                public bool[] linear_mandatory_axis = new bool[3] { false, false, false };

            } /* PhaseFSettings */ 
            
            public class PhaseGSettings
            {

                [JSONSettingsAttribute(phase:"phase_G", settings:"duration", unit:"ms")]
                public float duration;

            } /* PhaseGSettings */

            public class PhaseJSettings
            {

                [JSONSettingsAttribute(phase:"phase_J", settings:"duration", unit:"ms")]
                public float duration;

            } /* PhaseJSettings */

            public Phase0Settings phase_0_settings = new Phase0Settings(); 
            public PhaseASettings phase_A_settings = new PhaseASettings(); 
            public PhaseBSettings phase_B_settings = new PhaseBSettings();
            public PhaseCSettings phase_C_settings = new PhaseCSettings();
            public PhaseFSettings phase_F_settings = new PhaseFSettings();
            public PhaseGSettings phase_G_settings = new PhaseGSettings();
            public PhaseJSettings phase_J_settings = new PhaseJSettings();

            public bool ImportUXFSettings(UXF.Settings settings)
            {

                // encapsulate
                try
                {

                    // extract general trial settings
                    this.bIsTryCatch 
                        = settings.GetBool(
                            this.GetJSONSettingsAttributeName<Settings>("bIsTryCatch")
                        );
                    this.bIsActive 
                        = settings.GetBool(
                            this.GetJSONSettingsAttributeName<Settings>("bIsActive")
                        );
                    this.pattern_type 
                        = settings.GetString(
                            this.GetJSONSettingsAttributeName<Settings>("pattern_type")
                        );
                    
                    // current scenario
                    switch(
                        settings.GetString(
                            this.GetJSONSettingsAttributeName<Settings>("scenario_type")
                        )
                    ) {
                        
                        // vestibular only
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VestibularOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                            break;
                        }

                        // visual only
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VisualOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.VisualOnly;
                            break;
                        }
                        
                        // visuo-vestibular
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VisuoVestibular),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
                            break;
                        }

                        // default
                        default: 
                        {
                            this.scenario_type = Settings.ScenarioIDType.Undefined;
                            break;
                        }

                    } /* switch() */

                    // current passive intensity
                    switch(
                        settings.GetString(
                            this.GetJSONSettingsAttributeName<Settings>("passive_intensity_type")
                        )
                    ) {
                        
                        // Strong
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.IntensityIDType.Strong),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.passive_intensity_type = Settings.IntensityIDType.Strong;
                            break;
                        }

                        // Weak
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.IntensityIDType.Weak),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.passive_intensity_type = Settings.IntensityIDType.Weak;
                            break;
                        }

                        // default
                        default: 
                        {
                            this.passive_intensity_type = Settings.IntensityIDType.Undefined;
                            break;
                        }

                    } /* switch() */

                    // phase 0
                    this.phase_0_settings.duration 
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("duration")
                        );

                    // phase A
                    this.phase_A_settings.confirm_duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("confirm_duration")
                        );
                    this.phase_A_settings.response_max_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("response_max_duration")
                        );
                    
                    // phase B
                    this.phase_B_settings.stim_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("stim_duration")
                        );
                    this.phase_B_settings.total_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("total_duration")
                        );
                    this.phase_B_settings.angular_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.angular_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.angular_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.angular_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.angular_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.angular_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.angular_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_mandatory_axis")
                        ).ToArray();
                    this.phase_B_settings.linear_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.linear_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.linear_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.linear_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.linear_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.linear_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.linear_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_mandatory_axis")
                        ).ToArray();
                    
                    // phase C
                    this.phase_C_settings.inter_stim_timeout
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("inter_stim_timeout")
                        ).ToArray();
                    
                    // phase B
                    this.phase_F_settings.stim_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("stim_duration")
                        );
                    this.phase_F_settings.total_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("total_duration")
                        );
                    this.phase_F_settings.angular_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_F_settings.angular_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_F_settings.angular_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_F_settings.angular_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_F_settings.angular_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_F_settings.angular_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_F_settings.angular_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_mandatory_axis")
                        ).ToArray();
                    this.phase_F_settings.linear_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_F_settings.linear_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_F_settings.linear_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_F_settings.linear_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_F_settings.linear_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_F_settings.linear_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_F_settings.linear_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_mandatory_axis")
                        ).ToArray();
                    
                    // phase G
                    this.phase_G_settings.duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseGSettings>("duration")
                        );

                    // phase J
                    this.phase_J_settings.duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseJSettings>("duration")
                        );

                } 
                catch(System.Exception ex)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.Settings.ImportUXFSettings() : failed to import settings with error ["
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.Settings.LogUXFSettings() : imported current settings with pattern["
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
                        + ApollonEngine.GetEnumDescription(this.scenario_type)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings>("passive_intensity_type") 
                        + " : " 
                        + ApollonEngine.GetEnumDescription(this.passive_intensity_type)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("duration") 
                        + " : " 
                        + this.phase_0_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("confirm_duration") 
                        + " : " 
                        + this.phase_A_settings.confirm_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("response_max_duration") 
                        + " : " 
                        + this.phase_A_settings.response_max_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("stim_duration") 
                        + " : " 
                        + this.phase_B_settings.stim_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("total_duration") 
                        + " : " 
                        + this.phase_B_settings.total_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("angular_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.angular_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("linear_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.linear_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("inter_stim_timeout") 
                        + " : [" 
                            + this.phase_C_settings.inter_stim_timeout[0]
                            + ","
                            + this.phase_C_settings.inter_stim_timeout[1]
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("stim_duration") 
                        + " : " 
                        + this.phase_F_settings.stim_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("total_duration") 
                        + " : " 
                        + this.phase_F_settings.total_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("angular_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.angular_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("linear_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_F_settings.linear_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseGSettings>("duration") 
                        + " : " 
                        + this.phase_G_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseJSettings>("duration") 
                        + " : " 
                        + this.phase_J_settings.duration
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

            public class PhaseAResults 
                : DefaultPhaseTimingResults
            {

                #region user_*

                public int
                    user_command;

                public float
                    user_latency_unity_timestamp,
                    user_measured_latency,
                    user_randomized_stim1_latency;
                
                public string
                    user_latency_host_timestamp;

                #endregion

            } /* class PhaseAResult */

            public class PhaseEResults 
                : DefaultPhaseTimingResults
            {

                #region user_*

                public float
                    user_randomized_stim2_latency;

                #endregion

            } /* class PhaseEResults */

            public class PhaseHResults
                : DefaultPhaseTimingResults
            {

                #region user_*

                public float
                    user_response;

                #endregion

            } /* class PhaseHResult */

            public class PhaseIResults
                : DefaultPhaseTimingResults
            {

                #region user_*

                public float
                    user_confidence;

                #endregion

            } /* class PhaseIResult */

            public DefaultPhaseTimingResults
                phase_0_results = new DefaultPhaseTimingResults(),
                phase_B_results = new DefaultPhaseTimingResults(),
                phase_C_results = new DefaultPhaseTimingResults(),
                phase_D_results = new DefaultPhaseTimingResults(),
                phase_F_results = new DefaultPhaseTimingResults(),
                phase_G_results = new DefaultPhaseTimingResults(),
                phase_J_results = new DefaultPhaseTimingResults();

            public PhaseAResults phase_A_results = new PhaseAResults();
            public PhaseEResults phase_E_results = new PhaseEResults();
            public PhaseHResults phase_H_results = new PhaseHResults();
            public PhaseIResults phase_I_results = new PhaseIResults();

        } /* class Results */

        // properties
        public Settings CurrentSettings { get; } = new Settings();
        public Results CurrentResults { get; set; } = new Results();

        #endregion
        
        #region latency bucket mechanism

        private static readonly uint _latency_default_BucketSize = 5;
        private static readonly float _latency_default_timeout_lower_bound = 300.0f;
        private static readonly float _latency_default_timeout_upper_bound = 1500.0f;

        private uint m_currentInitialDefaultLatencyValueCount = 0;
        private System.Collections.Generic.Queue<float> m_currentLatencyBucket = null;
        private System.Collections.Generic.Queue<float> CurrentLatencyBucket 
        {

            get 
            {
                
                // is it initialized
                if(this.m_currentLatencyBucket == null)
                {

                    // is there a default user value from previous experiment ?
                    if(ApollonExperimentManager.Instance.Session.participantDetails["initial_latency_bucket"].ToString() != "")
                    {

                        // format == [value_0,value_1,...,value_N]
                        
                        // extract raw string
                        var raw_string 
                            = ApollonExperimentManager.Instance.Session.participantDetails["initial_latency_bucket"]
                            .ToString();
                        
                        // then pop first & last element & split from "," separator & convert to a float array
                        float[] raw_data 
                            = System.Array.ConvertAll(
                                raw_string.Substring(1, (raw_string.Length - 2)).Split(','),
                                float.Parse
                            );

                        // then initialize from raw & set initial value counter to 0
                        this.m_currentInitialDefaultLatencyValueCount = 0;
                        this.m_currentLatencyBucket = new System.Collections.Generic.Queue<float>(raw_data);

                    }
                    else
                    {
                        
                        // empty instance
                        this.m_currentLatencyBucket = new System.Collections.Generic.Queue<float>();

                        // uses default settings
                        this.m_currentInitialDefaultLatencyValueCount = _latency_default_BucketSize;
                        for(uint i = 0; i < this.m_currentInitialDefaultLatencyValueCount; ++i)
                        {
                            
                            this.m_currentLatencyBucket.Enqueue(
                                UnityEngine.Random.Range(
                                    _latency_default_timeout_lower_bound,
                                    _latency_default_timeout_upper_bound
                                )
                            );

                        } /* for() */

                    } /* if()*/

                } /* if() */

                // finally
                return this.m_currentLatencyBucket;
            
            } /* get */

        } /* property */

        public void AddUserLatencyToBucket(float new_latency)
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.AddUserLatencyToBucket() : adding new latency["
                    + new_latency
                + "] to current bucket["
                    + System.String.Join(",",this.CurrentLatencyBucket)
                + "]"
            );

            // iff. there is any remaining default value
            if(this.m_currentInitialDefaultLatencyValueCount > 0)
            {
            
                // firstly, pop last default element & decrement current default latency initializer
                this.CurrentLatencyBucket.Dequeue();
                --this.m_currentInitialDefaultLatencyValueCount;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.AddUserLatencyToBucket() : removing last default value, remaining default count["
                        + this.m_currentInitialDefaultLatencyValueCount
                    + "]"
                );

            } /* if() */
            
            // whatever, add new user latency to bucket
            this.CurrentLatencyBucket.Enqueue(new_latency);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.AddUserLatencyToBucket() : new latency bucket ["
                    + System.String.Join(",",this.CurrentLatencyBucket)
                + "]"
            );

        } /* AddUserLatencyToBucket() */

        public float GetRandomLatencyFromBucket()
        {
            
            // get a latency from random index in the current bucket
            return UnityEngine.Mathf.Clamp(
                this.CurrentLatencyBucket.ToList()[UnityEngine.Random.Range(0, this.CurrentLatencyBucket.Count - 1)],
                _latency_default_timeout_lower_bound,
                _latency_default_timeout_upper_bound 
            );

        } /* GetRandomLatencyFromBucket() */

        #endregion
        
        #region abstract implementation

        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" 
                + ApollonEngine.GetEnumDescription(this.ID) 
                + "]\n" 
                + ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
                + " | "
                + this.CurrentSettings.pattern_type
                + " | "
                + this.strongConditionCount
                + "(forte)/"
                + this.weakConditionCount
                + "(faible)" 
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return (
                (this.CurrentSettings.bIsActive) 
                ? ( 
                    (
                        (UXF.Session.instance.blocks.Count > 1) 
                            ? (UXF.Session.instance.CurrentBlock.number + "/" + UXF.Session.instance.blocks.Count + " | ")
                            : ""
                    )
                    + "faible("
                    + (
                        (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                        - this.weakConditionCount
                    ).ToString("D2")
                    + ")/forte(" 
                    + (
                        (UXF.Session.instance.CurrentBlock.trials.ToList().Count / 4) 
                        - this.strongConditionCount
                    ).ToString("D2")
                    + ")"
                )
                : ""
            );

        } /* getCurrentCounterStatusInfo() */

        public System.String CurrentInstruction { get; set; } = "";
        protected override System.String getCurrentInstructionStatusInfo()
        {

            // simply
            return this.CurrentInstruction;

        } /* getCurrentInstructionStatusInfo() */

        public override void onUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.onUpdate(sender, arg);

        } /* onUpdate() */

        public async override void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentSessionBegin() : begin"
            );

            // activate all motion system command/sensor
            gameplay.ApollonGameplayManager.Instance.setActive(
                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            gameplay.ApollonGameplayManager.Instance.setActive(
                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            gameplay.ApollonGameplayManager.Instance.setActive(
                gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );

            // fade in
            await this.DoFadeIn(2500.0f, false);

            // deactivate default DB & activate room setup
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;
            we_behaviour.References["DBTag_Default"].SetActive(false);
            we_behaviour.References["DBTag_Room"].SetActive(true);
            we_behaviour.References["DBTag_ExoFrontend"].SetActive(true);

            // base call
            base.onExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.onExperimentSessionEnd(sender, arg);

            // deactivate all motion system command/sensor
            gameplay.ApollonGameplayManager.Instance.setInactive(
                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            gameplay.ApollonGameplayManager.Instance.setInactive(
                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            gameplay.ApollonGameplayManager.Instance.setInactive(
                gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentSessionEnd() : end"
            );


        } /* onExperimentSessionEnd() */

        public override async void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentTrialBegin() : begin"
            );

            // // send event to motion system backend
            // (
            //     backend.ApollonBackendManager.Instance.GetValidHandle(
            //         backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
            //     ) as backend.handle.ApollonMotionSystemPS6TM550Handle
            // ).BeginTrial();
        
            // local
            // int currentIdx = ApollonExperimentManager.Instance.Session.currentTrialNum - 1;

            // activate the active seat entity
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity);

            // inactivate all visual cues through LINQ request
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;
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
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as gameplay.entity.ApollonActiveSeatEntityBridge
                    ).Dispatcher.RaiseVisualOnly();
                    break;
                }
                case "vestibular-only":
                {
                    we_behaviour.References["VCTag_Spot"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                    // transit to corresponding entity state
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as gameplay.entity.ApollonActiveSeatEntityBridge
                    ).Dispatcher.RaiseVestibularOnly();
                    break;
                }
                case "visuo-vestibular":
                {
                    we_behaviour.References["VCTag_Fan"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
                    // transit to corresponding entity state
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        ) as gameplay.entity.ApollonActiveSeatEntityBridge
                    ).Dispatcher.RaiseVisuoVestibular();
                    break;
                }
                default:
                {
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.Undefined;
                    break;
                }

            } /* switch() */

            // import current trial settings & log on completion
            if(this.CurrentSettings.ImportUXFSettings(arg.Trial.settings))
            {
                this.CurrentSettings.LogUXFSettings();
            }
           
            // activate world element & contriol system
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control);

            // base call
            base.onExperimentTrialBegin(sender, arg);

            // fade out
            await this.DoFadeOut(this._trial_fade_out_duration, false);

            // initialize to position on first trial - wait 5s
            if(ApollonExperimentManager.Instance.Session.FirstTrial == ApollonExperimentManager.Instance.Trial)
            {
                await this.DoSleep(5000.0f);
            }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentTrialBegin() : end " + UnityEngine.Time.fixedTime
            );

            // build protocol
            await this.DoRunProtocol(
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2Phase0(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseA(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseB(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseC(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseD(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseE(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseF(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseG(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseH(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseI(this) ); },
                async () => { await this.SetState( new phase.ApollonAgencyAndThresholdPerceptionV2PhaseJ(this) ); },
                async () => { await this.SetState( null ); }
            );
            
        } /* onExperimentTrialBegin() */

        public override async void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentTrialEnd() : begin"
            );
            
            // write the randomized scenario/pattern/conditions(s) as result for convenience
            ApollonExperimentManager.Instance.Trial.result["scenario"] = ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            ApollonExperimentManager.Instance.Trial.result["pattern"] = this.CurrentSettings.pattern_type;
            ApollonExperimentManager.Instance.Trial.result["active_condition"] = this.CurrentSettings.bIsActive.ToString();
            ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] = this.CurrentSettings.bIsTryCatch.ToString();
            ApollonExperimentManager.Instance.Trial.result["current_latency_bucket"] = "[" + System.String.Join(";",this.CurrentLatencyBucket) + "]";

            // phase 0 - RAZ input to neutral position
            ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_exit_host_timestamp;

            // phase A - user input selection + UI notification / validation
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_command"] 
                = this.CurrentResults.phase_A_results.user_command;
            ApollonExperimentManager.Instance.Trial.result["user_latency_unity_timestamp"] 
                = this.CurrentResults.phase_A_results.user_latency_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["user_latency_host_timestamp"] 
                = this.CurrentResults.phase_A_results.user_latency_host_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["user_measured_latency"] 
                = this.CurrentResults.phase_A_results.user_measured_latency.ToString();
            ApollonExperimentManager.Instance.Trial.result["user_randomized_stim1_latency"] 
                = this.CurrentResults.phase_A_results.user_randomized_stim1_latency.ToString();
            
            // phase B - first stim
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_host_timestamp;

            // phase C - inter strim break
            ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_host_timestamp;

            // phase D - UI notification / validation
            ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_host_timestamp;

            // phase E - latency
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_randomized_stim2_latency"] 
                = this.CurrentResults.phase_E_results.user_randomized_stim2_latency.ToString();

            // phase F - second stim
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_host_timestamp;
            
            // phase G - UI end notification
            ApollonExperimentManager.Instance.Trial.result["G_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_G_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["G_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_G_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["G_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_G_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["G_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_G_results.timing_on_exit_host_timestamp;
                
            // phase H - user response
            ApollonExperimentManager.Instance.Trial.result["H_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_H_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["H_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_H_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["H_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_H_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["H_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_H_results.timing_on_exit_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_response"]
                = this.CurrentResults.phase_H_results.user_response;
                
            // phase I - user confidence in response
            ApollonExperimentManager.Instance.Trial.result["I_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_I_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["I_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_I_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["I_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_I_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["I_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_I_results.timing_on_exit_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["user_confidence"]
                = this.CurrentResults.phase_I_results.user_confidence;
                
            // phase J - back to origin
            ApollonExperimentManager.Instance.Trial.result["J_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_J_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["J_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_J_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["J_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_J_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["J_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_J_results.timing_on_exit_host_timestamp;

            // base call
            base.onExperimentTrialEnd(sender, arg);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2Profile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class ApollonAgencyAndTBWExperimentProfile */

} /* } Labsim.apollon.experiment.profile */
