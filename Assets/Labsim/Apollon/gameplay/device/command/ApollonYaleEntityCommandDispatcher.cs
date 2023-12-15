// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.command
{
    public class ApollonYaleEntityCommandDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonYaleEntityCommandBridge>
    {
        #region event args class

        public class YaleEntityCommandEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public YaleEntityCommandEventArgs()
                : base()
            {} 

            public YaleEntityCommandEventArgs(
                float duration = 0.0f
            )
                : base()
            {
                this.Duration = duration;
            }

            // ctor
            public YaleEntityCommandEventArgs(YaleEntityCommandEventArgs rhs)
                : base(rhs)
            {
                this.Duration = rhs.Duration;
            }

            // property
            public float Duration { get; protected set; }

        } /* YaleEntityCommandEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<YaleEntityCommandEventArgs>> 
            _eventInitCommandList           = new System.Collections.Generic.List<System.EventHandler<YaleEntityCommandEventArgs>>(),
            _eventIdleCommandList           = new System.Collections.Generic.List<System.EventHandler<YaleEntityCommandEventArgs>>(),
            _eventControlCommandList        = new System.Collections.Generic.List<System.EventHandler<YaleEntityCommandEventArgs>>(),
            _eventResetCommandList          = new System.Collections.Generic.List<System.EventHandler<YaleEntityCommandEventArgs>>();

        #endregion

        // Constructor
        public ApollonYaleEntityCommandDispatcher()
        {

            // event table
            this._eventTable.Add("Init",       null);
            this._eventTable.Add("Idle",       null);
            this._eventTable.Add("Control",    null);
            this._eventTable.Add("Reset",      null);

        } /* ApollonYaleEntityCommandDispatcher() */

        #region actual events

        public event System.EventHandler<YaleEntityCommandEventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Init"] + value;
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
                        this._eventTable["Init"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<YaleEntityCommandEventArgs> IdleEvent
        {
            add
            {
                this._eventIdleCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Idle"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Idle"] + value;
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
                        this._eventTable["Idle"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Idle"] + eventIdle;
                    }
                }
            }

        } /* IdleEvent */

        public event System.EventHandler<YaleEntityCommandEventArgs> ControlEvent
        {
            add
            {
                this._eventControlCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Control"] + value;
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
                        this._eventTable["Control"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Control"] + eventControl;
                    }
                }
            }

        } /* ControlEvent */

        public event System.EventHandler<YaleEntityCommandEventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Reset"] + value;
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
                        this._eventTable["Reset"] = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Reset"] + eventReset;
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
                var callback = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Init"];
                callback?.Invoke(this, new YaleEntityCommandEventArgs());
            }

        } /* RaiseInit() */

        public void RaiseIdle()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Idle"];
                callback?.Invoke(this, new YaleEntityCommandEventArgs());
            }

        } /* RaiseIdle() */

        public void RaiseControl(
            float duration
        ) {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Control"];
                callback?.Invoke(
                    this, 
                    new YaleEntityCommandEventArgs(
                        duration : duration
                    )
                );
            }

        } /* RaiseControl() */

        public void RaiseReset(float duration = -1.0f)
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<YaleEntityCommandEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(this, new YaleEntityCommandEventArgs(
                    duration : duration
                ));
            }

        } /* RaiseReset() */

        #endregion

    } /* class ApollonYaleEntityCommandDispatcher */

} /* } Labsim.apollon.gameplay.device.command */