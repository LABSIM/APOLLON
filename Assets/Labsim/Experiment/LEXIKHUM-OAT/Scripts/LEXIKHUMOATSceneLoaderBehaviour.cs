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

    public class LEXIKHUMOATSceneLoaderBehaviour
        : UnityEngine.MonoBehaviour
    {    

        [UnityEngine.SerializeField]
        private string m_sceneName;

        [UnityEngine.SerializeField]
        private UnityEngine.SceneManagement.LoadSceneMode m_sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive;

        [UnityEngine.SerializeField]
        private bool m_asynchronousSceneLoading = true;

        #region Unity Monobehaviour

        private void OnEnable()
        {
            
            // check
            if(this.m_asynchronousSceneLoading)
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(this.m_sceneName, this.m_sceneMode);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(this.m_sceneName, this.m_sceneMode);
            }

        } /* OnEnable() */

        private void OnDisable()
        {

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(this.m_sceneName);

        } /* OnDisable() */

        #endregion

    } /* public class LEXIKHUMOATSceneLoaderBehaviour */

} /* } Labsim.experiment.LEXIKHUMOAT */