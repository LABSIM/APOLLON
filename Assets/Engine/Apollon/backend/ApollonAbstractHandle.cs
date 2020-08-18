
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

        protected virtual void Dispose(bool bDisposing = true)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractHandle.Dispose(" + bDisposing + ") : called."
            );

        } /* Dispose(bool) */

        #region event handling 

        public virtual void onHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // register
                ApollonBackendManager.Instance.RegisterHandle(this.ID, this);

            } /* if() */

        } /* onHandleActivationRequested() */

        // unregistration
        public virtual void onHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // unplug it
                this.Dispose();

                // unregister
                ApollonBackendManager.Instance.UnregisterHandle(this.ID, this);

            } /* if() */

        } /* onHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractInteroperabilityHandle */

} /* } namespace */