// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    public class CAVIARControlBehaviour 
        : apollon.gameplay.ApolloConcreteGameplayBehaviour<CAVIARControlBridge>
    {

        #region properties/members

        // controls
        public CAVIARControl Control { get; private set; } = null;

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
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : instantiating CAVIAR control");
                this.Control = new CAVIARControl();
            }

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnDisable() : call");
        
            // log & disable
            UnityEngine.Debug.Log("<color=Blue>Info: </color> CAVIARControlBehaviour.OnEnable() : inactivate subject controls only ");
            this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class CAVIARControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
