using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileRevertButtonBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private Leap.Unity.Interaction.InteractionButton m_button = null;
        private bool m_bHasInitialized = false;

        // bridge
        public TactileRevertButtonBridge Bridge { get; set; }

        // property
        public TactileResponseAreaBehaviour ResponseAreaBehaviour => TactileManager.Instance.getBridge(TactileManager.IDType.TactileResponseArea).Behaviour as TactileResponseAreaBehaviour;

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileRevertButtonBehaviour.Initialize() : begin");
            
            // bind the button if recquired 
            if((this.m_button = this.GetComponent<Leap.Unity.Interaction.InteractionButton>()) == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> TactileRevertButtonBehaviour.Start() : could not find component of type Leap.Unity.Interaction.InteractionButton from Unity..."
                );

            } /* if() */

            // add the callback to the action handler
            this.m_button.OnPress += this.Bridge.Dispatcher.RaisePressed;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Close() : begin");

            // remove the dispatch callback from the action handler
            this.m_button.OnPress -= this.Bridge.Dispatcher.RaisePressed;
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Close() : end");

        } /* Close() */

        #region MonoBehaviour Impl 
        
        private void Update()
        {
            
            // skip
            if(!this.m_button)
            {
                return;
            }

            // handle button activation
            if((this.ResponseAreaBehaviour.TouchpointList.Count > 0) && !this.m_button.controlEnabled)
            {
            
                this.m_button.controlEnabled = true;
            
            }
            else if((this.ResponseAreaBehaviour.TouchpointList.Count == 0) && this.m_button.controlEnabled)
            {

                this.m_button.controlEnabled = false;

            } /* if() */ 

        } /* Update() */

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileRevertButtonBehaviour.OnEnable() : initialize required");

                // call
                this.Initialize();

            } /* if() */

        } /* OnEnable() */

        private void OnDisable() 
        {

            // skip if it hasn't been initialized 
            if (this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileRevertButtonBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileRevertButtonBehaviour */

} /* } Labsim.experiment.tactile */