﻿// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV3ControlDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonAgencyAndThresholdPerceptionV3ControlBridge>
    {
        #region event args class

        public class AgencyAndThresholdPerceptionV3ControlEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public AgencyAndThresholdPerceptionV3ControlEventArgs(float throttle_axis_value = 0.0f, float joystick_axis_value = 0.0f, bool trigger_value = false)
                : base()
            {
                this.Throttle = throttle_axis_value;
                this.Joystick = joystick_axis_value;
                this.Response = trigger_value;
            }

            // ctor
            public AgencyAndThresholdPerceptionV3ControlEventArgs(AgencyAndThresholdPerceptionV3ControlEventArgs rhs)
                : base(rhs)
            {
                this.Throttle = rhs.Throttle;
                this.Joystick = rhs.Joystick;
                this.Response = rhs.Response;
            }

            // property
            public float Throttle { get; protected set; }
            public float Joystick { get; protected set; }
            public bool Response { get; protected set; }

        } /* AgencyAndThresholdPerceptionV3ControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>> 
            _eventThrottleValueChangedList             = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>(),
            _eventThrottleNeutralCommandTriggeredList  = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>(),
            _eventThrottlePositiveCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>(),
            _eventThrottleNegativeCommandTriggeredList = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>(),
            _eventJoystickValueChangedList             = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>(),
            _eventResponseTriggeredList                = new System.Collections.Generic.List<System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>>();

        #endregion

        // Constructor
        public ApollonAgencyAndThresholdPerceptionV3ControlDispatcher()
        {

            // event table
            this._eventTable.Add("ThrottleValueChanged",             null);
            this._eventTable.Add("ThrottleNeutralCommandTriggered",  null);
            this._eventTable.Add("ThrottlePositiveCommandTriggered", null);
            this._eventTable.Add("ThrottleNegativeCommandTriggered", null);
            this._eventTable.Add("JoystickValueChanged",             null);
            this._eventTable.Add("ResponseTriggered",                null);

        } /* ApollonAgencyAndThresholdPerceptionV3ControlDispatcher() */

        #region actual events

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> ThrottleValueChangedEvent
        {
            add
            {
                this._eventThrottleValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleValueChanged"] + value;
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
                        this._eventTable["ThrottleValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleValueChanged"] + eventThrottleValueChanged;
                    }
                }
            }

        } /* ThrottleValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> ThrottleNeutralCommandTriggeredEvent
        {
            add
            {
                this._eventThrottleNeutralCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"] + value;
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
                        this._eventTable["ThrottleNeutralCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"] + eventThrottleNeutralCommandTriggered;
                    }
                }
            }

        } /* ThrottleNeutralCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> ThrottlePositiveCommandTriggeredEvent
        {
            add
            {
                this._eventThrottlePositiveCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottlePositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"] + value;
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
                        this._eventTable["ThrottlePositiveCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"] + eventThrottlePositiveCommandTriggered;
                    }
                }
            }

        } /* ThrottlePositiveCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> ThrottleNegativeCommandTriggeredEvent
        {
            add
            {
                this._eventThrottleNegativeCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ThrottleNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"] + value;
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
                        this._eventTable["ThrottleNegativeCommandTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"] + eventThrottleNegativeCommandTriggered;
                    }
                }
            }

        } /* ThrottleNegativeCommandTriggeredEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> JoystickValueChangedEvent
        {
            add
            {
                this._eventJoystickValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["JoystickValueChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventJoystickValueChangedList.Contains(value))
                {
                    return;
                }
                this._eventJoystickValueChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickValueChanged"] = null;
                    foreach (var eventJoystickValueChanged in this._eventJoystickValueChangedList)
                    {
                        this._eventTable["JoystickValueChanged"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["JoystickValueChanged"] + eventJoystickValueChanged;
                    }
                }
            }

        } /* JoystickValueChangedEvent */

        public event System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs> ResponseTriggeredEvent
        {
            add
            {
                this._eventResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ResponseTriggered"] + value;
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
                        this._eventTable["ResponseTriggered"] = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ResponseTriggered"] + eventResponseTriggered;
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
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs(throttle_axis_value: value));
            }
        } /* RaiseThrottleValueChanged() */

        public void RaiseThrottleNeutralCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNeutralCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs());
            }
        } /* RaiseThrottleNeutralCommandTriggered() */

        public void RaiseThrottlePositiveCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottlePositiveCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs());
            }
        } /* RaiseThrottlePositiveCommandTriggered() */

        public void RaiseThrottleNegativeCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ThrottleNegativeCommandTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs());
            }
        } /* RaiseThrottleNegativeCommandTriggered() */

        public void RaiseJoystickValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["JoystickValueChanged"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs(joystick_axis_value: value));
            }
        } /* RaiseJoystickValueChanged() */

        public void RaiseResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AgencyAndThresholdPerceptionV3ControlEventArgs>)this._eventTable["ResponseTriggered"];
                callback?.Invoke(this, new AgencyAndThresholdPerceptionV3ControlEventArgs(trigger_value: true));
            }
        } /* RaiseResponseTriggered() */

        #endregion

    } /* class ApollonAgencyAndThresholdPerceptionV3ControlDispatcher */

} /* } Labsim.apollon.gameplay.control */