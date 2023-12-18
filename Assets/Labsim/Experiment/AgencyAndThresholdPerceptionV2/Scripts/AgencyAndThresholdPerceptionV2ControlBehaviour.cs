// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerceptionV2
{

    public class AgencyAndThresholdPerceptionV2ControlBehaviour
        : apollon.gameplay.ApollonConcreteGameplayBehaviour<AgencyAndThresholdPerceptionV2ControlBridge>
    {

        #region properties/members

        // controls
        public AgencyAndThresholdPerceptionV2Control Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake() 
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : instantiating AgencyAndThresholdPerception control");
                this.Control = new AgencyAndThresholdPerceptionV2Control();
            }

            // enable supervisor only
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBehaviour.Awake() : activate supervisor controls");
            this.Control.Supervisor.Enable();
        }

        public void OnEnable()
        {

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> AgencyAndThresholdPerceptionV2ControlBehaviour.OnDisable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class AgencyAndThresholdPerceptionV2ControlBehaviour */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV2 */
