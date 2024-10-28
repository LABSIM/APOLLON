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

    public class LEXIKHUMOATCheckpointBehaviour
        : UnityEngine.MonoBehaviour
        , System.IEquatable<LEXIKHUMOATCheckpointBehaviour>
        , System.IComparable<LEXIKHUMOATCheckpointBehaviour>
        , System.IEquatable<LEXIKHUMOATResults.PhaseCResults.Checkpoint>
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
                    // timing_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp(),
                    world_position         = new float[3]{ world_pos.x, world_pos.y, world_pos.z },
                    local_position         = new float[3]{ local_pos.x, local_pos.y, local_pos.z }
                }
            );

        } /* OnTriggerEnter() */

        #endregion

        #region IEquatable interface impl.

        public bool Equals(LEXIKHUMOATCheckpointBehaviour other)
        {

            return (
                (other != null)
                && (this.CheckpointKind == other.CheckpointKind)
                && (this.CheckpointSide == other.CheckpointSide)
                && (this.transform == other.transform)
                && (this.name == other.name)
            );

        } /* Equals(LEXIKHUMOATCheckpointBehaviour) */

        public bool Equals(LEXIKHUMOATResults.PhaseCResults.Checkpoint other)
        {

            const float _threshold = 0.5f; // 50 cm margin

            return (
                (other != null)
                && (this.CheckpointKind == other.kind)
                && (this.CheckpointSide == other.side)
                // because result checkpoint are intersection of entity with actual checkpoint behaviour, 
                // we only compare z plane distance
                // if we are close enough, say from a "_threshold" constant in S.I units, then it's ok
                && (
                    UnityEngine.Mathf.Abs(
                        UnityEngine.Mathf.Abs(this.transform.position.z) - UnityEngine.Mathf.Abs(other.world_position[2])
                    ) <= _threshold
                )
            );

        } /* Equals(LEXIKHUMOATResults.PhaseCResults.Checkpoint) */

        public override bool Equals(System.Object obj)
        {

            // try cast into indentical first
            var same = obj as LEXIKHUMOATCheckpointBehaviour;
            if(same != null)
            {
                return this.Equals(same);
            }

            // else, whatever
            return this.Equals(obj as LEXIKHUMOATResults.PhaseCResults.Checkpoint);

        } /* Equals(System.Object) */

        // public static bool operator == (LEXIKHUMOATCheckpointBehaviour left, LEXIKHUMOATCheckpointBehaviour right)
        // {

        //     if (((object)left) == null || ((object)right) == null)
        //     {
        //         return System.Object.Equals(left, right);
        //     }

        //     // otherwise
        //     return left.Equals(right);

        // } /*  operator==() */

        // public static bool operator == (LEXIKHUMOATCheckpointBehaviour left, LEXIKHUMOATResults.PhaseCResults.Checkpoint right)
        // {

        //     if (((object)left) == null || ((object)right) == null)
        //     {
        //         return System.Object.Equals(left, right);
        //     }

        //     // otherwise
        //     return left.Equals(right);

        // } /*  operator==() */

        // public static bool operator != (LEXIKHUMOATCheckpointBehaviour left, LEXIKHUMOATCheckpointBehaviour right)
        // {

        //     if (((object)left) == null || ((object)right) == null)
        //     {
        //         return !System.Object.Equals(left, right);
        //     }

        //     // otherwise
        //     return !left.Equals(right);

        // } /*  operator!=() */

        // public static bool operator != (LEXIKHUMOATCheckpointBehaviour left, LEXIKHUMOATResults.PhaseCResults.Checkpoint right)
        // {

        //     if (((object)left) == null || ((object)right) == null)
        //     {
        //         return !System.Object.Equals(left, right);
        //     }

        //     // otherwise
        //     return !left.Equals(right);

        // } /*  operator!=() */

        public override int GetHashCode()
        {

            // combine properties 
            return System.HashCode.Combine(
                this.CheckpointKind,
                this.CheckpointSide,
                this.transform,
                this.name
            );

        } /* GetHashCode() */

        #endregion

        #region IComparable interface impl.

        public int CompareTo(LEXIKHUMOATCheckpointBehaviour other)
        {

            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // The checkpoint comparison depends on the comparison of
            // the "z" deepness values.
            return this.transform.position.z.CompareTo(other.transform.position.z);

        } /* CompareTo(LEXIKHUMOATCheckpointBehaviour) */

        #endregion

    } /* public class LEXIKHUMOATCheckpointBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
