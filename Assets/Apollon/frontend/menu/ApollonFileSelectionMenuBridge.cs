
// avoid namespace pollution
namespace Labsim.apollon.frontend.menu
{

    public class ApollonFileSelectionMenuBridge : ApollonAbstractFrontendBridge
    {

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            foreach (UnityEngine.MonoBehaviour behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<ApollonFileSelectionMenuBehaviour>())
            {
                if (behaviour.transform.name == ApollonEngine.GetEnumDescription(ApollonFrontendManager.FrontendIDType.FileSelectionMenu))
                {
                    return behaviour;
                }
            }

            // log
            UnityEngine.Debug.LogWarning(
                "<color=Orange>Warning: </color> ApollonFileSelectionMenuBridge.WrapBehaviour() : could not find object of type ApollonFileSelectionMenuBehaviour from Unity."
            );

            return null;

        } /* WrapBehaviour() */

        protected override ApollonFrontendManager.FrontendIDType WrapID()
        {
            return ApollonFrontendManager.FrontendIDType.FileSelectionMenu;
        }

        public override void onActivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            switch (arg.ID)
            {
                case ApollonFrontendManager.FrontendIDType.None:
                {
                    if (this.Behaviour != null)
                    {
                        this.Behaviour.gameObject.SetActive(false);
                    }
                    else
                    {
                        // put in a queue of corroutines
                    }
                    break;
                }
                case ApollonFrontendManager.FrontendIDType.FileSelectionMenu:
                case ApollonFrontendManager.FrontendIDType.All:
                {
                    if (this.Behaviour != null)
                    {
                        this.Behaviour.gameObject.SetActive(true);
                    }
                    else
                    {
                        // put in a queue of corroutines
                    }
                    break;
                }
                default:
                    break;
            }

        } /* onActivationRequested() */

        public override void onInactivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            switch (arg.ID)
            {
                case ApollonFrontendManager.FrontendIDType.None:
                {
                    if (this.Behaviour != null)
                    {
                        this.Behaviour.gameObject.SetActive(true);
                    }
                    else
                    {
                        // put in a queue of corroutines
                    }
                    break;
                }
                case ApollonFrontendManager.FrontendIDType.FileSelectionMenu:
                case ApollonFrontendManager.FrontendIDType.All:
                {
                    if (this.Behaviour != null)
                    {
                        this.Behaviour.gameObject.SetActive(false);
                    }
                    else
                    {
                        // put in a queue of corroutines
                    }
                    break;
                }
                default:
                    break;
            }

        } /* onInactivationRequested() */

    }  /* class ApollonFileMenuBridge */

} /* } Labsim.apollon.frontend.menu */
