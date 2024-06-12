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
    public class LEXIKHUMOATEntityDispatcher
        : apollon.gameplay.ApollonConcreteGameplayDispatcher<LEXIKHUMOATEntityBridge>
    {
        
        #region event args class

        public class LEXIKHUMOATEntityEventArgs
            : apollon.gameplay.device.ApollonGeneric6DoFMotionSystemDispatcher.MotionSystemEventArgs
        {

            // ctor
            public LEXIKHUMOATEntityEventArgs()
                : base()
            {
                // nothing
            }

             public LEXIKHUMOATEntityEventArgs(
                float[] angular_acceleration_target = null,
                float[] angular_velocity_saturation_threshold = null,
                float[] linear_acceleration_target = null,
                float[] linear_velocity_saturation_threshold = null,
                float duration = 0.0f,
                bool inhibit_vestibular_motion = true
            )
                : base(
                    angular_acceleration_target,
                    angular_velocity_saturation_threshold,
                    null,
                    linear_acceleration_target,
                    linear_velocity_saturation_threshold,
                    null,
                    duration,
                    inhibit_vestibular_motion
                )
            {
                // nothing
            }

        } /* LEXIKHUMOATEntityEventArgs() */

        #endregion

        #region list of event

        private readonly System.Collections.Generic.List<System.EventHandler<LEXIKHUMOATEntityEventArgs>> 
            _eventInitCommandList    = new(),
            _eventHoldCommandList    = new(),
            _eventControlCommandList = new(),
            _eventResetCommandList   = new();

        #endregion

        // Constructor
        public LEXIKHUMOATEntityDispatcher()
        {

            // event table
            this._eventTable.Add("Init",    null);
            this._eventTable.Add("Hold",    null);
            this._eventTable.Add("Control", null);
            this._eventTable.Add("Reset",   null);

        } /* LEXIKHUMOATEntityDispatcher() */

        #region actual events

        public event System.EventHandler<LEXIKHUMOATEntityEventArgs> InitEvent
        {
            add
            {
                this._eventInitCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Init"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Init"] + value;
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
                        this._eventTable["Init"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Init"] + eventInit;
                    }
                }
            }

        } /* InitEvent */

        public event System.EventHandler<LEXIKHUMOATEntityEventArgs> HoldEvent
        {
            add
            {
                this._eventHoldCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Hold"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Hold"] + value;
                }
            }

            remove
            {
                if (!this._eventHoldCommandList.Contains(value))
                {
                    return;
                }
                this._eventHoldCommandList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["Hold"] = null;
                    foreach (var eventHold in this._eventHoldCommandList)
                    {
                        this._eventTable["Hold"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Hold"] + eventHold;
                    }
                }
            }

        } /* HoldEvent */

        public event System.EventHandler<LEXIKHUMOATEntityEventArgs> ControlEvent
        {
            add
            {
                this._eventControlCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Control"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Control"] + value;
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
                        this._eventTable["Control"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Control"] + eventControl;
                    }
                }
            }

        } /* ControlEvent */

        public event System.EventHandler<LEXIKHUMOATEntityEventArgs> ResetEvent
        {
            add
            {
                this._eventResetCommandList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["Reset"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Reset"] + value;
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
                        this._eventTable["Reset"] = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Reset"] + eventReset;
                    }
                }
            }

        } /* ResetEvent */

        #endregion

        #region raise events

        public void RaiseInit(
            float[] angular_acceleration_target,
            float[] angular_velocity_saturation_threshold,
            float[] linear_acceleration_target,
            float[] linear_velocity_saturation_threshold,
            float duration,
            bool without_motion
        )
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Init"];
                callback?.Invoke(
                    this, 
                    new LEXIKHUMOATEntityEventArgs(
                        angular_acceleration_target : angular_acceleration_target,
                        angular_velocity_saturation_threshold : angular_velocity_saturation_threshold,
                        linear_acceleration_target : linear_acceleration_target,
                        linear_velocity_saturation_threshold : linear_velocity_saturation_threshold, 
                        duration : duration,
                        inhibit_vestibular_motion : without_motion
                    )
                );
            }

        } /* RaiseInit() */

        public void RaiseHold()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Hold"];
                callback?.Invoke(this, new());
            }

        } /* RaiseHold() */

        public void RaiseControl()
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Control"];
                callback?.Invoke(this, new());
            }

        } /* RaiseControl() */

        public void RaiseReset(
            float[] angular_acceleration_target,
            float[] angular_velocity_saturation_threshold,
            float[] linear_acceleration_target,
            float[] linear_velocity_saturation_threshold,
            float duration,
            bool without_motion
        )
        {

            lock (this._eventTable)
            {
                var callback = (System.EventHandler<LEXIKHUMOATEntityEventArgs>)this._eventTable["Reset"];
                callback?.Invoke(
                    this,
                    new LEXIKHUMOATEntityEventArgs(
                        angular_acceleration_target : angular_acceleration_target,
                        angular_velocity_saturation_threshold : angular_velocity_saturation_threshold,
                        linear_acceleration_target : linear_acceleration_target,
                        linear_velocity_saturation_threshold : linear_velocity_saturation_threshold,
                        duration : duration,
                        inhibit_vestibular_motion : without_motion
                    )
                );
            }

        } /* RaiseReset() */

        #endregion

    } /* class LEXIKHUMOATEntityDispatcher */

} /* } Labsim.experiment.LEXIKHUM_OAT */