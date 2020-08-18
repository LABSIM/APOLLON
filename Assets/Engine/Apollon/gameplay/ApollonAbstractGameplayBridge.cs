
// avoid namespace pollution
namespace Labsim.apollon.gameplay
{

    public abstract class ApollonAbstractGameplayBridge
    {

        // ctor
        public ApollonAbstractGameplayBridge()
        {
            this.WrapID();
            this.WrapBehaviour();
        }

        // puublic properties
        private UnityEngine.MonoBehaviour m_behaviour = null;
        public UnityEngine.MonoBehaviour Behaviour
        {
            get
            {
                if (this.m_behaviour == null)
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warning: </color> ApollonAbstractGameplayBridge.Behaviour() : property is still null, trying to re wrap behaviour from Unity."
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

        private ApollonGameplayManager.GameplayIDType m_ID = ApollonGameplayManager.GameplayIDType.None;
        public ApollonGameplayManager.GameplayIDType ID
        {
            get
            {
                if (this.m_ID == ApollonGameplayManager.GameplayIDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractGameplayBridge.ID : property is uninitilized, trying to wrap ID."
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

        protected abstract ApollonGameplayManager.GameplayIDType WrapID();

        protected abstract void SetActive(bool value);

        // Callback methods

        public virtual void onActivationRequested(object sender, ApollonGameplayManager.GameplayEventArgs arg)
        {
           
            // check
            if (this.ID == arg.ID || ApollonGameplayManager.GameplayIDType.All == arg.ID)
            {
                this.SetActive(true);
            }
            // inhibit but strange
            else if (ApollonGameplayManager.GameplayIDType.None == arg.ID)
            {
                this.SetActive(false);
            }

        } /* onActivationRequested() */

        public virtual void onInactivationRequested(object sender, ApollonGameplayManager.GameplayEventArgs arg)
        {

            // check
            if (this.ID == arg.ID || ApollonGameplayManager.GameplayIDType.All == arg.ID)
            {
                this.SetActive(false);
            }
            // inhibit but strange
            else if (ApollonGameplayManager.GameplayIDType.None == arg.ID)
            {
                this.SetActive(true);
            }

        } /* onInactivationRequested() */

    }  /* abstract ApollonAbstractGameplayBridge */

} /* } Labsim.apollon.gameplay */