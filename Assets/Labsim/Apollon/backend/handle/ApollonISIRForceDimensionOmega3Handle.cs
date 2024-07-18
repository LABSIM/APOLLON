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
using System.Windows.Forms;
using UnityEngine.UIElements;

namespace Labsim.apollon.backend.handle
{

    public sealed class ApollonISIRForceDimensionOmega3Handle
        : ApollonAbstractROS2StreamHandle< 
            RosMessageTypes.LexikhumOatGateway.DownstreamMsg, 
            RosMessageTypes.LexikhumOatGateway.UpstreamMsg 
        >
    {

        protected sealed override ApollonBackendManager.HandleIDType WrapID()
        {
            return ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle;
        }

        #region ROS2 settings decl.

        public sealed class CurrentROS2Settings
            : IROS2Settings
        {

            private string m_ROS2UpstreamTopicName = "/gateway/ISIR_to_ONERA_Upstream";
            public string UpstreamTopicName 
            { 
                get => this.m_ROS2UpstreamTopicName; 
                private set => this.m_ROS2UpstreamTopicName = value; 
            }

            private string m_ROS2DownstreamTopicName = "/gateway/ONERA_to_ISIR_Downstream";
            public string DownstreamTopicName 
            { 
                get => this.m_ROS2DownstreamTopicName; 
                private set => this.m_ROS2DownstreamTopicName = value; 
            }

            private float m_ROS2DownstreamMessagePeriod = 0.2f;
            public float DownstreamMessagePeriod 
            { 
                get => this.m_ROS2DownstreamMessagePeriod; 
                private set => this.m_ROS2DownstreamMessagePeriod = value; 
            }

        } /* class CurrentROS2Settings */

        protected override IROS2Settings WrapROS2Settings()
        {
            return new ApollonISIRForceDimensionOmega3Handle.CurrentROS2Settings();
        }

        #endregion
        
        #region ROS2 callback decl. 

        private static ulong _s_upstream_uuid = 0;
        private static ulong _s_downstream_uuid = 0;

        protected sealed override void Upstream(RosMessageTypes.LexikhumOatGateway.UpstreamMsg upstream)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISIRForceDimensionOmega3Handle.Upstream() : call"
            );

            this.SensorObject.transform.localPosition 
                = new(
                    (float)upstream.haptic_arm_world_position.x,
                    (float)upstream.haptic_arm_world_position.y,
                    (float)upstream.haptic_arm_world_position.z
                );
            this.SensorObject.transform.localRotation = UnityEngine.Quaternion.identity;
            _s_upstream_uuid = upstream.uuid;

        } /* Upstream() */

        protected sealed override RosMessageTypes.LexikhumOatGateway.DownstreamMsg Downstream()
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISIRForceDimensionOmega3Handle.Downstream() : call"
            );

            return new(
                uuid: 
                    ++_s_downstream_uuid,
                entity_world_pose: 
                    new( 
                        position: new(
                            this.CommandObject.transform.position.x, 
                            this.CommandObject.transform.position.y, 
                            this.CommandObject.transform.position.z
                        ),
                        orientation: new(
                            this.CommandObject.transform.rotation.x, 
                            this.CommandObject.transform.rotation.y, 
                            this.CommandObject.transform.rotation.z, 
                            this.CommandObject.transform.rotation.w
                        )
                    ),
                param_gate_size_forward: 
                    new(),
                param_gate_size_dodge: 
                    new(),
                param_gate_gradiant_force:
                    new()
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
                .ConcreteBehaviour.ImpedenceReference.PhysicalWorld.Command;
            
            this.SensorObject
                = (
                    gameplay.ApollonGameplayManager.Instance.getBridge(
                        gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                    ) as gameplay.device.ApollonGeneric3DoFHapticArmBridge
                )
                .ConcreteBehaviour.ImpedenceReference.PhysicalWorld.Sensor;

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
