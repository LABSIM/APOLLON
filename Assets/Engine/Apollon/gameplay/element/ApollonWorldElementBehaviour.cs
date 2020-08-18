
// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonWorldElementBehaviour : UnityEngine.MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

    } /* public class ApollonWorldElementBehaviour */

} /* } Labsim.apollon.ui.element.behaviour */