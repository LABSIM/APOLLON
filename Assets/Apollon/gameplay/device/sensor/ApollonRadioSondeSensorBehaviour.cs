
// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonRadioSondeSensorBehaviour 
        : UnityEngine.MonoBehaviour
    {
        
        [UnityEngine.SerializeField]
        public UnityEngine.GameObject HitPointTracker = null;
        
        [UnityEngine.SerializeField]
        public System.Collections.Generic.List<UnityEngine.MonoBehaviour> RadiosondeFrontend = new System.Collections.Generic.List<UnityEngine.MonoBehaviour>();

        #region properties/members

        [UnityEngine.SerializeField]
        protected float SensorHitPointDistance { private set; get; }
        
        [UnityEngine.SerializeField]
        protected UnityEngine.Vector3 SensorHitPointPosition { private set; get; }       
        
        [UnityEngine.SerializeField]
        protected UnityEngine.Quaternion SensorHitPointOrientation { private set; get; }
    
        [UnityEngine.SerializeField]
        private UnityEngine.LayerMask terrainMask;

        // Apollon bridge

        public ApollonRadioSondeSensorBridge Bridge { get; set; }

        #endregion

        private void Initialize()
        {

        } /* Initialize() */

        #region Unity Mono Behaviour implementation

        private void Start()
        {

        }

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */


        public void OnEnable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonRadioSondeSensorBehaviour.OnEnable() : call");
            this.enabled = true;
            this.gameObject.SetActive(true);

        } /* OnEnable() */

        private void OnDisable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonRadioSondeSensorBehaviour.OnDisable() : call");

           
        } /* OnDisable() */

        private void FixedUpdate()
        {

            UnityEngine.RaycastHit hit;
            if ( 
                UnityEngine.Physics.Raycast(
                    origin:
                        this.transform.position, 
                    direction:
                        this.transform.TransformDirection(UnityEngine.Vector3.down), 
                    maxDistance:
                        UnityEngine.Mathf.Infinity, 
                    layerMask:
                        this.terrainMask,
                    hitInfo:
                        out hit
                )
            ) 
            {

                // update values 
                this.SensorHitPointDistance     = hit.distance;
                this.SensorHitPointPosition     = hit.point;
                this.SensorHitPointOrientation  = UnityEngine.Quaternion.Euler(hit.normal);

                // update tracker transform
                if(this.HitPointTracker)
                {

                    this.HitPointTracker.transform.SetPositionAndRotation(
                        this.SensorHitPointPosition,
                        this.SensorHitPointOrientation
                    );

                } /* if() */

                foreach(var frontend in this.RadiosondeFrontend)
                {
                    (frontend.GetComponent<RadiosondeFrontend>()).SetValue(
                        //this.transform.position.y - hit.point.y
                        hit.distance
                    );
                }

                // dispatch event
                this.Bridge.Dispatcher.RaiseHitChangedEvent(
                    this.SensorHitPointDistance,
                    this.SensorHitPointPosition,
                    this.SensorHitPointOrientation
                );
            
            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning("<color=Orange>Warning: </color> ApollonRadioSondeSensorBehaviour.OnFixedUpdate() : failed to get a raycast hit...");

            } /* if() */

        } /* FixedUpdate() */

        #endregion

    } /* public class ApollonActiveSeatEntityBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */
