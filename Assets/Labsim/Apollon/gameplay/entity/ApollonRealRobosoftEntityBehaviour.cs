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
namespace Labsim.apollon.gameplay.entity
{
   
    public class ApollonRealRobosoftEntityBehaviour
        : ApollonConcreteGameplayBehaviour<ApollonRealRobosoftEntityBridge>
    {

        public UnityEngine.GameObject m_IMU;

        private bool
            m_bIsSaturated = false,
            m_bIsMoving = false,
            m_bIsAccelerating = false;

        private static readonly UnityEngine.Vector3 _initialPosition = new UnityEngine.Vector3(0.0f, 0.2385f, 0.0f);
        private static readonly UnityEngine.Quaternion _initialRotation = UnityEngine.Quaternion.identity;

        private backend.handle.ApollonRobulabHandle m_handle = null;

        private void Start()
        {

            // activate the Robot :)
            backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(backend.ApollonBackendManager.HandleIDType.ApollonRobulabHandle);

            // tail 
            this.m_handle = backend.ApollonBackendManager.Instance.GetValidHandle(backend.ApollonBackendManager.HandleIDType.ApollonRobulabHandle) as backend.handle.ApollonRobulabHandle;

        } 

        private void OnApplicationQuit()
        {

            // remove 
            this.m_handle = null;

            // deactivate the Robot :(
            backend.ApollonBackendManager.Instance.RaiseHandleDeactivationRequestedEvent(backend.ApollonBackendManager.HandleIDType.ApollonRobulabHandle);

        }

        void Update()
        {

        }

        void Awake()
        {

        }

        void FixedUpdate()
        {

            // if fixed, then avoid any update & mark as unsaturated
            if (!this.m_bIsMoving)
            {
                this.m_bIsSaturated = false;
                this.m_bIsAccelerating = false;
                return;
            }

            // prepare local receipt variable
            float 
                odometry_x = 0.0f,
                odometry_y = 0.0f,
                odometry_orientation = 0.0f,
                ic4_phi = 0.0f,
                ic4_theta = 0.0f,
                ic4_psi = 0.0f,
                ic4_acc_x = 0.0f,
                ic4_acc_y = 0.0f,
                ic4_acc_z = 0.0f;

            UnityEngine.Vector3 position_odom = UnityEngine.Vector3.zero;
            UnityEngine.Quaternion rotation_odom = UnityEngine.Quaternion.identity;

            UnityEngine.Vector3 position_IMU = UnityEngine.Vector3.zero;
            UnityEngine.Quaternion rotation_IMU = UnityEngine.Quaternion.identity;

            // update current data

            // apply Robot Odometry feedback to current object

            this.m_handle.GetOdometry(
                ref odometry_x,
                ref odometry_y,
                ref odometry_orientation
            );

            //position_odom.Set(
            //    odometry_x,
            //    odometry_y,
            //    this.gameObject.transform.position.z
            //);
            position_odom.Set(
                odometry_x,
                odometry_y,
                (float)(System.DateTime.UtcNow - new System.DateTime(1970,1,1)).TotalMilliseconds
            ); 

            rotation_odom =
                UnityEngine.Quaternion.Euler(
                    this.gameObject.transform.rotation.eulerAngles.x,
                    this.gameObject.transform.rotation.eulerAngles.y,
                    odometry_orientation
                );

            this.gameObject.transform.SetPositionAndRotation(position_odom, rotation_odom);

            // apply IMU feedback to child

            this.m_handle.GetIMU(
                ref ic4_phi,
                ref ic4_theta,
                ref ic4_psi,
                ref ic4_acc_x,
                ref ic4_acc_y,
                ref ic4_acc_z
            );

            position_IMU.Set(
                ic4_acc_x,
                ic4_acc_y,
                ic4_acc_z
            );

            rotation_IMU =
                UnityEngine.Quaternion.Euler(
                    ic4_phi,
                    ic4_theta,
                    ic4_psi
                );

            this.m_IMU.transform.SetPositionAndRotation(position_IMU, rotation_IMU);

        } /* FixedUpdate() */

        void OnEnable()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonRealRobosoftEntityBehaviour.OnEnable() : robot activated !"
            );
            
            // reset odometry / localization
            this.m_handle.ResetMission();
            this.m_handle.ResetLocalization();
            this.m_handle.ResetOdometry();

            // initialize transform to origin
            this.gameObject.transform.SetPositionAndRotation(
                _initialPosition,
                _initialRotation
            );

        } /* OnEnable() */

        void OnDisable()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonRealRobosoftEntityBehaviour.OnDisable() : robot deactivated & prepared for next trial"
            );

            // Start current mission to rotate !! apply immediate change
            this.m_handle.StartMission();

            // reset transform to origin
            this.gameObject.transform.SetPositionAndRotation(
                _initialPosition,
                _initialRotation
            );

        } /* OnDisable() */

        public void Start(float initial_velocity)
        {

            // set current target speed
            this.m_handle.SetLinearVelocityTarget(initial_velocity);

            // set moving
            this.m_bIsMoving = true;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonRealRobosoftEntityBehaviour.Start() : applied initial velocity [ "
                + initial_velocity
                + " ]"
            );

        } /* Start() */

        public void Accelerate(float saturation_velocity, float acceleration_factor)
        {

            // set current target speed
            this.m_handle.SetLinearVelocityTarget(saturation_velocity);

            // mark as accelerating
            this.m_bIsAccelerating = true;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonRealRobosoftEntityBehaviour.Accelerate() : applied target saturation velocity [ "
                + saturation_velocity
                + " ]"
            );

        } /* Accelerate() */

        public void Stop()
        {

            // set fixed
            this.m_bIsMoving = false;

            // set current target speed
            this.m_handle.SetLinearVelocityTarget(0.0f);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonRealRobosoftEntityBehaviour.Stop() : applied target zero velocity"
            );

        } /* Stop() */

    } /* public class ApollonRealRobosoftEntityBehaviour */

} /* } Labsim.apollon.gameplay.entity */
