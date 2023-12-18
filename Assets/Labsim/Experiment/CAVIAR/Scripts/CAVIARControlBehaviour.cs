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
namespace Labsim.experiment.CAVIAR
{

    public class CAVIARControlBehaviour 
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<CAVIARControlBridge>
    {

        #region properties/members

        // controls
        public CAVIARControl Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        public void OnEnable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : instantiating CAVIAR control");
                this.Control = new CAVIARControl();
            }

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnDisable() : call");
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class CAVIARControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
