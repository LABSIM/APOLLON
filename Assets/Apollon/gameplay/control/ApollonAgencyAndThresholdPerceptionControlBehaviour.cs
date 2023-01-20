// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonAgencyAndThresholdPerceptionControlBehaviour
        : ApolloConcreteGameplayBehaviour<ApollonAgencyAndThresholdPerceptionControlBridge>
    {

        #region properties/members

        // controls
        public ApollonAgencyAndThresholdPerceptionControl Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new ApollonAgencyAndThresholdPerceptionControl();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionControlBehaviour.OnEnable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonAgencyAndThresholdPerceptionControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
