using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UXF
{
    /// <summary>
    /// Attach this component to a gameobject and assign it in the trackedObjects field in an ExperimentSession to automatically record position/rotation of the object at each frame.
    /// </summary>
    public class PositionRotationTracker : Tracker
    {
        /// <summary>
        /// Sets measurementDescriptor and customHeader to appropriate values
        /// </summary>
        protected override void SetupDescriptorAndHeader()
        {
            measurementDescriptor = "movement";
            
            customHeader = new string[]
            {
                "world_pos_x",
                "world_pos_y",
                "world_pos_z",
                "world_rot_x",
                "world_rot_y",
                "world_rot_z",
                "local_pos_x",
                "local_pos_y",
                "local_pos_z",
                "local_rot_x",
                "local_rot_y",
                "local_rot_z"
            };
        }

        /// <summary>
        /// Returns current position and rotation values
        /// </summary>
        /// <returns></returns>
        protected override UXFDataRow GetCurrentValues()
        {
            // get position and rotation
            Vector3 world_p = gameObject.transform.position;
            Vector3 world_r = gameObject.transform.eulerAngles;
            Vector3 local_p = gameObject.transform.localPosition;
            Vector3 local_r = gameObject.transform.localEulerAngles;

            // return world [position, rotation] (x, y, z) & local [position, rotation] (x, y, z) as an array
            var values = new UXFDataRow()
            {
                ("world_pos_x", world_p.x),
                ("world_pos_y", world_p.y),
                ("world_pos_z", world_p.z),
                ("world_rot_x", world_r.x),
                ("world_rot_y", world_r.y),
                ("world_rot_z", world_r.z),
                ("local_pos_x", local_p.x),
                ("local_pos_y", local_p.y),
                ("local_pos_z", local_p.z),
                ("local_rot_x", local_r.x),
                ("local_rot_y", local_r.y),
                ("local_rot_z", local_r.z)
            };

            return values;
        }
    }
}