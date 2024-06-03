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
using Unity.Logging.Sinks;

namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATCheckpointBehaviour
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        public LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType CheckpointKind { get; set; } = LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Undefined;
        
        [UnityEngine.SerializeField]
        public LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType CheckpointSide { get; set; } = LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Undefined;
        
        [UnityEngine.SerializeField]
        public LEXIKHUMOATCheckpointManagerBehaviour Manager { get; set; } = null;

        #region System action
        
        public event System.Action<string, LEXIKHUMOATResults.PhaseCResults.Checkpoint> checkpointReached;

        #endregion

        #region Unity Monobehaviour events

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            
            // skip unnecessary stuff by checking if collidee has LEXIKHUMOATEntityBehaviour component
            if(!other.gameObject.GetComponent<LEXIKHUMOATEntityBehaviour>())
            {
                return;   
            }

            // extract impact point
            UnityEngine.Vector3
                world_pos = other.ClosestPoint(this.transform.position),
                local_pos = this.transform.worldToLocalMatrix * other.ClosestPoint(world_pos);

            // build dict entry name
            var name = this.gameObject.transform.parent.parent.name;
            if(this.gameObject.name == "Cue") 
            {
                name += "_Cue";
            }
            // else if(this.gameObject.name == "StrongCue") 
            // {
            //     name += "_StrongCue";
            // }

            // invoke with current settings
            checkpointReached.Invoke(
                name,
                new() 
                {
                    kind                   = this.CheckpointKind,
                    side                   = this.CheckpointSide,
                    timing_unity_timestamp = UnityEngine.Time.time,
                    timing_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString(),
                    timing_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp(),
                    world_position         = new float[3]{ world_pos.x, world_pos.y, world_pos.z },
                    local_position         = new float[3]{ local_pos.x, local_pos.y, local_pos.z }
                }
            );

        } /* OnTriggerEnter() */

        #endregion

    } /* public class LEXIKHUMOATCheckpointBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
