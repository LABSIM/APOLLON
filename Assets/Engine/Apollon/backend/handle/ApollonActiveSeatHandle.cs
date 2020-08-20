// using directives 

// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{

    public class ApollonActiveSeatHandle
        : ApollonAbstractCANHandle
    {

        // ctor
        public ApollonActiveSeatHandle()
            : base()
        {
            this.m_handleID = ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle;
        }

    } /* class ApollonActiveSeatHandle */

} /* namespace Labsim.apollon.backend */
