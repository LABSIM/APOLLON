// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionV3ControlBehaviour 
        : ApolloConcreteGameplayBehaviour<ApollonAgencyAndThresholdPerceptionV3ControlBridge>
    {

        #region properties/members

        // controls
        public ApollonAgencyAndThresholdPerceptionV3Control Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new ApollonAgencyAndThresholdPerceptionV3Control();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionV3ControlBehaviour.OnDisable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonAgencyAndThresholdPerceptionV3ControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
