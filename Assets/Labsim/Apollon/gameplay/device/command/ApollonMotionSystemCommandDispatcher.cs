// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.command
{
    public class ApollonMotionSystemCommandDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonMotionSystemCommandBridge>
    {
        #region event args class

        public class MotionSystemCommandEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public MotionSystemCommandEventArgs()
                : base()
            {} 

            public MotionSystemCommandEventArgs(
                float[] angular_acceleration_target = null,
                float[] angular_velocity_saturation_threshold = null, 
                float[] angular_displacement_limiter = null, 
                float[] linear_acceleration_target = null,
                float[] linear_velocity_saturation_threshold = null, 
                float[] linear_displacement_limiter = null,
                float duration = 0.0f,
                bool inhibit_vestibular_motion = true
            )
                : base()
            {
                this.AngularAccelerationTarget = angular_acceleration_target;
                this.AngularVelocitySaturationThreshold = angular_velocity_saturation_threshold;
                this.AngularDisplacementLimiter = angular_displacement_limiter;
                this.LinearAccelerationTarget = linear_acceleration_target;
                this.LinearVelocitySaturationThreshold = linear_velocity_saturation_threshold;
                this.LinearDisplacementLimiter = linear_displacement_limiter;
                this.Duration = duration;
                this.InhibitVestibularMotion = inhibit_vestibular_motion;
            }

            // ctor
            public MotionSystemCommandEventArgs(MotionSystemCommandEventArgs rhs)
                : base(rhs)
            {
                this.AngularAccelerationTarget = rhs.AngularAccelerationTarget;
                this.AngularVelocitySaturationThreshold = rhs.AngularVelocitySaturationThreshold;
                this.AngularDisplacementLimiter = rhs.AngularDisplacementLimiter;
                this.LinearAccelerationTarget = rhs.LinearAccelerationTarget;
                this.LinearVelocitySaturationThreshold = rhs.LinearVelocitySaturationThreshold;
                this.LinearDisplacementLimiter = rhs.LinearDisplacementLimiter;
                this.Duration = rhs.Duration;
                this.InhibitVestibularMotion = rhs.InhibitVestibularMotion;
            }

            // property
            public float[] AngularAccelerationTarget { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float[] AngularVelocitySaturationThreshold { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float[] AngularDisplacementLimiter { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float[] LinearAccelerationTarget { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float[] LinearVelocitySaturationThreshold { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float[] LinearDisplacementLimiter { get; protected set; } = new float[3] { 0.0f, 0.0f, 0.0f };
            public float Duration { get; protected set; }
            public bool InhibitVestibularMotion { get; protected set; }

        } /* MotionSystemCommandEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>> 
            _eventInitCommandList           = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>(),
            _eventIdleCommandList           = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>(),
            _eventAccelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>(),
            _eventDecelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>(),
            _eventSaturationCommandList     = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<MotionSystemCommandEventArgs>>();

        #endregion

        // Constructor
        public ApollonMotionSystemCommandDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Accelerate", null);
            this._eventTable.Add("Decelerate", null);
            this._eventTable.Add("Saturation", null);
            this._eventTable.Add("Reset",      null);

        } /* ApollonMotionSystemCommandDispatcher() */

        #region actual events

        public event System.EventHandler<MotionSystemCommandEventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Init"] + value;
                }
            }

            remove
            {
                if (!this._eventInitCommandList.Contains(value))
                {
                    return;
                }
                this._eventInitCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = null;
                    foreach (var eventInit in this._eventInitCommandList)
                    {
                        this._eventTable["Init"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<MotionSystemCommandEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Idle"] + value;
                }
            }

            remove
            {
                if (!this._eventIdleCommandList.Contains(value))
                {
                    return;
                }
                this._eventIdleCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = null;
                    foreach (var eventIdle in this._eventIdleCommandList)
                    {
                        this._eventTable["Idle"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<MotionSystemCommandEventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Accelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventAccelerationCommandList.Contains(value))
                {
                    return;
                }
                this._eventAccelerationCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = null;
                    foreach (var eventAcceleration in this._eventAccelerationCommandList)
                    {
                        this._eventTable["Accelerate"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Accelerate"] + eventAcceleration;
                    }
                }
            }

        } /* AccelerateEvent */

        public event System.EventHandler<MotionSystemCommandEventArgs> DecelerateEvent
        {
            add
            {
                this._eventDecelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Decelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventDecelerationCommandList.Contains(value))
                {
                    return;
                }
                this._eventDecelerationCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = null;
                    foreach (var eventDeceleration in this._eventDecelerationCommandList)
                    {
                        this._eventTable["Decelerate"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Decelerate"] + eventDeceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<MotionSystemCommandEventArgs> SaturationEvent
        {
            add
            {
                this._eventSaturationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Saturation"] + value;
                }
            }

            remove
            {
                if (!this._eventSaturationCommandList.Contains(value))
                {
                    return;
                }
                this._eventSaturationCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = null;
                    foreach (var eventSaturation in this._eventSaturationCommandList)
                    {
                        this._eventTable["Saturation"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Saturation"] + eventSaturation;
                    }
                }
            }

        } /* SaturationEvent */

        public event System.EventHandler<MotionSystemCommandEventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Reset"] + value;
                }
            }

            remove
            {
                if (!this._eventResetCommandList.Contains(value))
                {
                    return;
                }
                this._eventResetCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = null;
                    foreach (var eventReset in this._eventResetCommandList)
                    {
                        this._eventTable["Reset"] = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Reset"] + eventReset;
                    }
                }
            }

        } /* ResetEvent */

        #endregion

        #region raise events

        public void RaiseInit()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new MotionSystemCommandEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new MotionSystemCommandEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseAccelerate(
            float[] angular_acceleration_target,
            float[] angular_velocity_saturation_threshold, 
            float[] angular_displacement_limiter,
            float[] linear_acceleration_target,
            float[] linear_velocity_saturation_threshold, 
            float[] linear_displacement_limiter,
            float duration,
            bool without_motion
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new MotionSystemCommandEventArgs(
                        angular_acceleration_target : angular_acceleration_target,
                        angular_velocity_saturation_threshold : angular_velocity_saturation_threshold, 
                        angular_displacement_limiter : angular_displacement_limiter,
                        linear_acceleration_target : linear_acceleration_target,
                        linear_velocity_saturation_threshold : linear_velocity_saturation_threshold, 
                        linear_displacement_limiter :linear_displacement_limiter,
                        duration : duration,
                        inhibit_vestibular_motion : without_motion
                    )
                );
            }

        } /* RaiseAccelerate() */

        public void RaiseDecelerate()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(this, new MotionSystemCommandEventArgs());
            }

        } /* RaiseDecelerate() */

        public void RaiseSaturation()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Saturation"];
                callback?.Invoke(this, new MotionSystemCommandEventArgs());
            }

        } /* RaiseSaturation() */

        public void RaiseReset(float duration = -1.0f)
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemCommandEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new MotionSystemCommandEventArgs(
                    duration : duration
                ));
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonMotionSystemCommandDispatcher */

} /* } Labsim.apollon.gameplay.device.command */