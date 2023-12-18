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
namespace Labsim.apollon.gameplay.device.command
{
    public class ApollonVirtualMotionSystemCommandDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonVirtualMotionSystemCommandBridge>
    {
        
        #region event args class

        public class VirtualMotionSystemCommandEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public VirtualMotionSystemCommandEventArgs()
                : base()
            {} 

            public VirtualMotionSystemCommandEventArgs(
                float[] angular_acceleration_target = null,
                float[] angular_velocity_saturation_threshold = null, 
                float[] angular_displacement_limiter = null,
                float[] linear_acceleration_target = null,
                float[] linear_velocity_saturation_threshold = null, 
                float[] linear_dispplacement_limiter = null,
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
                this.LinearDisplacementLimiter = linear_dispplacement_limiter;
                this.Duration = duration;
                this.InhibitVestibularMotion = inhibit_vestibular_motion;
            }

            // ctor
            public VirtualMotionSystemCommandEventArgs(VirtualMotionSystemCommandEventArgs rhs)
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

        } /* VirtualMotionSystemCommandEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>> 
            _eventInitCommandList           = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>(),
            _eventIdleCommandList           = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>(),
            _eventAccelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>(),
            _eventDecelerationCommandList   = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>(),
            _eventSaturationCommandList     = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<VirtualMotionSystemCommandEventArgs>>();

        #endregion

        // Constructor
        public ApollonVirtualMotionSystemCommandDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Accelerate", null);
            this._eventTable.Add("Decelerate", null);
            this._eventTable.Add("Saturation", null);
            this._eventTable.Add("Reset",      null);

        } /* ApollonVirtualMotionSystemCommandDispatcher() */

        #region actual events

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Init"] + value;
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
                        this._eventTable["Init"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Idle"] + value;
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
                        this._eventTable["Idle"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Accelerate"] + value;
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
                        this._eventTable["Accelerate"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Accelerate"] + eventAcceleration;
                    }
                }
            }

        } /* AccelerateEvent */

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> DecelerateEvent
        {
            add
            {
                this._eventDecelerationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Decelerate"] + value;
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
                        this._eventTable["Decelerate"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Decelerate"] + eventDeceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> SaturationEvent
        {
            add
            {
                this._eventSaturationCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Saturation"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Saturation"] + value;
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
                        this._eventTable["Saturation"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Saturation"] + eventSaturation;
                    }
                }
            }

        } /* SaturationEvent */

        public event System.EventHandler<VirtualMotionSystemCommandEventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Reset"] + value;
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
                        this._eventTable["Reset"] = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Reset"] + eventReset;
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
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new VirtualMotionSystemCommandEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new VirtualMotionSystemCommandEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseAccelerate(
            float[] angular_acceleration_target,
            float[] angular_velocity_saturation_threshold, 
            float[] angular_displacement_limiter,
            float[] linear_acceleration_target,
            float[] linear_velocity_saturation_threshold, 
            float[] linear_dispplacement_limiter,
            float duration,
            bool without_motion
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new VirtualMotionSystemCommandEventArgs(
                        angular_acceleration_target : angular_acceleration_target,
                        angular_velocity_saturation_threshold : angular_velocity_saturation_threshold, 
                        angular_displacement_limiter : angular_displacement_limiter,
                        linear_acceleration_target : linear_acceleration_target,
                        linear_velocity_saturation_threshold : linear_velocity_saturation_threshold, 
                        linear_dispplacement_limiter :linear_dispplacement_limiter,
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
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(this, new VirtualMotionSystemCommandEventArgs());
            }

        } /* RaiseDecelerate() */

        public void RaiseSaturation()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Saturation"];
                callback?.Invoke(this, new VirtualMotionSystemCommandEventArgs());
            }

        } /* RaiseSaturation() */

        public void RaiseReset(float duration = -1.0f)
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<VirtualMotionSystemCommandEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new VirtualMotionSystemCommandEventArgs(
                    duration : duration
                ));
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonVirtualMotionSystemCommandDispatcher */

} /* } Labsim.apollon.gameplay.device.command */