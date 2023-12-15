// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonRealRobosoftEntityBridge 
        : ApollonGameplayBridge<ApollonRealRobosoftEntityBridge>
    {

        // ctor
        public ApollonRealRobosoftEntityBridge()
            : base()
        { }
        
        public ApollonRealRobosoftEntityBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonRealRobosoftEntityBehaviour;

        #region Bridge abstract implementation 
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonRealRobosoftEntityBehaviour>(
                "ApollonRealRobosoftEntityBridge",
                "ApollonRealRobosoftEntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonRealRobosoftEntityBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.RealRobosoftEntity;
        }

        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate
                this.Behaviour.gameObject.SetActive(true);
               
            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // inactivate
                this.Behaviour.gameObject.SetActive(false);

            } /* if() */

        } /* SetActive() */
        
        #endregion

    }  /* class ApollonRealRobosoftEntityBridge */

} /* } Labsim.apollon.gameplay.entity */