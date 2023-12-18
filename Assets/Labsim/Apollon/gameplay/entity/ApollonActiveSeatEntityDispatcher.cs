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
namespace Labsim.apollon.gameplay.entity
{
    public class ApollonActiveSeatEntityDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonActiveSeatEntityBridge>
    {

        #region each list of event

        private readonly System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>> 
            _eventIdleCommandList = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>(),
            _eventVisualOnlyCommandList = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>(),
            _eventVestibularOnlyCommandList = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>(),
            _eventVisuoVestibularCommandList = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>();

        #endregion

        // Constructor
        public ApollonActiveSeatEntityDispatcher()
        {

            // event table filling
            this._eventTable.Add("Idle",            null);
            this._eventTable.Add("VisualOnly",      null);
            this._eventTable.Add("VestibularOnly",  null);
            this._eventTable.Add("VisuoVestibular", null);

        } /* ApollonActiveSeatEntityDispatcher() */

        #region actual events

        public event System.EventHandler<GameplayEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["Idle"] + value;
                }
            }

            remove
            {
                if (!this._eventIdleCommandList.Contains(value))
                {
                    return;
                }
                this._eventIdleCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = null;
                    foreach (var eventIdle in this._eventIdleCommandList)
                    {
                        this._eventTable["Idle"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<GameplayEventArgs> VisualOnlyEvent
        {
            add
            {
                this._eventVisualOnlyCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisualOnly"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisualOnly"] + value;
                }
            }

            remove
            {
                if (!this._eventVisualOnlyCommandList.Contains(value))
                {
                    return;
                }
                this._eventVisualOnlyCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisualOnly"] = null;
                    foreach (var eventVisualOnly in this._eventVisualOnlyCommandList)
                    {
                        this._eventTable["VisualOnly"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisualOnly"] + eventVisualOnly;
                    }
                }
            }

        } /* VisualOnlyEvent */

        public event System.EventHandler<GameplayEventArgs> VestibularOnlyEvent
        {
            add
            {
                this._eventVestibularOnlyCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VestibularOnly"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VestibularOnly"] + value;
                }
            }

            remove
            {
                if (!this._eventVestibularOnlyCommandList.Contains(value))
                {
                    return;
                }
                this._eventVestibularOnlyCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VestibularOnly"] = null;
                    foreach (var eventVestibularOnly in this._eventVestibularOnlyCommandList)
                    {
                        this._eventTable["VestibularOnly"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VestibularOnly"] + eventVestibularOnly;
                    }
                }
            }

        } /* VestibularOnlyEvent */

        public event System.EventHandler<GameplayEventArgs> VisuoVestibularEvent
        {
            add
            {
                this._eventVisuoVestibularCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisuoVestibular"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisuoVestibular"] + value;
                }
            }

            remove
            {
                if (!this._eventVisuoVestibularCommandList.Contains(value))
                {
                    return;
                }
                this._eventVisuoVestibularCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["VisuoVestibular"] = null;
                    foreach (var eventVisuoVestibular in this._eventVisuoVestibularCommandList)
                    {
                        this._eventTable["VisuoVestibular"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisuoVestibular"] + eventVisuoVestibular;
                    }
                }
            }

        } /* VisuoVestibularEvent */

        #endregion

        #region raise events

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new GameplayEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseVisualOnly()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisualOnly"];
                callback?.Invoke(this, new GameplayEventArgs());
            }

        } /* RaiseVisualOnly() */

        public void RaiseVestibularOnly()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["VestibularOnly"];
                callback?.Invoke(this, new GameplayEventArgs());
            }

        } /* RaiseVestibularOnly() */

        public void RaiseVisuoVestibular()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["VisuoVestibular"];
                callback?.Invoke(this, new GameplayEventArgs());
            }

        } /* RaiseVisuoVestibular() */

        #endregion

    } /* class ApollonActiveSeatEntityDispatcher */

} /* } Labsim.apollon.gameplay.entity */