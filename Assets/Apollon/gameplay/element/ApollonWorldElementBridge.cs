// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonWorldElementBridge
        : ApollonGameplayBridge<ApollonWorldElementBridge>
    {

        //ctor
        public ApollonWorldElementBridge()
            : base()
        { }

        public ApollonWorldElementBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonWorldElementBehaviour;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonWorldElementBehaviour>(
                "ApollonWorldElementBridge",
                "ApollonWorldElementBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonWorldElementBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.WorldElement;
        }

        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.enabled) { return; }

                // activate
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        
        
        #endregion

    }  /* class ApollonWorldElementBridge */

} /* } Labsim.apollon.gameplay.element */