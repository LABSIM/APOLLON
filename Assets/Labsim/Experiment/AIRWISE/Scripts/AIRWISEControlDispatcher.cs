//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEControlDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<AIRWISEControlBridge>
    {
        #region event args class

        public class AIRWISEControlEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public AIRWISEControlEventArgs(
                float joystick_horizontal_axis_value = 0.0f, 
                float joystick_vertical_axis_value = 0.0f, 
                bool trigger_value = false
            )
                : base()
            {
                this.JoystickHorizontal = joystick_horizontal_axis_value;
                this.JoystickVertical   = joystick_vertical_axis_value;
                this.Response           = trigger_value;
            }

            // ctor
            public AIRWISEControlEventArgs(AIRWISEControlEventArgs rhs)
                : base(rhs)
            {
                this.JoystickHorizontal = rhs.JoystickHorizontal;
                this.JoystickVertical   = rhs.JoystickVertical;
                this.Response           = rhs.Response;
            }

            // property
            public float JoystickHorizontal { get; protected set; }
            public float JoystickVertical { get; protected set; }
            public bool Response { get; protected set; }

        } /* AIRWISEControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<AIRWISEControlEventArgs>> 
            _eventJoystickHorizontalValueChangedList   = new System.Collections.Generic.List<System.EventHandler<AIRWISEControlEventArgs>>(),
            _eventJoystickVerticalValueChangedList     = new System.Collections.Generic.List<System.EventHandler<AIRWISEControlEventArgs>>(),
            _eventResponseTriggeredList                = new System.Collections.Generic.List<System.EventHandler<AIRWISEControlEventArgs>>();

        #endregion

        // Constructor
        public AIRWISEControlDispatcher()
        {

            // event table
            this._eventTable.Add("JoystickHorizontalValueChanged",   null);
            this._eventTable.Add("JoystickVerticalValueChanged",     null);
            this._eventTable.Add("ResponseTriggered",                null);

        } /* AIRWISEControlDispatcher() */

        #region actual events

        public event System.EventHandler<AIRWISEControlEventArgs> JoystickHorizontalValueChangedEvent
        {
            add
            {
                this._eventJoystickHorizontalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + value;
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
                        this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + eventJoystickHorizontalValueChanged;
                    }
                }
            }

        } /* JoystickHorizontalValueChangedEvent */

        public event System.EventHandler<AIRWISEControlEventArgs> JoystickVerticalValueChangedEvent
        {
            add
            {
                this._eventJoystickVerticalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + value;
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
                        this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + eventJoystickVerticalValueChanged;
                    }
                }
            }

        } /* JoystickVerticalValueChangedEvent */

        public event System.EventHandler<AIRWISEControlEventArgs> ResponseTriggeredEvent
        {
            add
            {
                this._eventResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ResponseTriggered"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["ResponseTriggered"] + value;
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
                        this._eventTable["ResponseTriggered"] = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["ResponseTriggered"] + eventResponseTriggered;
                    }
                }
            }

        } /* ResponseTriggeredEvent */

        #endregion

        #region raise events

        public void RaiseJoystickHorizontalValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"];
                callback?.Invoke(this, new AIRWISEControlEventArgs(joystick_horizontal_axis_value: value));
            }
        } /* RaiseJoystickHorizontalValueChanged() */

        public void RaiseJoystickVerticalValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"];
                callback?.Invoke(this, new AIRWISEControlEventArgs(joystick_vertical_axis_value: value));
            }
        } /* RaiseJoystickVerticalValueChanged() */

        public void RaiseResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEControlEventArgs>)this._eventTable["ResponseTriggered"];
                callback?.Invoke(this, new AIRWISEControlEventArgs(trigger_value: true));
            }
        } /* RaiseResponseTriggered() */

        #endregion

    } /* class AIRWISEControlDispatcher */

} /* } Labsim.experiment.AIRWISE */
