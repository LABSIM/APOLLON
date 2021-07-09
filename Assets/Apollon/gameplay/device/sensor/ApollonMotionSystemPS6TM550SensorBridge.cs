
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonMotionSystemPS6TM550SensorBridge 
         : ApollonAbstractGameplayBridge 
    {

        //ctor
        public ApollonMotionSystemPS6TM550SensorBridge()
            : base()
        { }

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonMotionSystemPS6TM550SensorBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonMotionSystemPS6TM550SensorBridge.WrapBehaviour() : could not find object of type behaviour.ApollonRealRobosoftEntityBehaviour from Unity."
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
            return ApollonGameplayManager.GameplayIDType.MotionSystemPS6TM550Sensor;
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

    }  /* class ApollonMotionSystemPS6TM550SensorBridge */

} /* } Labsim.apollon.gameplay.device.command */