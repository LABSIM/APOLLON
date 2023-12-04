
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonCAVIARControlBridge 
        : ApollonGameplayBridge<ApollonCAVIARControlBridge>
    {

        //ctor
        public ApollonCAVIARControlBridge()
            : base()
        { }

        public ApollonCAVIARControlBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonCAVIARControlBehaviour;

        public ApollonCAVIARControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonCAVIARControlDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonCAVIARControlBehaviour>(
                "ApollonCAVIARControlBridge",
                "ApollonCAVIARControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonCAVIARControlDispatcher>(
                "ApollonCAVIARControlBridge",
                "ApollonCAVIARControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.CAVIARControl;
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

                var behaviour = this.Behaviour as ApollonCAVIARControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ValueChanged.performed += this.OnAxisZValueChanged;
                behaviour.Control.Subject.NeutralCommandTriggered.performed += this.OnUserNeutralCommandTriggered;
                behaviour.Control.Subject.UserResponse.performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as ApollonCAVIARControlBehaviour;

                // remove them from bridge delegate
                behaviour.Control.Subject.ValueChanged.performed -= this.OnAxisZValueChanged;
                behaviour.Control.Subject.NeutralCommandTriggered.performed -= this.OnUserNeutralCommandTriggered;
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
                    "<color=Blue>Info: </color> ApollonCAVIARControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserNeutralCommandTriggered();

            } /* if() */

        } /* OnUserNeutralCommandTriggered() */

        private void OnUserResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARControlBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class ApollonCAVIARControlBridge */

} /* } Labsim.apollon.gameplay.control */
