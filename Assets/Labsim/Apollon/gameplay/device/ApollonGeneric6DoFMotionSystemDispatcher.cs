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
namespace Labsim.apollon.gameplay.device
{
    public class ApollonGeneric6DoFMotionSystemDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonGeneric6DoFMotionSystemBridge>
    {
        
        #region event args class

        public class MotionSystemEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public MotionSystemEventArgs()
                : base()
            {} 

            public MotionSystemEventArgs(
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
            public MotionSystemEventArgs(MotionSystemEventArgs rhs)
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

        } /* MotionSystemEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>> 
            _eventInitList           = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventIdleList           = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventAccelerationList   = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventDecelerationList   = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventSaturationList     = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventControlList        = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>(),
            _eventResetList          = new System.Collections.Generic.List<System.EventHandler<MotionSystemEventArgs>>();

        #endregion

        // Constructor
        public ApollonGeneric6DoFMotionSystemDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Accelerate", null);
            this._eventTable.Add("Decelerate", null);
            this._eventTable.Add("Saturation", null);
            this._eventTable.Add("Control",    null);
            this._eventTable.Add("Reset",      null);

        } /* ApollonGeneric6DoFMotionSystemDispatcher() */

        #region actual events

        public event System.EventHandler<MotionSystemEventArgs> InitEvent
        {
            add
            {
                this._eventInitList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Init"] + value;
                }
            }

            remove
            {
                if (!this._eventInitList.Contains(value))
                {
                    return;
                }
                this._eventInitList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = null;
                    foreach (var eventInit in this._eventInitList)
                    {
                        this._eventTable["Init"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<MotionSystemEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Idle"] + value;
                }
            }

            remove
            {
                if (!this._eventIdleList.Contains(value))
                {
                    return;
                }
                this._eventIdleList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = null;
                    foreach (var eventIdle in this._eventIdleList)
                    {
                        this._eventTable["Idle"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<MotionSystemEventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerationList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Accelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventAccelerationList.Contains(value))
                {
                    return;
                }
                this._eventAccelerationList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = null;
                    foreach (var eventAcceleration in this._eventAccelerationList)
                    {
                        this._eventTable["Accelerate"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Accelerate"] + eventAcceleration;
                    }
                }
            }

        } /* AccelerateEvent */

        public event System.EventHandler<MotionSystemEventArgs> DecelerateEvent
        {
            add
            {
                this._eventDecelerationList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Decelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventDecelerationList.Contains(value))
                {
                    return;
                }
                this._eventDecelerationList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = null;
                    foreach (var eventDeceleration in this._eventDecelerationList)
                    {
                        this._eventTable["Decelerate"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Decelerate"] + eventDeceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<MotionSystemEventArgs> SaturationEvent
        {
            add
            {
                this._eventSaturationList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Saturation"] + value;
                }
            }

            remove
            {
                if (!this._eventSaturationList.Contains(value))
                {
                    return;
                }
                this._eventSaturationList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = null;
                    foreach (var eventSaturation in this._eventSaturationList)
                    {
                        this._eventTable["Saturation"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Saturation"] + eventSaturation;
                    }
                }
            }

        } /* SaturationEvent */

        public event System.EventHandler<MotionSystemEventArgs> ControlEvent
        {
            add
            {
                this._eventControlList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Control"] + value;
                }
            }

            remove
            {
                if (!this._eventControlList.Contains(value))
                {
                    return;
                }
                this._eventControlList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = null;
                    foreach (var eventControl in this._eventControlList)
                    {
                        this._eventTable["Control"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Control"] + eventControl;
                    }
                }
            }

        } /* ControlEvent */

        public event System.EventHandler<MotionSystemEventArgs> ResetEvent
        {
            add
            {
                this._eventResetList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Reset"] + value;
                }
            }

            remove
            {
                if (!this._eventResetList.Contains(value))
                {
                    return;
                }
                this._eventResetList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = null;
                    foreach (var eventReset in this._eventResetList)
                    {
                        this._eventTable["Reset"] = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Reset"] + eventReset;
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
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new MotionSystemEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new MotionSystemEventArgs());
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
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new MotionSystemEventArgs(
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
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(this, new MotionSystemEventArgs());
            }

        } /* RaiseDecelerate() */

        public void RaiseSaturation()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Saturation"];
                callback?.Invoke(this, new MotionSystemEventArgs());
            }

        } /* RaiseSaturation() */

        public void RaiseControl()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Control"];
                callback?.Invoke(this, new MotionSystemEventArgs());
            }

        } /* RaiseControl() */

        public void RaiseReset(float duration = -1.0f)
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<MotionSystemEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new MotionSystemEventArgs(
                    duration : duration
                ));
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonGeneric6DoFMotionSystemDispatcher */

} /* } Labsim.apollon.gameplay.device.command */