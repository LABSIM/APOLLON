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
namespace Labsim.experiment.CAVIAR
{

    public class CAVIARControlDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<CAVIARControlBridge>
    {
        #region event args class

        public class CAVIARControlEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public CAVIARControlEventArgs(float z = 0.0f, bool button15 = false)
                : base()
            {
                this.Z = z;
                this.Button15 = Button15;
            }

            // ctor
            public CAVIARControlEventArgs(CAVIARControlEventArgs rhs)
                : base(rhs)
            {
                this.Z = rhs.Z;
                this.Button15 = rhs.Button15;
            }

            // property
            public float Z { get; protected set; }
            public bool Button15 { get; protected set; }

        } /* CAVIARControlEventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>> _eventAxisZValueChangedList
            = new System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>> _eventUserNeutralCommandTriggeredList
            = new System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>> _eventUserResponseTriggeredList
            = new System.Collections.Generic.List<System.EventHandler<CAVIARControlEventArgs>>();

        #endregion

        // Constructor
        public CAVIARControlDispatcher()
        {

            // event table
            this._eventTable.Add("AxisZValueChanged",           null);
            this._eventTable.Add("UserNeutralCommandTriggered", null);
            this._eventTable.Add("UserResponseTriggered",       null);
            
        } /* CAVIARControlDispatcher() */

        #region actual events

        public event System.EventHandler<CAVIARControlEventArgs> AxisZValueChangedEvent
        {
            add
            {
                this._eventAxisZValueChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["AxisZValueChanged"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["AxisZValueChanged"] + value;
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
                        this._eventTable["AxisZValueChanged"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["AxisZValueChanged"] + eventAxisZValueChanged;
                    }
                }
            }

        } /* AxisZValueChangedEvent */

        public event System.EventHandler<CAVIARControlEventArgs> UserNeutralCommandTriggeredEvent
        {
            add
            {
                this._eventUserNeutralCommandTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserNeutralCommandTriggered"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"] + value;
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
                        this._eventTable["UserNeutralCommandTriggered"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"] + eventUserNeutralCommandTriggered;
                    }
                }
            }

        } /* UserNeutralCommandTriggeredEvent */

        public event System.EventHandler<CAVIARControlEventArgs> UserResponseTriggeredEvent
        {
            add
            {
                this._eventUserResponseTriggeredList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["UserResponseTriggered"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserResponseTriggered"] + value;
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
                        this._eventTable["UserResponseTriggered"] = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserResponseTriggered"] + eventUserResponseTriggered;
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
                var callback = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["AxisZValueChanged"];
                callback?.Invoke(this, new CAVIARControlEventArgs(z: value));
            }
        } /* RaiseAxisZValueChanged() */
        
        public void RaiseUserNeutralCommandTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserNeutralCommandTriggered"];
                callback?.Invoke(this, new CAVIARControlEventArgs());
            }
        } /* RaiseUserNeutralCommandTriggered() */

        public void RaiseUserResponseTriggered()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<CAVIARControlEventArgs>)this._eventTable["UserResponseTriggered"];
                callback?.Invoke(this, new CAVIARControlEventArgs(button15: true));
            }
        } /* RaiseUserResponseTriggered() */

        #endregion

    } /* class CAVIARControlDispatcher */

} /* } Labsim.apollon.gameplay.control */
