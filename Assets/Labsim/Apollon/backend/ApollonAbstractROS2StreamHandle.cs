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

// using System.Windows.Forms;
// using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UIElements.Experimental;

namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractROS2StreamHandle<DownstreamMsgT, UpstreamMsgT> 
        : ApollonAbstractStandardHandle
        where DownstreamMsgT : Unity.Robotics.ROSTCPConnector.MessageGeneration.Message
        where UpstreamMsgT   : Unity.Robotics.ROSTCPConnector.MessageGeneration.Message
    {

        #region ROS2 settings decl.

        public interface IROS2Settings
        {

            public string DownstreamTopicName { get; }
            public string UpstreamTopicName { get; }
            public float DownstreamMessagePeriod { get; }

        } /* interface IROS2Settings */ 

        public sealed class DefaultROS2Settings
            : IROS2Settings
        {

            private string m_ROS2UpstreamTopicName = "/DefaultUpstream";
            public string UpstreamTopicName 
            { 
                get => this.m_ROS2UpstreamTopicName; 
                private set => this.m_ROS2UpstreamTopicName = value; 
            }

            private string m_ROS2DownstreamTopicName = "/DefaultDownstream";
            public string DownstreamTopicName 
            { 
                get => this.m_ROS2DownstreamTopicName; 
                private set => this.m_ROS2DownstreamTopicName = value; 
            }

            private float m_ROS2DownstreamMessagePeriod = 0.5f;
            public float DownstreamMessagePeriod 
            { 
                get => this.m_ROS2DownstreamMessagePeriod; 
                private set => this.m_ROS2DownstreamMessagePeriod = value; 
            }

        } /* class DefaultROS2Settings */

        protected virtual IROS2Settings WrapROS2Settings()
        {
            return new DefaultROS2Settings();
        }

        private IROS2Settings m_ROS2Settings = null;
        public IROS2Settings ROS2Settings 
        {
            get 
            {
                if(this.m_ROS2Settings == null)
                {
                    this.m_ROS2Settings = this.WrapROS2Settings();
                }
                return this.m_ROS2Settings;
            }
        }

        private Unity.Robotics.ROSTCPConnector.ROSConnection m_ROS2Connection = null;
        private float m_ROS2TimeElapsed = 0.0f;

        #endregion

        #region ROS2 callback decl. 

        private System.Action<UpstreamMsgT> UpstreamCallback { get; set; } = null;
        private System.Func<DownstreamMsgT> DownstreamCallback { get; set; } = null;

        protected abstract void Upstream(UpstreamMsgT upstream);
        protected abstract DownstreamMsgT Downstream();

        #endregion

        #region ROS2 <=> APOLLON/Unity integration

        private void OnEngineFixedUpdate(object sender, ApollonEngine.EngineEventArgs args)
        {

            // Tick !
            if(this.IsInitialized)
            {

                // ROS publishing
                this.m_ROS2TimeElapsed += UnityEngine.Time.fixedDeltaTime;

                if (this.m_ROS2TimeElapsed > this.ROS2Settings.DownstreamMessagePeriod)
                {

                    // acquire payload 
                    var payload = this.DownstreamCallback();

                    // Finally send the message to server_endpoint.py running in ROS2
                    this.m_ROS2Connection.Publish(this.ROS2Settings.DownstreamTopicName, payload);
                    this.m_ROS2TimeElapsed = 0.0f;

                } /* if() */

            } /* if() */

        } /* OnEngineFixedUpdate() */

        #endregion

        #region Concrete HandleInit/HandleClose pattern def.

        protected abstract StatusIDType ConcreteHandleInitialize();
        protected abstract StatusIDType ConcreteHandleClose();

        #endregion

        #region Standard HandleInit/HandleClose pattern impl.

        protected sealed override StatusIDType HandleInitialize()
        {            
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : call"
            );

            // encapsulate
            try
            {

                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : initializing ROS2 connection");

                // start the ROS2 connection
                this.m_ROS2Connection = Unity.Robotics.ROSTCPConnector.ROSConnection.GetOrCreateInstance();

                // bind callback to implementation
                this.UpstreamCallback   = new(this.Upstream);
                this.DownstreamCallback = new(this.Downstream);

                // ROS2 init
                this.m_ROS2Connection.RegisterPublisher<DownstreamMsgT>(
                    this.ROS2Settings.DownstreamTopicName
                );
                this.m_ROS2Connection.Subscribe<UpstreamMsgT>(
                    this.ROS2Settings.UpstreamTopicName, 
                    this.UpstreamCallback
                );

                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : ROS2 connection started, binding with Unity & APOLLON");
                
                // Apollon Engine
                ApollonEngine.Instance.EngineFixedUpdateEvent += this.OnEngineFixedUpdate;

                // return concrete result
                return this.ConcreteHandleInitialize();

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : caugh Exception => "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : Initialization failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleInitialize() */

        protected sealed override StatusIDType HandleClose()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleClose() : call"
            );

            // encapsulate
            try
            {

                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : closing ROS2 connection");

                // ROS2 Unsubscribing from all
                this.m_ROS2Connection.Unsubscribe(
                    this.ROS2Settings.UpstreamTopicName
                );

                // reseting callbacks
                this.DownstreamCallback = null;
                this.UpstreamCallback   = null;
                
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAbstractROS2StreamHandle.HandleInitialize() : ROS2 connection closed");
                
                // return concrete result
                return this.ConcreteHandleClose();

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractROS2StreamHandle.HandleClose() : caugh Exception => "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractROS2StreamHandle.HandleClose() : Closure failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleClose() */

        #endregion

    } /* class ApollonAbstractROS2StreamHandle */
    
} /* } namespace */
