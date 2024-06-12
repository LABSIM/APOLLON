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
    public class LEXIKHUMOATImpedenceModelBehaviour
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_inputObject = null;

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_outputObject = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_commandObject = null;

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_sensorObject = null;

        [UnityEngine.SerializeField]
        private enum ModeType
        {

            // both impedence factor == 0.0f
            None = 0,

            // only upstream : m_sensorObject =(m_upstreamImpedanceFactor)=> m_outputObject
            Sensor,

            // only downstream : m_inputObject =(m_downstreamImpedanceFactor)=> m_commandObject
            Command,

            // both upstream & downstream
            Haptic,
            
            // both impedence factor == 1.0f
            Direct,

        } /* ModeType */

        [UnityEngine.SerializeField]
        private ModeType m_mode = ModeType.None;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_downstreamImpedanceTranslationFactor = UnityEngine.Vector3.zero;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_downstreamImpedanceRotationFactor    = UnityEngine.Vector3.zero;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_downstreamImpedanceScalingFactor     = UnityEngine.Vector3.zero;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_upstreamImpedanceTranslationFactor   = UnityEngine.Vector3.zero;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_upstreamImpedanceRotationFactor      = UnityEngine.Vector3.zero;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_upstreamImpedanceScalingFactor       = UnityEngine.Vector3.zero;
        
        private void FixedUpdate()
        {

            switch(this.m_mode)
            {

                default:
                case ModeType.None:
                {
                    break;
                }

                case ModeType.Sensor:
                {
                    this.UpstreamProcessing();
                    break;
                }

                case ModeType.Command:
                {
                    this.DownstreamProcessing();
                    break;
                }
                
                case ModeType.Haptic:
                {
                    this.UpstreamProcessing();
                    this.DownstreamProcessing();
                    break;
                }
                
                case ModeType.Direct:
                {
                    
                    // direct assignement
                    this.m_outputObject.transform.SetPositionAndRotation(
                        this.m_sensorObject.transform.position,
                        this.m_sensorObject.transform.rotation
                    );
                    this.m_commandObject.transform.SetPositionAndRotation(
                        this.m_inputObject.transform.position,
                        this.m_inputObject.transform.rotation
                    );
                    
                    break;

                }

            } /* switch() */

        } /* FixedUpdate() */

        private void UpstreamProcessing()
        {
            
            if(!(this.m_outputObject != null && this.m_sensorObject != null))
                return;
            
            var output_matrix 
                = this.m_sensorObject.transform.localToWorldMatrix 
                * UnityEngine.Matrix4x4.TRS(
                    this.m_upstreamImpedanceTranslationFactor,
                    UnityEngine.Quaternion.Euler(this.m_upstreamImpedanceRotationFactor),
                    this.m_upstreamImpedanceScalingFactor
                );

            this.m_outputObject.transform.position   = output_matrix.GetPosition();
            this.m_outputObject.transform.rotation   = output_matrix.rotation;
            this.m_outputObject.transform.localScale = output_matrix.lossyScale;

        } /* UpstreamProcessing() */

        private void DownstreamProcessing()
        {

            if(!(this.m_inputObject != null && this.m_commandObject != null))
                return;
            
            var input_matrix 
                = this.m_inputObject.transform.localToWorldMatrix 
                * UnityEngine.Matrix4x4.TRS(
                    this.m_downstreamImpedanceTranslationFactor,
                    UnityEngine.Quaternion.Euler(this.m_downstreamImpedanceRotationFactor),
                    this.m_downstreamImpedanceScalingFactor
                );

            this.m_commandObject.transform.position   = input_matrix.GetPosition();
            this.m_commandObject.transform.rotation   = input_matrix.rotation;
            this.m_commandObject.transform.localScale = input_matrix.lossyScale;

        } /* DownstreamProcessing() */

    } /* public class LEXIKHUMOATImpedenceModelBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */

