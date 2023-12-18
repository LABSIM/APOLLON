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
namespace Labsim.apollon.gameplay
{

    public abstract class ApollonAbstractGameplayBridge
    {

        // public properties
        
        private ApollonGameplayManager.GameplayIDType m_ID = ApollonGameplayManager.GameplayIDType.None;
        public ApollonGameplayManager.GameplayIDType ID
        {
            get
            {
                if (this.m_ID == ApollonGameplayManager.GameplayIDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractGameplayBridge.ID : property is uninitilized, trying to wrap ID."
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

        private ApollonGameplayBehaviour m_behaviour = null;
        public ApollonGameplayBehaviour Behaviour
        {
            get
            {
                if (this.m_behaviour == null)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonAbstractGameplayBridge.Behaviour() : property is still null, trying to re wrap behaviour from Unity."
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

        private ApollonGameplayDispatcher m_dispatcher = null;
        public ApollonGameplayDispatcher Dispatcher
        {
            get
            {
                if (this.m_dispatcher == null)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonAbstractGameplayBridge.Dispatcher() : property is still null, trying to re wrap dispatchers."
                    );

                    this.m_dispatcher = this.WrapDispatcher();
                }
                return this.m_dispatcher;
            }
            private set
            {
                this.m_dispatcher = value;
            }
        }

        // force overriding in childs

        protected abstract ApollonGameplayBehaviour WrapBehaviour();

        protected ApollonGameplayBehaviour WrapBehaviour<T>(string bridge_name, string behaviour_name)
            where T : ApollonGameplayBehaviour
        {
            
            // retreive active only in scene
            var behaviours = new System.Collections.Generic.List<T>();
            foreach (var behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<T>())
            {

                if (
#if UNITY_EDITOR
                    !UnityEditor.EditorUtility.IsPersistent(behaviour.transform.root.gameObject) 
                    && !(
                        behaviour.hideFlags == UnityEngine.HideFlags.NotEditable 
                        || behaviour.hideFlags == UnityEngine.HideFlags.HideAndDontSave
                    )
#else
                    !(behaviour.gameObject.scene.name == null || behaviour.gameObject.scene.name == behaviour.gameObject.name)
#endif
                )
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> " + bridge_name + ".WrapBehaviour<" + behaviour_name + ">() : found active [" + behaviour.name + "] in scene."
                    );
                    
                    // match
                    behaviours.Add(behaviour);

                } 
                else
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> " + bridge_name + ".WrapBehaviour<" + behaviour_name + ">() : found non active [" + behaviour.name + "], skip."
                    );

                } /* if() */
            
            } /* foreach() */
        
            if (behaviours.Count == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> " + bridge_name + ".WrapBehaviour<" + behaviour_name + ">() : could not find object of type " + behaviour_name + " from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour<T>() */

        protected abstract ApollonGameplayDispatcher WrapDispatcher();
        
        protected ApollonGameplayDispatcher WrapDispatcher<T>(string bridge_name, string dispatcher_name)
            where T : ApollonGameplayDispatcher, new()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> " + bridge_name + ".WrapDispatcher<" + dispatcher_name + ">() : instantiate a custom dispatcher."
            );

            // instantiate
            var dispatcher = new T();
        
            // tail 
            dispatcher.Bridge = this;

            // return 
            return dispatcher;

        } /* WrapDispatcher() */

        protected abstract ApollonGameplayManager.GameplayIDType WrapID();

        protected abstract void SetActive(bool value);

        // Callback methods

        public virtual void onActivationRequested(object sender, ApollonGameplayManager.GameplayEventArgs arg)
        {
           
            // check
            if (this.ID == arg.ID || ApollonGameplayManager.GameplayIDType.All == arg.ID)
            {
                this.SetActive(true);
            }

        } /* onActivationRequested() */

        public virtual void onInactivationRequested(object sender, ApollonGameplayManager.GameplayEventArgs arg)
        {

            // check
            if (this.ID == arg.ID || ApollonGameplayManager.GameplayIDType.All == arg.ID)
            {
                this.SetActive(false);
            }

        } /* onInactivationRequested() */

    }  /* abstract ApollonAbstractGameplayBridge */

} /* } Labsim.apollon.gameplay */