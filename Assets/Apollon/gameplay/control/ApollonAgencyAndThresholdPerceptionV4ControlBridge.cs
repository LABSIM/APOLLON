
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV4ControlBridge 
        : ApollonGameplayBridge<ApollonAgencyAndThresholdPerceptionV4ControlBridge>
    {

        //ctor
        public ApollonAgencyAndThresholdPerceptionV4ControlBridge()
            : base()
        { }

        public ApollonAgencyAndThresholdPerceptionV4ControlBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonAgencyAndThresholdPerceptionV4ControlBehaviour;

        public ApollonAgencyAndThresholdPerceptionV4ControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonAgencyAndThresholdPerceptionV4ControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonAgencyAndThresholdPerceptionV4ControlBehaviour>(
                "ApollonAgencyAndThresholdPerceptionV4ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV4ControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonAgencyAndThresholdPerceptionV4ControlDispatcher>(
                "ApollonAgencyAndThresholdPerceptionV4ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV4ControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control;
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

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV4ControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ThrottleValueChanged.performed             += this.OnThrottleValueChanged;
                behaviour.Control.Subject.ThrottleNegativeCommandTriggered.performed += this.OnThrottleNegativeCommandTriggered;
                behaviour.Control.Subject.ThrottleNeutralCommandTriggered.performed  += this.OnThrottleNeutralCommandTriggered;
                behaviour.Control.Subject.ThrottlePositiveCommandTriggered.performed += this.OnThrottlePositiveCommandTriggered;
                behaviour.Control.Subject.JoystickHorizontalValueChanged.performed   += this.OnJoystickHorizontalValueChanged;
                behaviour.Control.Subject.JoystickVerticalValueChanged.performed     += this.OnJoystickVerticalValueChanged;
                behaviour.Control.Subject.ResponseTriggered.performed                += this.OnResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV4ControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.ThrottleValueChanged.performed             -= this.OnThrottleValueChanged;
                behaviour.Control.Subject.ThrottleNegativeCommandTriggered.performed -= this.OnThrottleNegativeCommandTriggered;
                behaviour.Control.Subject.ThrottleNeutralCommandTriggered.performed  -= this.OnThrottleNeutralCommandTriggered;
                behaviour.Control.Subject.ThrottlePositiveCommandTriggered.performed -= this.OnThrottlePositiveCommandTriggered;
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4ControlBridge.OnThrottleNeutralCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4ControlBridge.OnThrottlePositiveCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4ControlBridge.OnThrottleNegativeCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseThrottleNegativeCommandTriggered();

            } /* if() */

        } /* OnThrottleNegativeCommandTriggered() */

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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV4ControlBridge.OnResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseResponseTriggered();

            } /* if() */

        } /* OnResponseTriggered() */

        #endregion

    }  /* class ApollonAgencyAndThresholdPerceptionV4ControlBridge */

} /* } Labsim.apollon.gameplay.control */
