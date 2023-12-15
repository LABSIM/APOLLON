// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{
    public class CAVIAREntityDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<CAVIAREntityBridge>
    {
        #region event args class

        public class CAVIAREntityEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public CAVIAREntityEventArgs(float linear_acceleration = 0.0f, float linear_velocity = 0.0f)
                : base()
            {
                this.LinearAcceleration = linear_acceleration;
                this.LinearVelocity = linear_velocity;
            }

            // ctor
            public CAVIAREntityEventArgs(CAVIAREntityEventArgs rhs)
                : base(rhs)
            {
                this.LinearAcceleration = rhs.LinearAcceleration;
                this.LinearVelocity = rhs.LinearVelocity;
            }

            // property
            public float LinearAcceleration { get; protected set; }
            public float LinearVelocity { get; protected set; }

        } /* CAVIAREntityEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>> 
            _eventAccelerateList        = new System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>>(),
            _eventDecelerateList        = new System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>>(),
            _eventIdleList              = new System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>>(),
            _eventHoldList              = new System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>>(),
            _eventWaypointReachedList   = new System.Collections.Generic.List<System.EventHandler<CAVIAREntityEventArgs>>();

        #endregion

        // Constructor
        public CAVIAREntityDispatcher()
        {

            // event table
            this._eventTable.Add("Accelerate",      null);
            this._eventTable.Add("Decelerate",      null);
            this._eventTable.Add("Idle",            null);
            this._eventTable.Add("Hold",            null);
            this._eventTable.Add("WaypointReached", null);

        } /* CAVIAREntityDispatcher() */

        #region actual events

        public event System.EventHandler<CAVIAREntityEventArgs> AccelerateEvent
        {
            add
            {
                this._eventAccelerateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Accelerate"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Accelerate"] + value;
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
                        this._eventTable["Accelerate"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Accelerate"] + eventAcceleration;
                    }
                }
            }

        } /* AccelerateEvent */

        public event System.EventHandler<CAVIAREntityEventArgs> DecelerateEvent
        {
            add
            {
                this._eventDecelerateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Decelerate"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Decelerate"] + value;
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
                        this._eventTable["Decelerate"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Decelerate"] + eventAcceleration;
                    }
                }
            }

        } /* DecelerateEvent */

        public event System.EventHandler<CAVIAREntityEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Idle"] + value;
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
                        this._eventTable["Idle"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Idle"] + eventAcceleration;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<CAVIAREntityEventArgs> HoldEvent
        {
            add
            {
                this._eventHoldList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Hold"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Hold"] + value;
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
                        this._eventTable["Hold"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Hold"] + eventAcceleration;
                    }
                }
            }

        } /* HoldEvent */

        public event System.EventHandler<CAVIAREntityEventArgs> WaypointReachedEvent
        {
            add
            {
                this._eventWaypointReachedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["WaypointReached"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["WaypointReached"] + value;
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
                        this._eventTable["WaypointReached"] = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["WaypointReached"] + eventAcceleration;
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
                var callback = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Accelerate"];
                callback?.Invoke(
                    this, 
                    new CAVIAREntityEventArgs(
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
                var callback = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Decelerate"];
                callback?.Invoke(
                    this, 
                    new CAVIAREntityEventArgs(
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
                var callback = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new CAVIAREntityEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseHold()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["Hold"];
                callback?.Invoke(this, new CAVIAREntityEventArgs());
            }

        } /* RaiseHold() */

        public void RaiseWaypointReached()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<CAVIAREntityEventArgs>)this._eventTable["WaypointReached"];
                callback?.Invoke(this, new CAVIAREntityEventArgs());
            }

        } /* RaiseWaypointReached() */

        #endregion

    } /* class CAVIAREntityDispatcher */

} /* } Labsim.apollon.gameplay.entity */