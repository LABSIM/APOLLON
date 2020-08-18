
// avoid namespace pollution
namespace Labsim.apollon.frontend.menu
{

    public class ApollonMainMenuBehaviour : UnityEngine.MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        } /* Start() */

        // Update is called once per frame
        void Update()
        {

        }

        // callback       

        public void OnSelectButtonClicked()
        {
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.FileSelectionMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.MainMenu);
        }
        
        public void OnCreateButtonClicked()
        {
            io.ApollonIOManager.Instance.CreateInput();
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.ConfigureMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.MainMenu);
        }

        public void OnExitButtonClicked()
        {
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.All);
            UnityEngine.Application.Quit();
        }

    } /* public class ApollonMainMenuController */

} /* } Labsim.apollon.frontend.menu */