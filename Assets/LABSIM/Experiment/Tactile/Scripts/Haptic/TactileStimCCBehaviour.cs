using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileStimCCBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private bool m_bHasInitialized = false;

        // bridge
        public TactileStimCCBridge Bridge { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.Initialize() : begin");
            
            // todo

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.Close() : begin");
   
            // todo
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.Close() : end");

        } /* Close() */
        
        #region MonoBehaviour Impl         

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCCBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileStimCCBehaviour */

} /* } Labsim.experiment.tactile */