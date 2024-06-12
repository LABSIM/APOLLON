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
namespace Labsim.apollon.gameplay.device
{
    public class ApollonGeneric3DoFHapticArmDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonGeneric3DoFHapticArmBridge>
    {
        
        #region event args class

        public class HapticArmEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        { } 

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<HapticArmEventArgs>> 
            _eventInitList           = new System.Collections.Generic.List<System.EventHandler<HapticArmEventArgs>>(),
            _eventIdleList           = new System.Collections.Generic.List<System.EventHandler<HapticArmEventArgs>>(),
            _eventControlList        = new System.Collections.Generic.List<System.EventHandler<HapticArmEventArgs>>(),
            _eventResetList          = new System.Collections.Generic.List<System.EventHandler<HapticArmEventArgs>>();

        #endregion

        // Constructor
        public ApollonGeneric3DoFHapticArmDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Control",    null);
            this._eventTable.Add("Reset",      null);

        } /* ApollonGeneric3DoFHapticArmDispatcher() */

        #region actual events

        public event System.EventHandler<HapticArmEventArgs> InitEvent
        {
            add
            {
                this._eventInitList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Init"] + value;
                }
            }

            remove
            {
                if (!this._eventInitList.Contains(value))
                {
                    return;
                }
                this._eventInitList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = null;
                    foreach (var eventInit in this._eventInitList)
                    {
                        this._eventTable["Init"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<HapticArmEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Idle"] + value;
                }
            }

            remove
            {
                if (!this._eventIdleList.Contains(value))
                {
                    return;
                }
                this._eventIdleList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = null;
                    foreach (var eventIdle in this._eventIdleList)
                    {
                        this._eventTable["Idle"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<HapticArmEventArgs> ControlEvent
        {
            add
            {
                this._eventControlList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Control"] + value;
                }
            }

            remove
            {
                if (!this._eventControlList.Contains(value))
                {
                    return;
                }
                this._eventControlList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = null;
                    foreach (var eventControl in this._eventControlList)
                    {
                        this._eventTable["Control"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Control"] + eventControl;
                    }
                }
            }

        } /* ControlEvent */

        public event System.EventHandler<HapticArmEventArgs> ResetEvent
        {
            add
            {
                this._eventResetList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Reset"] + value;
                }
            }

            remove
            {
                if (!this._eventResetList.Contains(value))
                {
                    return;
                }
                this._eventResetList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = null;
                    foreach (var eventReset in this._eventResetList)
                    {
                        this._eventTable["Reset"] = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Reset"] + eventReset;
                    }
                }
            }

        } /* ResetEvent */

        #endregion

        #region raise events

        public void RaiseInit()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new HapticArmEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new HapticArmEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseControl()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Control"];
                callback?.Invoke(this, new HapticArmEventArgs());
            }

        } /* RaiseControl() */

        public void RaiseReset()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<HapticArmEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new HapticArmEventArgs());
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonGeneric3DoFHapticArmDispatcher */

} /* } Labsim.apollon.gameplay.device.command */