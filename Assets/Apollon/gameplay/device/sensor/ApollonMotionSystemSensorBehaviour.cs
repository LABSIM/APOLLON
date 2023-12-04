// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonMotionSystemSensorBehaviour
        : ApolloConcreteGameplayBehaviour<ApollonMotionSystemSensorBridge>
    {

        private bool m_bHasInitialized = false;

        void Awake()
        {

            // behaviour inactive by default & gameobject inactive
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        void OnEnable()
        {

                        
        } /* OnEnable()*/

        void OnDisable()
        {
            

        } /* OnDisable() */

    } /* public class ApollonMotionSystemSensorBehaviour */

} /* } Labsim.apollon.gameplay.device.command */
