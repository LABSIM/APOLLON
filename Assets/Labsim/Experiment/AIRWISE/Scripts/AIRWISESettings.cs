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
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISESettings
    {

        private AIRWISEProfile CurrentProfile { get; set; } = null;
        public AIRWISESettings(AIRWISEProfile profile)
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
                    "<color=Red>Info: </color> AIRWISESettings.GetJSONSettingsAttributeName() : failed to extract ("
                    + field_name
                    + ") with error ["
                    + ex.Message
                    + "]"
                );

                return "Error";

            } /* try */
        }  

        public enum ControlIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = 0,

            [System.ComponentModel.Description("Familiarisation")]
            Familiarisation,

            [System.ComponentModel.Description("PositionControl")]
            PositionControl,

            [System.ComponentModel.Description("SpeedControl")]
            SpeedControl,

            [System.ComponentModel.Description("AccelerationControl")]
            AccelerationControl

        } /* enum */
        
        public enum SceneIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = 0,

            [System.ComponentModel.Description("Familiarisation")]
            Familiarisation,

            [System.ComponentModel.Description("ID1")]
            ID1,

            [System.ComponentModel.Description("ID2")]
            ID2,

            [System.ComponentModel.Description("ID3")]
            ID3,

            [System.ComponentModel.Description("ID4")]
            ID4,

            [System.ComponentModel.Description("ID5")]
            ID5

        } /* enum */

        public enum VisualIDType
        {

            [System.ComponentModel.Description("Undefined")]
            Undefined = 0,

            [System.ComponentModel.Description("None")]
            None,

            [System.ComponentModel.Description("StreetA")]
            StreetA,

            [System.ComponentModel.Description("StreetB")]
            StreetB,

            [System.ComponentModel.Description("StreetC")]
            StreetC,

            [System.ComponentModel.Description("StreetD")]
            StreetD,

            [System.ComponentModel.Description("StreetE")]
            StreetE,

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
        
            [JSONSettingsAttribute("control_name")]
            public ControlIDType control_type;
            
            [JSONSettingsAttribute("scene_name")]
            public SceneIDType scene_type;
            
            [JSONSettingsAttribute("visual_name")]
            public VisualIDType visual_type;

            [JSONSettingsAttribute(settings:"performance_criteria_value", unit:"percent")]
            public float performance_criteria;

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

        // Question : Mental demand
        public class PhaseESettings
        {

        } /* class PhaseESettings */
        
        // Question : Physical demand
        public class PhaseFSettings
        {

        } /* class PhaseFSettings */
        
        // Question : Temporal demand
        public class PhaseGSettings
        {

        } /* class PhaseGSettings */
        
        // Question : Overall performance
        public class PhaseHSettings
        {

        } /* class PhaseHSettings */
        
        // Question : Effort
        public class PhaseISettings
        {

        } /* class PhaseISettings */
        
        // Question : Frustration
        public class PhaseJSettings
        {

        } /* class PhaseJSettings */

        public TrialSettings Trial { get; private set; } = new(); 
        public PhaseASettings PhaseA { get; private set; } = new(); 
        public PhaseBSettings PhaseB { get; private set; } = new();
        public PhaseCSettings PhaseC { get; private set; } = new();
        public PhaseDSettings PhaseD { get; private set; } = new();
        public PhaseESettings PhaseE { get; private set; } = new();
        public PhaseFSettings PhaseF { get; private set; } = new();
        public PhaseGSettings PhaseG { get; private set; } = new();
        public PhaseHSettings PhaseH { get; private set; } = new();
        public PhaseISettings PhaseI { get; private set; } = new();
        public PhaseJSettings PhaseJ { get; private set; } = new();

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
                        this.GetJSONSettingsAttributeName<TrialSettings>("performance_criteria_value")
                    );
                
                // current control
                switch(
                    settings.GetString(
                        this.GetJSONSettingsAttributeName<TrialSettings>("control_name")
                    )
                ) {
                    
                    // familiarisation
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(ControlIDType.Familiarisation),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.control_type = ControlIDType.Familiarisation;
                        break;
                    }

                    // position control
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(ControlIDType.PositionControl),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.control_type = ControlIDType.PositionControl;
                        break;
                    }

                    // position control
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(ControlIDType.SpeedControl),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.control_type = ControlIDType.SpeedControl;
                        break;
                    }

                    // acceleration control
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(ControlIDType.AccelerationControl),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.control_type = ControlIDType.AccelerationControl;
                        break;
                    }

                    // default
                    default: 
                    {
                        this.Trial.control_type = ControlIDType.Undefined;
                        break;
                    }

                } /* switch() */

                // current scene
                switch(
                    settings.GetString(
                        this.GetJSONSettingsAttributeName<TrialSettings>("scene_name")
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
                        this.GetJSONSettingsAttributeName<TrialSettings>("visual_name")
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

                    // StreetA
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.StreetA),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.StreetA;
                        break;
                    }

                    // StreetB
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.StreetB),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.StreetB;
                        break;
                    }

                    // StreetC
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.StreetC),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.StreetC;
                        break;
                    }

                    // StreetD
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.StreetD),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.StreetD;
                        break;
                    }

                    // StreetE
                    case string param when param.Equals(
                        apollon.ApollonEngine.GetEnumDescription(VisualIDType.StreetE),
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        this.Trial.visual_type = VisualIDType.StreetE;
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
                    "<color=Red>Info: </color> AIRWISESettings.ImportUXFSettings() : failed to import settings with error ["
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
                "<color=Blue>Info: </color> AIRWISESettings.LogUXFSettings() : imported current settings with pattern["
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
                    + this.GetJSONSettingsAttributeName<TrialSettings>("performance_criteria_value") 
                    + " : " 
                    + this.Trial.performance_criteria
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("control_name") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.Trial.control_type)
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("scene_name") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.Trial.scene_type)
                + "\n - " 
                    + this.GetJSONSettingsAttributeName<TrialSettings>("visual_name") 
                    + " : " 
                    + apollon.ApollonEngine.GetEnumDescription(this.Trial.visual_type)
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

    } /* class AIRWISESettings */

} /* } Labsim.experiment.AIRWISE */