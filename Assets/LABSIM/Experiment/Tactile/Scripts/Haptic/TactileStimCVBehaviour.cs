using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileStimCVBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private bool m_bHasInitialized = false;

        // bridge
        public TactileStimCVBridge Bridge { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.Initialize() : begin");
            
            // todo

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.Close() : begin");
   
            // todo
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.Close() : end");

        } /* Close() */
        
        #region MonoBehaviour Impl         

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileStimCVBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileStimCVBehaviour */

} /* } Labsim.experiment.tactile */