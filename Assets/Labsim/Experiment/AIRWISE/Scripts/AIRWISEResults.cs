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

    public class AIRWISEResults
    {

        private AIRWISEProfile CurrentProfile { get; set; } = null;
        public AIRWISEResults(AIRWISEProfile profile)
        {
            this.CurrentProfile = profile;
        }

        public class DefaultPhaseTimingResults
        {

            #region timing_*

            public long 
                timing_on_entry_varjo_timestamp,
                timing_on_exit_varjo_timestamp;

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

            #endregion

        } /* class PhaseCResults */
        
        public class PhaseDResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseDResult */
        
        public class PhaseEResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseEResults */
        
        public class PhaseFResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseFResults */
        
        public class PhaseGResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseGResults */
        
        public class PhaseHResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseHResults */
        
        public class PhaseIResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseIResult */
        
        public class PhaseJResults 
            : DefaultPhaseTimingResults
        {

            #region user_*

            #endregion

        } /* class PhaseJResults */

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
                results["scenario_name"]       = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.pattern_type);
                results["scene_name"]          = apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.scene_type);

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

                // phase D
                results["D_timing_on_entry_unity_timestamp"] = this.PhaseD.timing_on_entry_unity_timestamp.ToString();
                results["D_timing_on_exit_unity_timestamp"]  = this.PhaseD.timing_on_exit_unity_timestamp.ToString();
                results["D_timing_on_entry_host_timestamp"]  = this.PhaseD.timing_on_entry_host_timestamp;
                results["D_timing_on_exit_host_timestamp"]   = this.PhaseD.timing_on_exit_host_timestamp;
                results["D_timing_on_entry_varjo_timestamp"] = this.PhaseD.timing_on_entry_varjo_timestamp.ToString();
                results["D_timing_on_exit_varjo_timestamp"]  = this.PhaseD.timing_on_exit_varjo_timestamp.ToString();

                // phase E
                results["E_timing_on_entry_unity_timestamp"] = this.PhaseE.timing_on_entry_unity_timestamp.ToString();
                results["E_timing_on_exit_unity_timestamp"]  = this.PhaseE.timing_on_exit_unity_timestamp.ToString();
                results["E_timing_on_entry_host_timestamp"]  = this.PhaseE.timing_on_entry_host_timestamp;
                results["E_timing_on_exit_host_timestamp"]   = this.PhaseE.timing_on_exit_host_timestamp;
                results["E_timing_on_entry_varjo_timestamp"] = this.PhaseE.timing_on_entry_varjo_timestamp.ToString();
                results["E_timing_on_exit_varjo_timestamp"]  = this.PhaseE.timing_on_exit_varjo_timestamp.ToString();

                // phase F
                results["F_timing_on_entry_unity_timestamp"] = this.PhaseF.timing_on_entry_unity_timestamp.ToString();
                results["F_timing_on_exit_unity_timestamp"]  = this.PhaseF.timing_on_exit_unity_timestamp.ToString();
                results["F_timing_on_entry_host_timestamp"]  = this.PhaseF.timing_on_entry_host_timestamp;
                results["F_timing_on_exit_host_timestamp"]   = this.PhaseF.timing_on_exit_host_timestamp;
                results["F_timing_on_entry_varjo_timestamp"] = this.PhaseF.timing_on_entry_varjo_timestamp.ToString();
                results["F_timing_on_exit_varjo_timestamp"]  = this.PhaseF.timing_on_exit_varjo_timestamp.ToString();

                // phase G
                results["G_timing_on_entry_unity_timestamp"] = this.PhaseG.timing_on_entry_unity_timestamp.ToString();
                results["G_timing_on_exit_unity_timestamp"]  = this.PhaseG.timing_on_exit_unity_timestamp.ToString();
                results["G_timing_on_entry_host_timestamp"]  = this.PhaseG.timing_on_entry_host_timestamp;
                results["G_timing_on_exit_host_timestamp"]   = this.PhaseG.timing_on_exit_host_timestamp;
                results["G_timing_on_entry_varjo_timestamp"] = this.PhaseG.timing_on_entry_varjo_timestamp.ToString();
                results["G_timing_on_exit_varjo_timestamp"]  = this.PhaseG.timing_on_exit_varjo_timestamp.ToString();

                // phase H
                results["H_timing_on_entry_unity_timestamp"] = this.PhaseH.timing_on_entry_unity_timestamp.ToString();
                results["H_timing_on_exit_unity_timestamp"]  = this.PhaseH.timing_on_exit_unity_timestamp.ToString();
                results["H_timing_on_entry_host_timestamp"]  = this.PhaseH.timing_on_entry_host_timestamp;
                results["H_timing_on_exit_host_timestamp"]   = this.PhaseH.timing_on_exit_host_timestamp;
                results["H_timing_on_entry_varjo_timestamp"] = this.PhaseH.timing_on_entry_varjo_timestamp.ToString();
                results["H_timing_on_exit_varjo_timestamp"]  = this.PhaseH.timing_on_exit_varjo_timestamp.ToString();

                // phase I
                results["I_timing_on_entry_unity_timestamp"] = this.PhaseI.timing_on_entry_unity_timestamp.ToString();
                results["I_timing_on_exit_unity_timestamp"]  = this.PhaseI.timing_on_exit_unity_timestamp.ToString();
                results["I_timing_on_entry_host_timestamp"]  = this.PhaseI.timing_on_entry_host_timestamp;
                results["I_timing_on_exit_host_timestamp"]   = this.PhaseI.timing_on_exit_host_timestamp;
                results["I_timing_on_entry_varjo_timestamp"] = this.PhaseI.timing_on_entry_varjo_timestamp.ToString();
                results["I_timing_on_exit_varjo_timestamp"]  = this.PhaseI.timing_on_exit_varjo_timestamp.ToString();

                // phase J
                results["J_timing_on_entry_unity_timestamp"] = this.PhaseJ.timing_on_entry_unity_timestamp.ToString();
                results["J_timing_on_exit_unity_timestamp"]  = this.PhaseJ.timing_on_exit_unity_timestamp.ToString();
                results["J_timing_on_entry_host_timestamp"]  = this.PhaseJ.timing_on_entry_host_timestamp;
                results["J_timing_on_exit_host_timestamp"]   = this.PhaseJ.timing_on_exit_host_timestamp;
                results["J_timing_on_entry_varjo_timestamp"] = this.PhaseJ.timing_on_entry_varjo_timestamp.ToString();
                results["J_timing_on_exit_varjo_timestamp"]  = this.PhaseJ.timing_on_exit_varjo_timestamp.ToString();

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
                + "\n - scenario_name"                     
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.pattern_type)
                    + "]"
                + "\n - scene_name"                        
                    + "[" 
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentProfile.CurrentSettings.Trial.scene_type)
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
            );

        } /* LogUXFResults() */

    } /* class AIRWISEResults */
    
} /* } Labsim.experiment.AIRWISE */