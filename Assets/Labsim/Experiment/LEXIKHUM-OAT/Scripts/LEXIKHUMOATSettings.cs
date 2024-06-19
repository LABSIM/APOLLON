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

// avoid namespace pollution
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATSettings
    {

        private LEXIKHUMOATProfile CurrentProfile { get; set; } = null;
        public LEXIKHUMOATSettings(LEXIKHUMOATProfile profile)
        {
            this.CurrentProfile = profile;
        }
    
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
                    "<color=Red>Info: </color> LEXIKHUMOATSettings.GetJSONSettingsAttributeName() : failed to extract ("
                    + field_name
                    + ") with error ["
                    + ex.Message
                    + "]"
                );

                return "Error";

            } /* try */
        }  
        
        public enum SceneIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = -1,

            [System.ComponentModel.Description("Familiarisation")]
            Familiarisation,

            [System.ComponentModel.Description("ID1")]  ID1,
            [System.ComponentModel.Description("ID2")]  ID2,
            [System.ComponentModel.Description("ID3")]  ID3,
            [System.ComponentModel.Description("ID4")]  ID4,
            [System.ComponentModel.Description("ID5")]  ID5,
            [System.ComponentModel.Description("ID6")]  ID6,
            [System.ComponentModel.Description("ID7")]  ID7,
            [System.ComponentModel.Description("ID8")]  ID8,
            [System.ComponentModel.Description("ID9")]  ID9,
            [System.ComponentModel.Description("ID10")] ID10,
            [System.ComponentModel.Description("ID11")] ID11,
            [System.ComponentModel.Description("ID12")] ID12,
            [System.ComponentModel.Description("ID13")] ID13,
            [System.ComponentModel.Description("ID14")] ID14,
            [System.ComponentModel.Description("ID15")] ID15,
            [System.ComponentModel.Description("ID16")] ID16,
            [System.ComponentModel.Description("ID17")] ID17,
            [System.ComponentModel.Description("ID18")] ID18,
            [System.ComponentModel.Description("ID19")] ID19,
            [System.ComponentModel.Description("ID20")] ID20,
            [System.ComponentModel.Description("ID21")] ID21,
            [System.ComponentModel.Description("ID22")] ID22,
            [System.ComponentModel.Description("ID23")] ID23,
            [System.ComponentModel.Description("ID24")] ID24,
            [System.ComponentModel.Description("ID25")] ID25,
            [System.ComponentModel.Description("ID26")] ID26,
            [System.ComponentModel.Description("ID27")] ID27,
            [System.ComponentModel.Description("ID28")] ID28,
            [System.ComponentModel.Description("ID29")] ID29,
            [System.ComponentModel.Description("ID30")] ID30,
            [System.ComponentModel.Description("ID31")] ID31,
            [System.ComponentModel.Description("ID32")] ID32,
            [System.ComponentModel.Description("ID33")] ID33,
            [System.ComponentModel.Description("ID34")] ID34,
            [System.ComponentModel.Description("ID35")] ID35,
            [System.ComponentModel.Description("ID36")] ID36,
            [System.ComponentModel.Description("ID37")] ID37,
            [System.ComponentModel.Description("ID38")] ID38,
            [System.ComponentModel.Description("ID39")] ID39,
            [System.ComponentModel.Description("ID40")] ID40,
            [System.ComponentModel.Description("ID41")] ID41,
            [System.ComponentModel.Description("ID42")] ID42,
            [System.ComponentModel.Description("ID43")] ID43,
            [System.ComponentModel.Description("ID44")] ID44,
            [System.ComponentModel.Description("ID45")] ID45,
            [System.ComponentModel.Description("ID46")] ID46,
            [System.ComponentModel.Description("ID47")] ID47,
            [System.ComponentModel.Description("ID48")] ID48,
            [System.ComponentModel.Description("ID49")] ID49,
            [System.ComponentModel.Description("ID50")] ID50

        } /* enum */

        public enum VisualIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = -1,

            [System.ComponentModel.Description("None")]
            None,
            
            [System.ComponentModel.Description("Familiarisation")]
            Familiarisation,

            [System.ComponentModel.Description("A")]
            A,

            [System.ComponentModel.Description("B")]
            B,

            [System.ComponentModel.Description("C")]
            C,

            [System.ComponentModel.Description("D")]
            D,

            [System.ComponentModel.Description("E")]
            E,

        } /* enum */

        public enum SharedIntentionIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = -1,

            [System.ComponentModel.Description("None")]
            None,

            [System.ComponentModel.Description("Haptic")]
            Haptic,

            [System.ComponentModel.Description("Auditive")]
            Auditive,

            [System.ComponentModel.Description("Visual")]
            Visual,

            [System.ComponentModel.Description("Practice")]
            Practice

        } /* enum */

        // General trial
        public class TrialSettings
        {

            [JSONSettingsAttribute("current_pattern")]
            public string pattern_type;

            [JSONSettingsAttribute("is_active_condition")]
            public bool bIsActive;

            [JSONSettingsAttribute("is_catch_try_condition")]
            public bool bIsTryCatch;
            
            [JSONSettingsAttribute("scene_name")]
            public SceneIDType scene_type;
            
            [JSONSettingsAttribute("visual_name")]
            public VisualIDType visual_type; 

            [JSONSettingsAttribute(settings:"performance_criteria_value", unit:"percent")]
            public float performance_criteria;

            [JSONSettingsAttribute(settings:"performance_max_try_count")]
            public long performance_max_try;

            [JSONSettingsAttribute(settings:"fog_start_distance", unit:"m")]
            public float fog_start_distance;

            [JSONSettingsAttribute(settings:"fog_end_distance", unit:"m")]
            public float fog_end_distance;

            [JSONSettingsAttribute(settings:"fog_color", unit:"rvb")]
            public float[] fog_color = new float[3] { 1.0f, 1.0f, 1.0f };

        } /* TrialSettings */

        // Internal init
        public class PhaseASettings
        {

            [JSONSettingsAttribute(phase:"phase_A", settings:"duration", unit:"ms")]
            public float duration;

        } /* class PhaseASettings */

        // Subject init
        public class PhaseBSettings
        {

            [JSONSettingsAttribute(phase:"phase_B", settings:"acceleration_duration", unit:"ms")]
            public float acceleration_duration;

            [JSONSettingsAttribute(phase:"phase_B", settings:"total_duration", unit:"ms")]
            public float total_duration;

            [JSONSettingsAttribute(phase:"phase_B", settings:"angular_acceleration_target", unit:"deg_per_s2")]
            public float[] angular_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_B", settings:"angular_velocity_saturation_threshold", unit:"deg_per_s")]
            public float[] angular_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_B", settings:"linear_acceleration_target", unit:"m_per_s2")]
            public float[] linear_acceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_B", settings:"linear_velocity_saturation_threshold", unit:"m_per_s")]
            public float[] linear_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

        } /* class PhaseBSettings */

        // Slalom
        public class PhaseCSettings
        {
            
            [JSONSettingsAttribute(phase:"phase_C", settings:"shared_intention_name")]
            public SharedIntentionIDType shared_intention_type;

            [JSONSettingsAttribute(phase:"phase_C", settings:"shared_intention_offset", unit:"ms")]
            public float shared_intention_offset;

            [JSONSettingsAttribute(phase:"phase_C", settings:"shared_intention_duration", unit:"ms")]
            public float shared_intention_duration;

        } /* PhaseCSettings */

        // End subject + reset internal 
        public class PhaseDSettings
        {

            [JSONSettingsAttribute(phase:"phase_D", settings:"decceleration_duration", unit:"ms")]
            public float decceleration_duration;

            [JSONSettingsAttribute(phase:"phase_D", settings:"total_duration", unit:"ms")]
            public float total_duration;

            [JSONSettingsAttribute(phase:"phase_D", settings:"angular_decceleration_target", unit:"deg_per_s2")]
            public float[] angular_decceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_D", settings:"angular_velocity_saturation_threshold", unit:"deg_per_s")]
            public float[] angular_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_D", settings:"linear_decceleration_target", unit:"m_per_s2")]
            public float[] linear_decceleration_target = new float[3] { 0.0f, 0.0f, 0.0f };

            [JSONSettingsAttribute(phase:"phase_D", settings:"linear_velocity_saturation_threshold", unit:"m_per_s")]
            public float[] linear_velocity_saturation_threshold = new float[3] { 0.0f, 0.0f, 0.0f };

        } /* class PhaseDSettings */

        // Question : Agentivity demand
        public class PhaseESettings
        {

        } /* class PhaseESettings */
        
        public TrialSettings Trial { get; private set; } = new(); 
        public PhaseASettings PhaseA { get; private set; } = new(); 
        public PhaseBSettings PhaseB { get; private set; } = new();
        public PhaseCSettings PhaseC { get; private set; } = new();
        public PhaseDSettings PhaseD { get; private set; } = new();
        public PhaseESettings PhaseE { get; private set; } = new();

        public bool ImportUXFSettings(UXF.Settings settings)
        {

            // encapsulate
            try
            {

                // extract general trial settings
                this.Trial.pattern_type 
                    = settings.GetString(
                        this.GetJSONSettingsAttributeName<TrialSettings>("pattern_type")
                    );
                this.Trial.bIsTryCatch 
                    = settings.GetBool(
                        this.GetJSONSettingsAttributeName<TrialSettings>("bIsTryCatch")
                    );
                this.Trial.bIsActive 
                    = settings.GetBool(
                        this.GetJSONSettingsAttributeName<TrialSettings>("bIsActive")
                    );
                this.Trial.performance_criteria
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<TrialSettings>("performance_criteria")
                    );
                this.Trial.performance_max_try
                    = settings.GetLong(
                        this.GetJSONSettingsAttributeName<TrialSettings>("performance_max_try")
                    );
                this.Trial.fog_start_distance
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<TrialSettings>("fog_start_distance")
                    );
                this.Trial.fog_end_distance
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<TrialSettings>("fog_end_distance")
                    );
                this.Trial.fog_color
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<TrialSettings>("fog_color")
                    ).ToArray();
                
                // current scene
                switch(
                    settings.GetString(
                        this.GetJSONSettingsAttributeName<TrialSettings>("scene_type")
                    )
                ) {
                    
                    // familiarisation
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.Familiarisation),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.Familiarisation;
                        break;
                    }

                    // ID1
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID1),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID1;
                        break;
                    }

                    // ID2
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID2),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID2;
                        break;
                    }

                    // ID3
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID3),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID3;
                        break;
                    }

                    // ID4
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID4),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID4;
                        break;
                    }

                    // ID5
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID5),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID5;
                        break;
                    }

                    // ID6
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID6),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID6;
                        break;
                    }

                    // ID7
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID7),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID7;
                        break;
                    }

                    // ID8
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID8),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID8;
                        break;
                    }

                    // ID9
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID9),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID9;
                        break;
                    }


                    // ID10
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID10),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID10;
                        break;
                    }

                    // ID11
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID11),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID11;
                        break;
                    }

                    // ID12
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID12),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID12;
                        break;
                    }

                    // ID13
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID13),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID13;
                        break;
                    }

                    // ID14
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID14),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID14;
                        break;
                    }

                    // ID15
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID15),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID15;
                        break;
                    }

                    // ID16
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID16),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID16;
                        break;
                    }

                    // ID17
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID17),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID17;
                        break;
                    }

                    // ID18
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID18),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID18;
                        break;
                    }

                    // ID19
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID19),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID19;
                        break;
                    }

                    // ID20
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID20),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID20;
                        break;
                    }

                    // ID21
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID21),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID21;
                        break;
                    }

                    // ID22
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID22),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID22;
                        break;
                    }

                    // ID23
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID23),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID23;
                        break;
                    }

                    // ID24
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID24),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID24;
                        break;
                    }

                    // ID25
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID25),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID25;
                        break;
                    }

                    // ID26
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID26),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID26;
                        break;
                    }

                    // ID27
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID27),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID27;
                        break;
                    }

                    // ID28
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID28),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID28;
                        break;
                    }

                    // ID29
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID29),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID29;
                        break;
                    }

                    // ID30
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID30),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID30;
                        break;
                    }

                    // ID31
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID31),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID31;
                        break;
                    }

                    // ID32
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID32),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID32;
                        break;
                    }

                    // ID33
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID33),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID33;
                        break;
                    }

                    // ID34
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID34),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID34;
                        break;
                    }

                    // ID35
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID35),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID35;
                        break;
                    }

                    // ID36
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID36),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID36;
                        break;
                    }

                    // ID37
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID37),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID37;
                        break;
                    }

                    // ID38
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID38),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID38;
                        break;
                    }

                    // ID39
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID39),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID39;
                        break;
                    }

                    // ID40
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID40),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID40;
                        break;
                    }

                    // ID41
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID41),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID41;
                        break;
                    }

                    // ID42
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID42),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID42;
                        break;
                    }

                    // ID43
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID43),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID43;
                        break;
                    }

                    // ID44
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID44),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID44;
                        break;
                    }

                    // ID45
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID45),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID45;
                        break;
                    }

                    // ID46
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID46),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID46;
                        break;
                    }

                    // ID47
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID47),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID47;
                        break;
                    }

                    // ID48
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID48),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID48;
                        break;
                    }

                    // ID49
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID49),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID49;
                        break;
                    }

                    // ID50
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SceneIDType.ID50),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.scene_type = SceneIDType.ID50;
                        break;
                    }

                    // default
                    default: 
                    {
                        this.Trial.scene_type = SceneIDType.Undefined;
                        break;
                    }

                } /* switch() */

                // current visual
                switch(
                    settings.GetString(
                        this.GetJSONSettingsAttributeName<TrialSettings>("visual_type")
                    )
                ) {
                    
                    // None
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.None),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.None;
                        break;
                    }

                    // Familiarisation
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.Familiarisation),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.Familiarisation;
                        break;
                    }

                    // A
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.A),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.A;
                        break;
                    }

                    // B
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.B),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.B;
                        break;
                    }

                    // C
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.C),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.C;
                        break;
                    }

                    // D
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.D),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.D;
                        break;
                    }

                    // E
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.E),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.E;
                        break;
                    }

                    // default
                    default: 
                    {
                        this.Trial.visual_type = VisualIDType.Undefined;
                        break;
                    }

                } /* switch() */

                // phase A
                this.PhaseA.duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseASettings>("duration")
                    );

                // phase B
                this.PhaseB.acceleration_duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("acceleration_duration")
                    );
                this.PhaseB.total_duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("total_duration")
                    );
                this.PhaseB.linear_acceleration_target
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("linear_acceleration_target")
                    ).ToArray();
                this.PhaseB.linear_velocity_saturation_threshold
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("linear_velocity_saturation_threshold")
                    ).ToArray();
                this.PhaseB.angular_acceleration_target
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("angular_acceleration_target")
                    ).ToArray();
                this.PhaseB.angular_velocity_saturation_threshold
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseBSettings>("angular_velocity_saturation_threshold")
                    ).ToArray();

                // current shared intention
                switch(
                    settings.GetString(
                        this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_type")
                    )
                ) {
                    
                    // None
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SharedIntentionIDType.None),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.PhaseC.shared_intention_type = SharedIntentionIDType.None;
                        break;
                    }

                    // Haptic
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SharedIntentionIDType.Haptic),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.PhaseC.shared_intention_type = SharedIntentionIDType.Haptic;
                        break;
                    }

                    // Visual
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SharedIntentionIDType.Visual),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.PhaseC.shared_intention_type = SharedIntentionIDType.Visual;
                        break;
                    }

                    // Auditive
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(SharedIntentionIDType.Auditive),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.PhaseC.shared_intention_type = SharedIntentionIDType.Auditive;
                        break;
                    }
                    
                    // default
                    default: 
                    {
                        this.PhaseC.shared_intention_type = SharedIntentionIDType.Undefined;
                        break;
                    }

                } /* switch() */

                this.PhaseC.shared_intention_offset
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_offset")
                    );
                this.PhaseC.shared_intention_duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_duration")
                    );

                // phase D
                this.PhaseD.decceleration_duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("decceleration_duration")
                    );
                this.PhaseD.total_duration
                    = settings.GetFloat(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("total_duration")
                    );
                this.PhaseD.linear_decceleration_target
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("linear_decceleration_target")
                    ).ToArray();
                this.PhaseD.linear_velocity_saturation_threshold
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("linear_velocity_saturation_threshold")
                    ).ToArray();
                this.PhaseD.angular_decceleration_target
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("angular_decceleration_target")
                    ).ToArray();
                this.PhaseD.angular_velocity_saturation_threshold
                    = settings.GetFloatList(
                        this.GetJSONSettingsAttributeName<PhaseDSettings>("angular_velocity_saturation_threshold")
                    ).ToArray();

            } 
            catch(System.Exception ex)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Info: </color> LEXIKHUMOATSettings.ImportUXFSettings() : failed to import settings with error ["
                    + ex.Message
                    + "]"
                );

                // failure
                return false;

            } /* try */

            // success
            return true;

        } /* ImportUXFSettings() */
        
        public void LogUXFSettings()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATSettings.LogUXFSettings() : imported current settings with pattern["
                    + this.Trial.pattern_type
                + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("bIsTryCatch") 
                    + " : " 
                    + this.Trial.bIsTryCatch
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("bIsActive") 
                    + " : " 
                    + this.Trial.bIsActive
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("performance_criteria") 
                    + " : " 
                    + this.Trial.performance_criteria
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("scene_type") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.Trial.scene_type)
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("visual_type") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.Trial.visual_type)
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("fog_start_distance") 
                    + " : " 
                    + this.Trial.fog_start_distance
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("fog_end_distance") 
                    + " : " 
                    + this.Trial.fog_end_distance
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("fog_color") 
                    + " : [" 
                        + System.String.Join(",",this.Trial.fog_color) 
                    + "]"                    
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseASettings>("duration") 
                    + " : " 
                    + this.PhaseA.duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("acceleration_duration") 
                    + " : " 
                    + this.PhaseB.acceleration_duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("total_duration") 
                    + " : " 
                    + this.PhaseB.total_duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("linear_acceleration_target") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseB.linear_acceleration_target) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("linear_velocity_saturation_threshold") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseB.linear_velocity_saturation_threshold) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("angular_acceleration_target") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseB.angular_acceleration_target) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseBSettings>("angular_velocity_saturation_threshold") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseB.angular_velocity_saturation_threshold) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_type") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.PhaseC.shared_intention_type)
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_offset") 
                    + " : " 
                    + this.PhaseC.shared_intention_offset
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseCSettings>("shared_intention_duration") 
                    + " : " 
                    + this.PhaseC.shared_intention_duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("decceleration_duration") 
                    + " : " 
                    + this.PhaseD.decceleration_duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("total_duration") 
                    + " : " 
                    + this.PhaseD.total_duration
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("linear_decceleration_target") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseD.linear_decceleration_target) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("linear_velocity_saturation_threshold") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseD.linear_velocity_saturation_threshold) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("angular_decceleration_target") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseD.angular_decceleration_target) 
                    + "]"
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<PhaseDSettings>("angular_velocity_saturation_threshold") 
                    + " : [" 
                        + System.String.Join(",",this.PhaseD.angular_velocity_saturation_threshold) 
                    + "]"
            );

        } /* LogUXFSettings() */

    } /* class LEXIKHUMOATSettings */

} /* } Labsim.experiment.LEXIKHUM_OAT */