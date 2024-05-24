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

    public class LEXIKHUMOATCheckpointManagerBehaviour
        : UnityEngine.MonoBehaviour
    {

        private readonly static System.Collections.Generic.Dictionary<
            /* unity tag      */ string, 
            /* pair side/kind */ (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType,LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType)
        > c_tags_dict
            = new()
            {
                {"LEXIKHUMOATTag_LeftSuccess",  (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success  )},
                {"LEXIKHUMOATTag_LeftFail",     (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail     )},
                {"LEXIKHUMOATTag_LeftCue",      (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue      )},
                {"LEXIKHUMOATTag_RightSuccess", (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success  )},
                {"LEXIKHUMOATTag_RightFail",    (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail     )},
                {"LEXIKHUMOATTag_RightCue",     (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue      )},
                {"LEXIKHUMOATTag_Departure",    (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center, LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Departure)},
                {"LEXIKHUMOATTag_Arrival",      (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center, LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival  )}
            };

        [UnityEngine.SerializeField]
        private System.Collections.Generic.List<LEXIKHUMOATCheckpointBehaviour> m_slalom = new();
        public System.Collections.Generic.List<LEXIKHUMOATCheckpointBehaviour> Slalom => this.m_slalom;

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

        // [UnityEngine.SerializeField]
        // private LEXIKHUMOATSoundBehaviour m_soundBehaviour = null;

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

            // refs
            var cue_manager 
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour
                .References["EntityTag_Cues"]
                .GetComponent<LEXIKHUMOATCueManagerBehaviour>(); 

            // instantiate all components on marked objects
            foreach(var (key, (side, kind)) in c_tags_dict)
            {

                // try find objects in loaded scene
                foreach(var current_checkpoint in UnityEngine.GameObject.FindGameObjectsWithTag(key))
                {

                    // instantiate a fresh behaviour
                    var behaviour = current_checkpoint.AddComponent<LEXIKHUMOATCheckpointBehaviour>();

                    // configure it
                    behaviour.CheckpointSide = side;
                    behaviour.CheckpointKind = kind;
                    behaviour.Manager        = this;

                    // subscribe current & cue manager
                    behaviour.checkpointReached += this.OnCheckpointReached;
                    behaviour.checkpointReached += cue_manager.OnCheckpointReached;

                    // save
                    this.Slalom.Add(behaviour);

                } /* foreach() */

            } /* foreach() */            

        } /* OnSceneLoaded*() */

        private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {

            // refs
            var cue_manager 
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour
                .References["EntityTag_Cues"]
                .GetComponent<LEXIKHUMOATCueManagerBehaviour>(); 

            // destroy all checkpoint refs
            foreach(var checkpoint in this.Slalom)
            {
                
                // unsubscribe
                checkpoint.checkpointReached -= this.OnCheckpointReached;
                checkpoint.checkpointReached -= cue_manager.OnCheckpointReached;

                // mark as destroyable
                UnityEngine.GameObject.Destroy(checkpoint);
   
            } /* foreach() */

            // cleanup
            this.Slalom.Clear();
            this.m_currentCount 
                = this.m_currentSuccess 
                = 0;

        } /* OnSceneUnloaded*() */

        private void OnCheckpointReached(string parent_name, LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // shorcuts
            var checkpoints 
                = (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                    .CurrentResults
                    .PhaseC
                    .user_checkpoints_crossing;
            var current_run
                = (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
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
                    "<color=Blue>Info: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : run ["
                    + current_run
                    + "] new entry  ["
                    + parent_name
                    + "], instantiate & queue result :"
                    + "\n - crossing_kind[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.kind)
                    + "]"
                    + "\n - crossing_side[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.side)
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
                );

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warn: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : run ["
                    + current_run
                    + "] entry  ["
                    + parent_name
                    + "] already found, only queue current result but it will be ignored :"
                    + "\n - crossing_kind[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.kind)
                    + "]"
                    + "\n - crossing_side[" 
                        + apollon.ApollonEngine.GetEnumDescription(checkpoint.side)
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
            switch((checkpoint.kind, checkpoint.side))
            {

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Departure, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center):
                {

                    // propagate event
                    this.slalomStarted?.Invoke(this, null); 

                    break;

                } /* case (Departure, _) */

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center):
                {

                    // propagate event
                    this.slalomEnded?.Invoke(this, this.CurrentPerformance);

                    break;

                } /* case (Arrival, _) */

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                {

                    // update counters
                    this.m_currentCount++;
                    this.m_currentSuccess++;

                    break;

                } /* case (Success, _) */

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                {
                    
                    // update counter
                    this.m_currentCount++;

                    break;

                } /* case (Fail, _) */

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                {

                    // skip
                    break;

                } /* case (Cue, _) */

                default:
                {
                    
                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : (" 
                        + checkpoint.kind
                        + ","
                        + checkpoint.side
                        + ")... this is impossible ! :)"
                    );

                    break;

                } /* default || case Undefined */ 

            } /* switch() */

        } /* OnCheckpointReached() */

        #endregion

    } /* public class LEXIKHUMOATCheckpointManagerBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
