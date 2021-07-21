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
            : MotionSystems.ForceSeatMI_IPositioningInterface
        {
            static readonly int _accumulator_max_samples_capacity = 3;

            private bool m_firstCall = true;
            private UnityEngine.GameObject m_behaviour = null;

            public ApollonMotionSystemPS6TM550Command(UnityEngine.GameObject behaviour)
            {
                m_behaviour = behaviour;
            }

            public virtual void Begin()
            {
                m_firstCall = true;
            }

            public virtual void End()
            {
                m_firstCall = true;
            }

            private void UpdateMatrix(ref MotionSystems.FSMI_TopTableMatrixPhysical pos, float ax, float ay, float az, float dx, float dy, float dz)
            {
                float sinAX = UnityEngine.Mathf.Sin(ax);
                float cosAX = UnityEngine.Mathf.Cos(ax);
                float sinAY = UnityEngine.Mathf.Sin(ay);
                float cosAY = UnityEngine.Mathf.Cos(ay);
                float sinAZ = UnityEngine.Mathf.Sin(az);
                float cosAZ = UnityEngine.Mathf.Cos(az);

                pos.m11 = cosAY*cosAZ;
                pos.m12 = cosAZ*sinAX*sinAY - cosAX*sinAZ;
                pos.m13 = cosAX*cosAZ*sinAY + sinAX*sinAZ;
                pos.m14 = dx;

                pos.m21 = cosAY*sinAZ;
                pos.m22 = cosAX*cosAZ + sinAX*sinAY*sinAZ;
                pos.m23 = -cosAZ*sinAX + cosAX*sinAY*sinAZ;
                pos.m24 = dy;

                pos.m31 = -sinAY;
                pos.m32 = cosAY*sinAX;
                pos.m33 = cosAX*cosAY;
                pos.m34 = dz;

                pos.m41 = 0;
                pos.m42 = 0;
                pos.m43 = 0;
                pos.m44 = 1;

            } /* UpdateMatrix() */

            public virtual void Update(float deltaTime, ref MotionSystems.FSMI_TopTableMatrixPhysical matrix)
            { 

                // check first call
                if(this.m_firstCall) {

                    this.UpdateMatrix(
                        ref matrix,
                        0.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 0.0f
                    );

                    // reset
                    this.m_firstCall = false;
                    
                    // skip the rest
                    return;

                } /* if() */

                // update values
				this.UpdateMatrix(
                    ref matrix, 
                    /* pitch in rad */ -1.0f * this.m_behaviour.transform.localRotation.eulerAngles.x * UnityEngine.Mathf.Deg2Rad, 
                    /* roll in rad  */ -1.0f * this.m_behaviour.transform.localRotation.eulerAngles.z * UnityEngine.Mathf.Deg2Rad, 
                    /* yaw in rad   */ this.m_behaviour.transform.localRotation.eulerAngles.y * UnityEngine.Mathf.Deg2Rad, 
                    /* sway in mm   */ this.m_behaviour.transform.localPosition.x * 1000.0f, 
                    /* surge in mm  */ this.m_behaviour.transform.localPosition.z * 1000.0f, 
                    /* heave in mm  */ this.m_behaviour.transform.localPosition.y * 1000.0f
                );

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
                                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                            ).Behaviour.gameObject
                        );
                    this.m_FSMI_Sensor
                        = new ApollonMotionSystemPS6TM550Sensor(
                            gameplay.ApollonGameplayManager.Instance.getBridge(
                                gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
                            ).Behaviour.gameObject
                        );
                    this.m_FSMI_Updater = new ApollonMotionSystemPS6TM550Updater(this);

                    this.m_FSMI_UnityAPI.SetAppID(""); // If you have dedicated app id, remove ActivateProfile calls from your code
                    this.m_FSMI_UnityAPI.ActivateProfile("LABSIM - " + experiment.ApollonExperimentManager.Instance.getActiveProfile());
                    this.m_FSMI_UnityAPI.SetPositioningObject(this.m_FSMI_Command);
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
