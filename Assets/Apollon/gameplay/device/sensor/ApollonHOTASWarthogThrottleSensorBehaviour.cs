// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBehaviour 
        : ApolloConcreteGameplayBehaviour<ApollonHOTASWarthogThrottleSensorBridge>
    {

        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("")]
        [UnityEngine.Range(-180.0f, 180.0f)]
        private float m_axisZ_min_angle = -25.00f;        
        
        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("")]
        [UnityEngine.Range(-180.0f, 180.0f)]
        private float m_axisZ_max_angle = 25.00f;

        #region Unity Mono Behaviour implementation

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Control event

        public void OnAxisZValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // update tracked attitude 
            this.gameObject.transform.SetPositionAndRotation(
                this.gameObject.transform.position,
                UnityEngine.Quaternion.AngleAxis(
                    context.ReadValue<float>() * (this.m_axisZ_max_angle - this.m_axisZ_min_angle) / 2.0f,
                    UnityEngine.Vector3.right
                )
            );

        } /* OnAxisZValueChanged() */

        #endregion

    } /* public class ApollonHOTASWarthogThrottleSensorBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */