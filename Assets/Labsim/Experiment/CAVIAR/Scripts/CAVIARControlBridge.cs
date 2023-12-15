
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    public class CAVIARControlBridge 
        : apollon.gameplay.ApollonGameplayBridge<CAVIARControlBridge>
    {

        //ctor
        public CAVIARControlBridge()
            : base()
        { }

        public CAVIARControlBehaviour ConcreteBehaviour 
            => this.Behaviour as CAVIARControlBehaviour;

        public CAVIARControlDispatcher ConcreteDispatcher 
            => this.Dispatcher as CAVIARControlDispatcher;

        #region Bridge abstract implementation
        
        protected override apollon.gameplay.ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<CAVIARControlBehaviour>(
                "CAVIARControlBridge",
                "CAVIARControlBehaviour"
            );

        } /* WrapBehaviour() */

        protected override apollon.gameplay.ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<CAVIARControlDispatcher>(
                "CAVIARControlBridge",
                "CAVIARControlDispatcher"
            );

        } /* WrapDispatcher() */

        protected override apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl;
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

                var behaviour = this.Behaviour as CAVIARControlBehaviour;

                // add them a bridge delegate
                behaviour.Control.Subject.ValueChanged.performed += this.OnAxisZValueChanged;
                behaviour.Control.Subject.NeutralCommandTriggered.performed += this.OnUserNeutralCommandTriggered;
                behaviour.Control.Subject.UserResponse.performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as CAVIARControlBehaviour;

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
                    "<color=Blue>Info: </color> CAVIARControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
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
                    "<color=Blue>Info: </color> CAVIARControlBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.ConcreteDispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class CAVIARControlBridge */

} /* } Labsim.apollon.gameplay.control */
