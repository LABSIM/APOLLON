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

// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{
    public sealed class AIRWISEAttitudeTracker
        : UXF.Tracker
    {

        public override string MeasurementDescriptor => "AttitudeTracker";

        public override System.Collections.Generic.IEnumerable<string> CustomHeader 
            => new string[] { 
                "local_position_x", 
                "local_position_y", 
                "local_position_z",
                "local_orientation_x", 
                "local_orientation_y", 
                "local_orientation_z",
                "world_position_x", 
                "world_position_y", 
                "world_position_z",
                "world_orientation_x", 
                "world_orientation_y", 
                "world_orientation_z",
                "aero_position_x", 
                "aero_position_y", 
                "aero_position_z",
                "aero_orientation_x", 
                "aero_orientation_y", 
                "aero_orientation_z"
            };

        protected override UXF.UXFDataRow GetCurrentValues()
        {
           
            UnityEngine.Vector3
                aero_pos = Utilities.ToUnityFromAeroFrame(this.gameObject.transform.position), 
                aero_ori = Utilities.ZYXToMatrix(this.gameObject.transform.rotation.eulerAngles).rotation.eulerAngles;

            // return it
            return new UXF.UXFDataRow()
            {
                ("local_position_x",    this.gameObject.transform.localPosition.x),
                ("local_position_y",    this.gameObject.transform.localPosition.y),
                ("local_position_z",    this.gameObject.transform.localPosition.z),
                ("local_orientation_x", this.gameObject.transform.localRotation.eulerAngles.x),
                ("local_orientation_y", this.gameObject.transform.localRotation.eulerAngles.y),
                ("local_orientation_z", this.gameObject.transform.localRotation.eulerAngles.z),
                ("world_position_x",    this.gameObject.transform.position.x),
                ("world_position_y",    this.gameObject.transform.position.y),
                ("world_position_z",    this.gameObject.transform.position.z),
                ("world_orientation_x", this.gameObject.transform.rotation.eulerAngles.x),
                ("world_orientation_y", this.gameObject.transform.rotation.eulerAngles.y),
                ("world_orientation_z", this.gameObject.transform.rotation.eulerAngles.z),
                ("aero_position_x",     aero_pos.x),
                ("aero_position_y",     aero_pos.y),
                ("aero_position_z",     aero_pos.z),
                ("aero_orientation_x",  aero_ori.x),
                ("aero_orientation_y",  aero_ori.y),
                ("aero_orientation_z",  aero_ori.z)
            };

        } /* GetCurrentValues() */

    } /* public sealed class AIRWISEAttitudeTracker */

} /* } Labsim.experiment.AIRWISE */