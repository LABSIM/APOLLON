using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileHapticEntityDispatcher
    {

        #region event args class

        public class EventArgs
            : apollon.ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(/*float foo = 0.0f*/)
                : base()
            {
                // this.Foo = foo;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                // this.Foo = rhs.Foo;
            }

            // property
            // public float Foo { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> 
            _eventRequestStimCCList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventRequestStimCVList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventRequestStimVCList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>(),
            _eventRequestStimVVList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public TactileHapticEntityDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "RequestStimCC", null },
                { "RequestStimCV", null },
                { "RequestStimVC", null },
                { "RequestStimVV", null }
            };

        } /* TactileHapticEntityDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> RequestStimCCEvent
        {
            add
            {
                this._eventRequestStimCCList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimCC"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCC"] + value;
                }
            }

            remove
            {
                if (!this._eventRequestStimCCList.Contains(value))
                {
                    return;
                }
                this._eventRequestStimCCList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimCC"] = null;
                    foreach (var eventAcceleration in this._eventRequestStimCCList)
                    {
                        this._eventTable["RequestStimCC"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCC"] + eventAcceleration;
                    }
                }
            }

        } /* RequestStimCCEvent */

        public event System.EventHandler<EventArgs> RequestStimCVEvent
        {
            add
            {
                this._eventRequestStimCVList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimCV"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCV"] + value;
                }
            }

            remove
            {
                if (!this._eventRequestStimCVList.Contains(value))
                {
                    return;
                }
                this._eventRequestStimCVList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimCV"] = null;
                    foreach (var eventAcceleration in this._eventRequestStimCVList)
                    {
                        this._eventTable["RequestStimCV"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCV"] + eventAcceleration;
                    }
                }
            }

        } /* RequestStimCVEvent */

        public event System.EventHandler<EventArgs> RequestStimVCEvent
        {
            add
            {
                this._eventRequestStimVCList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimVC"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVC"] + value;
                }
            }

            remove
            {
                if (!this._eventRequestStimVCList.Contains(value))
                {
                    return;
                }
                this._eventRequestStimVCList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimVC"] = null;
                    foreach (var eventAcceleration in this._eventRequestStimVCList)
                    {
                        this._eventTable["RequestStimVC"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVC"] + eventAcceleration;
                    }
                }
            }

        } /* RequestStimVCEvent */

        public event System.EventHandler<EventArgs> RequestStimVVEvent
        {
            add
            {
                this._eventRequestStimVVList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimVV"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVV"] + value;
                }
            }

            remove
            {
                if (!this._eventRequestStimVVList.Contains(value))
                {
                    return;
                }
                this._eventRequestStimVVList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["RequestStimVV"] = null;
                    foreach (var eventAcceleration in this._eventRequestStimVVList)
                    {
                        this._eventTable["RequestStimVV"] = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVV"] + eventAcceleration;
                    }
                }
            }

        } /* RequestStimVVEvent */

        #endregion

        #region raise events

        public void RaiseStimCCRequested()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCC"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseStimCCRequested() */

        public void RaiseStimCVRequested()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["RequestStimCV"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseStimCVRequested() */
        
        public void RaiseStimVCRequested()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVC"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseStimVCRequested() */
        
        public void RaiseStimVVRequested()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["RequestStimVV"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseStimVVRequested() */

        #endregion

    } /* class TactileHapticEntityDispatcher */

} /* } Labsim.experiment.tactile */