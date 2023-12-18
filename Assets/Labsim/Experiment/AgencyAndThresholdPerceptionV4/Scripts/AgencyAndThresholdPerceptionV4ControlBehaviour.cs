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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    public class AgencyAndThresholdPerceptionV4ControlBehaviour 
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<AgencyAndThresholdPerceptionV4ControlBridge>
    {

        #region properties/members

        // controls
        public AgencyAndThresholdPerceptionV4Control Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4ControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4ControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new AgencyAndThresholdPerceptionV4Control();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4ControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4ControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4ControlBehaviour.OnDisable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class AgencyAndThresholdPerceptionV4ControlBehaviour */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
