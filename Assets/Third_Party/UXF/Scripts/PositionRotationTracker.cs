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
        protected override string[] GetCurrentValues()
        {
            // get position and rotation
            Vector3 world_p = gameObject.transform.position;
            Vector3 world_r = gameObject.transform.eulerAngles;
            Vector3 local_p = gameObject.transform.localPosition;
            Vector3 local_r = gameObject.transform.localEulerAngles;

            string format = "0.####";

            // return position, rotation (x, y, z) as an array
            var values =  new string[]
            {
                world_p.x.ToString(format),
                world_p.y.ToString(format),
                world_p.z.ToString(format),
                world_r.x.ToString(format),
                world_r.y.ToString(format),
                world_r.z.ToString(format),
                local_p.x.ToString(format),
                local_p.y.ToString(format),
                local_p.z.ToString(format),
                local_r.x.ToString(format),
                local_r.y.ToString(format),
                local_r.z.ToString(format)
            };

            return values;
        }
    }
}