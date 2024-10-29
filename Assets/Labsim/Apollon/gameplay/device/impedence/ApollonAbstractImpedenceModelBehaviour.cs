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

    public abstract class ApollonAbstractImpedenceModelBehaviour
        : UnityEngine.MonoBehaviour
        , IApollonImpedenceModel
    {

        #region interface IApollonImpedenceModel impl.

        [UnityEngine.SerializeField]
        private IApollonImpedenceModel.ProcessingModeType m_processingMode = IApollonImpedenceModel.ProcessingModeType.None;
        public IApollonImpedenceModel.ProcessingModeType ProcessingMode => this.m_processingMode;

        [UnityEngine.SerializeField]
        private IApollonImpedenceModel.TickingModeType m_tickingMode = IApollonImpedenceModel.TickingModeType.FixedUpdate;
        public IApollonImpedenceModel.TickingModeType TickingMode => this.m_tickingMode;
        
        [UnityEngine.SerializeField]
        private ApollonImpedenceIOMapping m_virtualWorld = new();
        public ApollonImpedenceIOMapping VirtualWorld => this.m_virtualWorld;

        [UnityEngine.SerializeField]
        private ApollonImpedenceIOMapping m_physicalWorld = new();
        public ApollonImpedenceIOMapping PhysicalWorld => this.m_physicalWorld;
        
        [UnityEngine.SerializeField]
        private ApollonStandardTRSImpedenceFactor m_downstreamFactor = new();
        public ApollonStandardTRSImpedenceFactor DownstreamFactor => this.m_downstreamFactor;
                
        [UnityEngine.SerializeField]
        private ApollonStandardTRSImpedenceFactor m_upstreamFactor = new();
        public ApollonStandardTRSImpedenceFactor UpstreamFactor => this.m_upstreamFactor;

        public void Tick()
        {

            switch(this.ProcessingMode)
            {

                default:
                case IApollonImpedenceModel.ProcessingModeType.None:
                {
                    this.NoProcessing();
                    break;
                }

                case IApollonImpedenceModel.ProcessingModeType.Sensor:
                {
                    this.UpstreamProcessing();
                    break;
                }

                case IApollonImpedenceModel.ProcessingModeType.Command:
                {
                    this.DownstreamProcessing();
                    break;
                }
                
                case IApollonImpedenceModel.ProcessingModeType.Haptic:
                {
                    this.UpstreamProcessing();
                    this.DownstreamProcessing();
                    break;
                }
                
                case IApollonImpedenceModel.ProcessingModeType.Direct:
                {
                    this.DirectProcessing();
                    break;
                }

            } /* switch() */

        } /* Tick() */

        #endregion

        #region Monobehaviour impl.

        private void Update()
        {
           
            // check
            if(this.TickingMode != IApollonImpedenceModel.TickingModeType.Update)
            {
                // skip 
                return;
            }
            
             // simply process upstream & downstream
            this.Tick();
            
        } /* Update() */

        private void FixedUpdate()
        {
           
            // check
            if(this.TickingMode != IApollonImpedenceModel.TickingModeType.FixedUpdate)
            {
                // skip 
                return;
            }

             // simply process upstream & downstream
            this.Tick();
            
        } /* FixedUpdate() */

        private void LateUpdate()
        {
           
            // check
            if(this.TickingMode != IApollonImpedenceModel.TickingModeType.LateUpdate)
            {
                // skip 
                return;
            }
            
             // simply process upstream & downstream
            this.Tick();
            
        } /* LateUpdate() */

        #endregion

        #region Abstract methods decl.

        public abstract void NoProcessing();

        public abstract void UpstreamProcessing();

        public abstract void DownstreamProcessing();

        public abstract void DirectProcessing();

        #endregion

    } /* public class ApollonAbstractImpedenceModelBehaviour */

} /* } Labsim.apollon.gameplay.device.impedence */

