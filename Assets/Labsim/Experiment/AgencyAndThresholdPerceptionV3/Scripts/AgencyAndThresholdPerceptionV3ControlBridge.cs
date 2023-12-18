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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV3
{

    public class AgencyAndThresholdPerceptionV3ControlBridge 
        : apollon.gameplay.ApollonGameplayBridge<AgencyAndThresholdPerceptionV3ControlBridge>
    {

        //ctor
        public AgencyAndThresholdPerceptionV3ControlBridge()
            : base()
        { }

        public AgencyAndThresholdPerceptionV3ControlBehaviour ConcreteBehaviour 
            => this.Behaviour as AgencyAndThresholdPerceptionV3ControlBehaviour;

        public AgencyAndThresholdPerceptionV3ControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as AgencyAndThresholdPerceptionV3ControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<AgencyAndThresholdPerceptionV3ControlBehaviour>(
                "AgencyAndThresholdPerceptionV3ControlBridge",
                "AgencyAndThresholdPerceptionV3ControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<AgencyAndThresholdPerceptionV3ControlDispatcher>(
                "AgencyAndThresholdPerceptionV3ControlBridge",
                "AgencyAndThresholdPerceptionV3ControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control;
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

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionV3ControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ThrottleValueChanged.performed             += this.OnThrottleValueChanged;
                behaviour.Control.Subject.ThrottleNegativeCommandTriggered.performed += this.OnThrottleNegativeCommandTriggered;
                behaviour.Control.Subject.ThrottleNeutralCommandTriggered.performed  += this.OnThrottleNeutralCommandTriggered;
                behaviour.Control.Subject.ThrottlePositiveCommandTriggered.performed += this.OnThrottlePositiveCommandTriggered;
                behaviour.Control.Subject.JoystickValueChanged.performed             += this.OnJoystickValueChanged;
                behaviour.Control.Subject.ResponseTriggered.performed                += this.OnResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionV3ControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.ThrottleValueChanged.performed             -= this.OnThrottleValueChanged;
                behaviour.Control.Subject.ThrottleNegativeCommandTriggered.performed -= this.OnThrottleNegativeCommandTriggered;
                behaviour.Control.Subject.ThrottleNeutralCommandTriggered.performed  -= this.OnThrottleNeutralCommandTriggered;
                behaviour.Control.Subject.ThrottlePositiveCommandTriggered.performed -= this.OnThrottlePositiveCommandTriggered;
                behaviour.Control.Subject.JoystickValueChanged.performed             -= this.OnJoystickValueChanged;
                behaviour.Control.Subject.ResponseTriggered.performed                -= this.OnResponseTriggered;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Event delegate

        private void OnThrottleValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.ConcreteDispatcher.RaiseThrottleValueChanged(context.ReadValue<float>());

        } /* OnThrottleValueChanged() */

        private void OnThrottleNeutralCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBridge.OnThrottleNeutralCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseThrottleNeutralCommandTriggered();

            } /* if() */

        } /* OnThrottleNeutralCommandTriggered() */

        private void OnThrottlePositiveCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBridge.OnThrottlePositiveCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseThrottlePositiveCommandTriggered();

            } /* if() */

        } /* OnThrottlePositiveCommandTriggered() */

        private void OnThrottleNegativeCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBridge.OnThrottleNegativeCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseThrottleNegativeCommandTriggered();

            } /* if() */

        } /* OnThrottleNegativeCommandTriggered() */

        private void OnJoystickValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.ConcreteDispatcher.RaiseJoystickValueChanged(context.ReadValue<float>());

        } /* OnJoystickValueChanged() */

        private void OnResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBridge.OnResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseResponseTriggered();

            } /* if() */

        } /* OnResponseTriggered() */

        #endregion

    }  /* class AgencyAndThresholdPerceptionV3ControlBridge */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV3 */
