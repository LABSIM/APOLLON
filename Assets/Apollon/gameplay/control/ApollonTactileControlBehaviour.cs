// avoid namespace pollution
namespace Labsim.apollon.gameplay.control
{

    public class ApollonTactileControlBehaviour 
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        // bridge
        public ApollonTactileControlBridge Bridge { get; set; } = null;

        // controls
        public ApollonTactileControl Control { get; private set; } = null;

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
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonTactileControlBehaviour.OnEnable() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonTactileControlBehaviour.OnEnable() : instantiating Tactile control");
                this.Control = new ApollonTactileControl();
            }

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonTactileControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonTactileControlBehaviour.OnDisable() : call");
        
            // log & disable
            // UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonTactileControlBehaviour.OnEnable() : inactivate subject controls only ");
            // this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class ApollonTactileControlBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
