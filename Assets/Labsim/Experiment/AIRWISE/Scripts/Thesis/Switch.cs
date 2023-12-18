using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsToHandle;
    [SerializeField]
    private GameObject[] objectsToHandleNeg;
    private bool m_finished = false;

    // Unity workflow-based methods

    private void Awake()
    {
        this.activateObjects(this.objectsToHandle);
        this.deactivateObjects(this.objectsToHandleNeg);
    }

    private void Start()
    {
        this.enableObjects(this.objectsToHandle);
        this.disableObjects(this.objectsToHandleNeg);
    }

    private void FixedUpdate() {
        // Check if all iterations are passed
        if (!this.m_finished) {
            if (Manager.Instance.EndTrialOnButtonCondition()) {
                Debug.Log(
                    "<color=blue>Info: </color> " + this.GetType() + ".FixedUpdate(): Stopping run on Button 2 click."
                );
                this.gameObject.SetActive(false);
                this.disableObjects(this.objectsToHandle);
                this.enableObjects(this.objectsToHandleNeg);
            }
            if (Manager.Instance.EndTrialOnFinalTimeCondition()) {
                Debug.Log(
                    "<color=blue>Info: </color> " + this.GetType() + ".FixedUpdate(): Stopping run at " 
                    + (Manager.Instance.GetCorrectedElapsedTime()).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) 
                    + " seconds."
                );
                this.gameObject.SetActive(false);
                this.disableObjects(this.objectsToHandle);
                this.enableObjects(this.objectsToHandleNeg);
            }
            if (Manager.Instance.EndTrialOnTaskEndCondition()) {
                Debug.Log(
                    "<color=blue>Info: </color> " + this.GetType() + ".FixedUpdate(): Stopping run after reaching task end state. Arrival at " 
                    + Manager.Instance.GetDistanceToArrival()
                    + "m."
                );
                this.gameObject.SetActive(false);
                this.disableObjects(this.objectsToHandle);
                this.enableObjects(this.objectsToHandleNeg);
            }
            if (Manager.Instance.EndTrialOnIterationCountCondition()) {
                Debug.Log(
                    "<color=blue>Info: </color> " + this.GetType() + ".FixedUpdate(): Stopping run after final block."
                );
                this.gameObject.SetActive(false);
                this.disableObjects(this.objectsToHandle);
                this.enableObjects(this.objectsToHandleNeg);
                this.m_finished = true;
                Debug.Break();
                Application.Quit();
            }
        }
    }

    // Event-based methods

    private void OnEnable()
    {
        if (!this.m_finished) {
            if (Manager.Instance.EndTrialOnIterationCountCondition()) {
                Debug.Log(
                    "<color=blue>Info: </color> " + this.GetType() + ".OnEnable(): Stopped run after final block."
                );
                this.gameObject.SetActive(true);
                return;
            }
            this.enableObjects(this.objectsToHandle);
            this.activateObjects(this.objectsToHandle);
            this.disableObjects(this.objectsToHandleNeg);
            this.deactivateObjects(this.objectsToHandleNeg);
        } else {            
            Debug.Break();
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        this.disableObjects(this.objectsToHandle);
        this.deactivateObjects(this.objectsToHandle);
        this.enableObjects(this.objectsToHandleNeg);
        this.activateObjects(this.objectsToHandleNeg);
        if (this.m_finished) {            
            Debug.Break();
            Application.Quit();
        }
    }

    // Triggers Awake
    private void activateObjects(GameObject[] objs) 
    {
        foreach(var obj in objs)
        {
            obj.SetActive(true);
        }
    }

    private void deactivateObjects(GameObject[] objs) 
    {
        foreach(var obj in objs)
        {
            obj.SetActive(false);
        }
    }

    // Triggers onEnable
    private void enableObjects(GameObject[] objs) 
    {
        foreach(var obj in objs)
        {
            foreach(var comp in obj.GetComponents<MonoBehaviour>())
            {
                comp.enabled = true;
            }
        }
    }

    // Triggers onDisable
    private void disableObjects(GameObject[] objs) 
    {
        foreach(var obj in objs)
        {
            foreach(var comp in obj.GetComponents<MonoBehaviour>())
            {
                comp.enabled = false;
            }
        }
    }
}
