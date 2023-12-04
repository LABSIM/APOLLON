
using UXF;

// avoid namespace pollution
namespace Labsim.apollon
{

    public class ApollonEngineComponent : UnityEngine.MonoBehaviour
    {

        private System.Collections.Generic.Queue<System.Action> m_pendingAction = new System.Collections.Generic.Queue<System.Action>();
        public void PendAction(System.Action task)
        {

            lock(this.m_pendingAction)
            {
                this.m_pendingAction.Enqueue(task);
            }
            
        } /* PendAction() */


        // Use this for initialization
        public void Start()
        {
            // follow call
            ApollonEngine.Instance.Start();

            // then bind event to delegate 
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += this.SceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += this.SceneUnloaded;

        } /* Start() */

        // on activation
        public void Awake()
        {
            ApollonEngine.Instance.Awake();
        }

        // Update is called once per frame
        public void Update()
        {
            ApollonEngine.Instance.Update();
        }

        // Update is fixed call
        public void FixedUpdate()
        {

            lock(this.m_pendingAction)
            {

                // flush pending action from Apollon engine 
                while(this.m_pendingAction.Count != 0)
                {
                    
                    // execute them
                    this.m_pendingAction.Dequeue()();

                } /* while() */

            } /* lock() */

            ApollonEngine.Instance.FixedUpdate();
        
        } /* FixedUpdate() */ 

        // Call whenever a new experimentation session is launched
        public void ExperimentSessionBegin(UXF.Session experimentSession)
        {
            ApollonEngine.Instance.ExperimentSessionBegin(experimentSession);
        }

        // Call whenever an experimentation session is stopped
        public void ExperimentSessionEnd()
        {
            ApollonEngine.Instance.ExperimentSessionEnd();
        }

        // Call whenever a new trial session is launched
        public void ExperimentTrialBegin(UXF.Trial experimentTrial)
        {
            ApollonEngine.Instance.ExperimentTrialBegin(experimentTrial);
        }

        // Call whenever an experimentation trial is stopped
        public void ExperimentTrialEnd()
        {
            ApollonEngine.Instance.ExperimentTrialEnd();
        }

        // Call whenever a new scene is loaded
        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            ApollonEngine.Instance.SceneLoaded(scene, mode);
        }

        // Call whenever a new scene is unloaded
        public void SceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {
            ApollonEngine.Instance.SceneUnloaded(scene);
        }

    } /* class AppollonEngineComponent */

} /* namespace Labsim.apollo */
