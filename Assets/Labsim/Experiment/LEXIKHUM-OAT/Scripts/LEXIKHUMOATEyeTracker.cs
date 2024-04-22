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

using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.LEXIKHUM_OAT
{
    public sealed class LEXIKHUMOATEyeTracker
        : UXF.Tracker
    {

        public override string MeasurementDescriptor => "VarjoXREyeTracker";

        private System.Collections.Generic.List<Varjo.XR.VarjoEyeTracking.GazeData> m_dataSinceLastUpdate;
        private System.Collections.Generic.List<Varjo.XR.VarjoEyeTracking.EyeMeasurements> m_eyeMeasurementsSinceLastUpdate;

        public override System.Collections.Generic.IEnumerable<string> CustomHeader 
            => new string[] { 
                "frame_number",
                "capture_time",
                "gaze_status",
                "gaze_forward",
                "gaze_origin",
                "focus_distance",
                "focus_stability",
                "inter_pupillary_distance",
                "right_eye_gaze_status",
                "right_eye_gaze_forward",
                "right_eye_gaze_origin",
                "right_pupillris_diameter_ratio",
                "right_pupil_diameter",
                "right_iris_diameter",
                "right_eye_openness",
                "left_eye_gaze_status",
                "left_eye_gaze_forward",
                "left_eye_gaze_origin",
                "left_pupillris_diameter_ratio",
                "left_pupil_diameter",
                "left_iris_diameter",
                "left_eye_openness"
            };

        protected override UXF.UXFDataRow GetCurrentValues()
        {
            
            // vars
            bool 
                bGazeValid      = false,
                bRightGazeValid = false,
                bLeftGazeValid  = false;
            const string cEmptyField = "";
            int dataCount 
                = Varjo.XR.VarjoEyeTracking.GetGazeList(
                    out this.m_dataSinceLastUpdate, 
                    out this.m_eyeMeasurementsSinceLastUpdate
                );

            // lambda
            System.Func<UnityEngine.Vector3, string> Vector3ToString 
                = (vec) => 
                {
                    return "[" + System.String.Join(";", new System.Collections.Generic.List<float>{ vec.x, vec.y, vec.z }) + "]";
                };
            
            // if no data, strange but ...
            if(dataCount == 0)
            {

                return new UXF.UXFDataRow()
                {
                    ("frame_number",                   cEmptyField),
                    ("capture_time",                   cEmptyField),
                    ("gaze_status",                    cEmptyField),
                    ("gaze_forward_x",                 cEmptyField),
                    ("gaze_forward_y",                 cEmptyField),
                    ("gaze_forward_z",                 cEmptyField),
                    ("gaze_origin_x",                  cEmptyField),
                    ("gaze_origin_y",                  cEmptyField),
                    ("gaze_origin_z",                  cEmptyField),
                    ("focus_distance",                 cEmptyField),
                    ("focus_stability",                cEmptyField),
                    ("inter_pupillary_distance",       cEmptyField),
                    ("right_eye_gaze_status",          cEmptyField),
                    ("right_eye_gaze_forward_x",       cEmptyField),
                    ("right_eye_gaze_forward_y",       cEmptyField),
                    ("right_eye_gaze_forward_z",       cEmptyField),
                    ("right_eye_gaze_origin_x",        cEmptyField),
                    ("right_eye_gaze_origin_y",        cEmptyField),
                    ("right_eye_gaze_origin_z",        cEmptyField),
                    ("right_pupillris_diameter_ratio", cEmptyField),
                    ("right_pupil_diameter",           cEmptyField),
                    ("right_iris_diameter",            cEmptyField),
                    ("right_eye_openness",             cEmptyField),
                    ("left_eye_gaze_status",           cEmptyField),
                    ("left_eye_gaze_forward_x",        cEmptyField),
                    ("left_eye_gaze_forward_y",        cEmptyField),
                    ("left_eye_gaze_forward_z",        cEmptyField),
                    ("left_eye_gaze_origin_x",         cEmptyField),
                    ("left_eye_gaze_origin_y",         cEmptyField),
                    ("left_eye_gaze_origin_z",         cEmptyField),
                    ("left_pupillris_diameter_ratio",  cEmptyField),
                    ("left_pupil_diameter",            cEmptyField),
                    ("left_iris_diameter",             cEmptyField),
                    ("left_eye_openness",              cEmptyField)
                };

            } /* if() */

            // flush all previous data
            for (int idx = 0; idx < (dataCount - 1); ++idx)
            {

                bGazeValid      = (this.m_dataSinceLastUpdate[idx].status      == Varjo.XR.VarjoEyeTracking.GazeStatus.Invalid);
                bRightGazeValid = (this.m_dataSinceLastUpdate[idx].rightStatus == Varjo.XR.VarjoEyeTracking.GazeEyeStatus.Invalid);
                bLeftGazeValid  = (this.m_dataSinceLastUpdate[idx].rightStatus == Varjo.XR.VarjoEyeTracking.GazeEyeStatus.Invalid);

                var newRow 
                    = new UXF.UXFDataRow()
                    {
                        ("host_timestamp",                 apollon.ApollonHighResolutionTime.Now.ToString()),
                        ("unity_timestamp",                UnityEngine.Time.time.ToString()),
                        ("varjo_timestamp",                Varjo.XR.VarjoTime.GetVarjoTimestamp().ToString()),
                        ("frame_number",                   this.m_dataSinceLastUpdate[idx].frameNumber.ToString()),
                        ("capture_time",                   this.m_dataSinceLastUpdate[idx].captureTime.ToString()),
                        ("gaze_status",                    this.m_dataSinceLastUpdate[idx].status),
                        ("gaze_forward_x",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.forward.x),
                        ("gaze_forward_y",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.forward.y),
                        ("gaze_forward_z",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.forward.z),
                        ("gaze_origin_x",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.origin.x),
                        ("gaze_origin_y",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.origin.y),
                        ("gaze_origin_z",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].gaze.origin.z),
                        ("focus_distance",                 this.m_dataSinceLastUpdate[idx].focusDistance.ToString()),
                        ("focus_stability",                this.m_dataSinceLastUpdate[idx].focusStability.ToString()),
                        ("inter_pupillary_distance",       this.m_eyeMeasurementsSinceLastUpdate[idx].interPupillaryDistanceInMM.ToString()),
                        ("right_eye_gaze_status",          this.m_dataSinceLastUpdate[idx].rightStatus),
                        ("right_eye_gaze_forward_x",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.forward.x),
                        ("right_eye_gaze_forward_y",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.forward.y),
                        ("right_eye_gaze_forward_z",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.forward.z),
                        ("right_eye_gaze_origin_x",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.origin.x),
                        ("right_eye_gaze_origin_y",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.origin.y),
                        ("right_eye_gaze_origin_z",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].right.origin.z),
                        ("right_pupillris_diameter_ratio", bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].rightPupilIrisDiameterRatio.ToString("F3")),
                        ("right_pupil_diameter",           bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].rightPupilDiameterInMM.ToString("F3")),
                        ("right_iris_diameter",            bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].rightIrisDiameterInMM.ToString("F3")),
                        ("right_eye_openness",             bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].rightEyeOpenness.ToString("F3")),
                        ("left_eye_gaze_status",           this.m_dataSinceLastUpdate[idx].leftStatus),
                        ("left_eye_gaze_forward_x",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.forward.x),
                        ("left_eye_gaze_forward_y",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.forward.y),
                        ("left_eye_gaze_forward_z",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.forward.z),
                        ("left_eye_gaze_origin_x",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.origin.x),
                        ("left_eye_gaze_origin_y",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.origin.y),
                        ("left_eye_gaze_origin_z",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate[idx].left.origin.z),
                        ("left_pupillris_diameter_ratio",  bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].leftPupilIrisDiameterRatio.ToString("F3")),
                        ("left_pupil_diameter",            bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].leftPupilDiameterInMM.ToString("F3")),
                        ("left_iris_diameter",             bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].leftIrisDiameterInMM.ToString("F3")),
                        ("left_eye_openness",              bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate[idx].leftEyeOpenness.ToString("F3")),
                    };

                this.Data.AddCompleteRow(newRow);

            } /* for() */

            // finally extract the last one
            bGazeValid      = (this.m_dataSinceLastUpdate.Last().status      == Varjo.XR.VarjoEyeTracking.GazeStatus.Invalid);
            bRightGazeValid = (this.m_dataSinceLastUpdate.Last().rightStatus == Varjo.XR.VarjoEyeTracking.GazeEyeStatus.Invalid);
            bLeftGazeValid  = (this.m_dataSinceLastUpdate.Last().rightStatus == Varjo.XR.VarjoEyeTracking.GazeEyeStatus.Invalid);
            var lastRow 
                = new UXF.UXFDataRow()
                {
                    ("frame_number",                   this.m_dataSinceLastUpdate.Last().frameNumber.ToString()),
                    ("capture_time",                   this.m_dataSinceLastUpdate.Last().captureTime.ToString()),
                    ("gaze_status",                    this.m_dataSinceLastUpdate.Last().status),
                    ("gaze_forward_x",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.forward.x),
                    ("gaze_forward_y",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.forward.y),
                    ("gaze_forward_z",                 bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.forward.z),
                    ("gaze_origin_x",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.origin.x),
                    ("gaze_origin_y",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.origin.y),
                    ("gaze_origin_z",                  bGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().gaze.origin.z),
                    ("focus_distance",                 this.m_dataSinceLastUpdate.Last().focusDistance.ToString()),
                    ("focus_stability",                this.m_dataSinceLastUpdate.Last().focusStability.ToString()),
                    ("inter_pupillary_distance",       this.m_eyeMeasurementsSinceLastUpdate.Last().interPupillaryDistanceInMM.ToString()),
                    ("right_eye_gaze_status",          this.m_dataSinceLastUpdate.Last().rightStatus),
                    ("right_eye_gaze_forward_x",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.forward.x),
                    ("right_eye_gaze_forward_y",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.forward.y),
                    ("right_eye_gaze_forward_z",       bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.forward.z),
                    ("right_eye_gaze_origin_x",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.origin.x),
                    ("right_eye_gaze_origin_y",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.origin.y),
                    ("right_eye_gaze_origin_z",        bRightGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().right.origin.z),
                    ("right_pupillris_diameter_ratio", bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().rightPupilIrisDiameterRatio.ToString("F3")),
                    ("right_pupil_diameter",           bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().rightPupilDiameterInMM.ToString("F3")),
                    ("right_iris_diameter",            bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().rightIrisDiameterInMM.ToString("F3")),
                    ("right_eye_openness",             bRightGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().rightEyeOpenness.ToString("F3")),
                    ("left_eye_gaze_status",           this.m_dataSinceLastUpdate.Last().leftStatus),
                    ("left_eye_gaze_forward_x",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.forward.x),
                    ("left_eye_gaze_forward_y",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.forward.y),
                    ("left_eye_gaze_forward_z",        bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.forward.z),
                    ("left_eye_gaze_origin_x",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.origin.x),
                    ("left_eye_gaze_origin_y",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.origin.y),
                    ("left_eye_gaze_origin_z",         bLeftGazeValid ? cEmptyField : this.m_dataSinceLastUpdate.Last().left.origin.z),
                    ("left_pupillris_diameter_ratio",  bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().leftPupilIrisDiameterRatio.ToString("F3")),
                    ("left_pupil_diameter",            bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().leftPupilDiameterInMM.ToString("F3")),
                    ("left_iris_diameter",             bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().leftIrisDiameterInMM.ToString("F3")),
                    ("left_eye_openness",              bLeftGazeValid ? cEmptyField : this.m_eyeMeasurementsSinceLastUpdate.Last().leftEyeOpenness.ToString("F3")),
                };
            
            // return it
            return lastRow;

        } /* GetCurrentValues() */

    } /* public sealed class LEXIKHUMOATEyeTracker */

} /* } Labsim.experiment.LEXIKHUM_OAT */