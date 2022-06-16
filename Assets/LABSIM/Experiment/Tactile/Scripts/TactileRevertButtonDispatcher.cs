using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileRevertButtonDispatcher
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
            _eventPressedList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public TactileRevertButtonDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "Pressed", null }
            };

        } /* TactileRevertButtonDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> PressedEvent
        {
            add
            {
                this._eventPressedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Pressed"] = (System.EventHandler<EventArgs>)this._eventTable["Pressed"] + value;
                }
            }

            remove
            {
                if (!this._eventPressedList.Contains(value))
                {
                    return;
                }
                this._eventPressedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Pressed"] = null;
                    foreach (var eventAcceleration in this._eventPressedList)
                    {
                        this._eventTable["Pressed"] = (System.EventHandler<EventArgs>)this._eventTable["Pressed"] + eventAcceleration;
                    }
                }
            }

        } /* PressedEvent */

        #endregion

        #region raise events

        public void RaisePressed()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["Pressed"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaisePressed() */

        #endregion

    } /* class TactileRevertButtonDispatcher */

} /* } Labsim.experiment.tactile */