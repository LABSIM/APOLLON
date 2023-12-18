// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    public class AgencyAndThresholdPerceptionV4ControlDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<AgencyAndThresholdPerceptionV4ControlBridge>
    {
        #region event args class

        public class AgencyAndThresholdPerceptionV4ControlEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public AgencyAndThresholdPerceptionV4ControlEventArgs(
                float throttle_axis_value = 0.0f, 
                float joystick_horizontal_axis_value = 0.0f, 
                float joystick_vertical_axis_value = 0.0f, 
                bool trigger_value = false
            )
                : base()
            {
                this.Throttle           = throttle_axis_value;
                this.JoystickHorizontal = joystick_horizontal_axis_value;
                this.JoystickVertical   = joystick_vertical_axis_value;
                this.Response           = trigger_value;
            }

            // ctor
            public AgencyAndThresholdPerceptionV4ControlEventArgs(AgencyAndThresholdPerceptionV4ControlEventArgs rhs)
                : base(rhs)
            {
                this.Throttle           = rhs.Throttle;
                this.JoystickHorizontal = rhs.JoystickHorizontal;
                this.JoystickVertical   = rhs.JoystickVertical;
                this.Response           = rhs.Response;
            }

            // property
            public float Throttle { get; protected set; }
            public float JoystickHorizontal { get; protected set; }
            public float JoystickVertical { get; protected set; }
            public bool Response { get; protected set; }

        } /* AgencyAndThresholdPerceptionV4ControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>> 
            _eventThrottleValueChangedList             = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventThrottleNeutralCommandTriggeredList  = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventThrottlePositiveCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventThrottleNegativeCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventJoystickHorizontalValueChangedList   = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventJoystickVerticalValueChangedList     = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>(),
            _eventResponseTriggeredList                = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>>();

        #endregion

        // Constructor
        public AgencyAndThresholdPerceptionV4ControlDispatcher()
        {

            // event table
            this._eventTable.Add("ThrottleValueChanged",             null);
            this._eventTable.Add("ThrottleNeutralCommandTriggered",  null);
            this._eventTable.Add("ThrottlePositiveCommandTriggered", null);
            this._eventTable.Add("ThrottleNegativeCommandTriggered", null);
            this._eventTable.Add("JoystickHorizontalValueChanged",   null);
            this._eventTable.Add("JoystickVerticalValueChanged",     null);
            this._eventTable.Add("ResponseTriggered",                null);

        } /* AgencyAndThresholdPerceptionV4ControlDispatcher() */

        #region actual events

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> ThrottleValueChangedEvent
        {
            add
            {
                this._eventThrottleValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleValueChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventThrottleValueChangedList.Contains(value))
                {
                    return;
                }
                this._eventThrottleValueChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleValueChanged"] = null;
                    foreach (var eventThrottleValueChanged in this._eventThrottleValueChangedList)
                    {
                        this._eventTable["ThrottleValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleValueChanged"] + eventThrottleValueChanged;
                    }
                }
            }

        } /* ThrottleValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> ThrottleNeutralCommandTriggeredEvent
        {
            add
            {
                this._eventThrottleNeutralCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventThrottleNeutralCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventThrottleNeutralCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNeutralCommandTriggered"] = null;
                    foreach (var eventThrottleNeutralCommandTriggered in this._eventThrottleNeutralCommandTriggeredList)
                    {
                        this._eventTable["ThrottleNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"] + eventThrottleNeutralCommandTriggered;
                    }
                }
            }

        } /* ThrottleNeutralCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> ThrottlePositiveCommandTriggeredEvent
        {
            add
            {
                this._eventThrottlePositiveCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottlePositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventThrottlePositiveCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventThrottlePositiveCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottlePositiveCommandTriggered"] = null;
                    foreach (var eventThrottlePositiveCommandTriggered in this._eventThrottlePositiveCommandTriggeredList)
                    {
                        this._eventTable["ThrottlePositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"] + eventThrottlePositiveCommandTriggered;
                    }
                }
            }

        } /* ThrottlePositiveCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> ThrottleNegativeCommandTriggeredEvent
        {
            add
            {
                this._eventThrottleNegativeCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventThrottleNegativeCommandTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventThrottleNegativeCommandTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNegativeCommandTriggered"] = null;
                    foreach (var eventThrottleNegativeCommandTriggered in this._eventThrottleNegativeCommandTriggeredList)
                    {
                        this._eventTable["ThrottleNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"] + eventThrottleNegativeCommandTriggered;
                    }
                }
            }

        } /* ThrottleNegativeCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> JoystickHorizontalValueChangedEvent
        {
            add
            {
                this._eventJoystickHorizontalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventJoystickHorizontalValueChangedList.Contains(value))
                {
                    return;
                }
                this._eventJoystickHorizontalValueChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickHorizontalValueChanged"] = null;
                    foreach (var eventJoystickHorizontalValueChanged in this._eventJoystickHorizontalValueChangedList)
                    {
                        this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + eventJoystickHorizontalValueChanged;
                    }
                }
            }

        } /* JoystickHorizontalValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> JoystickVerticalValueChangedEvent
        {
            add
            {
                this._eventJoystickVerticalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventJoystickVerticalValueChangedList.Contains(value))
                {
                    return;
                }
                this._eventJoystickVerticalValueChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickVerticalValueChanged"] = null;
                    foreach (var eventJoystickVerticalValueChanged in this._eventJoystickVerticalValueChangedList)
                    {
                        this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + eventJoystickVerticalValueChanged;
                    }
                }
            }

        } /* JoystickVerticalValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs> ResponseTriggeredEvent
        {
            add
            {
                this._eventResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ResponseTriggered"] + value;
                }
            }

            remove
            {
                if (!this._eventResponseTriggeredList.Contains(value))
                {
                    return;
                }
                this._eventResponseTriggeredList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ResponseTriggered"] = null;
                    foreach (var eventResponseTriggered in this._eventResponseTriggeredList)
                    {
                        this._eventTable["ResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ResponseTriggered"] + eventResponseTriggered;
                    }
                }
            }

        } /* ResponseTriggeredEvent */

        #endregion

        #region raise events

        public void RaiseThrottleValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs(throttle_axis_value: value));
            }
        } /* RaiseThrottleValueChanged() */

        public void RaiseThrottleNeutralCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs());
            }
        } /* RaiseThrottleNeutralCommandTriggered() */

        public void RaiseThrottlePositiveCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs());
            }
        } /* RaiseThrottlePositiveCommandTriggered() */

        public void RaiseThrottleNegativeCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs());
            }
        } /* RaiseThrottleNegativeCommandTriggered() */

        public void RaiseJoystickHorizontalValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs(joystick_horizontal_axis_value: value));
            }
        } /* RaiseJoystickHorizontalValueChanged() */

        public void RaiseJoystickVerticalValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs(joystick_vertical_axis_value: value));
            }
        } /* RaiseJoystickVerticalValueChanged() */

        public void RaiseResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV4ControlEventArgs>)this._eventTable["ResponseTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV4ControlEventArgs(trigger_value: true));
            }
        } /* RaiseResponseTriggered() */

        #endregion

    } /* class AgencyAndThresholdPerceptionV4ControlDispatcher */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
