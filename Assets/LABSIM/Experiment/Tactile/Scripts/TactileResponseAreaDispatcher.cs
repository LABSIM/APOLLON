using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileResponseAreaDispatcher
    {

        #region event args class

        public class EventArgs
            : apollon.ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(/*float z = 0.0f, bool button15 = false*/)
                : base()
            {
                // this.Z = z;
                // this.Button15 = Button15;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                // this.Z = rhs.Z;
                // this.Button15 = rhs.Button15;
            }

            // property
            // public float Z { get; protected set; }
            // public bool Button15 { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> 
            _eventIdleList                    = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventSpatialConditionList        = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventTemporalConditionList       = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventSpatioTemporalConditionList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public TactileResponseAreaDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Idle", null },
                { "SpatialCondition", null },
                { "TemporalCondition", null },
                { "SpatioTemporalCondition", null }
            };

        } /* TactileResponseAreaDispatcher() */

        #region actual events

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

        public event System.EventHandler<EventArgs> SpatialConditionEvent
        {
            add
            {
                this._eventSpatialConditionList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["SpatialCondition"] = (System.EventHandler<EventArgs>)this._eventTable["SpatialCondition"] + value;
                }
            }

            remove
            {
                if (!this._eventSpatialConditionList.Contains(value))
                {
                    return;
                }
                this._eventSpatialConditionList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["SpatialCondition"] = null;
                    foreach (var eventAcceleration in this._eventSpatialConditionList)
                    {
                        this._eventTable["SpatialCondition"] = (System.EventHandler<EventArgs>)this._eventTable["SpatialCondition"] + eventAcceleration;
                    }
                }
            }

        } /* SpatialConditionEvent */

        public event System.EventHandler<EventArgs> TemporalConditionEvent
        {
            add
            {
                this._eventTemporalConditionList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["TemporalCondition"] = (System.EventHandler<EventArgs>)this._eventTable["TemporalCondition"] + value;
                }
            }

            remove
            {
                if (!this._eventTemporalConditionList.Contains(value))
                {
                    return;
                }
                this._eventTemporalConditionList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["TemporalCondition"] = null;
                    foreach (var eventAcceleration in this._eventTemporalConditionList)
                    {
                        this._eventTable["TemporalCondition"] = (System.EventHandler<EventArgs>)this._eventTable["TemporalCondition"] + eventAcceleration;
                    }
                }
            }

        } /* TemporalConditionEvent */

        public event System.EventHandler<EventArgs> SpatioTemporalConditionEvent
        {
            add
            {
                this._eventSpatioTemporalConditionList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["SpatioTemporalCondition"] = (System.EventHandler<EventArgs>)this._eventTable["SpatioTemporalCondition"] + value;
                }
            }

            remove
            {
                if (!this._eventSpatioTemporalConditionList.Contains(value))
                {
                    return;
                }
                this._eventSpatioTemporalConditionList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["SpatioTemporalCondition"] = null;
                    foreach (var eventAcceleration in this._eventSpatioTemporalConditionList)
                    {
                        this._eventTable["SpatioTemporalCondition"] = (System.EventHandler<EventArgs>)this._eventTable["SpatioTemporalCondition"] + eventAcceleration;
                    }
                }
            }

        } /* SpatioTemporalConditionEvent */

        #endregion

        #region raise events

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseSpatialCondition() 
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["SpatialCondition"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseSpatialCondition() */

        public void RaiseTemporalCondition() 
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["TemporalCondition"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseTemporalCondition() */

        public void RaiseSpatioTemporalCondition()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["SpatioTemporalCondition"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseSpatioTemporalCondition() */

        #endregion

    } /* class TactileResponseAreaDispatcher */

} /* } Labsim.experiment.tactile */