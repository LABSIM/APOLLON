// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonSpeedSelectionGUIBridge : ApollonAbstractFrontendBridge
    {

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            foreach (UnityEngine.MonoBehaviour behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<ApollonSpeedSelectionGUIBehaviour>())
            {
                if (behaviour.transform.name == ApollonEngine.GetEnumDescription(ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI))
                {
                    return behaviour;
                }
            }

            // log
            UnityEngine.Debug.LogWarning(
                "<color=Orange>Warning: </color> ApollonSpeedSelectionGUIBridge.WrapBehaviour() : could not find object of type ApollonSpeedSelectionGUIBehaviour from Unity."
            );

            return null;

        } /* WrapBehaviour() */

        protected override ApollonFrontendManager.FrontendIDType WrapID()
        {
            return ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI;
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
                case ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI:
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
                case ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI:
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

    }  /* class ApollonSpeedSelectionGUIBridge */

} /* } Labsim.apollon.frontend.gui */