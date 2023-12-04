// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionControlDispatcher
        : ApolloConcreteGameplayDispatcher<ApollonAgencyAndThresholdPerceptionControlBridge>
    {
        #region event args class

        public class AgencyAndThresholdPerceptionControlEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public AgencyAndThresholdPerceptionControlEventArgs(float z = 0.0f, bool button15 = false)
                : base()
            {
                this.Z = z;
                this.Button15 = Button15;
            }

            // ctor
            public AgencyAndThresholdPerceptionControlEventArgs(AgencyAndThresholdPerceptionControlEventArgs rhs)
                : base(rhs)
            {
                this.Z = rhs.Z;
                this.Button15 = rhs.Button15;
            }

            // property
            public float Z { get; protected set; }
            public bool Button15 { get; protected set; }

        } /* AgencyAndThresholdPerceptionControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>> 
            _eventAxisZValueChangedList            = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>>(),
            _eventUserNeutralCommandTriggeredList  = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>>(),
            _eventUserPositiveCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>>(),
            _eventUserNegativeCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>>(),
            _eventUserResponseTriggeredList        = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>>();

        #endregion

        // Constructor
        public ApollonAgencyAndThresholdPerceptionControlDispatcher()
        {

            // event table
            this._eventTable.Add("AxisZValueChanged",            null);
            this._eventTable.Add("UserNeutralCommandTriggered",  null);
            this._eventTable.Add("UserPositiveCommandTriggered", null);
            this._eventTable.Add("UserNegativeCommandTriggered", null);
            this._eventTable.Add("UserResponseTriggered",        null);

        } /* ApollonAgencyAndThresholdPerceptionControlDispatcher() */

        #region actual events

        public event System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs> AxisZValueChangedEvent
        {
            add
            {
                this._eventAxisZValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["AxisZValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["AxisZValueChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventAxisZValueChangedList.Contains(value))
                {
                    return;
                }
                this._eventAxisZValueChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["AxisZValueChanged"] = null;
                    foreach (var eventAxisZValueChanged in this._eventAxisZValueChangedList)
                    {
                        this._eventTable["AxisZValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["AxisZValueChanged"] + eventAxisZValueChanged;
                    }
                }
            }

        } /* AxisZValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs> UserNeutralCommandTriggeredEvent
        {
            add
            {
                this._eventUserNeutralCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventUserNeutralCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventUserNeutralCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserNeutralCommandTriggered"] = null;
                    foreach (var eventUserNeutralCommandTriggered in this._eventUserNeutralCommandTriggeredList)
                    {
                        this._eventTable["UserNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"] + eventUserNeutralCommandTriggered;
                    }
                }
            }

        } /* UserNeutralCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs> UserPositiveCommandTriggeredEvent
        {
            add
            {
                this._eventUserPositiveCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserPositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserPositiveCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventUserPositiveCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventUserPositiveCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserPositiveCommandTriggered"] = null;
                    foreach (var eventUserPositiveCommandTriggered in this._eventUserPositiveCommandTriggeredList)
                    {
                        this._eventTable["UserPositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserPositiveCommandTriggered"] + eventUserPositiveCommandTriggered;
                    }
                }
            }

        } /* UserPositiveCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs> UserNegativeCommandTriggeredEvent
        {
            add
            {
                this._eventUserNegativeCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNegativeCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventUserNegativeCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventUserNegativeCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserNegativeCommandTriggered"] = null;
                    foreach (var eventUserNegativeCommandTriggered in this._eventUserNegativeCommandTriggeredList)
                    {
                        this._eventTable["UserNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNegativeCommandTriggered"] + eventUserNegativeCommandTriggered;
                    }
                }
            }

        } /* UserNegativeCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs> UserResponseTriggeredEvent
        {
            add
            {
                this._eventUserResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserResponseTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventUserResponseTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventUserResponseTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserResponseTriggered"] = null;
                    foreach (var eventUserResponseTriggered in this._eventUserResponseTriggeredList)
                    {
                        this._eventTable["UserResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserResponseTriggered"] + eventUserResponseTriggered;
                    }
                }
            }

        } /* UserNeutralCommandTriggeredEvent */

        #endregion

        #region raise events

        public void RaiseAxisZValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["AxisZValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionControlEventArgs(z: value));
            }
        } /* RaiseAxisZValueChanged() */

        public void RaiseUserNeutralCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionControlEventArgs());
            }
        } /* RaiseUserNeutralCommandTriggered() */

        public void RaiseUserPositiveCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserPositiveCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionControlEventArgs());
            }
        } /* RaiseUserPositiveCommandTriggered() */

        public void RaiseUserNegativeCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserNegativeCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionControlEventArgs());
            }
        } /* RaiseUserNegativeCommandTriggered() */

        public void RaiseUserResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionControlEventArgs>)this._eventTable["UserResponseTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionControlEventArgs(button15: true));
            }
        } /* RaiseUserResponseTriggered() */

        #endregion

    } /* class ApollonAgencyAndThresholdPerceptionControlDispatcher */

} /* } Labsim.apollon.gameplay.control */
