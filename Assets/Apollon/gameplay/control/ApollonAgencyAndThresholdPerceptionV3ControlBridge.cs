
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV3ControlBridge 
        : ApollonGameplayBridge<ApollonAgencyAndThresholdPerceptionV3ControlBridge>
    {

        //ctor
        public ApollonAgencyAndThresholdPerceptionV3ControlBridge()
            : base()
        { }

        public ApollonAgencyAndThresholdPerceptionV3ControlBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonAgencyAndThresholdPerceptionV3ControlBehaviour;

        public ApollonAgencyAndThresholdPerceptionV3ControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonAgencyAndThresholdPerceptionV3ControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonAgencyAndThresholdPerceptionV3ControlBehaviour>(
                "ApollonAgencyAndThresholdPerceptionV3ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV3ControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonAgencyAndThresholdPerceptionV3ControlDispatcher>(
                "ApollonAgencyAndThresholdPerceptionV3ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV3ControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV3Control;
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

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV3ControlBehaviour;

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

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV3ControlBehaviour;

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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBridge.OnThrottleNeutralCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBridge.OnThrottlePositiveCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBridge.OnThrottleNegativeCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBridge.OnResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseResponseTriggered();

            } /* if() */

        } /* OnResponseTriggered() */

        #endregion

    }  /* class ApollonAgencyAndThresholdPerceptionV3ControlBridge */

} /* } Labsim.apollon.gameplay.control */
