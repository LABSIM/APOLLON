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

    public class AIRWISECheckpointManagerBehaviour
        : UnityEngine.MonoBehaviour
    {

        private readonly static System.Collections.Generic.Dictionary<string, AIRWISEResults.PhaseCResults.Checkpoint.KindIDType> c_tags_dict
            = new()
            {
                {"AIRWISETag_Gates",      AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.GateSuccess },
                {"AIRWISETag_LeftGates",  AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.LeftFail    },
                {"AIRWISETag_RightGates", AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.RightFail   },
                {"AIRWISETag_Departure",  AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Departure   },
                {"AIRWISETag_Arrival",    AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Arrival     }
            };

        [UnityEngine.SerializeField]
        private System.Collections.Generic.List<AIRWISECheckpointBehaviour> m_slalom = new();
        public System.Collections.Generic.List<AIRWISECheckpointBehaviour> Slalom => this.m_slalom;

        [UnityEngine.SerializeField]
        private int m_currentSuccess = 0;
        public int CurrentSuccess => this.m_currentSuccess;

        [UnityEngine.SerializeField]
        private int m_currentCount = 0;
        public int CurrentCount => this.m_currentCount;

        [UnityEngine.SerializeField]
        public float CurrentPerformance
            => (float)this.CurrentSuccess / (float)this.CurrentCount * 100.0f;

        // [UnityEngine.SerializeField]
        // public int TotalCount 
        //     => this.Slalom.Count 
        //     - /* departure + arrival */ 2;

        [UnityEngine.SerializeField]
        private AIRWISESoundBehaviour m_soundBehaviour = null;

        #region System action
        
        public event System.EventHandler<float> slalomEnded;
        public event System.EventHandler slalomStarted;

        #endregion
        
        #region Unity Monobehaviour methods

        private void Awake()
        {
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded   += this.OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += this.OnSceneUnloaded;

        } /* Awake() */

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {

            // instantiate all components on marked objects
            foreach(var (key, checkpoint) in c_tags_dict)
            {

                // try find objects in loaded scene
                foreach(var current_checkpoint in UnityEngine.GameObject.FindGameObjectsWithTag(key))
                {

                    // instantiate a fresh behaviour
                    var behaviour = current_checkpoint.AddComponent<AIRWISECheckpointBehaviour>();

                    // configure it
                    behaviour.CheckpointType = checkpoint;
                    behaviour.Manager        = this;

                    // subscribe
                    behaviour.checkpointReached += this.OnCheckpointReached;

                    // save
                    this.Slalom.Add(behaviour);

                } /* foreach() */

            } /* foreach() */            

        } /* OnSceneLoaded*() */

        private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {

            // destroy all checkpoint refs
            foreach(var checkpoint in this.Slalom)
            {
                
                // unsubscribe
                checkpoint.checkpointReached -= this.OnCheckpointReached;

                // mark as destroyable
                UnityEngine.GameObject.Destroy(checkpoint);
   
            } /* foreach() */

            // cleanup
            this.Slalom.Clear();
            this.m_currentCount 
                = this.m_currentSuccess 
                = 0;

        } /* OnSceneUnloaded*() */

        private void OnCheckpointReached(string parent_name, AIRWISEResults.PhaseCResults.Checkpoint checkpoint)
        {

            // check if Departure and if already logged
            // if (checkpoint.kind == AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Departure)
            // {
            //     if ((checkpoints as System.Collections.Generic.List<AIRWISEResults.PhaseCResults.Checkpoint>).Count != 0) 
            //     {
            //         if (checkpoints[0] != null) 
            //         {
            //             if (checkpoints[0].kind == AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Departure) 
            //             {
            //                 return;
            //             }
            //         }
            //     }
            // }

            // shorcuts
            var checkpoints 
                = (apollon.experiment.ApollonExperimentManager.Instance.Profile as AIRWISEProfile)
                    .CurrentResults
                    .PhaseC
                    .user_checkpoints_crossing;
            var current_run
                = (apollon.experiment.ApollonExperimentManager.Instance.Profile as AIRWISEProfile)
                    .CurrentResults
                    .Trial
                    .user_performance_try_count;

            // check if it's a new run 
            if(checkpoints.Count - 1 <= current_run)
            {

                // add a new entry
                checkpoints.Add(new());
                checkpoints.TrimExcess();

                // reset counter 
                this.m_currentCount 
                    = this.m_currentSuccess 
                    = 0;

            }  /* if() */

            // check dictionary entries first, then queue it to current collecton & update results if requested
            bool bSkip = false;
            if(checkpoints[(int)current_run].TryAdd(parent_name, new()))
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISECheckpointManagerBehaviour.OnCheckpointReached() : run ["
                    + current_run
                    + "] new entry  ["
                    + parent_name
                    + "], instantiate & queue result :"
                    + "\n - crossing_kind[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.kind)
                    + "]"
                    + "\n - timing_unity_timestamp[" 
                        + checkpoint.timing_unity_timestamp.ToString()
                    + "]"
                    + "\n - timing_host_timestamp[" 
                        + checkpoint.timing_host_timestamp
                    + "]"
                    + "\n - timing_varjo_timestamp[" 
                        + checkpoint.timing_varjo_timestamp.ToString()
                    + "]"
                    + "\n - local_position[" 
                        + "[" + System.String.Join(";", checkpoint.local_position) + "]"
                    + "]"
                    + "\n - world_position[" 
                        + "[" + System.String.Join(";", checkpoint.world_position) + "]"
                    + "]"
                    + "\n - aero_position[" 
                        + "[" + System.String.Join(";", checkpoint.aero_position) + "]"
                    + "]"
                );

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warn: </color> AIRWISECheckpointManagerBehaviour.OnCheckpointReached() : run ["
                    + current_run
                    + "] entry  ["
                    + parent_name
                    + "] already found, only queue current result but it will be ignored :"
                    + "\n - crossing_kind[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.kind)
                    + "]"
                    + "\n - timing_unity_timestamp[" 
                        + checkpoint.timing_unity_timestamp.ToString()
                    + "]"
                    + "\n - timing_host_timestamp[" 
                        + checkpoint.timing_host_timestamp
                    + "]"
                    + "\n - timing_varjo_timestamp[" 
                        + checkpoint.timing_varjo_timestamp.ToString()
                    + "]"
                    + "\n - local_position[" 
                        + "[" + System.String.Join(";", checkpoint.local_position) + "]"
                    + "]"
                    + "\n - world_position[" 
                        + "[" + System.String.Join(";", checkpoint.world_position) + "]"
                    + "]"
                    + "\n - aero_position[" 
                        + "[" + System.String.Join(";", checkpoint.aero_position) + "]"
                    + "]"
                );

                // raise skip flag
                bSkip = true;

            } /* if() */

            checkpoints[(int)current_run][parent_name].Enqueue(checkpoint);

            // check
            if(bSkip)
            {

                // do not update result as it is a double
                return;
            
            } /* if() */

            // update data & raise events 
            switch(checkpoint.kind)
            {

                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Departure:
                {

                    // propagate event
                    this.slalomStarted?.Invoke(this, null); 

                    break;

                } /* case Departure */ 

                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Arrival:
                {

                    // propagate event
                    this.slalomEnded?.Invoke(this, this.CurrentPerformance);

                    break;

                } /* case Arrival */ 

                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.GateSuccess:
                {

                    // update counters
                    this.m_currentCount++;
                    this.m_currentSuccess++; 

                    // play sound 
                    if(apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentBlock.settings.GetBool("is_practice_condition"))
                    {
                    
                        this.m_soundBehaviour?.PlaySuccessClip();

                    } /* if() */ 

                    break;

                } /* case GateSuccess */ 

                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.LeftFail:
                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.RightFail:
                {
                    
                    // update counter
                    this.m_currentCount++;

                    // play sound 
                    if(apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentBlock.settings.GetBool("is_practice_condition"))
                    {

                        this.m_soundBehaviour?.PlayFailureClip();

                    } /* if() */ 

                    break;

                } /* case LeftFail || RightFail */ 

                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Undefined:
                default:
                {
                    
                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISECheckpointManagerBehaviour.OnCheckpointReached() :  checkpoint.kind is Undefined or Unknown... this is impossible ! :)"
                    );

                    break;

                } /* default || case Undefined */ 

            } /* switch() */

        } /* OnCheckpointReached() */

        #endregion

    } /* public class AIRWISECheckpointManagerBehaviour */

} /* } Labsim.experiment.AIRWISE */
