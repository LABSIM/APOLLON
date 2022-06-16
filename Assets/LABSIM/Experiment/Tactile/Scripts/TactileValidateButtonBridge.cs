using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileValidateButtonBridge
        : TactileAbstractBridge
    {

        //ctor
        public TactileValidateButtonBridge()
            : base()
        { }

        public TactileValidateButtonDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileValidateButtonBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileValidateButtonBridge.WrapBehaviour() : could not find object of type behaviour.TactileValidateButtonBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileValidateButtonDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileManager.IDType WrapID()
        {
            return TactileManager.IDType.TactileValidateButton;
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

                // bind
                this.Dispatcher.PressedEvent += this.OnPressed;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // bind
                this.Dispatcher.PressedEvent -= this.OnPressed;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Dispatcher event delegate

        private async void OnPressed(object sender, TactileValidateButtonDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileValidateButtonBehaviour.OnPressed() : begin"
            );

            // actions
            // (this.Behaviour as TactileRevertButtonBehaviour).ResponseAreaBehaviour.ClearAllTouchpoint();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileValidateButtonBehaviour.OnPressed() : end"
            );

        } /* OnPressed() */

        #endregion

    } /* class TactileValidateButtonBehaviour */

} /* } Labsim.experiment.tactile */