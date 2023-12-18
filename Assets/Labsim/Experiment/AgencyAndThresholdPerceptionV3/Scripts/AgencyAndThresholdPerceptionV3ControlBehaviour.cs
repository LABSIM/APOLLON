// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV3
{

    public class AgencyAndThresholdPerceptionV3ControlBehaviour 
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<AgencyAndThresholdPerceptionV3ControlBridge>
    {

        #region properties/members

        // controls
        public AgencyAndThresholdPerceptionV3Control Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new AgencyAndThresholdPerceptionV3Control();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV3ControlBehaviour.OnDisable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class AgencyAndThresholdPerceptionV3ControlBehaviour */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV3 */
