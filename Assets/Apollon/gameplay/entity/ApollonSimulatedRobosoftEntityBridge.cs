// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonSimulatedRobosoftEntityBridge
        : ApollonGameplayBridge<ApollonSimulatedRobosoftEntityBridge>
    {

        // ctor
        public ApollonSimulatedRobosoftEntityBridge()
            : base()
        { }
        
        public ApollonSimulatedRobosoftEntityBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonSimulatedRobosoftEntityBehaviour;

        #region Bridge abstract implementation 
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonSimulatedRobosoftEntityBehaviour>(
                "ApollonSimulatedRobosoftEntityBridge",
                "ApollonSimulatedRobosoftEntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonSimulatedRobosoftEntityBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity;
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

    }  /* class ApollonSimulatedRobosoftEntityBridge */

} /* } Labsim.apollon.gameplay.entity */