// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerception
{

    public class AgencyAndThresholdPerceptionControlBehaviour
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<AgencyAndThresholdPerceptionControlBridge>
    {

        #region properties/members

        // controls
        public AgencyAndThresholdPerceptionControl Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new AgencyAndThresholdPerceptionControl();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionControlBehaviour.OnEnable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class AgencyAndThresholdPerceptionControlBehaviour */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
