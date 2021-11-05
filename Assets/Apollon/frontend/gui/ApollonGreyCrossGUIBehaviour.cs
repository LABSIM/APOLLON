using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonGreyCrossGUIBehaviour : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        TMPro.TextMeshPro counterUI = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnEnable() 
        {

            if(counterUI != null)
            {

                counterUI.text = experiment.ApollonExperimentManager.Instance.Profile.CounterStatus;

            } /*if() */

        } /* OnEnable() */

    } /* public class ApollonGreyCrossGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */