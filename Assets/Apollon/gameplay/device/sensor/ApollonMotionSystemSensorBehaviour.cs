// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonMotionSystemSensorBehaviour
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        public ApollonMotionSystemSensorBridge Bridge { get; set; }

        private bool m_bHasInitialized = false;

        #endregion

        #region MonoBehaviour implementation

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

        #endregion

    } /* public class ApollonMotionSystemSensorBehaviour */

} /* } Labsim.apollon.gameplay.device.command */
