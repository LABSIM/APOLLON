// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonResponseSliderGUIBehaviour 
        : UnityEngine.MonoBehaviour
    {

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

            if(instructionUI != null)
            {

                instructionUI.text  = experiment.ApollonExperimentManager.Instance.Profile.InstructionStatus;
                instructionUI.color = /* black */ UnityEngine.Color.black;

            } /*if() */

        } /* OnEnable() */

    } /* public class ApollonResponseSliderGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */