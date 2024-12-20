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

namespace Labsim.apollon.gameplay.device.impedence
{
    
    [System.Serializable]
    public class ApollonImpedenceIOMapping
    {

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_commandObject = null;
        public UnityEngine.GameObject Command => this.m_commandObject;

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject m_sensorObject = null;
        public UnityEngine.GameObject Sensor => this.m_sensorObject;

    } /* class ApollonImpedenceIOMapping */

    [System.Serializable]
    public class ApollonStandardTRSImpedenceFactor
    {

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_translationFactor = UnityEngine.Vector3.one;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_rotationFactor = UnityEngine.Vector3.one;

        [UnityEngine.SerializeField]
        private UnityEngine.Vector3 m_scaleFactor = UnityEngine.Vector3.one;

        public UnityEngine.Vector3 TranslationFactor => this.m_translationFactor;
        public UnityEngine.Vector3 RotationFactor => this.m_rotationFactor;
        public UnityEngine.Vector3 ScaleFactor => this.m_scaleFactor;

        // public UnityEngine.Matrix4x4 TRS 
        //     => UnityEngine.Matrix4x4.TRS(
        //         this.Translation,
        //         UnityEngine.Quaternion.Euler(this.Rotation),
        //         this.Scale
        //     );

    } /* class ApollonStandardTRSImpedenceFactor */

    public interface IApollonImpedenceModel
    {

        public enum ProcessingModeType
        {

            // both impedence factor == 0.0f
            None = 0,

            // only upstream : physical sensor ==(m_upstreamImpedanceFactor)==> virtual command
            Sensor,

            // only downstream : virtual command ==(m_downstreamImpedanceFactor)==> physical sensor
            Command,

            // both upstream & downstream
            Haptic,
            
            // both impedence factor == 1.0f
            Direct,

        } /* enum ModeType */

        public ProcessingModeType ProcessingMode { get; }

        public enum TickingModeType
        {

            None = 0,
            Update,
            FixedUpdate,
            LateUpdate

        } /* enum TickingModeType */

        public TickingModeType TickingMode { get; }
        
        public ApollonImpedenceIOMapping VirtualWorld { get; }
        
        public ApollonImpedenceIOMapping PhysicalWorld { get; }

        // Virtual to Physical downstream
        public ApollonStandardTRSImpedenceFactor DownstreamFactor { get; }

        // Physical to Virtual upstream
        public ApollonStandardTRSImpedenceFactor UpstreamFactor { get; }

        public void Tick();

        public void NoProcessing();

        public void UpstreamProcessing();

        public void DownstreamProcessing();

        public void DirectProcessing();

    } /* public interface IApollonImpedenceModel */

} /* } Labsim.apollon.gameplay.device.impedence */

