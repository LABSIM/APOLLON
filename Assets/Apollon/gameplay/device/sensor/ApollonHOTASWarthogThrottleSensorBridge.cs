// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBridge 
        : ApollonAbstractGameplayBridge
    {

        //ctor
        public ApollonHOTASWarthogThrottleSensorBridge()
            : base()
        { }
        
        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonHOTASWarthogThrottleSensorBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonHOTASWarthogThrottleSensorBridge.WrapBehaviour() : could not find object of type ApollonHOTASWarthogThrottleSensorBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

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