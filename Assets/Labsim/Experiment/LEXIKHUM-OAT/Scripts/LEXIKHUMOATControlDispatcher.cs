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
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATControlDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<LEXIKHUMOATControlBridge>
    {
        #region event args class

        public class LEXIKHUMOATControlEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public LEXIKHUMOATControlEventArgs(
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
            public LEXIKHUMOATControlEventArgs(LEXIKHUMOATControlEventArgs rhs)
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

        } /* LEXIKHUMOATControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<LEXIKHUMOATControlEventArgs>> 
            _eventJoystickHorizontalValueChangedList = new(),
            _eventJoystickVerticalValueChangedList   = new(),
            _eventResponseTriggeredList              = new();

        #endregion

        // Constructor
        public LEXIKHUMOATControlDispatcher()
        {

            // event table
            this._eventTable.Add("JoystickHorizontalValueChanged",   null);
            this._eventTable.Add("JoystickVerticalValueChanged",     null);
            this._eventTable.Add("ResponseTriggered",                null);

        } /* LEXIKHUMOATControlDispatcher() */

        #region actual events

        public event System.EventHandler<LEXIKHUMOATControlEventArgs> JoystickHorizontalValueChangedEvent
        {
            add
            {
                this._eventJoystickHorizontalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + value;
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
                        this._eventTable["JoystickHorizontalValueChanged"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"] + eventJoystickHorizontalValueChanged;
                    }
                }
            }

        } /* JoystickHorizontalValueChangedEvent */

        public event System.EventHandler<LEXIKHUMOATControlEventArgs> JoystickVerticalValueChangedEvent
        {
            add
            {
                this._eventJoystickVerticalValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + value;
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
                        this._eventTable["JoystickVerticalValueChanged"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"] + eventJoystickVerticalValueChanged;
                    }
                }
            }

        } /* JoystickVerticalValueChangedEvent */

        public event System.EventHandler<LEXIKHUMOATControlEventArgs> ResponseTriggeredEvent
        {
            add
            {
                this._eventResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ResponseTriggered"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["ResponseTriggered"] + value;
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
                        this._eventTable["ResponseTriggered"] = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["ResponseTriggered"] + eventResponseTriggered;
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
                var callback = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickHorizontalValueChanged"];
                callback?.Invoke(this, new(joystick_horizontal_axis_value: value));
            }
        } /* RaiseJoystickHorizontalValueChanged() */

        public void RaiseJoystickVerticalValueChanged(float value)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["JoystickVerticalValueChanged"];
                callback?.Invoke(this, new(joystick_vertical_axis_value: value));
            }
        } /* RaiseJoystickVerticalValueChanged() */

        public void RaiseResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATControlEventArgs>)this._eventTable["ResponseTriggered"];
                callback?.Invoke(this, new(trigger_value: true));
            }
        } /* RaiseResponseTriggered() */

        #endregion

    } /* class LEXIKHUMOATControlDispatcher */

} /* } Labsim.experiment.LEXIKHUM_OAT */
