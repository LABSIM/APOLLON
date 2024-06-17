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
            
            if(!(this.VirtualWorld.Sensor != null && this.PhysicalWorld.Sensor != null))
                return;
            
            var output_matrix 
                = this.PhysicalWorld.Sensor.transform.localToWorldMatrix 
                * this.UpstreamFactor.TRS;

            this.VirtualWorld.Sensor.transform.position   = output_matrix.GetPosition();
            this.VirtualWorld.Sensor.transform.rotation   = output_matrix.rotation;
            this.VirtualWorld.Sensor.transform.localScale = output_matrix.lossyScale;

        } /* UpstreamProcessing() */

        public sealed override void DownstreamProcessing()
        {

            if(!(this.PhysicalWorld.Command != null && this.VirtualWorld.Command != null))
                return;
            
            var input_matrix 
                = this.VirtualWorld.Command.transform.localToWorldMatrix 
                * this.DownstreamFactor.TRS;

            this.PhysicalWorld.Command.transform.position   = input_matrix.GetPosition();
            this.PhysicalWorld.Command.transform.rotation   = input_matrix.rotation;
            this.PhysicalWorld.Command.transform.localScale = input_matrix.lossyScale;

        } /* DownstreamProcessing() */

        public sealed override void DirectProcessing()
        {

            // direct assignement
            this.PhysicalWorld.Command.transform.SetPositionAndRotation(
                this.VirtualWorld.Command.transform.position,
                this.VirtualWorld.Command.transform.rotation
            );
            this.VirtualWorld.Sensor.transform.SetPositionAndRotation(
                this.PhysicalWorld.Sensor.transform.position,
                this.PhysicalWorld.Sensor.transform.rotation
            );
            
        } /* DirectProcessing() */

        #endregion

    } /* public class ApollonGenericImpedenceModelBaheviour */

} /* } Labsim.apollon.gameplay.device.impedence */

