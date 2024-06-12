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
namespace Labsim.apollon.backend.handle
{

    public class ApollonISIRForceDimensionOmega3Handle
        : ApollonAbstractROS2StreamHandle< RosMessageTypes.UnityRoboticsDemo.PosRotMsg, RosMessageTypes.UnityRoboticsDemo.PosRotMsg>
    {

        protected sealed override ApollonBackendManager.HandleIDType WrapID()
        {
            return ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle;;
        }
        
        #region ROS2 callback decl. 

        protected sealed override void Upstream(RosMessageTypes.UnityRoboticsDemo.PosRotMsg upstream)
        {

            this.SensorObject.transform.localPosition 
                = new(
                    upstream.pos_x,
                    upstream.pos_y,
                    upstream.pos_z
                );
            this.SensorObject.transform.localRotation
                = new(
                    upstream.rot_x,
                    upstream.rot_y,
                    upstream.rot_z,
                    upstream.rot_w
                );

        } /* Upstream() */

        protected sealed override RosMessageTypes.UnityRoboticsDemo.PosRotMsg Downstream()
        {
            return new(
                this.CommandObject.transform.position.x,
                this.CommandObject.transform.position.y,
                this.CommandObject.transform.position.z,
                this.CommandObject.transform.rotation.x,
                this.CommandObject.transform.rotation.y,
                this.CommandObject.transform.rotation.z,
                this.CommandObject.transform.rotation.w
            );
        }

        #endregion 

        #region Dispose pattern impl.

        protected sealed override void Dispose(bool bDisposing = true)
        {

            // TODO ?

        } /* Dispose(bool) */

        #endregion

        #region Concrete HandleInit/HandleClose pattern impl.

        protected sealed override StatusIDType ConcreteHandleInitialize()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISIRForceDimensionOmega3Handle.ConcreteHandleInitialize() : Tail object from scene"
            );

            this.CommandObject
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                    ) as gameplay.device.ApollonGeneric3DoFHapticArmBridge
                )
                .ConcreteBehaviour.CommandReference;
            
            this.SensorObject
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                    ) as gameplay.device.ApollonGeneric3DoFHapticArmBridge
                )
                .ConcreteBehaviour.SensorReference;

            // success 
            return StatusIDType.Status_OK;
            
        } /* HandleInitialize() */

        protected sealed override StatusIDType ConcreteHandleClose()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISIRForceDimensionOmega3Handle.ConcreteHandleClose() : clean internal "
            );

            this.CommandObject = this.SensorObject = null;

            // success
            return StatusIDType.Status_OK;

        }

        #endregion

    } /* class ApollonMotionSystemPS6TM550Handle */
    
} /* namespace Labsim.apollon.backend */
