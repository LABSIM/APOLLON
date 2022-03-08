// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV2ControlBehaviour 
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        // bridge
        public ApollonAgencyAndThresholdPerceptionV2ControlBridge Bridge { get; set; } = null;

        // controls
        public ApollonAgencyAndThresholdPerceptionV2Control Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new ApollonAgencyAndThresholdPerceptionV2Control();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV2ControlBehaviour.OnDisable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonAgencyAndThresholdPerceptionV2ControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
