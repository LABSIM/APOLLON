
// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonRedCrossGUIBehaviour : UnityEngine.MonoBehaviour
    {

        
        [UnityEngine.SerializeField]
        TMPro.TextMeshPro counterUI = null;
        
        [UnityEngine.SerializeField]
        TMPro.TextMeshPro instructionUI = null;

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

                counterUI.text  = experiment.ApollonExperimentManager.Instance.Profile.CounterStatus;
                counterUI.color = /* strong red */ new UnityEngine.Color(0.8f, 0.02f, 0.02f, 1.0f);

            } /*if() */

            if(instructionUI != null)
            {

                instructionUI.text  = experiment.ApollonExperimentManager.Instance.Profile.InstructionStatus;
                instructionUI.color = /* strong red */ new UnityEngine.Color(0.8f, 0.02f, 0.02f, 1.0f);

            } /*if() */

        } /* OnEnable() */

    } /* public class ApollonRedCrossElementBehaviour */

} /* } Labsim.apollon.frontend.gui */