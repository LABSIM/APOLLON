// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBridge 
        : ApollonGameplayBridge<ApollonHOTASWarthogThrottleSensorBridge>
    {

        //ctor
        public ApollonHOTASWarthogThrottleSensorBridge()
            : base()
        { }

        public ApollonHOTASWarthogThrottleSensorBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour;
        
        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonHOTASWarthogThrottleSensorBehaviour>(
                "ApollonHOTASWarthogThrottleSensorBridge",
                "ApollonHOTASWarthogThrottleSensorBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonHOTASWarthogThrottleSensorBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.HOTASWarthogthrottleSensor;
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

    }  /* public class ApollonHOTASWarthogThrottleSensorBridge */

} /* } Labsim.apollon.gameplay.device.sensor */