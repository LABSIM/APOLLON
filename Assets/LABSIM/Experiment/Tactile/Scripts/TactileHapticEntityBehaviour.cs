using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileHapticEntityBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private bool m_bHasInitialized = false;

        // properties
        public TactileHapticEntityBridge Bridge { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var Idle = this.gameObject.AddComponent<IdleController>();
            var StimCC = this.gameObject.AddComponent<StimCCController>();
            var StimCV = this.gameObject.AddComponent<StimCVController>();
            var StimVC = this.gameObject.AddComponent<StimVCController>();
            var StimVV = this.gameObject.AddComponent<StimVVController>();

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Initialize() : state controller added as gameObject's component");

            // do

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Close() : begin");

            // do
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.Close() : end");

        } /* Close() */

        #region Controllers section

        internal sealed class IdleController
            : UnityEngine.MonoBehaviour
        {

            private TactileHapticEntityBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // do deactivate all stim
                TactileManager.Instance.setInactive(TactileManager.IDType.TactileStimCC);
                TactileManager.Instance.setInactive(TactileManager.IDType.TactileStimCV);
                TactileManager.Instance.setInactive(TactileManager.IDType.TactileStimVC);
                TactileManager.Instance.setInactive(TactileManager.IDType.TactileStimVV);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class IdleController */

        internal sealed class StimCCController
            : UnityEngine.MonoBehaviour
        {

            private TactileHapticEntityBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCCController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimCCController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // activate stim
                TactileManager.Instance.setActive(TactileManager.IDType.TactileStimCC);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCCController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCCController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimCCController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCCController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class StimCCController */

        internal sealed class StimCVController
            : UnityEngine.MonoBehaviour
        {

            private TactileHapticEntityBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCVController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimCVController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // activate stim
                TactileManager.Instance.setActive(TactileManager.IDType.TactileStimCV);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCVController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCVController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimCVController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimCVController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class StimCVController */

        internal sealed class StimVCController
            : UnityEngine.MonoBehaviour
        {

            private TactileHapticEntityBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVCController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimVCController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // activate stim
                TactileManager.Instance.setActive(TactileManager.IDType.TactileStimVC);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVCController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVCController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimVCController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVCController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class StimVCController */

        internal sealed class StimVVController
            : UnityEngine.MonoBehaviour
        {

            private TactileHapticEntityBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVVController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimVVController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // activate stim
                TactileManager.Instance.setActive(TactileManager.IDType.TactileStimVV);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVVController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVVController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileHapticEntityBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileHapticEntityBehaviour.StimVVController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileHapticEntityBehaviour.StimVVController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class StimVVController */

        #endregion

        #region MonoBehaviour Impl 

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHapticEntityBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileHapticEntityBehaviour */

} /* } Labsim.experiment.tactile */