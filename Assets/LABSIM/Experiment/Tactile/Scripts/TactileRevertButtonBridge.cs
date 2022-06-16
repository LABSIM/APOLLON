using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileRevertButtonBridge
        : TactileAbstractBridge
    {

        //ctor
        public TactileRevertButtonBridge()
            : base()
        { }

        public TactileRevertButtonDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileRevertButtonBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileRevertButtonBridge.WrapBehaviour() : could not find object of type behaviour.TactileRevertButtonBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileRevertButtonDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileManager.IDType WrapID()
        {
            return TactileManager.IDType.TactileRevertButton;
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

                // unbind
                this.Dispatcher.PressedEvent -= this.OnPressed;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Dispatcher event delegate

        private async void OnPressed(object sender, TactileResponseAreaDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileRevertButtonBridge.OnPressed() : begin"
            );

            // actions
            (this.Behaviour as TactileRevertButtonBehaviour).ResponseAreaBehaviour.ClearAllTouchpoint();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileRevertButtonBridge.OnPressed() : end"
            );

        } /* OnPressed() */

        #endregion

    } /* class TactileRevertButtonBridge */

} /* } Labsim.experiment.tactile */