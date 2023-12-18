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
namespace Labsim.apollon.gameplay.element
{

    public class ApollonFogElementDispatcher
        : ApollonConcreteGameplayDispatcher<ApollonFogElementBridge>
    {

        #region Event args class

        public class FogElementEventArgs
            : ApollonGameplayDispatcher.GameplayEventArgs
        {

            // ctor
            public FogElementEventArgs(
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
            public FogElementEventArgs(FogElementEventArgs rhs)
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

        } /* FogElementEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<FogElementEventArgs>> _eventParameterChangedList
            = new System.Collections.Generic.List<System.EventHandler<FogElementEventArgs>>();

        #endregion

        // Constructor
        public ApollonFogElementDispatcher()
        {

            // event table
            this._eventTable.Add("ParameterChanged", null);

        } /* ApollonFogElementDispatcher() */

        #region actual events

        public event System.EventHandler<FogElementEventArgs> ParameterChangedEvent
        {
            add
            {
                this._eventParameterChangedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ParameterChanged"] = (System.EventHandler<FogElementEventArgs>)this._eventTable["ParameterChanged"] + value;
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
                        this._eventTable["ParameterChanged"] = (System.EventHandler<FogElementEventArgs>)this._eventTable["ParameterChanged"] + eventParameterChanged;
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
                var callback = (System.EventHandler<FogElementEventArgs>)this._eventTable["ParameterChanged"];
                callback?.Invoke(
                    this, 
                    new FogElementEventArgs(
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
                var callback = (System.EventHandler<FogElementEventArgs>)this._eventTable["ParameterChanged"];
                callback?.Invoke(
                    this, 
                    new FogElementEventArgs(
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
