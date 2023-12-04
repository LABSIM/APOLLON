
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonRadioSondeSensorBridge 
        : ApollonGameplayBridge<ApollonRadioSondeSensorBridge>
    {

        //ctor
        public ApollonRadioSondeSensorBridge()
            : base()
        { }

        public ApollonRadioSondeSensorBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonRadioSondeSensorBehaviour;

        public ApollonRadioSondeSensorDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonRadioSondeSensorDispatcher;
        
        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonRadioSondeSensorBehaviour>(
                "ApollonRadioSondeSensorBridge",
                "ApollonRadioSondeSensorBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonRadioSondeSensorDispatcher>(
                "ApollonRadioSondeSensorBridge",
                "ApollonRadioSondeSensorDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.RadioSondeSensor;
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

        #region Event delegate


        #endregion

    }  /* class ApollonRadioSondeSensorBridge */

} /* } Labsim.apollon.gameplay.device.sensor */
