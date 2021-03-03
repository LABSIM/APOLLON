// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonRadioSondeSensorDispatcher
    {
        #region event args class

        public class EventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(
                float distance_from_sensor_to_hit = -1.0f, 
                UnityEngine.Vector3 hit_position = new UnityEngine.Vector3(),
                UnityEngine.Quaternion hit_orientation = new UnityEngine.Quaternion()
            )
                : base()
            {
                // assign
                this.DistanceFromSensorToHit = distance_from_sensor_to_hit;
                this.HitWorldPosition = hit_position;
                this.HitWorldOrientation = hit_orientation;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                this.DistanceFromSensorToHit = rhs.DistanceFromSensorToHit;
                this.HitWorldPosition = rhs.HitWorldPosition;
                this.HitWorldOrientation = rhs.HitWorldOrientation;
            }

            // property
            public float DistanceFromSensorToHit { get; protected set; }
            public UnityEngine.Vector3 HitWorldPosition { get; protected set; }
            public UnityEngine.Quaternion HitWorldOrientation { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventDistanceFromSensorToHitChangedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();
        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventHitPositionChangedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();
        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventHitOrientationChangedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();
        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventHitChangedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonRadioSondeSensorDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "DistanceFromSensorToHitChanged", null },
                { "HitPositionChanged", null },
                { "HitOrientationChanged", null },
                { "HitChanged", null }
            };

        } /* ApollonRadioSondeSensorDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> DistanceFromSensorToHitChangedEvent
        {
            add
            {
                this._eventDistanceFromSensorToHitChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["DistanceFromSensorToHitChanged"] = (System.EventHandler<EventArgs>)this._eventTable["DistanceFromSensorToHitChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventDistanceFromSensorToHitChangedList.Contains(value))
                {
                    return;
                }
                this._eventDistanceFromSensorToHitChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["DistanceFromSensorToHitChanged"] = null;
                    foreach (var eventDistanceFromSensorToHitChanged in this._eventDistanceFromSensorToHitChangedList)
                    {
                        this._eventTable["DistanceFromSensorToHitChanged"] = (System.EventHandler<EventArgs>)this._eventTable["DistanceFromSensorToHitChanged"] + eventDistanceFromSensorToHitChanged;
                    }
                }
            }

        } /* DistanceFromSensorToHitChangedEvent */

        public event System.EventHandler<EventArgs> HitPositionChangedEvent
        {
            add
            {
                this._eventHitPositionChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitPositionChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitPositionChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventHitPositionChangedList.Contains(value))
                {
                    return;
                }
                this._eventHitPositionChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitPositionChanged"] = null;
                    foreach (var eventHitPositionChanged in this._eventHitPositionChangedList)
                    {
                        this._eventTable["HitPositionChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitPositionChanged"] + eventHitPositionChanged;
                    }
                }
            }

        } /* HitPositionChangedEvent */

        public event System.EventHandler<EventArgs> HitOrientationChangedEvent
        {
            add
            {
                this._eventHitOrientationChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitOrientationChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitOrientationChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventHitOrientationChangedList.Contains(value))
                {
                    return;
                }
                this._eventHitOrientationChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitOrientationChanged"] = null;
                    foreach (var eventHitOrientationChanged in this._eventHitOrientationChangedList)
                    {
                        this._eventTable["HitOrientationChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitOrientationChanged"] + eventHitOrientationChanged;
                    }
                }
            }

        } /* HitOrientationChangedEvent */

        public event System.EventHandler<EventArgs> HitChangedEvent
        {
            add
            {
                this._eventHitChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventHitChangedList.Contains(value))
                {
                    return;
                }
                this._eventHitChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["HitChanged"] = null;
                    foreach (var eventHitChanged in this._eventHitChangedList)
                    {
                        this._eventTable["HitChanged"] = (System.EventHandler<EventArgs>)this._eventTable["HitChanged"] + eventHitChanged;
                    }
                }
            }

        } /* HitChangedEvent */

        #endregion

        #region raise events

        public void RaiseDistanceFromSensorToHitChangedEvent(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["DistanceFromSensorToHitChanged"];
                callback?.Invoke(this, new EventArgs(distance_from_sensor_to_hit: value));
            }
        } /* RaiseDistanceFromSensorToHitChangedEvent() */

        public void RaiseHitPositionChangedEvent(UnityEngine.Vector3 value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["HitPositionChanged"];
                callback?.Invoke(this, new EventArgs(hit_position: value));
            }
        } /* RaiseHitPositionChangedEvent() */
        
        public void RaiseHitOrientationChangedEvent(UnityEngine.Quaternion value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["HitOrientationChanged"];
                callback?.Invoke(this, new EventArgs(hit_orientation: value));
            }
        } /* RaiseHitOrientationChangedEvent() */

        public void RaiseHitChangedEvent(
            float distance,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion orientation
        )
        {
            // current event subscriber notification
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["HitChanged"];
                callback?.Invoke(this, new EventArgs(distance, position, orientation));
            }

            // follow call to sub event subscriber
            this.RaiseDistanceFromSensorToHitChangedEvent(distance);
            this.RaiseHitPositionChangedEvent(position);
            this.RaiseHitOrientationChangedEvent(orientation);

        } /* RaiseHitChangedEvent() */

        #endregion

    } /* class ApollonRadioSondeSensorDispatcher */

} /* } Labsim.apollon.gameplay.device.sensor */
