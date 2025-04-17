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
using System.Linq;

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
                {"LEXIKHUMOATTag_LeftSuccess",    (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success  )},
                {"LEXIKHUMOATTag_LeftFail",       (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail     )},
                {"LEXIKHUMOATTag_LeftCue",        (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue      )},
                {"LEXIKHUMOATTag_LeftStrongCue",  (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left,   LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue)},
                {"LEXIKHUMOATTag_RightSuccess",   (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success  )},
                {"LEXIKHUMOATTag_RightFail",      (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail     )},
                {"LEXIKHUMOATTag_RightCue",       (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue      )},
                {"LEXIKHUMOATTag_RightStrongCue", (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right,  LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue)},
                {"LEXIKHUMOATTag_Departure",      (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center, LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Departure)},
                {"LEXIKHUMOATTag_Arrival",        (LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center, LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival  )}
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

            // finally reoder Slalom by checkpoints "deepness" 
            // --> see. IComparable concept implementation in LEXIKHUMOATCheckpointBehaviour
            this.Slalom.Sort((x,y) => x.CompareTo(y));

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
                    // + "\n - timing_varjo_timestamp[" 
                    //     + checkpoint.timing_varjo_timestamp.ToString()
                    // + "]"
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
                    // + "\n - timing_varjo_timestamp[" 
                    //     + checkpoint.timing_varjo_timestamp.ToString()
                    // + "]"
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

            // get an ISIR backend reference
            var backend 
                = apollon.backend.ApollonBackendManager.Instance.GetValidHandle(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
                ) as apollon.backend.handle.ApollonISIRForceDimensionOmega3Handle;

            // update ISIR backend with current & next
            var current_behaviour
                = this.Slalom.Find(item => item.Equals(checkpoint));
            var bLast = this.Slalom.Last().Equals(current_behaviour);
            var next_behaviour
                = bLast
                ? current_behaviour
                : this.Slalom[
                    this.Slalom.FindIndex(
                        /* start index == next one */ 
                        this.Slalom.FindIndex(item => item.Equals(current_behaviour)) + 1,
                        /* predicate */
                        item => (
                            (item.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue)
                            || (item.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue)
                            || (item.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success)
                            || (item.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival)
                        )
                    )
                ];

            if( (current_behaviour != null) 
                && (next_behaviour != null)
            )
            {
                
                // ok, update backend ref data for downstream
                backend.NextGateKind
                    = apollon.ApollonEngine.GetEnumDescription(
                        bLast 
                        ? LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Undefined
                        : next_behaviour.CheckpointKind
                    );
                backend.NextGateSide
                    = apollon.ApollonEngine.GetEnumDescription(
                        bLast 
                        ? LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Undefined
                        : next_behaviour.CheckpointSide
                    );
                // iff. pos. offset ?
                backend.NextGateOffset
                    = UnityEngine.Mathf.RoundToInt(
                        UnityEngine.Mathf.Clamp(
                            (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                                .CurrentSettings
                                .PhaseC
                                .shared_intention_offset, 
                            /* extract only positive value or clamp to 0 */
                            .0f, 
                            float.MaxValue
                        )
                    ).ToString("0000");
                backend.SharedIntentionMode
                    = apollon.ApollonEngine.GetEnumDescription(
                            bLast 
                            ? LEXIKHUMOATSettings.SharedIntentionIDType.Undefined
                            : (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                                .CurrentSettings
                                .PhaseC
                                .shared_intention_type
                        );

                // depend on next target
                if(bLast)
                {

                    // reset to default
                    backend.NextGateWorldPosition = new(0.0f, 0.0f, 0.0f);
                    backend.NextGateWidth         = 0.0f;

                }
                else if(next_behaviour.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success)
                {
                    
                    // Obstacle base prefab structure :
                    // ================================
                    // +- [prefab]
                    // | +- offset
                    // | | +- Cylinder [3DObj]         <==
                    // | | +- ExternalCylinder [3DObj] <==
                    // | | +- Cue [Collider]
                    // | | +- Left [Collider]
                    // | | +- Right [Collider] 
                    // | +
                    // +
                    var center_cylinder   = next_behaviour.transform.parent.Find("Cylinder");
                    var external_cylinder = next_behaviour.transform.parent.Find("ExternalCylinder");

                    if( (center_cylinder != null) 
                        && (external_cylinder != null)
                    )
                    {

                        // find gate center position & width
                        backend.NextGateWorldPosition       
                            = UnityEngine.Vector3.Lerp(
                                center_cylinder.position, 
                                external_cylinder.position, 
                                0.5f
                            );
                        backend.NextGateWidth 
                            = UnityEngine.Mathf.Abs(
                                UnityEngine.Vector3.Distance(
                                    center_cylinder.position,
                                    external_cylinder.position
                                )
                            );
                            
                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : error, could not find gate reference object... ( center:"
                            + center_cylinder != null ? "found" : "not_found"
                            + ", external:"
                            + external_cylinder != null ? "found" : "not_found"
                            + ")"
                        );
                            
                    } /* if() */
                
                }
                else if(next_behaviour.CheckpointKind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival)
                {
                    
                    // Arrival/Departure prefab structure :
                    // ================================
                    // +- [prefab]
                    // | +- offset
                    // | | +- Plane [Collider] <==
                    // | +
                    // +
                    var plane = next_behaviour.transform.parent.Find("Plane");

                    if(plane != null)
                    {

                        var profile 
                            = (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile);
                        var offset 
                            = UnityEngine.Mathf.Clamp(
                                profile.CurrentSettings.PhaseC.shared_intention_offset, 
                                /* extract only positive value or clamp to 0 */
                                .0f, 
                                float.MaxValue
                            ) / 1000.0f
                            * UnityEngine.Mathf.Clamp(
                                /* z */ 
                                profile.CurrentSettings.PhaseB.linear_acceleration_target[2] * (profile.CurrentSettings.PhaseB.acceleration_duration / 1000.0f)
                                .0f, 
                                /* cap at max linear z speed  */ 
                                profile.CurrentSettings.PhaseB.linear_velocity_saturation_threshold[2]
                            );

                        // so center it at (z + offset) deepness & infinite width
                        backend.NextGateWorldPosition       
                            = new(
                                0.0f, 
                                0.0f, 
                                plane.position.z + offset
                            );
                        backend.NextGateWidth 
                            = float.PositiveInfinity;
                            
                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : error, could not find Plane reference object..."
                        );
                            
                    } /* if() */
                
                }
                else
                {
                
                    // Obstacle base prefab structure :
                    // ================================
                    // +- [prefab]
                    // | +- offset
                    // | | +- Cylinder [3DObj]
                    // | | +- ExternalCylinder [3DObj]
                    // | | +- Cue [Collider]           <==
                    // | | +- Left [Collider]
                    // | | +- Right [Collider] 
                    // | +
                    // +
                    var cue = next_behaviour.transform.parent.Find("Cue");

                    if(cue != null)
                    {

                        // so center it at z deepness & infinite width
                        backend.NextGateWorldPosition       
                            = new(0.0f, 0.0f, cue.position.z);
                        backend.NextGateWidth 
                            = float.PositiveInfinity;
                            
                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : error, could not find Cue reference object..."
                        );
                            
                    } /* if() */

                } /* if() */

            }
            else
            {
                
                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATCheckpointManagerBehaviour.OnCheckpointReached() : error, one of the raised behaviour or child was not found in current slalom... ( current:" 
                    + current_behaviour != null ? "found" : "not_found"
                    + ", next:"
                    + next_behaviour != null ? "found" : "not_found"
                    + ")"
                );

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

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
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
