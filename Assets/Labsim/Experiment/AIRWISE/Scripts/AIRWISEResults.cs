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
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEResults
    {

        private AIRWISEProfile CurrentProfile { get; set; } = null;
        public AIRWISEResults(AIRWISEProfile profile)
        {
            this.CurrentProfile = profile;
        }

        public class TrialResults
        {

            #region user_*

            public long 
                user_performance_try_count = 0;

            public float 
                user_performance_value = float.NaN;

            #endregion

        } /* class Trial */

        public class DefaultPhaseTimingResults
        {

            #region timing_*

            public long 
                timing_on_entry_varjo_timestamp = -1,
                timing_on_exit_varjo_timestamp  = -1;

            public float 
                timing_on_entry_unity_timestamp = float.NaN,
                timing_on_exit_unity_timestamp  = float.NaN;

            public string
                timing_on_entry_host_timestamp = "",
                timing_on_exit_host_timestamp  = "";
            
            #endregion

        } /* class DefaultPhaseTimingResults */

        public class PhaseAResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseAResult */

        public class PhaseBResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseBResults */

        public class PhaseCResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public class Checkpoint
            {

                public Checkpoint() 
                {
                }

                public enum KindIDType
                {

                    [System.ComponentModel.Description("Undefined")]
                    Undefined = -1,

                    [System.ComponentModel.Description("Gate_Success")]
                    GateSuccess,

                    [System.ComponentModel.Description("Left_Fail")]
                    LeftFail,

                    [System.ComponentModel.Description("Right_Fail")]
                    RightFail,

                    [System.ComponentModel.Description("Arrival")]
                    Arrival,

                    [System.ComponentModel.Description("Departure")]
                    Departure

                } /* enum */

                public KindIDType kind = KindIDType.Undefined;

                public float timing_unity_timestamp = float.NaN;

                public string timing_host_timestamp = "";

                public long timing_varjo_timestamp = -1;

                public float[] local_position = new float[3]{ float.NaN, float.NaN, float.NaN };

                public float[] world_position = new float[3]{ float.NaN, float.NaN, float.NaN };

                public float[] aero_position = new float[3]{ float.NaN, float.NaN, float.NaN };

            } /* class Checkpoint */

            /* each retry/run in current trial */
            public System.Collections.Generic.List<
                /* Gates dict entries for current run */ 
                System.Collections.Generic.Dictionary<
                    /* Parent gate name */
                    string, 
                    /* Crossed checkpoint information, may be multiple if we cross at the intersection of 2 plane collider*/
                    System.Collections.Generic.Queue<Checkpoint>
                >
            > user_checkpoints_crossing = new();

            // override ToString() standard output
            public override string ToString()
            {

                string stream = new("");

                // for each run in trial
                foreach(
                    var (run_value, run_index) 
                    in  this.user_checkpoints_crossing
                            .Select((value, index) => (value, index))
                )
                {
                    // only print the first element crossed  
                    foreach(
                        var (value, index) 
                        in  run_value.Values
                                .Select(x => x.Peek())
                                .ToList()
                                .Select((value, index) => (value, index))
                    )
                    {

                        // debug ?
                        if(value == null)
                        {
                            continue;
                        }
                    
                        stream 
                            += "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_kind[" 
                                + apollon.ApollonEngine.GetEnumDescription(value.kind)
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_unity_timestamp[" 
                                + value.timing_unity_timestamp.ToString()
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_host_timestamp[" 
                                + value.timing_host_timestamp
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_varjo_timestamp[" 
                                + value.timing_varjo_timestamp.ToString()
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_local_position[" 
                                + "[" + System.String.Join(";", value.local_position) + "]"
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_world_position[" 
                                + "[" + System.String.Join(";", value.world_position) + "]"
                            + "]"
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_aero_position[" 
                                + "[" + System.String.Join(";", value.aero_position) + "]"
                            + "]";
                    
                    } /* foreach() */

                } /* foreach() */

                return stream;

            } /* ToString() */

            #endregion

        } /* class PhaseCResults */
        
        public class PhaseDResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseDResult */
        
        // Question : Mental demand
        public class PhaseEResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseEResults */
        
        // Question : Physical demand
        public class PhaseFResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseFResults */
        
        // Question : Temporal demand
        public class PhaseGResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;
            
            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseGResults */
        
        // Question : Overall performance
        public class PhaseHResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;
            
            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseHResults */
        
        // Question : Effort
        public class PhaseIResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;
            
            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseIResult */
        
        // Question : Frustration
        public class PhaseJResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;
            
            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseJResults */

        public TrialResults Trial { get; set; } = new(); 
        public PhaseAResults PhaseA { get; set; } = new(); 
        public PhaseBResults PhaseB { get; set; } = new();
        public PhaseCResults PhaseC { get; set; } = new();
        public PhaseDResults PhaseD { get; set; } = new();
        public PhaseEResults PhaseE { get; set; } = new();
        public PhaseFResults PhaseF { get; set; } = new();
        public PhaseGResults PhaseG { get; set; } = new();
        public PhaseHResults PhaseH { get; set; } = new();
        public PhaseIResults PhaseI { get; set; } = new();
        public PhaseJResults PhaseJ { get; set; } = new();

        public bool ExportUXFResults(UXF.ResultsDictionary results)
        {

            // encapsulate
            try
            {

                // write the general trial settings as result for convenience
                results["pattern"]             = this.CurrentProfile.CurrentSettings.Trial.pattern_type;
                results["active_condition"]    = this.CurrentProfile.CurrentSettings.Trial.bIsActive.ToString();
                results["catch_try_condition"] = this.CurrentProfile.CurrentSettings.Trial.bIsTryCatch.ToString();
                results["control_name"]        = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.control_type);
                results["visual_name"]         = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.visual_type);
                results["scene_name"]          = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.scene_type);

                // current trials results
                results["user_performance_try_count"] = this.Trial.user_performance_try_count.ToString();
                results["user_performance_value"]     = this.Trial.user_performance_value.ToString();

                // phase A
                results["A_timing_on_entry_unity_timestamp"] = this.PhaseA.timing_on_entry_unity_timestamp.ToString();
                results["A_timing_on_exit_unity_timestamp"]  = this.PhaseA.timing_on_exit_unity_timestamp.ToString();
                results["A_timing_on_entry_host_timestamp"]  = this.PhaseA.timing_on_entry_host_timestamp;
                results["A_timing_on_exit_host_timestamp"]   = this.PhaseA.timing_on_exit_host_timestamp;
                results["A_timing_on_entry_varjo_timestamp"] = this.PhaseA.timing_on_entry_varjo_timestamp.ToString();
                results["A_timing_on_exit_varjo_timestamp"]  = this.PhaseA.timing_on_exit_varjo_timestamp.ToString();

                // phase B
                results["B_timing_on_entry_unity_timestamp"] = this.PhaseB.timing_on_entry_unity_timestamp.ToString();
                results["B_timing_on_exit_unity_timestamp"]  = this.PhaseB.timing_on_exit_unity_timestamp.ToString();
                results["B_timing_on_entry_host_timestamp"]  = this.PhaseB.timing_on_entry_host_timestamp;
                results["B_timing_on_exit_host_timestamp"]   = this.PhaseB.timing_on_exit_host_timestamp;
                results["B_timing_on_entry_varjo_timestamp"] = this.PhaseB.timing_on_entry_varjo_timestamp.ToString();
                results["B_timing_on_exit_varjo_timestamp"]  = this.PhaseB.timing_on_exit_varjo_timestamp.ToString();

                // phase C
                results["C_timing_on_entry_unity_timestamp"] = this.PhaseC.timing_on_entry_unity_timestamp.ToString();
                results["C_timing_on_exit_unity_timestamp"]  = this.PhaseC.timing_on_exit_unity_timestamp.ToString();
                results["C_timing_on_entry_host_timestamp"]  = this.PhaseC.timing_on_entry_host_timestamp;
                results["C_timing_on_exit_host_timestamp"]   = this.PhaseC.timing_on_exit_host_timestamp;
                results["C_timing_on_entry_varjo_timestamp"] = this.PhaseC.timing_on_entry_varjo_timestamp.ToString();
                results["C_timing_on_exit_varjo_timestamp"]  = this.PhaseC.timing_on_exit_varjo_timestamp.ToString();                

                 // for each run in trial
                foreach(
                    var (run_value, run_index) 
                    in  this.PhaseC.user_checkpoints_crossing
                            .Select((value, index) => (value, index))
                )
                {
                    // only print the first element crossed  
                    foreach(
                        var (value, index) 
                        in  run_value.Values
                                .Select(x => x.Peek())
                                .ToList()
                                .Select((value, index) => (value, index))
                    )
                    {
                
                    
                        // debug
                        if(value == null)
                        {
                            continue;
                        }

                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_kind"]                   = apollon.ApollonEngine.GetEnumDescription(value.kind);
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_unity_timestamp"] = value.timing_unity_timestamp.ToString();
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_host_timestamp"]  = value.timing_host_timestamp;
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_varjo_timestamp"] = value.timing_varjo_timestamp.ToString();
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_local_position"]         = "[" + System.String.Join(";", value.local_position)    + "]";
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_world_position"]         = "[" + System.String.Join(";", value.world_position)    + "]";
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_aero_position"]          = "[" + System.String.Join(";", value.aero_position)    + "]";
                        
                    } /* foreach() */

                } /* foreach() */

                // phase D
                results["D_timing_on_entry_unity_timestamp"] = this.PhaseD.timing_on_entry_unity_timestamp.ToString();
                results["D_timing_on_exit_unity_timestamp"]  = this.PhaseD.timing_on_exit_unity_timestamp.ToString();
                results["D_timing_on_entry_host_timestamp"]  = this.PhaseD.timing_on_entry_host_timestamp;
                results["D_timing_on_exit_host_timestamp"]   = this.PhaseD.timing_on_exit_host_timestamp;
                results["D_timing_on_entry_varjo_timestamp"] = this.PhaseD.timing_on_entry_varjo_timestamp.ToString();
                results["D_timing_on_exit_varjo_timestamp"]  = this.PhaseD.timing_on_exit_varjo_timestamp.ToString();

                // phase E
                results["E_timing_on_entry_unity_timestamp"]      = this.PhaseE.timing_on_entry_unity_timestamp.ToString();
                results["E_timing_on_exit_unity_timestamp"]       = this.PhaseE.timing_on_exit_unity_timestamp.ToString();
                results["E_timing_on_entry_host_timestamp"]       = this.PhaseE.timing_on_entry_host_timestamp;
                results["E_timing_on_exit_host_timestamp"]        = this.PhaseE.timing_on_exit_host_timestamp;
                results["E_timing_on_entry_varjo_timestamp"]      = this.PhaseE.timing_on_entry_varjo_timestamp.ToString();
                results["E_timing_on_exit_varjo_timestamp"]       = this.PhaseE.timing_on_exit_varjo_timestamp.ToString();     
                results["E_user_response_timing_unity_timestamp"] = this.PhaseE.user_response_timing_unity_timestamp.ToString();
                results["E_user_response_timing_host_timestamp"]  = this.PhaseE.user_response_timing_host_timestamp;
                results["E_user_response_timing_varjo_timestamp"] = this.PhaseE.user_response_timing_varjo_timestamp.ToString();
                results["E_user_response_value"]                  = this.PhaseE.user_response_value.ToString();
                results["E_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

                // phase F
                results["F_timing_on_entry_unity_timestamp"]      = this.PhaseF.timing_on_entry_unity_timestamp.ToString();
                results["F_timing_on_exit_unity_timestamp"]       = this.PhaseF.timing_on_exit_unity_timestamp.ToString();
                results["F_timing_on_entry_host_timestamp"]       = this.PhaseF.timing_on_entry_host_timestamp;
                results["F_timing_on_exit_host_timestamp"]        = this.PhaseF.timing_on_exit_host_timestamp;
                results["F_timing_on_entry_varjo_timestamp"]      = this.PhaseF.timing_on_entry_varjo_timestamp.ToString();
                results["F_timing_on_exit_varjo_timestamp"]       = this.PhaseF.timing_on_exit_varjo_timestamp.ToString();     
                results["F_user_response_timing_unity_timestamp"] = this.PhaseF.user_response_timing_unity_timestamp.ToString();
                results["F_user_response_timing_host_timestamp"]  = this.PhaseF.user_response_timing_host_timestamp;
                results["F_user_response_timing_varjo_timestamp"] = this.PhaseF.user_response_timing_varjo_timestamp.ToString();
                results["F_user_response_value"]                  = this.PhaseF.user_response_value.ToString();
                results["F_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

                // phase G
                results["G_timing_on_entry_unity_timestamp"]      = this.PhaseG.timing_on_entry_unity_timestamp.ToString();
                results["G_timing_on_exit_unity_timestamp"]       = this.PhaseG.timing_on_exit_unity_timestamp.ToString();
                results["G_timing_on_entry_host_timestamp"]       = this.PhaseG.timing_on_entry_host_timestamp;
                results["G_timing_on_exit_host_timestamp"]        = this.PhaseG.timing_on_exit_host_timestamp;
                results["G_timing_on_entry_varjo_timestamp"]      = this.PhaseG.timing_on_entry_varjo_timestamp.ToString();
                results["G_timing_on_exit_varjo_timestamp"]       = this.PhaseG.timing_on_exit_varjo_timestamp.ToString();     
                results["G_user_response_timing_unity_timestamp"] = this.PhaseG.user_response_timing_unity_timestamp.ToString();
                results["G_user_response_timing_host_timestamp"]  = this.PhaseG.user_response_timing_host_timestamp;
                results["G_user_response_timing_varjo_timestamp"] = this.PhaseG.user_response_timing_varjo_timestamp.ToString();
                results["G_user_response_value"]                  = this.PhaseG.user_response_value.ToString();
                results["G_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

                // phase H
                results["H_timing_on_entry_unity_timestamp"]      = this.PhaseH.timing_on_entry_unity_timestamp.ToString();
                results["H_timing_on_exit_unity_timestamp"]       = this.PhaseH.timing_on_exit_unity_timestamp.ToString();
                results["H_timing_on_entry_host_timestamp"]       = this.PhaseH.timing_on_entry_host_timestamp;
                results["H_timing_on_exit_host_timestamp"]        = this.PhaseH.timing_on_exit_host_timestamp;
                results["H_timing_on_entry_varjo_timestamp"]      = this.PhaseH.timing_on_entry_varjo_timestamp.ToString();
                results["H_timing_on_exit_varjo_timestamp"]       = this.PhaseH.timing_on_exit_varjo_timestamp.ToString();     
                results["H_user_response_timing_unity_timestamp"] = this.PhaseH.user_response_timing_unity_timestamp.ToString();
                results["H_user_response_timing_host_timestamp"]  = this.PhaseH.user_response_timing_host_timestamp;
                results["H_user_response_timing_varjo_timestamp"] = this.PhaseH.user_response_timing_varjo_timestamp.ToString();
                results["H_user_response_value"]                  = this.PhaseH.user_response_value.ToString();
                results["H_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

                // phase I
                results["I_timing_on_entry_unity_timestamp"]      = this.PhaseI.timing_on_entry_unity_timestamp.ToString();
                results["I_timing_on_exit_unity_timestamp"]       = this.PhaseI.timing_on_exit_unity_timestamp.ToString();
                results["I_timing_on_entry_host_timestamp"]       = this.PhaseI.timing_on_entry_host_timestamp;
                results["I_timing_on_exit_host_timestamp"]        = this.PhaseI.timing_on_exit_host_timestamp;
                results["I_timing_on_entry_varjo_timestamp"]      = this.PhaseI.timing_on_entry_varjo_timestamp.ToString();
                results["I_timing_on_exit_varjo_timestamp"]       = this.PhaseI.timing_on_exit_varjo_timestamp.ToString();     
                results["I_user_response_timing_unity_timestamp"] = this.PhaseI.user_response_timing_unity_timestamp.ToString();
                results["I_user_response_timing_host_timestamp"]  = this.PhaseI.user_response_timing_host_timestamp;
                results["I_user_response_timing_varjo_timestamp"] = this.PhaseI.user_response_timing_varjo_timestamp.ToString();
                results["I_user_response_value"]                  = this.PhaseI.user_response_value.ToString();
                results["I_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

                // phase J
                results["J_timing_on_entry_unity_timestamp"]      = this.PhaseJ.timing_on_entry_unity_timestamp.ToString();
                results["J_timing_on_exit_unity_timestamp"]       = this.PhaseJ.timing_on_exit_unity_timestamp.ToString();
                results["J_timing_on_entry_host_timestamp"]       = this.PhaseJ.timing_on_entry_host_timestamp;
                results["J_timing_on_exit_host_timestamp"]        = this.PhaseJ.timing_on_exit_host_timestamp;
                results["J_timing_on_entry_varjo_timestamp"]      = this.PhaseJ.timing_on_entry_varjo_timestamp.ToString();
                results["J_timing_on_exit_varjo_timestamp"]       = this.PhaseJ.timing_on_exit_varjo_timestamp.ToString();     
                results["J_user_response_timing_unity_timestamp"] = this.PhaseJ.user_response_timing_unity_timestamp.ToString();
                results["J_user_response_timing_host_timestamp"]  = this.PhaseJ.user_response_timing_host_timestamp;
                results["J_user_response_timing_varjo_timestamp"] = this.PhaseJ.user_response_timing_varjo_timestamp.ToString();
                results["J_user_response_value"]                  = this.PhaseJ.user_response_value.ToString();
                results["J_user_elapsed_ms_since_entry"]          = this.PhaseE.user_elapsed_ms_since_entry.ToString();

            } 
            catch(System.Exception ex)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Info: </color> AIRWISEResults.ExportUXFResults() : failed to export results with error ["
                    + ex.Message
                    + "]"
                );

                // failure
                return false;

            } /* try */

            // success
            return true;

        } /* ExportUXFResults */
        
        public void LogUXFResults()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEResults.ExportUXFResults() : exported current results"
                + "\n - pattern"                           
                    + "[" 
                    + this.CurrentProfile.CurrentSettings.Trial.pattern_type
                    + "]"
                + "\n - active_condition"                  
                    + "[" 
                    + this.CurrentProfile.CurrentSettings.Trial.bIsActive.ToString()
                    + "]"
                + "\n - catch_try_condition"               
                    + "[" 
                    + this.CurrentProfile.CurrentSettings.Trial.bIsTryCatch.ToString()
                    + "]"
                + "\n - control_name"                     
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.control_type)
                    + "]"
                + "\n - visual_name"                     
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.visual_type)
                    + "]"
                + "\n - scene_name"                        
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.scene_type)
                    + "]"
                + "\n - user_performance_try_count"                        
                    + "[" 
                    + this.Trial.user_performance_try_count
                    + "]"
                + "\n - user_performance_value"                        
                    + "[" 
                    + this.Trial.user_performance_value
                    + "]"
                + "\n - A_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseA.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - A_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseA.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - A_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseA.timing_on_entry_host_timestamp
                    + "]"
                + "\n - A_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseA.timing_on_exit_host_timestamp
                    + "]"
                + "\n - A_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseA.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - A_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseA.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - B_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseB.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - B_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseB.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - B_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseB.timing_on_entry_host_timestamp
                    + "]"
                + "\n - B_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseB.timing_on_exit_host_timestamp
                    + "]"
                + "\n - B_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseB.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - B_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseB.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - C_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseC.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - C_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseC.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - C_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseC.timing_on_entry_host_timestamp
                    + "]"
                + "\n - C_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseC.timing_on_exit_host_timestamp
                    + "]"
                + "\n - C_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseC.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - C_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseC.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + this.PhaseC.ToString()
                + "\n - D_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseD.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - D_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseD.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - D_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseD.timing_on_entry_host_timestamp
                    + "]"
                + "\n - D_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseD.timing_on_exit_host_timestamp
                    + "]"
                + "\n - D_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseD.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - D_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseD.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - E_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseE.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - E_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseE.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - E_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseE.timing_on_entry_host_timestamp
                    + "]"
                + "\n - E_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseE.timing_on_exit_host_timestamp
                    + "]"
                + "\n - E_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseE.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - E_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseE.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - E_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseE.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - E_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseE.user_response_timing_host_timestamp
                    + "]"
                + "\n - E_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseE.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - E_user_response_value"  
                    + "[" 
                    + this.PhaseE.user_response_value.ToString()
                    + "]"
                + "\n - E_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseE.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - F_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseF.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - F_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseF.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - F_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseF.timing_on_entry_host_timestamp
                    + "]"
                + "\n - F_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseF.timing_on_exit_host_timestamp
                    + "]"
                + "\n - F_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseF.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - F_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseF.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - F_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseF.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - F_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseF.user_response_timing_host_timestamp
                    + "]"
                + "\n - F_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseF.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - F_user_response_value"  
                    + "[" 
                    + this.PhaseF.user_response_value.ToString()
                    + "]"
                + "\n - F_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseF.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - G_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseG.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - G_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseG.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - G_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseG.timing_on_entry_host_timestamp
                    + "]"
                + "\n - G_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseG.timing_on_exit_host_timestamp
                    + "]"
                + "\n - G_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseG.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - G_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseG.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - G_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseG.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - G_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseG.user_response_timing_host_timestamp
                    + "]"
                + "\n - G_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseG.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - G_user_response_value"  
                    + "[" 
                    + this.PhaseG.user_response_value.ToString()
                    + "]"
                + "\n - G_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseG.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - H_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseH.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - H_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseH.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - H_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseH.timing_on_entry_host_timestamp
                    + "]"
                + "\n - H_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseH.timing_on_exit_host_timestamp
                    + "]"
                + "\n - H_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseH.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - H_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseH.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - H_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseH.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - H_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseH.user_response_timing_host_timestamp
                    + "]"
                + "\n - H_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseH.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - H_user_response_value"  
                    + "[" 
                    + this.PhaseH.user_response_value.ToString()
                    + "]"
                + "\n - H_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseH.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - I_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseI.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - I_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseI.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - I_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseI.timing_on_entry_host_timestamp
                    + "]"
                + "\n - I_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseI.timing_on_exit_host_timestamp
                    + "]"
                + "\n - I_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseI.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - I_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseI.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - I_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseI.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - I_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseI.user_response_timing_host_timestamp
                    + "]"
                + "\n - I_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseI.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - I_user_response_value"  
                    + "[" 
                    + this.PhaseI.user_response_value.ToString()
                    + "]"
                + "\n - I_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseI.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - J_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseJ.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - J_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseJ.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - J_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseJ.timing_on_entry_host_timestamp
                    + "]"
                + "\n - J_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseJ.timing_on_exit_host_timestamp
                    + "]"
                + "\n - J_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseJ.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - J_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseJ.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - J_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseJ.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - J_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseJ.user_response_timing_host_timestamp
                    + "]"
                + "\n - J_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseJ.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - J_user_response_value"  
                    + "[" 
                    + this.PhaseJ.user_response_value.ToString()
                    + "]"
                + "\n - J_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseJ.user_elapsed_ms_since_entry.ToString()
                    + "]"
            );

        } /* LogUXFResults() */

    } /* class AIRWISEResults */
    
} /* } Labsim.experiment.AIRWISE */