﻿//
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
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATControlBridge 
        : apollon.gameplay.ApollonGameplayBridge<LEXIKHUMOATControlBridge>
    {

        //ctor
        public LEXIKHUMOATControlBridge()
            : base()
        { }

        public LEXIKHUMOATControlBehaviour ConcreteBehaviour 
            => this.Behaviour as LEXIKHUMOATControlBehaviour;

        public LEXIKHUMOATControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as LEXIKHUMOATControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<LEXIKHUMOATControlBehaviour>(
                "LEXIKHUMOATControlBridge",
                "LEXIKHUMOATControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<LEXIKHUMOATControlDispatcher>(
                "LEXIKHUMOATControlBridge",
                "LEXIKHUMOATControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATControl;
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

                var behaviour = this.Behaviour as LEXIKHUMOATControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.JoystickHorizontalValueChanged.performed   += this.OnJoystickHorizontalValueChanged;
                behaviour.Control.Subject.JoystickVerticalValueChanged.performed     += this.OnJoystickVerticalValueChanged;
                behaviour.Control.Subject.ResponseTriggered.performed                += this.OnResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as LEXIKHUMOATControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.JoystickHorizontalValueChanged.performed   -= this.OnJoystickHorizontalValueChanged;
                behaviour.Control.Subject.JoystickVerticalValueChanged.performed     -= this.OnJoystickVerticalValueChanged;
                behaviour.Control.Subject.ResponseTriggered.performed                -= this.OnResponseTriggered;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Event delegate

        private void OnJoystickHorizontalValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.ConcreteDispatcher.RaiseJoystickHorizontalValueChanged(context.ReadValue<float>());

        } /* OnJoystickHorizontalValueChanged() */

        private void OnJoystickVerticalValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.ConcreteDispatcher.RaiseJoystickVerticalValueChanged(context.ReadValue<float>());

        } /* OnJoystickVerticalValueChanged() */

        private void OnResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATControlBridge.OnResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseResponseTriggered();

            } /* if() */

        } /* OnResponseTriggered() */

        #endregion

    }  /* class LEXIKHUMOATControlBridge */

} /* } Labsim.experiment.LEXIKHUM_OAT */
