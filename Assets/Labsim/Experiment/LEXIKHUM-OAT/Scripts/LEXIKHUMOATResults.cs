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
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATResults
    {

        private LEXIKHUMOATProfile CurrentProfile { get; set; } = null;
        public LEXIKHUMOATResults(LEXIKHUMOATProfile profile)
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

                    [System.ComponentModel.Description("None")]
                    None,

                    [System.ComponentModel.Description("Success")]
                    Success,

                    [System.ComponentModel.Description("Fail")]
                    Fail,

                    [System.ComponentModel.Description("Cue")]
                    Cue,

                    [System.ComponentModel.Description("Arrival")]
                    Arrival,

                    [System.ComponentModel.Description("Departure")]
                    Departure

                } /* enum */

                public enum SideIDType
                {

                    [System.ComponentModel.Description("Undefined")]
                    Undefined = -1,

                    [System.ComponentModel.Description("Left")]
                    Left,

                    [System.ComponentModel.Description("Right")]
                    Right,

                    [System.ComponentModel.Description("Center")]
                    Center

                } /* enum */

                public KindIDType kind = KindIDType.Undefined;
                public SideIDType side = SideIDType.Undefined;

                public float timing_unity_timestamp = float.NaN;

                public string timing_host_timestamp = "";

                public long timing_varjo_timestamp = -1;

                public float[] local_position = new float[3]{ float.NaN, float.NaN, float.NaN };

                public float[] world_position = new float[3]{ float.NaN, float.NaN, float.NaN };

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
                            + "\n - C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_side[" 
                                + apollon.ApollonEngine.GetEnumDescription(value.side)
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
        
        // Question : Control usage
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

        // Question : Control feeling
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

        // Question : acceptability re-usage
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

        // Question : acceptability statisfaction 
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

        // Question : colaboration efficiency
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

        } /* class PhaseIResults */

         // Question : colaboration useful
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

        // Question : fluid interaction
        public class PhaseKResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseKResults */

         // Question : system contribution to fluidity
        public class PhaseLResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseLResults */

         // Question : communication efficiency
        public class PhaseMResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseMResults */

         // Question : communication return
        public class PhaseNResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseNResults */

         // Question : usability
        public class PhaseOResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhaseOResults */

         // Question : system confidence
        public class PhasePResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            public float user_response_timing_unity_timestamp = float.NaN;

            public string user_response_timing_host_timestamp = "";

            public long user_response_timing_varjo_timestamp = -1;

            public float user_response_value = float.NaN;

            public long user_elapsed_ms_since_entry = -1;

            #endregion

        } /* class PhasePResults */

        // experimental protocol
        public TrialResults Trial { get; set; } = new(); 
        public PhaseAResults PhaseA { get; set; } = new(); 
        public PhaseBResults PhaseB { get; set; } = new();
        public PhaseCResults PhaseC { get; set; } = new();
        public PhaseDResults PhaseD { get; set; } = new();

        // end trial questions
        public PhaseEResults PhaseE { get; set; } = new();
        public PhaseFResults PhaseF { get; set; } = new();
        public PhaseGResults PhaseG { get; set; } = new();
        public PhaseHResults PhaseH { get; set; } = new();

        // end bloc questions
        public PhaseIResults PhaseI { get; set; } = new();
        public PhaseJResults PhaseJ { get; set; } = new();
        public PhaseKResults PhaseK { get; set; } = new();
        public PhaseLResults PhaseL { get; set; } = new();
        public PhaseMResults PhaseM { get; set; } = new();
        public PhaseNResults PhaseN { get; set; } = new();
        public PhaseOResults PhaseO { get; set; } = new();
        public PhasePResults PhaseP { get; set; } = new();

        public bool ExportUXFResults(UXF.ResultsDictionary results)
        {

            // encapsulate
            try
            {

                // write the general trial settings as result for convenience
                results["pattern"]               = this.CurrentProfile.CurrentSettings.Trial.pattern_type;
                results["active_condition"]      = this.CurrentProfile.CurrentSettings.Trial.bIsActive.ToString();
                results["catch_try_condition"]   = this.CurrentProfile.CurrentSettings.Trial.bIsTryCatch.ToString();
                results["shared_intention_name"] = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.PhaseC.shared_intention_type);
                results["visual_name"]           = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.visual_type);
                results["scene_name"]            = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.scene_type);

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
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_side"]                   = apollon.ApollonEngine.GetEnumDescription(value.side);
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_unity_timestamp"] = value.timing_unity_timestamp.ToString();
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_host_timestamp"]  = value.timing_host_timestamp;
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_timing_varjo_timestamp"] = value.timing_varjo_timestamp.ToString();
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_local_position"]         = "[" + System.String.Join(";", value.local_position)    + "]";
                        results["C_run_" + run_index + "_user_checkpoint_" + index + "_crossing_world_position"]         = "[" + System.String.Join(";", value.world_position)    + "]";
                        
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
                results["F_user_elapsed_ms_since_entry"]          = this.PhaseF.user_elapsed_ms_since_entry.ToString();

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
                results["G_user_elapsed_ms_since_entry"]          = this.PhaseG.user_elapsed_ms_since_entry.ToString();

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
                results["H_user_elapsed_ms_since_entry"]          = this.PhaseH.user_elapsed_ms_since_entry.ToString();

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
                results["I_user_elapsed_ms_since_entry"]          = this.PhaseI.user_elapsed_ms_since_entry.ToString();

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
                results["J_user_elapsed_ms_since_entry"]          = this.PhaseJ.user_elapsed_ms_since_entry.ToString();

                // phase K
                results["K_timing_on_entry_unity_timestamp"]      = this.PhaseK.timing_on_entry_unity_timestamp.ToString();
                results["K_timing_on_exit_unity_timestamp"]       = this.PhaseK.timing_on_exit_unity_timestamp.ToString();
                results["K_timing_on_entry_host_timestamp"]       = this.PhaseK.timing_on_entry_host_timestamp;
                results["K_timing_on_exit_host_timestamp"]        = this.PhaseK.timing_on_exit_host_timestamp;
                results["K_timing_on_entry_varjo_timestamp"]      = this.PhaseK.timing_on_entry_varjo_timestamp.ToString();
                results["K_timing_on_exit_varjo_timestamp"]       = this.PhaseK.timing_on_exit_varjo_timestamp.ToString();     
                results["K_user_response_timing_unity_timestamp"] = this.PhaseK.user_response_timing_unity_timestamp.ToString();
                results["K_user_response_timing_host_timestamp"]  = this.PhaseK.user_response_timing_host_timestamp;
                results["K_user_response_timing_varjo_timestamp"] = this.PhaseK.user_response_timing_varjo_timestamp.ToString();
                results["K_user_response_value"]                  = this.PhaseK.user_response_value.ToString();
                results["K_user_elapsed_ms_since_entry"]          = this.PhaseK.user_elapsed_ms_since_entry.ToString();

                // phase L
                results["L_timing_on_entry_unity_timestamp"]      = this.PhaseL.timing_on_entry_unity_timestamp.ToString();
                results["L_timing_on_exit_unity_timestamp"]       = this.PhaseL.timing_on_exit_unity_timestamp.ToString();
                results["L_timing_on_entry_host_timestamp"]       = this.PhaseL.timing_on_entry_host_timestamp;
                results["L_timing_on_exit_host_timestamp"]        = this.PhaseL.timing_on_exit_host_timestamp;
                results["L_timing_on_entry_varjo_timestamp"]      = this.PhaseL.timing_on_entry_varjo_timestamp.ToString();
                results["L_timing_on_exit_varjo_timestamp"]       = this.PhaseL.timing_on_exit_varjo_timestamp.ToString();     
                results["L_user_response_timing_unity_timestamp"] = this.PhaseL.user_response_timing_unity_timestamp.ToString();
                results["L_user_response_timing_host_timestamp"]  = this.PhaseL.user_response_timing_host_timestamp;
                results["L_user_response_timing_varjo_timestamp"] = this.PhaseL.user_response_timing_varjo_timestamp.ToString();
                results["L_user_response_value"]                  = this.PhaseL.user_response_value.ToString();
                results["L_user_elapsed_ms_since_entry"]          = this.PhaseL.user_elapsed_ms_since_entry.ToString();

                // phase M
                results["M_timing_on_entry_unity_timestamp"]      = this.PhaseM.timing_on_entry_unity_timestamp.ToString();
                results["M_timing_on_exit_unity_timestamp"]       = this.PhaseM.timing_on_exit_unity_timestamp.ToString();
                results["M_timing_on_entry_host_timestamp"]       = this.PhaseM.timing_on_entry_host_timestamp;
                results["M_timing_on_exit_host_timestamp"]        = this.PhaseM.timing_on_exit_host_timestamp;
                results["M_timing_on_entry_varjo_timestamp"]      = this.PhaseM.timing_on_entry_varjo_timestamp.ToString();
                results["M_timing_on_exit_varjo_timestamp"]       = this.PhaseM.timing_on_exit_varjo_timestamp.ToString();     
                results["M_user_response_timing_unity_timestamp"] = this.PhaseM.user_response_timing_unity_timestamp.ToString();
                results["M_user_response_timing_host_timestamp"]  = this.PhaseM.user_response_timing_host_timestamp;
                results["M_user_response_timing_varjo_timestamp"] = this.PhaseM.user_response_timing_varjo_timestamp.ToString();
                results["M_user_response_value"]                  = this.PhaseM.user_response_value.ToString();
                results["M_user_elapsed_ms_since_entry"]          = this.PhaseM.user_elapsed_ms_since_entry.ToString();

                // phase N
                results["N_timing_on_entry_unity_timestamp"]      = this.PhaseN.timing_on_entry_unity_timestamp.ToString();
                results["N_timing_on_exit_unity_timestamp"]       = this.PhaseN.timing_on_exit_unity_timestamp.ToString();
                results["N_timing_on_entry_host_timestamp"]       = this.PhaseN.timing_on_entry_host_timestamp;
                results["N_timing_on_exit_host_timestamp"]        = this.PhaseN.timing_on_exit_host_timestamp;
                results["N_timing_on_entry_varjo_timestamp"]      = this.PhaseN.timing_on_entry_varjo_timestamp.ToString();
                results["N_timing_on_exit_varjo_timestamp"]       = this.PhaseN.timing_on_exit_varjo_timestamp.ToString();     
                results["N_user_response_timing_unity_timestamp"] = this.PhaseN.user_response_timing_unity_timestamp.ToString();
                results["N_user_response_timing_host_timestamp"]  = this.PhaseN.user_response_timing_host_timestamp;
                results["N_user_response_timing_varjo_timestamp"] = this.PhaseN.user_response_timing_varjo_timestamp.ToString();
                results["N_user_response_value"]                  = this.PhaseN.user_response_value.ToString();
                results["N_user_elapsed_ms_since_entry"]          = this.PhaseN.user_elapsed_ms_since_entry.ToString();

                // phase O
                results["O_timing_on_entry_unity_timestamp"]      = this.PhaseO.timing_on_entry_unity_timestamp.ToString();
                results["O_timing_on_exit_unity_timestamp"]       = this.PhaseO.timing_on_exit_unity_timestamp.ToString();
                results["O_timing_on_entry_host_timestamp"]       = this.PhaseO.timing_on_entry_host_timestamp;
                results["O_timing_on_exit_host_timestamp"]        = this.PhaseO.timing_on_exit_host_timestamp;
                results["O_timing_on_entry_varjo_timestamp"]      = this.PhaseO.timing_on_entry_varjo_timestamp.ToString();
                results["O_timing_on_exit_varjo_timestamp"]       = this.PhaseO.timing_on_exit_varjo_timestamp.ToString();     
                results["O_user_response_timing_unity_timestamp"] = this.PhaseO.user_response_timing_unity_timestamp.ToString();
                results["O_user_response_timing_host_timestamp"]  = this.PhaseO.user_response_timing_host_timestamp;
                results["O_user_response_timing_varjo_timestamp"] = this.PhaseO.user_response_timing_varjo_timestamp.ToString();
                results["O_user_response_value"]                  = this.PhaseO.user_response_value.ToString();
                results["O_user_elapsed_ms_since_entry"]          = this.PhaseO.user_elapsed_ms_since_entry.ToString();

                // phase P
                results["P_timing_on_entry_unity_timestamp"]      = this.PhaseP.timing_on_entry_unity_timestamp.ToString();
                results["P_timing_on_exit_unity_timestamp"]       = this.PhaseP.timing_on_exit_unity_timestamp.ToString();
                results["P_timing_on_entry_host_timestamp"]       = this.PhaseP.timing_on_entry_host_timestamp;
                results["P_timing_on_exit_host_timestamp"]        = this.PhaseP.timing_on_exit_host_timestamp;
                results["P_timing_on_entry_varjo_timestamp"]      = this.PhaseP.timing_on_entry_varjo_timestamp.ToString();
                results["P_timing_on_exit_varjo_timestamp"]       = this.PhaseP.timing_on_exit_varjo_timestamp.ToString();     
                results["P_user_response_timing_unity_timestamp"] = this.PhaseP.user_response_timing_unity_timestamp.ToString();
                results["P_user_response_timing_host_timestamp"]  = this.PhaseP.user_response_timing_host_timestamp;
                results["P_user_response_timing_varjo_timestamp"] = this.PhaseP.user_response_timing_varjo_timestamp.ToString();
                results["P_user_response_value"]                  = this.PhaseP.user_response_value.ToString();
                results["P_user_elapsed_ms_since_entry"]          = this.PhaseP.user_elapsed_ms_since_entry.ToString();

            } 
            catch(System.Exception ex)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Info: </color> LEXIKHUMOATResults.ExportUXFResults() : failed to export results with error ["
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
                "<color=Blue>Info: </color> LEXIKHUMOATResults.ExportUXFResults() : exported current results"
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
                + "\n - shared_intention_name"                     
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.PhaseC.shared_intention_type)
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
                + "\n - K_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseK.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - K_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseK.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - K_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseK.timing_on_entry_host_timestamp
                    + "]"
                + "\n - K_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseK.timing_on_exit_host_timestamp
                    + "]"
                + "\n - K_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseK.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - K_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseK.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - K_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseK.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - K_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseK.user_response_timing_host_timestamp
                    + "]"
                + "\n - K_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseK.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - K_user_response_value"  
                    + "[" 
                    + this.PhaseK.user_response_value.ToString()
                    + "]"
                + "\n - K_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseK.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - L_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseL.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - L_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseL.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - L_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseL.timing_on_entry_host_timestamp
                    + "]"
                + "\n - L_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseL.timing_on_exit_host_timestamp
                    + "]"
                + "\n - L_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseL.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - L_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseL.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - L_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseL.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - L_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseL.user_response_timing_host_timestamp
                    + "]"
                + "\n - L_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseL.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - L_user_response_value"  
                    + "[" 
                    + this.PhaseL.user_response_value.ToString()
                    + "]"
                + "\n - L_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseL.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - M_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseM.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - M_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseM.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - M_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseM.timing_on_entry_host_timestamp
                    + "]"
                + "\n - M_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseM.timing_on_exit_host_timestamp
                    + "]"
                + "\n - M_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseM.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - M_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseM.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - M_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseM.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - M_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseM.user_response_timing_host_timestamp
                    + "]"
                + "\n - M_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseM.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - M_user_response_value"  
                    + "[" 
                    + this.PhaseM.user_response_value.ToString()
                    + "]"
                + "\n - M_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseM.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - N_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseN.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - N_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseN.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - N_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseN.timing_on_entry_host_timestamp
                    + "]"
                + "\n - N_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseN.timing_on_exit_host_timestamp
                    + "]"
                + "\n - N_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseN.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - N_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseN.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - N_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseN.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - N_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseN.user_response_timing_host_timestamp
                    + "]"
                + "\n - N_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseN.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - N_user_response_value"  
                    + "[" 
                    + this.PhaseN.user_response_value.ToString()
                    + "]"
                + "\n - N_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseN.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - O_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseO.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - O_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseO.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - O_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseO.timing_on_entry_host_timestamp
                    + "]"
                + "\n - O_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseO.timing_on_exit_host_timestamp
                    + "]"
                + "\n - O_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseO.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - O_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseO.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - O_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseO.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - O_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseO.user_response_timing_host_timestamp
                    + "]"
                + "\n - O_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseO.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - O_user_response_value"  
                    + "[" 
                    + this.PhaseO.user_response_value.ToString()
                    + "]"
                + "\n - O_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseO.user_elapsed_ms_since_entry.ToString()
                    + "]"
                + "\n - P_timing_on_entry_unity_timestamp" 
                    + "[" 
                    + this.PhaseP.timing_on_entry_unity_timestamp.ToString()
                    + "]"
                + "\n - P_timing_on_exit_unity_timestamp"  
                    + "[" 
                    + this.PhaseP.timing_on_exit_unity_timestamp.ToString()
                    + "]"
                + "\n - P_timing_on_entry_host_timestamp"  
                    + "[" 
                    + this.PhaseP.timing_on_entry_host_timestamp
                    + "]"
                + "\n - P_timing_on_exit_host_timestamp"   
                    + "[" 
                    + this.PhaseP.timing_on_exit_host_timestamp
                    + "]"
                + "\n - P_timing_on_entry_varjo_timestamp" 
                    + "[" 
                    + this.PhaseP.timing_on_entry_varjo_timestamp.ToString()
                    + "]"
                + "\n - P_timing_on_exit_varjo_timestamp"  
                    + "[" 
                    + this.PhaseP.timing_on_exit_varjo_timestamp.ToString()
                    + "]"
                + "\n - P_user_response_timing_unity_timestamp"  
                    + "[" 
                    + this.PhaseP.user_response_timing_unity_timestamp.ToString()
                    + "]"
                + "\n - P_user_response_timing_host_timestamp"  
                    + "[" 
                    + this.PhaseP.user_response_timing_host_timestamp
                    + "]"
                + "\n - P_user_response_timing_varjo_timestamp"  
                    + "[" 
                    + this.PhaseP.user_response_timing_varjo_timestamp.ToString()
                    + "]"
                + "\n - P_user_response_value"  
                    + "[" 
                    + this.PhaseP.user_response_value.ToString()
                    + "]"
                + "\n - P_user_elapsed_ms_since_entry"  
                    + "[" 
                    + this.PhaseP.user_elapsed_ms_since_entry.ToString()
                    + "]"
            );

        } /* LogUXFResults() */

    } /* class LEXIKHUMOATResults */
    
} /* } Labsim.experiment.LEXIKHUM_OAT */