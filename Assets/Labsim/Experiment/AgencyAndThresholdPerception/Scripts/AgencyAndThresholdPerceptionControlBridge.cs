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

// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerception
{

    public class AgencyAndThresholdPerceptionControlBridge 
        : apollon.gameplay.ApollonGameplayBridge<AgencyAndThresholdPerceptionControlBridge>
    {

        //ctor
        public AgencyAndThresholdPerceptionControlBridge()
            : base()
        { }

        public AgencyAndThresholdPerceptionControlBehaviour ConcreteBehaviour 
            => this.Behaviour as AgencyAndThresholdPerceptionControlBehaviour;

        public AgencyAndThresholdPerceptionControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as AgencyAndThresholdPerceptionControlDispatcher;
        
        #region Bridge abstract implementation 
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<AgencyAndThresholdPerceptionControlBehaviour>(
                "AgencyAndThresholdPerceptionControlBridge",
                "AgencyAndThresholdPerceptionControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<AgencyAndThresholdPerceptionControlDispatcher>(
                "AgencyAndThresholdPerceptionControlBridge",
                "AgencyAndThresholdPerceptionControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl;
        }
        
        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate 
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ValueChanged.performed += this.OnAxisZValueChanged;
                behaviour.Control.Subject.NeutralCommandTriggered.performed += this.OnUserNeutralCommandTriggered;
                behaviour.Control.Subject.PositiveCommandTriggered.performed += this.OnUserPositiveCommandTriggered;
                behaviour.Control.Subject.NegativeCommandTriggered.performed += this.OnUserNegativeCommandTriggered;
                behaviour.Control.Subject.UserResponse.performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.ValueChanged.performed -= this.OnAxisZValueChanged;
                behaviour.Control.Subject.NeutralCommandTriggered.performed -= this.OnUserNeutralCommandTriggered;
                behaviour.Control.Subject.PositiveCommandTriggered.performed -= this.OnUserPositiveCommandTriggered;
                behaviour.Control.Subject.NegativeCommandTriggered.performed -= this.OnUserNegativeCommandTriggered;
                behaviour.Control.Subject.UserResponse.performed -= this.OnUserResponseTriggered;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Event delegate

        private void OnAxisZValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.ConcreteDispatcher.RaiseAxisZValueChanged(context.ReadValue<float>());

        } /* OnAxisZValueChanged() */

        private void OnUserNeutralCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserNeutralCommandTriggered();

            } /* if() */

        } /* OnUserNeutralCommandTriggered() */

        private void OnUserPositiveCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBridge.OnUserPositiveCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserPositiveCommandTriggered();

            } /* if() */

        } /* OnUserPositiveCommandTriggered() */

        private void OnUserNegativeCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBridge.OnUserNegativeCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserNegativeCommandTriggered();

            } /* if() */

        } /* OnUserNegativeCommandTriggered() */

        private void OnUserResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class AgencyAndThresholdPerceptionControlBridge */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
