// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{
    public class ApollonCAVIAREntityDispatcher
    {
        #region event args class

        public class EventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(float linear_acceleration = 0.0f, float linear_velocity = 0.0f)
                : base()
            {
                this.LinearAcceleration = linear_acceleration;
                this.LinearVelocity = linear_velocity;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                this.LinearAcceleration = rhs.LinearAcceleration;
                this.LinearVelocity = rhs.LinearVelocity;
            }

            // property
            public float LinearAcceleration { get; protected set; }
            public float LinearVelocity { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> 
            _eventAccelerateList        = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventDecelerateList        = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventIdleList              = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventHoldList              = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventWaypointReachedList   = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonCAVIAREntityDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Accelerate", null },
                { "Decelerate", null },
                { "Idle", null },
                { "Hold", null },
                { "WaypointReached", null }
            };

        } /* ApollonCAVIAREntityDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Accelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventAccelerateList.Contains(value))
                {
                    return;
                }
                this._eventAccelerateList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = null;
                    foreach (var eventAcceleration in this._eventAccelerateList)
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
                this._eventDecelerateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"] + value;
                }
            }

            remove
            {
                if (!this._eventDecelerateList.Contains(value))
                {
                    return;
                }
                this._eventDecelerateList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = null;
                    foreach (var eventAcceleration in this._eventDecelerateList)
                    {
                        this._eventTable["Decelerate"] = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"] + eventAcceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<EventArgs> IdleEvent
        {
            add
            {
                this._eventIdleList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<EventArgs>)this._eventTable["Idle"] + value;
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
                    foreach (var eventAcceleration in this._eventIdleList)
                    {
                        this._eventTable["Idle"] = (System.EventHandler<EventArgs>)this._eventTable["Idle"] + eventAcceleration;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<EventArgs> HoldEvent
        {
            add
            {
                this._eventHoldList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Hold"] = (System.EventHandler<EventArgs>)this._eventTable["Hold"] + value;
                }
            }

            remove
            {
                if (!this._eventHoldList.Contains(value))
                {
                    return;
                }
                this._eventHoldList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Hold"] = null;
                    foreach (var eventAcceleration in this._eventHoldList)
                    {
                        this._eventTable["Hold"] = (System.EventHandler<EventArgs>)this._eventTable["Hold"] + eventAcceleration;
                    }
                }
            }

        } /* HoldEvent */

        public event System.EventHandler<EventArgs> WaypointReachedEvent
        {
            add
            {
                this._eventWaypointReachedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["WaypointReached"] = (System.EventHandler<EventArgs>)this._eventTable["WaypointReached"] + value;
                }
            }

            remove
            {
                if (!this._eventWaypointReachedList.Contains(value))
                {
                    return;
                }
                this._eventWaypointReachedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["WaypointReached"] = null;
                    foreach (var eventAcceleration in this._eventWaypointReachedList)
                    {
                        this._eventTable["WaypointReached"] = (System.EventHandler<EventArgs>)this._eventTable["WaypointReached"] + eventAcceleration;
                    }
                }
            }

        } /* WaypointReachedEvent */

        #endregion

        #region raise events

        public void RaiseAccelerate(
            float linear_acceleration_value, 
            float linear_volocity_value
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new EventArgs(
                        linear_acceleration: linear_acceleration_value, 
                        linear_velocity: linear_volocity_value
                    )
                );
            }

        } /* RaiseAccelerate() */

        public void RaiseDecelerate(
            float linear_acceleration_value, 
            float linear_volocity_value
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(
                    this, 
                    new EventArgs(
                        linear_acceleration: linear_acceleration_value, 
                        linear_velocity: linear_volocity_value
                    )
                );
            }

        } /* RaiseDecelerate() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseHold()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Hold"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseHold() */

        public void RaiseWaypointReached()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["WaypointReached"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseWaypointReached() */

        #endregion

    } /* class ApollonCAVIAREntityDispatcher */

} /* } Labsim.apollon.gameplay.entity */