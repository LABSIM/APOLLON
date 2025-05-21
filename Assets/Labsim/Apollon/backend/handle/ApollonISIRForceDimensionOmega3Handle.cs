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

        #region Custom datas

        public string SharedIntentionMode { get; set; } = null;

        private UnityEngine.GameObject EffectorSensorObject { get; set; } = null;
        private UnityEngine.GameObject EffectorCommandObject { get; set; } = null;

        private UnityEngine.GameObject ForceFeedbackGragiantSensorObject { get; set; } = null;
        private UnityEngine.GameObject ForceFeedbackGragiantCommandObject { get; set; } = null;

        private UnityEngine.GameObject ForceFeedbackObjectiveSensorObject { get; set; } = null;
        private UnityEngine.GameObject ForceFeedbackObjectiveCommandObject { get; set; } = null;


        public string NextGateSide { get; set; } = null;
        public string NextGateKind { get; set; } = null;
        public string NextGateOffset { get; set; } = null;
        public float NextGateWidth { get; set; } = float.NaN;
        public UnityEngine.Vector3 NextGateWorldPosition { get; set; } = UnityEngine.Vector3.zero;

        #endregion

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

            private float m_ROS2DownstreamMessagePeriod = 0.01f;
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

            // the effector transform
            this.EffectorSensorObject.transform.localPosition 
                = new(
                    (float)upstream.effector_world_position.x,
                    (float)upstream.effector_world_position.y,
                    (float)upstream.effector_world_position.z
                );
            this.EffectorSensorObject.transform.localRotation 
                = UnityEngine.Quaternion.identity;
            this.EffectorSensorObject.transform.localScale
                = UnityEngine.Vector3.one;

            // the haptic feedback objective position
            this.ForceFeedbackObjectiveSensorObject.transform.localPosition
                = new(
                    (float)upstream.force_feedback_objective_world_position.x,
                    (float)upstream.force_feedback_objective_world_position.y,
                    (float)upstream.force_feedback_objective_world_position.z
                );
            this.ForceFeedbackObjectiveSensorObject.transform.localRotation 
                = UnityEngine.Quaternion.identity;
            this.ForceFeedbackObjectiveSensorObject.transform.localScale
                = UnityEngine.Vector3.one;

            // the haptic feedback target gradiant
            this.ForceFeedbackGragiantSensorObject.transform.localPosition
                = new(  
                    (float)upstream.force_feedback_gradiant_force.x,
                    (float)upstream.force_feedback_gradiant_force.y,
                    (float)upstream.force_feedback_gradiant_force.z
                );
            this.ForceFeedbackGragiantSensorObject.transform.localRotation 
                = UnityEngine.Quaternion.identity;
            this.ForceFeedbackGragiantSensorObject.transform.localScale
                = UnityEngine.Vector3.one;

            // the message uuid
            _s_upstream_uuid = upstream.uuid;

        } /* Upstream() */

        protected sealed override RosMessageTypes.LexikhumOatGateway.DownstreamMsg Downstream()
        {
            
            return new(
                uuid: 
                    ++_s_downstream_uuid,
                entity_world_pose: 
                    new( 
                        position: new(
                            this.EffectorCommandObject.transform.position.x, 
                            this.EffectorCommandObject.transform.position.y, 
                            this.EffectorCommandObject.transform.position.z
                        ),
                        orientation: new(
                            this.EffectorCommandObject.transform.rotation.x, 
                            this.EffectorCommandObject.transform.rotation.y, 
                            this.EffectorCommandObject.transform.rotation.z, 
                            this.EffectorCommandObject.transform.rotation.w
                        )
                    ),
                current_gate_center: 
                    new(
                        this.NextGateWorldPosition.x,
                        this.NextGateWorldPosition.y,
                        this.NextGateWorldPosition.z
                    ),
                current_gate_width: 
                    (ulong)this.NextGateWidth,
                current_mode :
                    this.SharedIntentionMode,
                current_phase:
                    this.NextGateKind + "_" + this.NextGateSide // + "_" + NextGateOffset
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

            var generic3DoFHapticArmBridge 
                = gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    gameplay.device.ApollonGeneric3DoFHapticArmBridge
                >(
                    gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
                );

            this.EffectorCommandObject = generic3DoFHapticArmBridge.ConcreteBehaviour.EffectorImpedence.PhysicalWorld.Command;
            this.EffectorSensorObject  = generic3DoFHapticArmBridge.ConcreteBehaviour.EffectorImpedence.PhysicalWorld.Sensor;

            this.ForceFeedbackGragiantCommandObject = generic3DoFHapticArmBridge.ConcreteBehaviour.ForceFeedbackGragiantImpedence.PhysicalWorld.Command;
            this.ForceFeedbackGragiantSensorObject  = generic3DoFHapticArmBridge.ConcreteBehaviour.ForceFeedbackGragiantImpedence.PhysicalWorld.Sensor;

            this.ForceFeedbackObjectiveCommandObject = generic3DoFHapticArmBridge.ConcreteBehaviour.ForceFeedbackObjectiveImpedence.PhysicalWorld.Command;
            this.ForceFeedbackObjectiveSensorObject  = generic3DoFHapticArmBridge.ConcreteBehaviour.ForceFeedbackObjectiveImpedence.PhysicalWorld.Sensor;

            // success 
            return StatusIDType.Status_OK;
            
        } /* HandleInitialize() */

        protected sealed override StatusIDType ConcreteHandleClose()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISIRForceDimensionOmega3Handle.ConcreteHandleClose() : clean internal "
            );

            this.EffectorCommandObject 
                = this.EffectorSensorObject 
                = null;

            // success
            return StatusIDType.Status_OK;

        }

        #endregion

    } /* class ApollonMotionSystemPS6TM550Handle */
    
} /* namespace Labsim.apollon.backend */
