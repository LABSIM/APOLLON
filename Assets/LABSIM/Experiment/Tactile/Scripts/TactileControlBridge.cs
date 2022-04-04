
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileControlBridge 
        : Labsim.apollon.gameplay.ApollonAbstractGameplayBridge
    {

        //ctor
        public TactileControlBridge()
            : base()
        { }

        public TactileControlDispatcher Dispatcher { private set; get; } = null;
        
        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileControlBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileControlBridge.WrapBehaviour() : could not find object of type behaviour.TactileControlBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileControlDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override Labsim.apollon.gameplay.ApollonGameplayManager.GameplayIDType WrapID()
        {
            return Labsim.apollon.gameplay.ApollonGameplayManager.GameplayIDType.TactileControl;
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

                var behaviour = this.Behaviour as TactileControlBehaviour;

                // add them a bridge delegate
                // behaviour.Control.Subject.ValueChanged.performed += this.OnAxisZValueChanged;
                // behaviour.Control.Subject.NeutralCommandTriggered.performed += this.OnUserNeutralCommandTriggered;
                // behaviour.Control.Subject.UserResponse.performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                var behaviour = this.Behaviour as TactileControlBehaviour;

                // remove them from bridge delegate
                // behaviour.Control.Subject.ValueChanged.performed -= this.OnAxisZValueChanged;
                // behaviour.Control.Subject.NeutralCommandTriggered.performed -= this.OnUserNeutralCommandTriggered;
                // behaviour.Control.Subject.UserResponse.performed -= this.OnUserResponseTriggered;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Event delegate

        // private void OnAxisZValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        // {
           
        //     // dispatch event
        //     this.Dispatcher.RaiseAxisZValueChanged(context.ReadValue<float>());

        // } /* OnAxisZValueChanged() */

        // private void OnUserNeutralCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        // {
            
        //     // check
        //     if (context.performed)
        //     {

        //         // log
        //         UnityEngine.Debug.Log(
        //             "<color=Blue>Info: </color> TactileControlBridge.OnUserNeutralCommandTriggered() : event triggered !"
        //         );

        //         // dispatch event
        //         this.Dispatcher.RaiseUserNeutralCommandTriggered();

        //     } /* if() */

        // } /* OnUserNeutralCommandTriggered() */

        // private void OnUserResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        // {

        //     // check
        //     if (context.performed)
        //     {

        //         // log
        //         UnityEngine.Debug.Log(
        //             "<color=Blue>Info: </color> TactileControlBridge.OnUserResponseTriggered() : event triggered !"
        //         );

        //         // dispatch event
        //         this.Dispatcher.RaiseUserResponseTriggered();

        //     } /* if() */

        // } /* OnUserResponseTriggered() */

        #endregion

    }  /* class TactileControlBridge */

} /* } Labsim.experiment.tactile */
