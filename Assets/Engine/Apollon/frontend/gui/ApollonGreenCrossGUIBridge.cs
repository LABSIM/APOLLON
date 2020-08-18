// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonGreenCrossGUIBridge : ApollonAbstractFrontendBridge
    {

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            foreach (UnityEngine.MonoBehaviour behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<ApollonGreenCrossGUIBehaviour>())
            {
                if (behaviour.transform.name == ApollonEngine.GetEnumDescription(ApollonFrontendManager.FrontendIDType.GreenCrossGUI))
                {
                    return behaviour;
                }
            }

            // log
            UnityEngine.Debug.LogWarning(
                "<color=Orange>Warning: </color> ApollonGreenCrossGUIBridge.WrapBehaviour() : could not find object of type behaviour.ApollonGreenCrossGUIBehaviour from Unity."
            );

            return null;

        } /* WrapBehaviour() */

        protected override ApollonFrontendManager.FrontendIDType WrapID()
        {
            return ApollonFrontendManager.FrontendIDType.GreenCrossGUI;
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
                case ApollonFrontendManager.FrontendIDType.GreenCrossGUI:
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
                case ApollonFrontendManager.FrontendIDType.GreenCrossGUI:
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

    }  /* class ApollonGreenCrossGUIBridge */

} /* } Labsim.apollon.frontend.gui */