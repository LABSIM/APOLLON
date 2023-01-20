
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV2ControlBridge 
        : ApollonGameplayBridge<ApollonAgencyAndThresholdPerceptionV2ControlBridge>
    {

        //ctor
        public ApollonAgencyAndThresholdPerceptionV2ControlBridge()
            : base()
        { }

        public ApollonAgencyAndThresholdPerceptionV2ControlBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonAgencyAndThresholdPerceptionV2ControlBehaviour;

        public ApollonAgencyAndThresholdPerceptionV2ControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonAgencyAndThresholdPerceptionV2ControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonAgencyAndThresholdPerceptionV2ControlBehaviour>(
                "ApollonAgencyAndThresholdPerceptionV2ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV2ControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonAgencyAndThresholdPerceptionV2ControlDispatcher>(
                "ApollonAgencyAndThresholdPerceptionV2ControlBridge",
                "ApollonAgencyAndThresholdPerceptionV2ControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control;
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

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV2ControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ValueChanged.performed += this.OnAxisZValueChanged;
                // behaviour.Control.Subject.NeutralCommandTriggered.performed += this.OnUserNeutralCommandTriggered;
                // behaviour.Control.Subject.PositiveCommandTriggered.performed += this.OnUserPositiveCommandTriggered;
                behaviour.Control.Subject.NegativeCommandTriggered.performed += this.OnUserNegativeCommandTriggered;
                behaviour.Control.Subject.UserResponse.performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as ApollonAgencyAndThresholdPerceptionV2ControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.ValueChanged.performed -= this.OnAxisZValueChanged;
                // behaviour.Control.Subject.NeutralCommandTriggered.performed -= this.OnUserNeutralCommandTriggered;
                // behaviour.Control.Subject.PositiveCommandTriggered.performed -= this.OnUserPositiveCommandTriggered;
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBridge.OnUserPositiveCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBridge.OnUserNegativeCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class ApollonAgencyAndThresholdPerceptionV2ControlBridge */

} /* } Labsim.apollon.gameplay.control */
