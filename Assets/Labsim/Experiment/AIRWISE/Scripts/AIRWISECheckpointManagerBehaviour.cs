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
        private long m_currentSuccess = 0;
        public long CurrentSuccess => this.m_currentSuccess;

        [UnityEngine.SerializeField]
        private long m_currentCount = 0;
        public long CurrentCount => this.m_currentCount;

        [UnityEngine.SerializeField]
        public float CurrentPerformance
            => (this.CurrentSuccess / this.CurrentCount) * 100.0f;

        [UnityEngine.SerializeField]
        public long TotalCount 
            => this.Slalom.Count 
            - /* departure + arrival */ 2;

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
            foreach(var (key, value) in c_tags_dict)
            {

                // try find objects in loaded scene
                foreach(var current_checkpoint in UnityEngine.GameObject.FindGameObjectsWithTag(key))
                {

                    // instantiate a fresh behaviour
                    var behaviour = current_checkpoint.AddComponent<AIRWISECheckpointBehaviour>();

                    // configure it
                    behaviour.CheckpointType = value;
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

        private void OnCheckpointReached(AIRWISEResults.PhaseCResults.Checkpoint checkpoint)
        {
            
            // simply add it to current results
            (apollon.experiment.ApollonExperimentManager.Instance.Profile as AIRWISEProfile)
                .CurrentResults
                .PhaseC
                .user_checkpoints_crossing[this.CurrentCount]
                = checkpoint;

            // update datas & raise events 
            this.m_currentCount++;
            switch(checkpoint.kind)
            {
                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.GateSuccess:
                {
                    this.m_currentSuccess++; 
                    break;
                }
                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Arrival:
                {
                    this.slalomEnded?.Invoke(this, this.CurrentPerformance); 
                    break;
                }
                case AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Departure:
                {
                    this.slalomStarted?.Invoke(this, null); 
                    break;
                }
            }

        } /* OnCheckpointReached() */

        #endregion

    } /* public class AIRWISECheckpointManagerBehaviour */

} /* } Labsim.experiment.AIRWISE */
