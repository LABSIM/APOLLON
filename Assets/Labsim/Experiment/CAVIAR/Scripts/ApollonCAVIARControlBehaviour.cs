// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonCAVIARControlBehaviour 
        : ApolloConcreteGameplayBehaviour<ApollonCAVIARControlBridge>
    {

        #region properties/members

        // controls
        public ApollonCAVIARControl Control { get; private set; } = null;

        #endregion

        #region Unity Mono Behaviour implementation

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        public void OnEnable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIARControlBehaviour.OnEnable() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIARControlBehaviour.OnEnable() : instantiating CAVIAR control");
                this.Control = new ApollonCAVIARControl();
            }

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIARControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIARControlBehaviour.OnDisable() : call");
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonCAVIARControlBehaviour.OnEnable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonCAVIARControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
