
// avoid namespace pollution
namespace Labsim.apollon.frontend.menu
{

    public class ApollonOptionMenuBehaviour : UnityEngine.MonoBehaviour
    {

	    // Use this for initialization
        void Start()
        {
	    
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnRunButtonClick()
        {
            // TODO 
        }

        public void OnConfigureButtonClick()
        {
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.ConfigureMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.OptionMenu);
        }

        // load seleected content & rewind
        public void OnBackButtonClick()
        {
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.FileSelectionMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.OptionMenu);
        }

    } /* public class ApollonOptionMenuBehaviour */

} /* } Labsim.apollon.frontend.menu */