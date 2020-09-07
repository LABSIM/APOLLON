
// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractHandle
        : System.IDisposable
    {

        // Handle ID field
        protected ApollonBackendManager.HandleIDType m_handleID = ApollonBackendManager.HandleIDType.None;
        public ApollonBackendManager.HandleIDType ID
        {
            get
            {
                return this.m_handleID;
            }
        }

        // ctor
        public ApollonAbstractHandle()
        { }
        
        // Dispose pattern
        public void Dispose()
        {

            this.Dispose(true);
            System.GC.SuppressFinalize(this);

        } /* Dispose() */

        protected abstract void Dispose(bool bDisposing = true);

        #region event handling 

        public virtual void onHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // validate
                ApollonBackendManager.Instance.ValidateHandle(this.ID, this);

            } /* if() */

        } /* onHandleActivationRequested() */

        // unregistration
        public virtual void onHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // unplug - comment: it let the system doing it's job
                //this.Dispose();

                // unvalidate
                ApollonBackendManager.Instance.InvalidateHandle(this.ID, this);

            } /* if() */

        } /* onHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractInteroperabilityHandle */

} /* } namespace */