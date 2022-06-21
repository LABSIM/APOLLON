using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileHandMeshClonerDispatcher
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
            _eventButtonClonePressedList = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public TactileHandMeshClonerDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "ButtonClonePressed", null }
            };

        } /* TactileHandMeshClonerDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> ButtonClonePressedEvent
        {
            add
            {
                this._eventButtonClonePressedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ButtonClonePressed"] = (System.EventHandler<EventArgs>)this._eventTable["ButtonClonePressed"] + value;
                }
            }

            remove
            {
                if (!this._eventButtonClonePressedList.Contains(value))
                {
                    return;
                }
                this._eventButtonClonePressedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ButtonClonePressed"] = null;
                    foreach (var eventAcceleration in this._eventButtonClonePressedList)
                    {
                        this._eventTable["ButtonClonePressed"] = (System.EventHandler<EventArgs>)this._eventTable["ButtonClonePressed"] + eventAcceleration;
                    }
                }
            }

        } /* ButtonClonePressedEvent */

        #endregion

        #region raise events

        public void RaiseButtonClonePressed()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["ButtonClonePressed"];
                callback?.Invoke(this, new EventArgs());
            }

        } /* RaiseButtonClonePressed() */

        #endregion

    } /* class TactileHandMeshClonerDispatcher */

} /* } Labsim.experiment.tactile */