// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonSimpleFrameGUIBridge : ApollonAbstractFrontendBridge
    {

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            foreach (UnityEngine.MonoBehaviour behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<ApollonSimpleFrameGUIBehaviour>())
            {
                if (behaviour.transform.name == ApollonEngine.GetEnumDescription(ApollonFrontendManager.FrontendIDType.SimpleFrameGUI))
                {
                    return behaviour;
                }
            }

            // log
            UnityEngine.Debug.LogWarning(
                "<color=Orange>Warning: </color> ApollonSimpleFrameGUIBridge.WrapBehaviour() : could not find object of type behaviour.ApollonSimpleFrameGUIBehaviour from Unity."
            );

            return null;

        } /* WrapBehaviour() */

        protected override ApollonFrontendManager.FrontendIDType WrapID()
        {
            return ApollonFrontendManager.FrontendIDType.SimpleFrameGUI;
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
                case ApollonFrontendManager.FrontendIDType.SimpleFrameGUI:
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
                case ApollonFrontendManager.FrontendIDType.SimpleFrameGUI:
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

    }  /* class ApollonSimpleFrameGUIBridge */

} /* } Labsim.apollon.frontend.gui */