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
            public EventArgs()
                : base()
            {

            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
            }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> 
            _eventIdleCommandList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventVisualOnlyCommandList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventVestibularOnlyCommandList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventVisuoVestibularCommandList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonActiveSeatEntityDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Idle", null },
                { "VisualOnly", null },
                { "VestibularOnly", null },
                { "VisuoVestibular", null }
            };

        } /* ApollonActiveSeatEntityDispatcher() */

        #region actual events

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

        public event System.EventHandler<EventArgs> VisualOnlyEvent
        {
            add
            {
                this._eventVisualOnlyCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisualOnly"] = (System.EventHandler<EventArgs>)this._eventTable["VisualOnly"] + value;
                }
            }

            remove
            {
                if (!this._eventVisualOnlyCommandList.Contains(value))
                {
                    return;
                }
                this._eventVisualOnlyCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisualOnly"] = null;
                    foreach (var eventVisualOnly in this._eventVisualOnlyCommandList)
                    {
                        this._eventTable["VisualOnly"] = (System.EventHandler<EventArgs>)this._eventTable["VisualOnly"] + eventVisualOnly;
                    }
                }
            }

        } /* VisualOnlyEvent */

        public event System.EventHandler<EventArgs> VestibularOnlyEvent
        {
            add
            {
                this._eventVestibularOnlyCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VestibularOnly"] = (System.EventHandler<EventArgs>)this._eventTable["VestibularOnly"] + value;
                }
            }

            remove
            {
                if (!this._eventVestibularOnlyCommandList.Contains(value))
                {
                    return;
                }
                this._eventVestibularOnlyCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VestibularOnly"] = null;
                    foreach (var eventVestibularOnly in this._eventVestibularOnlyCommandList)
                    {
                        this._eventTable["VestibularOnly"] = (System.EventHandler<EventArgs>)this._eventTable["VestibularOnly"] + eventVestibularOnly;
                    }
                }
            }

        } /* VestibularOnlyEvent */

        public event System.EventHandler<EventArgs> VisuoVestibularEvent
        {
            add
            {
                this._eventVisuoVestibularCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisuoVestibular"] = (System.EventHandler<EventArgs>)this._eventTable["VisuoVestibular"] + value;
                }
            }

            remove
            {
                if (!this._eventVisuoVestibularCommandList.Contains(value))
                {
                    return;
                }
                this._eventVisuoVestibularCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisuoVestibular"] = null;
                    foreach (var eventVisuoVestibular in this._eventVisuoVestibularCommandList)
                    {
                        this._eventTable["VisuoVestibular"] = (System.EventHandler<EventArgs>)this._eventTable["VisuoVestibular"] + eventVisuoVestibular;
                    }
                }
            }

        } /* VisuoVestibularEvent */

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

        public void RaiseVisualOnly()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["VisualOnly"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseVisualOnly() */

        public void RaiseVestibularOnly()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["VestibularOnly"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseVestibularOnly() */

        public void RaiseVisuoVestibular()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["VisuoVestibular"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseVisuoVestibular() */

        #endregion

    } /* class ApollonActiveSeatEntityDispatcher */

} /* } Labsim.apollon.gameplay.entity */