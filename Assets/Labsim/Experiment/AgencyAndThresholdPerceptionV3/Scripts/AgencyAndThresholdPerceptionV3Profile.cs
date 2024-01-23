//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV3
{

    public class AgencyAndThresholdPerceptionV3Profile 
        : apollon.experiment.ApollonAbstractExperimentFiniteStateMachine< AgencyAndThresholdPerceptionV3Profile >
    {

        // Ctor
        public AgencyAndThresholdPerceptionV3Profile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.AgencyAndThresholdPerceptionV3;
        }

        // fast hack
        public uint
            strongConditionCount = 0,
            weakConditionCount   = 0;

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
                        "<color=Red>Info: </color> AgencyAndThresholdPerceptionV3Profile.Settings.GetJSONSettingsAttributeName() : failed to extract ("
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

                [JSONSettingsAttribute(phase:"phase_0", settings:"angular_acceleration_target", unit:"deg_per_s2")]
                public float[] angular_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_0", settings:"angular_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] angular_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_0", settings:"angular_displacement_limiter", unit:"deg")]
                public float[] angular_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_0", settings:"linear_acceleration_target", unit:"m_per_s2")]
                public float[] linear_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_0", settings:"linear_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] linear_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_0", settings:"linear_displacement_limiter", unit:"m")]
                public float[] linear_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };

            } /* class Phase0Settings */

            public class PhaseASettings
            {

                [JSONSettingsAttribute(phase:"phase_A", settings:"confirm_duration", unit:"ms")]
                public float confirm_duration;
                
                [JSONSettingsAttribute(phase:"phase_A", settings:"reset_duration", unit:"ms")]
                public float reset_duration;

                [JSONSettingsAttribute(phase:"phase_A", settings:"response_max_duration", unit:"ms")]
                public float response_max_duration;

            } /* class PhaseASettings */

            public class PhaseBSettings
            {

                [JSONSettingsAttribute(phase:"phase_B", settings:"SOA", unit:"ms")]
                public float SOA;
                
                [JSONSettingsAttribute(phase:"phase_B", settings:"duration", unit:"ms")]
                public float duration;
                
                #region primary stim settings 

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_stim_duration", unit:"ms")]
                public float primary_stim_duration;


                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_strong_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] primary_angular_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_strong_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] primary_angular_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_strong_displacement_limiter", unit:"deg")]
                public float[] primary_angular_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_weak_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] primary_angular_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_weak_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] primary_angular_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };
                
                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_weak_displacement_limiter", unit:"deg")]
                public float[] primary_angular_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };

                
                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_angular_mandatory_axis")]
                public bool[] primary_angular_mandatory_axis = new bool[3] { false, false, false };


                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_strong_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] primary_linear_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_strong_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] primary_linear_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_strong_displacement_limiter", unit:"m")]
                public float[] primary_linear_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_weak_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] primary_linear_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_weak_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] primary_linear_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_weak_displacement_limiter", unit:"m")]
                public float[] primary_linear_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"primary_linear_mandatory_axis")]
                public bool[] primary_linear_mandatory_axis = new bool[3] { false, false, false };

                #endregion
                
                #region secondary stim settings 

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_stim_duration", unit:"ms")]
                public float secondary_stim_duration;


                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_strong_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] secondary_angular_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_strong_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] secondary_angular_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_strong_displacement_limiter", unit:"deg")]
                public float[] secondary_angular_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_weak_acceleration_offset_from_reference", unit:"deg_per_s2")]
                public float[] secondary_angular_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_weak_velocity_saturation_threshold", unit:"deg_per_s")]
                public float[] secondary_angular_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };
                
                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_weak_displacement_limiter", unit:"deg")]
                public float[] secondary_angular_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };

                
                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_angular_mandatory_axis")]
                public bool[] secondary_angular_mandatory_axis = new bool[3] { false, false, false };


                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_strong_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] secondary_linear_strong_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_strong_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] secondary_linear_strong_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_strong_displacement_limiter", unit:"m")]
                public float[] secondary_linear_strong_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_weak_acceleration_offset_from_reference", unit:"m_per_s2")]
                public float[] secondary_linear_weak_acceleration_offset_from_reference = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_weak_velocity_saturation_threshold", unit:"m_per_s")]
                public float[] secondary_linear_weak_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_weak_displacement_limiter", unit:"m")]
                public float[] secondary_linear_weak_displacement_limiter = new float[3] { 0.0f, 0.0f, 0.0f };


                [JSONSettingsAttribute(phase:"phase_B", settings:"secondary_linear_mandatory_axis")]
                public bool[] secondary_linear_mandatory_axis = new bool[3] { false, false, false };

                #endregion

            } /* PhaseBSettings */ 
            
            public class PhaseCSettings
            {

                [JSONSettingsAttribute(phase:"phase_C", settings:"duration", unit:"ms")]
                public float duration;

            } /* PhaseCSettings */

            public class PhaseFSettings
            {

                [JSONSettingsAttribute(phase:"phase_F", settings:"duration", unit:"ms")]
                public float duration;

            } /* PhaseFSettings */

            public Phase0Settings phase_0_settings = new Phase0Settings(); 
            public PhaseASettings phase_A_settings = new PhaseASettings(); 
            public PhaseBSettings phase_B_settings = new PhaseBSettings();
            public PhaseCSettings phase_C_settings = new PhaseCSettings();
            public PhaseFSettings phase_F_settings = new PhaseFSettings();

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
                            apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VestibularOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                            break;
                        }

                        // visual only
                        case string param when param.Equals(
                            apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VisualOnly),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.scenario_type = Settings.ScenarioIDType.VisualOnly;
                            break;
                        }
                        
                        // visuo-vestibular
                        case string param when param.Equals(
                            apollon.ApollonEngine.GetEnumDescription(Settings.ScenarioIDType.VisuoVestibular),
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
                            apollon.ApollonEngine.GetEnumDescription(Settings.IntensityIDType.Strong),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            this.passive_intensity_type = Settings.IntensityIDType.Strong;
                            break;
                        }

                        // Weak
                        case string param when param.Equals(
                            apollon.ApollonEngine.GetEnumDescription(Settings.IntensityIDType.Weak),
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
                    this.phase_0_settings.angular_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_displacement_limiter")
                        ).ToArray();
                    this.phase_0_settings.angular_acceleration_target
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_acceleration_target")
                        ).ToArray();
                    this.phase_0_settings.angular_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_0_settings.linear_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_displacement_limiter")
                        ).ToArray();
                    this.phase_0_settings.linear_acceleration_target
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_acceleration_target")
                        ).ToArray();
                    this.phase_0_settings.linear_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_velocity_saturation_threshold")
                        ).ToArray();

                    // phase A
                    this.phase_A_settings.confirm_duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("confirm_duration")
                        );
                    this.phase_A_settings.reset_duration                     
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("reset_duration")
                        );
                    this.phase_A_settings.response_max_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("response_max_duration")
                        );
                    
                    // phase B

                    this.phase_B_settings.duration 
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("duration")
                        );
                        
                    this.phase_B_settings.SOA 
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("SOA")
                        );

                    this.phase_B_settings.primary_stim_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_stim_duration")
                        );
                    this.phase_B_settings.primary_angular_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.primary_angular_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_mandatory_axis")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.primary_linear_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_mandatory_axis")
                        ).ToArray();

                    this.phase_B_settings.secondary_stim_duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_stim_duration")
                        );
                    this.phase_B_settings.secondary_angular_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.secondary_angular_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_mandatory_axis")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_strong_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_strong_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_strong_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_weak_displacement_limiter
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_displacement_limiter")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_weak_acceleration_offset_from_reference
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_acceleration_offset_from_reference")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_weak_velocity_saturation_threshold
                        = settings.GetFloatList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_velocity_saturation_threshold")
                        ).ToArray();
                    this.phase_B_settings.secondary_linear_mandatory_axis
                        = settings.GetBoolList(
                            this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_mandatory_axis")
                        ).ToArray();
                    
                    // phase C
                    this.phase_C_settings.duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("duration")
                        );

                    // phase F
                    this.phase_F_settings.duration
                        = settings.GetFloat(
                            this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("duration")
                        );

                } 
                catch(System.Exception ex)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Info: </color> AgencyAndThresholdPerceptionV3Profile.Settings.ImportUXFSettings() : failed to import settings with error ["
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.Settings.LogUXFSettings() : imported current settings with pattern["
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
                        + apollon.ApollonEngine.GetEnumDescription(this.scenario_type)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings>("passive_intensity_type") 
                        + " : " 
                        + apollon.ApollonEngine.GetEnumDescription(this.passive_intensity_type)
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("duration") 
                        + " : " 
                        + this.phase_0_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_acceleration_target") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.angular_acceleration_target) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.angular_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("angular_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.angular_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_acceleration_target") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.linear_acceleration_target) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.linear_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.Phase0Settings>("linear_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_0_settings.linear_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("confirm_duration") 
                        + " : " 
                        + this.phase_A_settings.confirm_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("reset_duration") 
                        + " : " 
                        + this.phase_A_settings.reset_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseASettings>("response_max_duration") 
                        + " : " 
                        + this.phase_A_settings.response_max_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("SOA") 
                        + " : " 
                        + this.phase_B_settings.SOA
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("duration") 
                        + " : " 
                        + this.phase_B_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_stim_duration") 
                        + " : " 
                        + this.phase_B_settings.primary_stim_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_angular_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_angular_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("primary_linear_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.primary_linear_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_stim_duration") 
                        + " : " 
                        + this.phase_B_settings.secondary_stim_duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_angular_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_angular_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_strong_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_strong_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_strong_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_strong_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_acceleration_offset_from_reference") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_weak_acceleration_offset_from_reference) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_velocity_saturation_threshold") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_weak_velocity_saturation_threshold) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_weak_displacement_limiter") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_weak_displacement_limiter) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseBSettings>("secondary_linear_mandatory_axis") 
                        + " : [" 
                            + System.String.Join(",",this.phase_B_settings.secondary_linear_mandatory_axis) 
                        + "]"
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseCSettings>("duration") 
                        + " : " 
                        + this.phase_C_settings.duration
                    + "\n - " 
                        + this.GetJSONSettingsAttributeName<Settings.PhaseFSettings>("duration") 
                        + " : " 
                        + this.phase_F_settings.duration
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

                public bool step_is_valid = false;

                #region user_*

                public int
                    user_command;

                public float
                    user_latency_unity_timestamp,
                    user_measured_latency,
                    user_randomized_stim_latency;
                
                public string
                    user_latency_host_timestamp;

                #endregion

            } /* class PhaseAResult */

            public class PhaseDResults
                : DefaultPhaseTimingResults
            {

                #region user_*

                public bool
                    user_response;

                #endregion

            } /* class PhaseDResults */

            public class PhaseEResults
                : DefaultPhaseTimingResults
            {

                #region user_*

                public float
                    user_confidence;

                #endregion

            } /* class PhaseEResults */

            public DefaultPhaseTimingResults
                phase_0_results = new DefaultPhaseTimingResults(),
                phase_B_results = new DefaultPhaseTimingResults(),
                phase_C_results = new DefaultPhaseTimingResults(),
                phase_F_results = new DefaultPhaseTimingResults();

            public PhaseAResults phase_A_results = new PhaseAResults();
            public PhaseDResults phase_D_results = new PhaseDResults();
            public PhaseEResults phase_E_results = new PhaseEResults();

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
                    if(apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["initial_latency_bucket"].ToString() != "")
                    {

                        // format == [value_0,value_1,...,value_N]
                        
                        // extract raw string
                        var raw_string 
                            = apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["initial_latency_bucket"]
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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.AddUserLatencyToBucket() : adding new latency["
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.AddUserLatencyToBucket() : removing last default value, remaining default count["
                        + this.m_currentInitialDefaultLatencyValueCount
                    + "]"
                );

            } /* if() */
            
            // whatever, add new user latency to bucket
            this.CurrentLatencyBucket.Enqueue(new_latency);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.AddUserLatencyToBucket() : new latency bucket ["
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
                + apollon.ApollonEngine.GetEnumDescription(this.ID) 
                + "]\n" 
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type)
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

        public override void OnUpdate(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.OnUpdate(sender, arg);

        } /* onUpdate() */

        public async override void OnExperimentSessionBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : begin"
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

            // log 
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : sync wait 5000.0ms for motion system init"
            );

            // wait for motion system init
            await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);
            
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : will async fade in 1000.0ms"
            );

            // fade in
            this.DoLightFadeIn(1000.0f);

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : async raise reset to initial position in 5000.0ms"
            );

            // Raise reset motion event
            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                apollon.gameplay.device.command.ApollonMotionSystemCommandBridge
            >(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            ).ConcreteDispatcher.RaiseReset(5000.0f);     

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : switch scene setup"
            );

            // deactivate default DB & activate room setup
            var we_behaviour
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
            we_behaviour.References["DBTag_Default"].SetActive(false);
            we_behaviour.References["DBTag_Room"].SetActive(true);
            we_behaviour.References["DBTag_ExoFrontend"].SetActive(true);

            // log 
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : sync wait 5000.0ms init pause"
            );

            // wait for motion system init
            await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);

            // base call
            base.OnExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void OnExperimentSessionEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.OnExperimentSessionEnd(sender, arg);

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
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentSessionEnd() : end"
            );


        } /* onExperimentSessionEnd() */

        public override async void OnExperimentTrialBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialBegin() : begin"
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
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity);

            // inactivate all visual cues through LINQ request
            var we_behaviour
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
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
                        apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                            apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                        >(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        )
                    ).ConcreteDispatcher.RaiseVisualOnly();
                    break;
                }
                case "vestibular-only":
                {
                    we_behaviour.References["VCTag_Spot"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VestibularOnly;
                    // transit to corresponding entity state
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                            apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                        >(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        )
                    ).ConcreteDispatcher.RaiseVestibularOnly();
                    break;
                }
                case "visuo-vestibular":
                {
                    we_behaviour.References["VCTag_Fan"].SetActive(true);
                    this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
                    // transit to corresponding entity state
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                            apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                        >(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                        )
                    ).ConcreteDispatcher.RaiseVisuoVestibular();
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
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement);
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control);

            // base call
            base.OnExperimentTrialBegin(sender, arg);

            // fade out
            await this.DoLightFadeOut(this._trial_fade_out_duration, false);

            // initialize to position on first trial - wait 5s
            if(apollon.experiment.ApollonExperimentManager.Instance.Session.FirstTrial == apollon.experiment.ApollonExperimentManager.Instance.Trial)
            {
                await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);
            }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialBegin() : trial protocol will start"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => {
                    await this.DoWhileLoop(
                        () => { 
                            return !(this.CurrentResults.phase_A_results.step_is_valid);
                        },
                        async () => { await this.SetState( new AgencyAndThresholdPerceptionV3Phase0(this) ); },
                        async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseA(this) ); }
                    );
                },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseB(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseC(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseD(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseE(this) ); },
                async () => { await this.SetState( new AgencyAndThresholdPerceptionV3PhaseF(this) ); },
                async () => { await this.SetState( null ); }
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialBegin() : end"
            );

        } /* onExperimentTrialBegin() */

        public override async void OnExperimentTrialEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialEnd() : begin"
            );

            // reset internal
            this.CurrentResults.phase_A_results.step_is_valid = false;
            
            // write the randomized scenario/pattern/conditions(s) as result for convenience
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["scenario"] = apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["pattern"] = this.CurrentSettings.pattern_type;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["active_condition"] = this.CurrentSettings.bIsActive.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] = this.CurrentSettings.bIsTryCatch.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["current_latency_bucket"] = "[" + System.String.Join(";",this.CurrentLatencyBucket) + "]";

            // phase 0 - RAZ input to neutral position
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_0_results.timing_on_exit_host_timestamp;

            // phase A - user input selection + UI notification / validation
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_command"] 
                = this.CurrentResults.phase_A_results.user_command;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_latency_unity_timestamp"] 
                = this.CurrentResults.phase_A_results.user_latency_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_latency_host_timestamp"] 
                = this.CurrentResults.phase_A_results.user_latency_host_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_measured_latency"] 
                = this.CurrentResults.phase_A_results.user_measured_latency.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_randomized_stim_latency"] 
                = this.CurrentResults.phase_A_results.user_randomized_stim_latency.ToString();
            
            // phase B - primary stim -> SOA -> secondary stim
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_host_timestamp;
            
            // phase C - UI end notification
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_C_results.timing_on_exit_host_timestamp;
                
            // phase D - user response
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_D_results.timing_on_exit_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response"]
                = this.CurrentResults.phase_D_results.user_response;
                
            // phase E - user confidence in response
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_confidence"]
                = this.CurrentResults.phase_E_results.user_confidence;

            // phase F - back to origin
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp.ToString();
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_host_timestamp;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_host_timestamp;

            // base call
            base.OnExperimentTrialEnd(sender, arg);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3Profile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class ApollonAgencyAndTBWExperimentProfile */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV3 */
