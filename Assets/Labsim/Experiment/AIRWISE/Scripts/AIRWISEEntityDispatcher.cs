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
    public class AIRWISEEntityDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<AIRWISEEntityBridge>
    {
        #region event args class

        public class AIRWISEEntityEventArgs
            : apollon.gameplay.ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public AIRWISEEntityEventArgs()
                : base()
            {} 

            public AIRWISEEntityEventArgs(
                float duration = 0.0f
            )
                : base()
            {
                this.Duration = duration;
            }

            // ctor
            public AIRWISEEntityEventArgs(AIRWISEEntityEventArgs rhs)
                : base(rhs)
            {
                this.Duration = rhs.Duration;
            }

            // property
            public float Duration { get; protected set; }

        } /* AIRWISEEntityEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<AIRWISEEntityEventArgs>> 
            _eventInitCommandList           = new System.Collections.Generic.List<System.EventHandler<AIRWISEEntityEventArgs>>(),
            _eventIdleCommandList           = new System.Collections.Generic.List<System.EventHandler<AIRWISEEntityEventArgs>>(),
            _eventControlCommandList        = new System.Collections.Generic.List<System.EventHandler<AIRWISEEntityEventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<AIRWISEEntityEventArgs>>();

        #endregion

        // Constructor
        public AIRWISEEntityDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Control",    null);
            this._eventTable.Add("Reset",      null);

        } /* AIRWISEEntityDispatcher() */

        #region actual events

        public event System.EventHandler<AIRWISEEntityEventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Init"] + value;
                }
            }

            remove
            {
                if (!this._eventInitCommandList.Contains(value))
                {
                    return;
                }
                this._eventInitCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = null;
                    foreach (var eventInit in this._eventInitCommandList)
                    {
                        this._eventTable["Init"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<AIRWISEEntityEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Idle"] + value;
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
                        this._eventTable["Idle"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<AIRWISEEntityEventArgs> ControlEvent
        {
            add
            {
                this._eventControlCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Control"] + value;
                }
            }

            remove
            {
                if (!this._eventControlCommandList.Contains(value))
                {
                    return;
                }
                this._eventControlCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = null;
                    foreach (var eventControl in this._eventControlCommandList)
                    {
                        this._eventTable["Control"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Control"] + eventControl;
                    }
                }
            }

        } /* ControlEvent */

        public event System.EventHandler<AIRWISEEntityEventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Reset"] + value;
                }
            }

            remove
            {
                if (!this._eventResetCommandList.Contains(value))
                {
                    return;
                }
                this._eventResetCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = null;
                    foreach (var eventReset in this._eventResetCommandList)
                    {
                        this._eventTable["Reset"] = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Reset"] + eventReset;
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
                var callback = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new AIRWISEEntityEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new AIRWISEEntityEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseControl(
            float duration
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Control"];
                callback?.Invoke(
                    this, 
                    new AIRWISEEntityEventArgs(
                        duration : duration
                    )
                );
            }

        } /* RaiseControl() */

        public void RaiseReset(float duration = -1.0f)
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<AIRWISEEntityEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new AIRWISEEntityEventArgs(
                    duration : duration
                ));
            }

        } /* RaiseReset() */

        #endregion

    } /* class AIRWISEEntityDispatcher */

} /* } Labsim.experiment.AIRWISE */