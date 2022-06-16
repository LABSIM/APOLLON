using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileValidateButtonBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private Leap.Unity.Interaction.InteractionButton m_button = null;
        private bool m_bHasInitialized = false;

        // bridge
        public TactileValidateButtonBridge Bridge { get; set; }

        // property
        private TactileResponseAreaBehaviour ResponseAreaBehaviour => TactileManager.Instance.getBridge(TactileManager.IDType.TactileResponseArea).Behaviour as TactileResponseAreaBehaviour;
        
        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.Initialize() : begin");
            
            // bind the button if recquired 
            if((this.m_button = this.GetComponent<Leap.Unity.Interaction.InteractionButton>()) == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> TactileValidateButtonBehaviour.Start() : could not find component of type Leap.Unity.Interaction.InteractionButton from Unity..."
                );

            } /* if() */

            // add the callback to the action handler
            this.m_button.OnPress += this.Bridge.Dispatcher.RaisePressed;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.Close() : begin");

            // remove the dispatch callback from the action handler
            this.m_button.OnPress -= this.Bridge.Dispatcher.RaisePressed;
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.Close() : end");

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
            if((this.ResponseAreaBehaviour.TouchpointList.Count >= TactileResponseAreaBehaviour.s_touchpointMaxCount) && !this.m_button.controlEnabled)
            {
            
                this.m_button.controlEnabled = true;
            
            }
            else if((this.ResponseAreaBehaviour.TouchpointList.Count < TactileResponseAreaBehaviour.s_touchpointMaxCount) && this.m_button.controlEnabled)
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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileValidateButtonBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileValidateButtonBehaviour */

} /* } Labsim.experiment.tactile */