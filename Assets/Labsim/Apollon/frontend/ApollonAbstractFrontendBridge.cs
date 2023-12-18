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
namespace Labsim.apollon.frontend
{

    public abstract class ApollonAbstractFrontendBridge
    {

        // ctor

        public ApollonAbstractFrontendBridge()
        {
            this.WrapBehaviour();
        }

        // public properties

        private UnityEngine.MonoBehaviour m_behaviour = null;
        public UnityEngine.MonoBehaviour Behaviour
        {
            get
            {
                if(this.m_behaviour == null) 
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonAbstractFrontendBridge.Behaviour : property is still null, trying to re wrap behaviour from Unity."
                    );

                    this.m_behaviour = this.WrapBehaviour();
                }
                return this.m_behaviour;
            }
            private set
            {
                this.m_behaviour = value;
            }
        }

        private ApollonFrontendManager.FrontendIDType m_ID = ApollonFrontendManager.FrontendIDType.None;
        public ApollonFrontendManager.FrontendIDType ID
        {
            get
            {
                if (this.m_ID == ApollonFrontendManager.FrontendIDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractFrontendBridge.ID : property is uninitilized, trying to wrap ID."
                    );

                    this.m_ID = this.WrapID();
                }
                return this.m_ID;
            }
            private set
            {
                this.m_ID = value;
            }
        }

        // force overriding in childs

        protected abstract UnityEngine.MonoBehaviour WrapBehaviour();

        protected abstract ApollonFrontendManager.FrontendIDType WrapID();

        // Callback methods

        public virtual void onActivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onInactivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            /* nothing by default */
        }

    }  /* abstract ApollonAbstractFrontendBridge */

} /* } Labsim.apollon.frontend */