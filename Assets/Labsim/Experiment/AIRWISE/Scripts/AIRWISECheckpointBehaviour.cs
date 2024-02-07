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

    public class AIRWISECheckpointBehaviour
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        public AIRWISEResults.PhaseCResults.Checkpoint.KindIDType CheckpointType { get; set; } = AIRWISEResults.PhaseCResults.Checkpoint.KindIDType.Undefined;
        
        [UnityEngine.SerializeField]
        public AIRWISECheckpointManagerBehaviour Manager { get; set; } = null;

        #region System action
        
        public event System.Action<AIRWISEResults.PhaseCResults.Checkpoint> checkpointReached;

        #endregion

        #region Unity Monobehaviour events

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            
            // skip unnecessary stuff by checking if collidee has QuadController component
            if(!other.gameObject.GetComponent<QuadController>())
            {
                return;   
            }

            // extract impact point
            UnityEngine.Vector3
                world_pos = other.ClosestPoint(this.transform.position),
                local_pos = this.transform.worldToLocalMatrix * other.ClosestPoint(world_pos),
                aero_pos  = Utilities.ToUnityFromAeroFrame(world_pos);

            // invoke with current settings
            checkpointReached.Invoke( 
                new() 
                {
                    kind                   = this.CheckpointType,
                    timing_unity_timestamp = UnityEngine.Time.time,
                    timing_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString(),
                    timing_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp(),
                    world_position         = new float[3]{ world_pos.x, world_pos.y, world_pos.z },
                    local_position         = new float[3]{ local_pos.x, local_pos.y, local_pos.z }, 
                    aero_position          = new float[3]{ aero_pos.x,  aero_pos.y,  aero_pos.z  }
                }
            );

        } /* OnTriggerEnter() */

        #endregion

    } /* public class AIRWISECheckpointBehaviour */

} /* } Labsim.experiment.AIRWISE */
