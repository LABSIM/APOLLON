using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileSpatialResponseAreaBehaviour 
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        private TactileTouchpointListBehaviour TouchpointListBehaviour = null;

        private void Awake()
        {

        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        #region UltraLeap Events

        public void OnContactBegin()
        {
            this.TouchpointListBehaviour.AddTouchpoint();
        }

        #endregion

    } /* class TactileSpatialResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */
