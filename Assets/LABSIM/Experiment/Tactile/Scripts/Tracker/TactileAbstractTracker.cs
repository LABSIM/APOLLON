// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public abstract class TactileAbstractTracker
    {

        // ctor
        public TactileAbstractTracker()
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
                        "<color=Orange>Warning: </color> TactileAbstractTracker.Behaviour() : property is still null, trying to re wrap behaviour from Unity."
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

        private TactileTrackerManager.IDType m_ID = TactileTrackerManager.IDType.None;
        public TactileTrackerManager.IDType ID
        {
            get
            {
                if (this.m_ID == TactileTrackerManager.IDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> TactileAbstractTracker.ID : property is uninitilized, trying to wrap ID."
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

        protected abstract TactileTrackerManager.IDType WrapID();

        protected abstract void SetActive(bool value);

        // Callback methods

        public virtual void onActivationRequested(object sender, TactileTrackerManager.EventArgs arg)
        {
           
            // check
            if (this.ID == arg.ID || TactileTrackerManager.IDType.All == arg.ID)
            {
                this.SetActive(true);
            }
            // inhibit but strange
            else if (TactileTrackerManager.IDType.None == arg.ID)
            {
                this.SetActive(false);
            }

        } /* onActivationRequested() */

        public virtual void onInactivationRequested(object sender, TactileTrackerManager.EventArgs arg)
        {

            // check
            if (this.ID == arg.ID || TactileTrackerManager.IDType.All == arg.ID)
            {
                this.SetActive(false);
            }
            // inhibit but strange
            else if (TactileTrackerManager.IDType.None == arg.ID)
            {
                this.SetActive(true);
            }

        } /* onInactivationRequested() */

    }  /* abstract TactileAbstractTracker */

} /* } Labsim.experiment.tactile */