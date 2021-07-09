// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonFogElementDispatcher
    {
        #region Event args class

        public class EventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(
                float smoothing_duration = -1.0f, 
                UnityEngine.FogMode fog_mode = UnityEngine.FogMode.Linear, 
                float fog_start_distance = -1.0f,
                float fog_end_distance = -1.0f,
                UnityEngine.Color fog_color = new UnityEngine.Color()
            ) : base()
            {
                // assign
                this.SmoothingDuration = smoothing_duration;
                this.FogMode = fog_mode;
                this.FogStartDistance = fog_start_distance;
                this.FogEndDistance = fog_end_distance;
                this.FogColor = fog_color;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                // assign
                this.SmoothingDuration = rhs.SmoothingDuration;
                this.FogMode = rhs.FogMode;
                this.FogStartDistance = rhs.FogStartDistance ;
                this.FogEndDistance = rhs.FogEndDistance;
                this.FogColor = rhs.FogColor;
            }

            // property
            public float SmoothingDuration { get; protected set; }
            public UnityEngine.FogMode FogMode { get; protected set; }
            public float FogStartDistance { get; protected set; }
            public float FogEndDistance { get; protected set; }
            public UnityEngine.Color FogColor { get; protected set; }

        } /* EventArgs() */

        #endregion

        #region Dictionary & each list of event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable = null;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventParameterChangedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        #endregion

        // Constructor
        public ApollonFogElementDispatcher()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "ParameterChanged", null }
            };

        } /* ApollonFogElementDispatcher() */

        #region actual events

        public event System.EventHandler<EventArgs> ParameterChangedEvent
        {
            add
            {
                this._eventParameterChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ParameterChanged"] = (System.EventHandler<EventArgs>)this._eventTable["ParameterChanged"] + value;
                }
            }

            remove
            {
                if (!this._eventParameterChangedList.Contains(value))
                {
                    return;
                }
                this._eventParameterChangedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ParameterChanged"] = null;
                    foreach (var eventParameterChanged in this._eventParameterChangedList)
                    {
                        this._eventTable["ParameterChanged"] = (System.EventHandler<EventArgs>)this._eventTable["ParameterChanged"] + eventParameterChanged;
                    }
                }
            }

        } /* ParameterChangedEvent */

        #endregion

        #region raise events

        public void RaiseImmediateLinearFogRequested(
            float start_distance,
            float end_distance,
            UnityEngine.Color color
        ) {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["ParameterChanged"];
                callback?.Invoke(
                    this, 
                    new EventArgs(
                        fog_mode: UnityEngine.FogMode.Linear,
                        fog_start_distance: start_distance,
                        fog_end_distance: end_distance,
                        fog_color: color
                    )
                );
            }
        } /* RaiseImmediateLinearFogRequested() */

        public void RaiseSmoothLinearFogRequested(
            float start_distance,
            float end_distance,
            UnityEngine.Color color,
            float duration_ms
        ) {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["ParameterChanged"];
                callback?.Invoke(
                    this, 
                    new EventArgs(
                        fog_mode: UnityEngine.FogMode.Linear,
                        fog_start_distance: start_distance,
                        fog_end_distance: end_distance,
                        fog_color: color,
                        smoothing_duration: duration_ms
                    )
                );
            }
        } /* RaiseSmoothLinearFogRequested() */
        
        #endregion

    } /* class ApollonFogElementDispatcher */

} /* } Labsim.apollon.gameplay.device.sensor */
