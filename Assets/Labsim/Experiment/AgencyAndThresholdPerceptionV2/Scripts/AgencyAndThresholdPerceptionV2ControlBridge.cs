
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV2
{

    public class AgencyAndThresholdPerceptionV2ControlBridge 
        : apollon.gameplay.ApollonGameplayBridge<AgencyAndThresholdPerceptionV2ControlBridge>
    {

        //ctor
        public AgencyAndThresholdPerceptionV2ControlBridge()
            : base()
        { }

        public AgencyAndThresholdPerceptionV2ControlBehaviour ConcreteBehaviour 
            => this.Behaviour as AgencyAndThresholdPerceptionV2ControlBehaviour;

        public AgencyAndThresholdPerceptionV2ControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as AgencyAndThresholdPerceptionV2ControlDispatcher;
        
        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<AgencyAndThresholdPerceptionV2ControlBehaviour>(
                "AgencyAndThresholdPerceptionV2ControlBridge",
                "AgencyAndThresholdPerceptionV2ControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<AgencyAndThresholdPerceptionV2ControlDispatcher>(
                "AgencyAndThresholdPerceptionV2ControlBridge",
                "AgencyAndThresholdPerceptionV2ControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV2Control;
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

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionV2ControlBehaviour;

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

                var behaviour = this.Behaviour as AgencyAndThresholdPerceptionV2ControlBehaviour;

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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBridge.OnUserPositiveCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBridge.OnUserNegativeCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class AgencyAndThresholdPerceptionV2ControlBridge */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
