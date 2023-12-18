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
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBehaviour 
        : ApollonConcreteGameplayBehaviour<ApollonHOTASWarthogThrottleSensorBridge>
    {

        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("")]
        [UnityEngine.Range(-180.0f, 180.0f)]
        private float m_axisZ_min_angle = -25.00f;        
        
        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("")]
        [UnityEngine.Range(-180.0f, 180.0f)]
        private float m_axisZ_max_angle = 25.00f;

        #region Unity Mono Behaviour implementation

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Control event

        public void OnAxisZValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // update tracked attitude 
            this.gameObject.transform.SetPositionAndRotation(
                this.gameObject.transform.position,
                UnityEngine.Quaternion.AngleAxis(
                    context.ReadValue<float>() * (this.m_axisZ_max_angle - this.m_axisZ_min_angle) / 2.0f,
                    UnityEngine.Vector3.right
                )
            );

        } /* OnAxisZValueChanged() */

        #endregion

    } /* public class ApollonHOTASWarthogThrottleSensorBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */