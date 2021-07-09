// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{

    public class ApollonMotionSystemPS6TM550Handle
        : ApollonAbstractDefaultHandle
    {

        // ctor     
        public ApollonMotionSystemPS6TM550Handle()
            : base()
        { this.m_handleID = ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle; }

        #region Motion System API implementation : Gateway / Updater / Command / Sensor

        private class ApollonMotionSystemPS6TM550Updater
        {

            private ApollonMotionSystemPS6TM550Handle m_handle = null;

            public ApollonMotionSystemPS6TM550Updater(ApollonMotionSystemPS6TM550Handle handle)
            {
                // register
                this.m_handle = handle;
                ApollonEngine.Instance.EngineFixedUpdateEvent += this.OnEngineFixedUpdate;
            }

            private void OnEngineFixedUpdate(object sender, ApollonEngine.EngineEventArgs arg)
            {

                // ForceSeatMI - BEGIN

                // Use extra parameters to generate custom effects, for exmp. vibrations. They will NOT be
                // filtered, smoothed or processed in any way.

                this.m_handle.m_FSMI_CommandExtraParameters.yaw   = 0.0f;
                this.m_handle.m_FSMI_CommandExtraParameters.pitch = 0.0f;
                this.m_handle.m_FSMI_CommandExtraParameters.roll  = 0.0f;
                this.m_handle.m_FSMI_CommandExtraParameters.sway  = 0.0f;
                this.m_handle.m_FSMI_CommandExtraParameters.heave = 0.0f;
                this.m_handle.m_FSMI_CommandExtraParameters.surge = 0.0f;

                this.m_handle.m_FSMI_UnityAPI.AddExtra(this.m_handle.m_FSMI_CommandExtraParameters);
                this.m_handle.m_FSMI_UnityAPI.Update(UnityEngine.Time.fixedDeltaTime);

                // ForceSeatMI - END

            } /* OnEngineFixedUpdate() */
        
        } /* private class ApollonMotionSystemPS6TM550Updater */

        private class ApollonMotionSystemPS6TM550Command
            : MotionSystems.ForceSeatMI_ITelemetryInterface
        {

            private bool m_firstCall = true;
            private float m_prevSurgeSpeed = 0.0f;
            private float m_prevSwaySpeed = 0.0f;
            private float m_prevHeaveSpeed = 0.0f;
            private float m_prevYawSpeed = 0.0f;
            private float m_prevPitchSpeed = 0.0f;
            private float m_prevRollSpeed = 0.0f;
            private UnityEngine.Rigidbody m_rb = null;

            public bool IsMotionLocked { get; set; } = false;

            public ApollonMotionSystemPS6TM550Command(UnityEngine.Rigidbody rb)
            {
                m_rb = rb;
            }

            public virtual void Begin()
            {
                m_firstCall = true;
            }

            public virtual void End()
            {
                m_firstCall = true;
            }

            public virtual void Update(float deltaTime, ref MotionSystems.FSMI_Telemetry telemetry)
            { 
                
                // check locking motion component or first call
                if(/*this.IsMotionLocked ||*/ this.m_firstCall) {

                    telemetry.speed 
                        = telemetry.yaw 
                        = telemetry.yawSpeed
                        = telemetry.yawAcceleration
                        = telemetry.pitch 
                        = telemetry.pitchSpeed
                        = telemetry.pitchAcceleration
                        = telemetry.roll 
                        = telemetry.rollSpeed
                        = telemetry.rollAcceleration
                        = telemetry.surgeSpeed
                        = telemetry.surgeAcceleration
                        = telemetry.heaveSpeed
                        = telemetry.heaveAcceleration
                        = telemetry.swaySpeed
                        = telemetry.swayAcceleration
                        = 0.0f;

                    this.m_prevSurgeSpeed 
                        = this.m_prevSwaySpeed
                        = this.m_prevHeaveSpeed
                        = this.m_prevYawSpeed
                        = this.m_prevPitchSpeed
                        = this.m_prevRollSpeed
                        = 0.0f;
                    
                    // reset signal if necessaray
                    if (this.m_firstCall)
                    {
                        this.m_firstCall = false;
                    }

                    // skip the rest
                    return;

                } /* if() */

                // extract local Rb speeds
                var linear_velocity  = this.m_rb.transform.InverseTransformDirection(this.m_rb.velocity);
                var angular_velocity  = this.m_rb.transform.InverseTransformDirection(this.m_rb.angularVelocity);
                var localRotation = this.m_rb.transform.localRotation;
                
                // update telemetry

                telemetry.surgeAcceleration = (linear_velocity.z - this.m_prevSurgeSpeed) / deltaTime;
                telemetry.swayAcceleration  = (linear_velocity.x - this.m_prevSwaySpeed) / deltaTime;
                telemetry.heaveAcceleration = (linear_velocity.y - this.m_prevHeaveSpeed) / deltaTime;

                telemetry.surgeSpeed = linear_velocity.z;
                telemetry.swaySpeed  = linear_velocity.x;
                telemetry.heaveSpeed = linear_velocity.y;

                telemetry.yawAcceleration = (angular_velocity.y - this.m_prevYawSpeed) / deltaTime;
                telemetry.pitchAcceleration  = -1.0f * (angular_velocity.x - this.m_prevPitchSpeed) / deltaTime;
                telemetry.rollAcceleration = -1.0f * (angular_velocity.z - this.m_prevRollSpeed) / deltaTime;

                telemetry.yawSpeed   = angular_velocity.y;
                telemetry.pitchSpeed = -1.0f * angular_velocity.x;
                telemetry.rollSpeed  = -1.0f * angular_velocity.z;

                telemetry.yaw
                    = ( 
                        UnityEngine.Mathf.Deg2Rad 
                        * (localRotation.eulerAngles.y > 180 ? localRotation.eulerAngles.y - 360 : localRotation.eulerAngles.y)
                    );
                telemetry.pitch
                    = (
                        -1.0f 
                        * UnityEngine.Mathf.Deg2Rad 
                        * (localRotation.eulerAngles.x > 180 ? localRotation.eulerAngles.x - 360 : localRotation.eulerAngles.x)
                    );
                telemetry.roll       
                    = (
                        -1.0f 
                        * UnityEngine.Mathf.Deg2Rad 
                        * (localRotation.eulerAngles.z > 180 ? localRotation.eulerAngles.z - 360 : localRotation.eulerAngles.z)
                    );

                // backup for next iteration  
                this.m_prevSurgeSpeed = telemetry.surgeSpeed;
                this.m_prevSwaySpeed  = telemetry.swaySpeed;
                this.m_prevHeaveSpeed = telemetry.heaveSpeed;
                this.m_prevYawSpeed   = telemetry.yawSpeed;
                this.m_prevPitchSpeed = telemetry.pitchSpeed;
                this.m_prevRollSpeed  = telemetry.rollSpeed;

            } /* Update() */

            public virtual void Pause(bool paused)
            {
                m_firstCall = true;
            }

        } /* private class ApollonMotionSystemPS6TM550Command */

        private class ApollonMotionSystemPS6TM550Sensor
            : MotionSystems.ForceSeatMI_IPlatformInfoInterface
        {

            private UnityEngine.GameObject m_behaviour = null;


            public ApollonMotionSystemPS6TM550Sensor(UnityEngine.GameObject behaviour)
            {
                m_behaviour = behaviour;
            }

            public virtual void Begin()
            {
            }

            public virtual void End()
            {
            }

            public virtual void Update(float deltaTime, ref MotionSystems.FSMI_PlatformInfo platformInfo)
            { 

                this.m_behaviour.transform.SetPositionAndRotation(
                    new UnityEngine.Vector3(
                        platformInfo.fkSway,
                        platformInfo.fkHeave,
                        platformInfo.fkSurge
                    ),
                    UnityEngine.Quaternion.Euler(
                        -1.0f * platformInfo.fkPitch_deg,
                        platformInfo.fkYaw_deg,
                        -1.0f * platformInfo.fkRoll_deg
                    )
                );
                
            } /* Update() */

            public virtual void Pause(bool paused)
            {
            }

        } /* private class ApollonMotionSystemPS6TM550Sensor */

        // ForceSeatMI API
        private MotionSystems.ForceSeatMI_Unity m_FSMI_UnityAPI = null;
        private MotionSystems.ForceSeatMI_Unity.ExtraParameters m_FSMI_CommandExtraParameters;
        private ApollonMotionSystemPS6TM550Command m_FSMI_Command = null;
        private ApollonMotionSystemPS6TM550Sensor m_FSMI_Sensor = null;
        private ApollonMotionSystemPS6TM550Updater m_FSMI_Updater = null;

        #endregion

        #region Abstract Apollon handle overriding 

        public bool Initialized { get; private set; } = false;

        protected override void Dispose(bool bDisposing = true)
        {

        } /* Dispose(bool) */
        
        public override void OnHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemPS6TM550Handle.OnHandleActivationRequested() : requesting activation"
                );
                
                if(!this.Initialized) 
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemPS6TM550Handle.OnHandleActivationRequested() : initialize motion system API"
                    );
                    
                    // ForceSeatMI - BEGIN
                    this.m_FSMI_UnityAPI = new MotionSystems.ForceSeatMI_Unity();
                    this.m_FSMI_CommandExtraParameters = new MotionSystems.ForceSeatMI_Unity.ExtraParameters();
                    this.m_FSMI_Command 
                        = new ApollonMotionSystemPS6TM550Command(
                            gameplay.ApollonGameplayManager.Instance.getBridge(
                                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemPS6TM550Command
                            ).Behaviour.GetComponent<UnityEngine.Rigidbody>()
                        );
                    this.m_FSMI_Sensor
                        = new ApollonMotionSystemPS6TM550Sensor(
                            gameplay.ApollonGameplayManager.Instance.getBridge(
                                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemPS6TM550Sensor
                            ).Behaviour.gameObject
                        );
                    this.m_FSMI_Updater = new ApollonMotionSystemPS6TM550Updater(this);

                    this.m_FSMI_UnityAPI.SetAppID(""); // If you have dedicated app id, remove ActivateProfile calls from your code
                    this.m_FSMI_UnityAPI.ActivateProfile("LABSIM - " + experiment.ApollonExperimentManager.Instance.getActiveProfile());
                    this.m_FSMI_UnityAPI.SetTelemetryObject(this.m_FSMI_Command);
                    this.m_FSMI_UnityAPI.SetPlatformInfoObject(this.m_FSMI_Sensor);
                    this.m_FSMI_UnityAPI.Pause(false);
                    this.m_FSMI_UnityAPI.Begin();
                    // ForceSeatMI - END

                    // mark as init
                    this.Initialized = true;

                } /* if() */

                // pull-up
                base.OnHandleActivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleActivationRequested() */

        // unregistration
        public override void OnHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonMotionSystemPS6TM550Handle.OnHandleDeactivationRequested() : requesting deactivation "
                );

                if(this.Initialized) 
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonMotionSystemPS6TM550Handle.OnHandleDeactivationRequested() : finalize motion system API"
                    );
                    
                    // ForceSeatMI - BEGIN
                    this.m_FSMI_UnityAPI.Pause(true);
                    this.m_FSMI_UnityAPI.End();
                    // ForceSeatMI - END

                    // mark as init
                    this.Initialized = false;

                } /* if() */

                // pull-up
                base.OnHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleDeactivationRequested() */

        #endregion

    } /* class ApollonMotionSystemPS6TM550Handle */
    
} /* namespace Labsim.apollon.backend */
