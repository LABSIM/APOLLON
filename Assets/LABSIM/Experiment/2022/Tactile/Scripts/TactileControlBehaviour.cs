// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileControlBehaviour 
        : UnityEngine.MonoBehaviour
    {

        #region properties/members

        // bridge
        public TactileControlBridge Bridge { get; set; } = null;

        // controls
        public TactileControl Control { get; private set; } = null;

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
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileControlBehaviour.OnEnable() : call");

            // instantiate controls map
            if(this.Control == null)
            {
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileControlBehaviour.OnEnable() : instantiating Tactile control");
                this.Control = new TactileControl();
            }

            // enable it
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileControlBehaviour.OnEnable() : activate all controls");
            this.Control.Enable();

        } /* OnEnable() */

        private void OnDisable()
        {

            //log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileControlBehaviour.OnDisable() : call");
        
            // log & disable
            // UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileControlBehaviour.OnEnable() : inactivate subject controls only ");
            // this.Control.Subject.Disable();
           
        } /* OnDisable() */

        #endregion

    } /* public class TactileControlBehaviour */

} /* } Labsim.experiment.tactile */
