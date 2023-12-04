
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonMotionSystemSensorBridge 
        : ApollonGameplayBridge<ApollonMotionSystemSensorBridge>
    {

        //ctor
        public ApollonMotionSystemSensorBridge()
            : base()
        { }

        public ApollonMotionSystemSensorBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonMotionSystemSensorBehaviour;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonMotionSystemSensorBehaviour>(
                "ApollonMotionSystemSensorBridge",
                "ApollonMotionSystemSensorBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonMotionSystemSensorBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.MotionSystemSensor;
        }

        protected override async void SetActive(bool value)
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

    }  /* class ApollonMotionSystemSensorBridge */

} /* } Labsim.apollon.gameplay.device.command */