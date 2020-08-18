
// avoid namespace pollution
namespace Labsim.apollon.frontend
{

    public abstract class ApollonAbstractFrontendBridge
    {

        // ctor

        public ApollonAbstractFrontendBridge()
        {
            this.WrapBehaviour();
        }

        // public properties

        private UnityEngine.MonoBehaviour m_behaviour = null;
        public UnityEngine.MonoBehaviour Behaviour
        {
            get
            {
                if(this.m_behaviour == null) 
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonAbstractFrontendBridge.Behaviour : property is still null, trying to re wrap behaviour from Unity."
                    );

                    this.m_behaviour = this.WrapBehaviour();
                }
                return this.m_behaviour;
            }
            private set
            {
                this.m_behaviour = value;
            }
        }

        private ApollonFrontendManager.FrontendIDType m_ID = ApollonFrontendManager.FrontendIDType.None;
        public ApollonFrontendManager.FrontendIDType ID
        {
            get
            {
                if (this.m_ID == ApollonFrontendManager.FrontendIDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractFrontendBridge.ID : property is uninitilized, trying to wrap ID."
                    );

                    this.m_ID = this.WrapID();
                }
                return this.m_ID;
            }
            private set
            {
                this.m_ID = value;
            }
        }

        // force overriding in childs

        protected abstract UnityEngine.MonoBehaviour WrapBehaviour();

        protected abstract ApollonFrontendManager.FrontendIDType WrapID();

        // Callback methods

        public virtual void onActivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onInactivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            /* nothing by default */
        }

    }  /* abstract ApollonAbstractFrontendBridge */

} /* } Labsim.apollon.frontend */