
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

    public class ApollonSimulatedRobosoftEntityBehaviour 
        : ApollonConcreteGameplayBehaviour<ApollonSimulatedRobosoftEntityBridge>
    {

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject skeletonRoot = null;

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject headRoot = null;

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject leapRig = null;

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject hoverKit = null;

        #region private members

        private UnityEngine.Transform
            m_skeletonRootPreviousParent = null,
            m_leapRigPreviousParent = null,
            m_hoverKitPreviousParent = null;

        private UnityEngine.Vector3 
            m_initialVelocity = new UnityEngine.Vector3(),
            m_targetVelocity = new UnityEngine.Vector3(),
            m_continuousAcceleration = new UnityEngine.Vector3(),
            m_initialRigibodyForce = new UnityEngine.Vector3();

        private bool 
            m_bIsSaturated = false,
            m_bIsMoving = false,
            m_bIsAccelerating = false;

        private static readonly UnityEngine.Vector3 _initialPosition = new UnityEngine.Vector3(0.0f, 0.2385f, 0.0f);
        private static readonly UnityEngine.Quaternion _initialRotation = UnityEngine.Quaternion.identity;

        #endregion

        void Start()
        {
            
            // save initial state
            this.m_initialRigibodyForce = this.gameObject.GetComponent<UnityEngine.Rigidbody>().transform.position;
           
        } /* Start() */

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
          
            // check
            if(!this.m_bIsSaturated && this.m_bIsAccelerating)
            {
                // saturate velocity when max speed is reached
                if (this.gameObject.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude > this.m_targetVelocity.magnitude)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.FixedUpdate() : saturation event ! [ actual:"
                        + this.gameObject.GetComponent<UnityEngine.Rigidbody>().velocity + "{mag:" + this.gameObject.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude + "}"
                        + " > target:"
                        + this.m_targetVelocity + "{mag:" + this.m_targetVelocity.magnitude + "}"
                        + " ]"
                    );

                    // saturate
                    this.gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(this.m_targetVelocity, UnityEngine.ForceMode.VelocityChange);

                    // mark as saturated
                    this.m_bIsSaturated = true;

                } /* if() */

            } /* if() */

            // so now apply acceleration depending on current saturation
            if (!this.m_bIsSaturated && this.m_bIsAccelerating)
            {
                // continuous accel
                this.gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(this.m_continuousAcceleration, UnityEngine.ForceMode.Acceleration);
                
            } /* if() */

            // update linked game object transform first from current behaviour transform

            UnityEngine.Vector3 position = UnityEngine.Vector3.zero;
            UnityEngine.Quaternion rotation = UnityEngine.Quaternion.identity;

            position.Set(
                this.gameObject.transform.position.x,
                this.gameObject.transform.position.y,
                this.gameObject.transform.position.z
            );
            rotation.Set(
                this.gameObject.transform.rotation.x,
                this.gameObject.transform.rotation.y,
                this.gameObject.transform.rotation.z,
                this.gameObject.transform.rotation.w
            );
            
            this.skeletonRoot.transform.SetPositionAndRotation(position, rotation);

        } /* FixedUpdate */

        void OnEnable()
        {

            // get current user mass detail
            float user_mass = 60.0f;
            if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["mass"].ToString(), out user_mass))
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Yellow>Warning: </color> ApollonSimulatedRobosoftEntityBehaviour.OnEnable() : failed to get current participant mass detail, setup mass to default value [ "
                    + user_mass
                    + " ]"
                );

            } /* if() */

            // add to settings 
            this.gameObject.GetComponent<UnityEngine.Rigidbody>().mass += user_mass;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.OnEnable() : added participant mass detail to simulated setup, total mass value is [ "
                + this.gameObject.GetComponent<UnityEngine.Rigidbody>().mass
                + " ]"
            );

            // initialize transform to origin

            this.gameObject.transform.SetPositionAndRotation(
                _initialPosition,
                _initialRotation
            );

            // get ref to parent
            this.m_leapRigPreviousParent = this.leapRig.transform.parent;
            this.m_hoverKitPreviousParent = this.hoverKit.transform.parent;
            this.m_skeletonRootPreviousParent = this.skeletonRoot.transform.parent;

        } /* OnEnable() */

        void OnDisable()
        {

            // get current user mass detail
            float user_mass = 60.0f;
            if (!float.TryParse(experiment.ApollonExperimentManager.Instance.Session.participantDetails["mass"].ToString(), out user_mass))
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Yellow>Warning: </color> ApollonSimulatedRobosoftEntityBehaviour.OnDisable() : failed to get current participant mass detail, setup mass to default value [ "
                    + user_mass
                    + " ]"
                );

            } /* if() */

            // remove from settings 
            this.gameObject.GetComponent<UnityEngine.Rigidbody>().mass -= user_mass;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.OnDisable() : removed participant mass detail from simulated setup, now total mass value is [ "
                + this.gameObject.GetComponent<UnityEngine.Rigidbody>().mass
                + " ]"
            );

            // reset transform to origin

            this.gameObject.transform.SetPositionAndRotation(
                _initialPosition,
                _initialRotation
            );

            // nullify ref to parent
            this.m_leapRigPreviousParent 
                = this.m_hoverKitPreviousParent
                = this.m_skeletonRootPreviousParent
                = null;

        } /* OnDisable() */

        public void Start(float initial_velocity)
        {

            // get current target speed vector
            this.m_initialVelocity = UnityEngine.Vector3.forward * initial_velocity;

            // apply immediate change
            this.gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(this.m_initialVelocity, UnityEngine.ForceMode.VelocityChange);

            // set moving
            this.m_bIsMoving = true;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.Start() : applied initial velocity [ "
                + this.m_initialVelocity + "{mag:" + this.m_initialVelocity.magnitude + "}"
                + " ]"
            );

        } /* Start() */

        public void Accelerate(float saturation_velocity, float acceleration_factor)
        {

            // get current target speed vector
            this.m_targetVelocity = UnityEngine.Vector3.forward * saturation_velocity;

            // get continuous acceleration vector
            this.m_continuousAcceleration = UnityEngine.Vector3.forward * acceleration_factor;

            // mark as accelerating
            this.m_bIsAccelerating = true;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.Accelerate() : applied target saturation velocity [ "
                + this.m_targetVelocity + "{mag:" + this.m_targetVelocity.magnitude + "}"
                + " ] with a continuous acceleration of [ "
                + this.m_continuousAcceleration + "{mag:" + this.m_continuousAcceleration.magnitude + "}"
                + " ]"
            );

        } /* Accelerate() */

        public void Stop()
        {

            // set fixed
            this.m_bIsMoving = false;

            // reset velocity/acceleration vector
            this.m_targetVelocity
               = this.m_initialVelocity
               = this.m_continuousAcceleration
               = UnityEngine.Vector3.zero;

            // -------------------- // 
            //   reset rigidbody    // 
            // -------------------- // 
            /* zero velocity        */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(this.m_targetVelocity, UnityEngine.ForceMode.VelocityChange);
            /* zero accel           */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().AddForce(this.m_continuousAcceleration, UnityEngine.ForceMode.Acceleration);
            /* zero transform       */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().transform.position = this.m_initialRigibodyForce;
            /* force zero velocity  */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().velocity = UnityEngine.Vector3.zero;
            /* reset center of mass */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().ResetCenterOfMass();
            /* reset inertia tensor */ this.gameObject.GetComponent<UnityEngine.Rigidbody>().ResetInertiaTensor();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonSimulatedRobosoftEntityBehaviour.Stop() : applied target zero velocity [ "
                + this.m_targetVelocity + "{mag:" + this.m_targetVelocity.magnitude + "}"
                + " ]"
            );

        } /* Stop() */

    } /* public class ApollonSimulatedRobosoftEntityBehaviour */

} /* } Labsim.apollon.gameplay.entity */