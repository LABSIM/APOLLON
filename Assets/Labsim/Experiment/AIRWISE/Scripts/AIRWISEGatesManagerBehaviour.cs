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

    public class AIRWISEGatesManagerBehaviour
        : UnityEngine.MonoBehaviour
    {

        private readonly static System.Collections.Generic.Dictionary<string, AIRWISEGatesBehaviour.GatesTypeID> c_tags_dict
            = new()
            {
                {"AIRWISETag_Gates",      AIRWISEGatesBehaviour.GatesTypeID.Center    },
                {"AIRWISETag_LeftGates",  AIRWISEGatesBehaviour.GatesTypeID.Left      },
                {"AIRWISETag_RightGates", AIRWISEGatesBehaviour.GatesTypeID.Right     },
                {"AIRWISETag_Departure",  AIRWISEGatesBehaviour.GatesTypeID.Departure },
                {"AIRWISETag_Arrival",    AIRWISEGatesBehaviour.GatesTypeID.Arrival   }
            };

        #region Unity Monobehaviour events

        private void Awake()
        {
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded   += this.OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += this.OnSceneUnloaded;

        } /* Awake() */

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {

            // instantiate all components on marked objects
            foreach(var (key, value) in c_tags_dict)
            {

                // try find objects in loaded scene
                foreach(var current_gates in UnityEngine.GameObject.FindGameObjectsWithTag(key))
                {

                    // instantiate a fresh behaviour
                    var behaviour = current_gates.AddComponent<AIRWISEGatesBehaviour>();

                    // configure it
                    behaviour.GatesType = value;
                    behaviour.Manager = this;

                } /* foreach() */

            } /* foreach() */            

        } /* OnSceneLoaded*() */

        private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {

            // instantiate all components on marked objects
            foreach(var (key, value) in c_tags_dict)
            {

                // try find objects in loaded scene
                foreach(var current_gates in UnityEngine.GameObject.FindGameObjectsWithTag(key))
                {

                    // instantiate a fresh behaviour
                    var behaviour = current_gates.AddComponent<AIRWISEGatesBehaviour>();

                    // configure it
                    behaviour.GatesType = value;
                    behaviour.Manager = this;

                } /* foreach() */

            } /* foreach() */

        } /* OnSceneUnloaded*() */

        #endregion

    } /* public class AIRWISEGatesManagerBehaviour */

} /* } Labsim.experiment.AIRWISE */
