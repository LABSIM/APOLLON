using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileHandMeshClonerBridge
        : TactileAbstractBridge
    {

        //ctor
        public TactileHandMeshClonerBridge()
            : base()
        { }

        public TactileHandMeshClonerDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileHandMeshClonerBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileHandMeshClonerBridge.WrapBehaviour() : could not find object of type behaviour.TactileHandMeshClonerBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new TactileHandMeshClonerDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileManager.IDType WrapID()
        {
            return TactileManager.IDType.TactileHandMeshCloner;
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
                this.Dispatcher.ButtonClonePressedEvent += this.OnButtonClonePressed;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // unbind
                this.Dispatcher.ButtonClonePressedEvent -= this.OnButtonClonePressed;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Dispatcher event delegate

        private async void OnButtonClonePressed(object sender, TactileHandMeshClonerDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHandMeshClonerBridge.OnButtonClonePressed() : begin"
            );

            // actions
            (this.Behaviour as TactileHandMeshClonerBehaviour).CloneHandMesh();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileHandMeshClonerBridge.OnButtonClonePressed() : end"
            );

        } /* OnPressed() */

        #endregion

    } /* class TactileHandMeshClonerBridge */

} /* } Labsim.experiment.tactile */