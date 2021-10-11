// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{
    public class ApollonActiveSeatEntityDispatcher
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
            _eventAccelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventInteruptCommandList       = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventSaturationCommandList     = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventStopCommandList           = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonActiveSeatEntityDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Accelerate", null },
                { "Interupt", null },
                { "Saturation", null },
                { "Stop", null },
                { "Reset", null }
            };

        } /* ApollonActiveSeatEntityDispatcher() */

        #region actual events

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

        public event System.EventHandler<EventArgs> InteruptEvent
        {
            add
            {
                this._eventInteruptCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Interupt"] = (System.EventHandler<EventArgs>)this._eventTable["Interupt"] + value;
                }
            }

            remove
            {
                if (!this._eventInteruptCommandList.Contains(value))
                {
                    return;
                }
                this._eventInteruptCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Interupt"] = null;
                    foreach (var eventInterupt in this._eventInteruptCommandList)
                    {
                        this._eventTable["Interupt"] = (System.EventHandler<EventArgs>)this._eventTable["Interupt"] + eventInterupt;
                    }
                }
            }

        } /* InteruptEvent */

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

        public event System.EventHandler<EventArgs> StopEvent
        {
            add
            {
                this._eventStopCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Stop"] = (System.EventHandler<EventArgs>)this._eventTable["Stop"] + value;
                }
            }

            remove
            {
                if (!this._eventStopCommandList.Contains(value))
                {
                    return;
                }
                this._eventStopCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Stop"] = null;
                    foreach (var eventStop in this._eventStopCommandList)
                    {
                        this._eventTable["Stop"] = (System.EventHandler<EventArgs>)this._eventTable["Stop"] + eventStop;
                    }
                }
            }

        } /* StopEvent */

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

        public void RaiseInterupt()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Interupt"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseInterupt() */

        public void RaiseSaturation()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Saturation"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseSaturation() */

        public void RaiseStop()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Stop"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseStop() */

        public void RaiseReset()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonActiveSeatEntityDispatcher */

} /* } Labsim.apollon.gameplay.entity */