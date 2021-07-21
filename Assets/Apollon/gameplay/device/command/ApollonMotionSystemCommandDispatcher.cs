// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.command
{
    public class ApollonMotionSystemCommandDispatcher
    {
        #region event args class

        public class EventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(float ang_acc = 0.0f, float ang_vel_sat = 0.0f, float duration = 0.0f, float stop_angle = 0.0f, bool inhibit_vestibular_motion = true)
                : base()
            {
                this.AngularAcceleration = ang_acc;
                this.AngularVelocitySaturation = ang_vel_sat;
                this.Duration = duration;
                this.StopAngle = stop_angle;
                this.InhibitVestibularMotion = inhibit_vestibular_motion;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                this.AngularAcceleration = rhs.AngularAcceleration;
                this.AngularVelocitySaturation = rhs.AngularVelocitySaturation;
                this.Duration = rhs.Duration;
                this.StopAngle = rhs.StopAngle;
                this.InhibitVestibularMotion = rhs.InhibitVestibularMotion;
            }

            // property
            public float AngularAcceleration { get; protected set; }
            public float AngularVelocitySaturation { get; protected set; }
            public float Duration { get; protected set; }
            public float StopAngle { get; protected set; }
            public bool InhibitVestibularMotion { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> 
            _eventInitCommandList           = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventIdleCommandList           = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventAccelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventDecelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventSaturationCommandList     = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonMotionSystemCommandDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Init", null },
                { "Idle", null },
                { "Accelerate", null },
                { "Decelerate", null },
                { "Saturation", null },
                { "Reset", null }
            };

        } /* ApollonMotionSystemCommandDispatcher() */

        #region actual events

    public event System.EventHandler<EventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<EventArgs>)this._eventTable["Init"] + value;
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
                        this._eventTable["Init"] = (System.EventHandler<EventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<EventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<EventArgs>)this._eventTable["Idle"] + value;
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
                        this._eventTable["Idle"] = (System.EventHandler<EventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<EventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Accelerate"] + value;
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
                        this._eventTable["Accelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Accelerate"] + eventAcceleration;
                    }
                }
            }

        } /* AccelerateEvent */

        public event System.EventHandler<EventArgs> DecelerateEvent
        {
            add
            {
                this._eventDecelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"] + value;
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
                        this._eventTable["Decelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"] + eventDeceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<EventArgs> SaturationEvent
        {
            add
            {
                this._eventSaturationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = (System.EventHandler<EventArgs>)this._eventTable["Saturation"] + value;
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
                        this._eventTable["Saturation"] = (System.EventHandler<EventArgs>)this._eventTable["Saturation"] + eventSaturation;
                    }
                }
            }

        } /* SaturationEvent */

        public event System.EventHandler<EventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<EventArgs>)this._eventTable["Reset"] + value;
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
                        this._eventTable["Reset"] = (System.EventHandler<EventArgs>)this._eventTable["Reset"] + eventReset;
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
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseAccelerate(
            float angular_acceleration_value, 
            float angular_velocity_saturation_value, 
            float duration_value, 
            float stop_angle_value,
            bool without_motion
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new EventArgs(
                        ang_acc: angular_acceleration_value, 
                        ang_vel_sat: angular_velocity_saturation_value, 
                        duration: duration_value, 
                        stop_angle: stop_angle_value,
                        inhibit_vestibular_motion: without_motion
                    )
                );
            }

        } /* RaiseAccelerate() */

        public void RaiseDecelerate()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseDecelerate() */

        public void RaiseSaturation()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Saturation"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseSaturation() */

        public void RaiseReset()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonMotionSystemCommandDispatcher */

} /* } Labsim.apollon.gameplay.device.command */