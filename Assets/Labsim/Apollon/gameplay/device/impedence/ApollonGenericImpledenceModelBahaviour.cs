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

    public class ApollonGenericImpedenceModelBaheviour
        : ApollonAbstractImpedenceModelBehaviour
    {

        #region Abstract methods impl.

        public sealed override void NoProcessing()
        {
            
        } /* NoProcessing() */

        public sealed override void UpstreamProcessing()
        {
            
            if(!(this.VirtualWorld.Command != null && this.PhysicalWorld.Sensor != null))
            {                
                return;
            }

            this.VirtualWorld.Command.transform.position 
                = UnityEngine.Vector3.Scale(
                    this.PhysicalWorld.Sensor.transform.position,   
                    this.UpstreamFactor.TranslationFactor
                );
            this.VirtualWorld.Command.transform.rotation 
                = UnityEngine.Quaternion.Euler(
                    UnityEngine.Vector3.Scale(
                        this.PhysicalWorld.Sensor.transform.rotation.eulerAngles, 
                        this.UpstreamFactor.RotationFactor
                    )
                );
            this.VirtualWorld.Command.transform.localScale 
                = UnityEngine.Vector3.Scale(
                    this.PhysicalWorld.Sensor.transform.lossyScale, 
                    this.UpstreamFactor.ScaleFactor
                );

        } /* UpstreamProcessing() */

        public sealed override void DownstreamProcessing()
        {

            if(!(this.PhysicalWorld.Command != null && this.VirtualWorld.Sensor != null))
            {                
                return;
            }

            this.PhysicalWorld.Command.transform.position 
                = UnityEngine.Vector3.Scale(
                    this.VirtualWorld.Sensor.transform.position,   
                    this.DownstreamFactor.TranslationFactor
                );
            this.PhysicalWorld.Command.transform.rotation 
                = UnityEngine.Quaternion.Euler(
                    UnityEngine.Vector3.Scale(
                        this.VirtualWorld.Sensor.transform.rotation.eulerAngles, 
                        this.DownstreamFactor.RotationFactor
                    )
                );
            this.PhysicalWorld.Command.transform.localScale 
                = UnityEngine.Vector3.Scale(
                    this.VirtualWorld.Sensor.transform.lossyScale, 
                    this.DownstreamFactor.ScaleFactor
                );

        } /* DownstreamProcessing() */

        public sealed override void DirectProcessing()
        {
        
            // direct assignement
                
            if(!(
                this.PhysicalWorld.Command   != null
                && this.PhysicalWorld.Sensor != null
                && this.VirtualWorld.Command != null 
                && this.VirtualWorld.Sensor  != null
            ))
            {                
                return;
            } 
            
            this.PhysicalWorld.Command.transform.position   = this.VirtualWorld.Sensor.transform.position;
            this.PhysicalWorld.Command.transform.rotation   = this.VirtualWorld.Sensor.transform.rotation;
            this.PhysicalWorld.Command.transform.localScale = this.VirtualWorld.Sensor.transform.lossyScale;
            
            this.VirtualWorld.Command.transform.position    = this.PhysicalWorld.Sensor.transform.position;
            this.VirtualWorld.Command.transform.rotation    = this.PhysicalWorld.Sensor.transform.rotation;
            this.VirtualWorld.Command.transform.localScale  = this.PhysicalWorld.Sensor.transform.lossyScale;
            
        } /* DirectProcessing() */

        #endregion

    } /* public class ApollonGenericImpedenceModelBaheviour */

} /* } Labsim.apollon.gameplay.device.impedence */

