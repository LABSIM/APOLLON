using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileResponseAreaBehaviour
        : UnityEngine.MonoBehaviour
    {

        // static
        public static readonly uint s_touchpointMaxCount = 5;

        // members
        private bool m_bHasInitialized = false;

        // properties
        public TactileResponseAreaBridge Bridge { get; set; }
        public System.Collections.Generic.List<ITactileTouchpoint> TouchpointList { get; set; } = new System.Collections.Generic.List<ITactileTouchpoint>();

        // methods 
        public void AddTouchpoint(ITactileTouchpoint touchpoint)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBehaviour.AddTouchpoint() : call"
            );

            // finally, track current into list
            this.TouchpointList.Add(touchpoint);
            
        } /* AddTouchpoint() */

        public void ClearAllTouchpoint()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBehaviour.ClearAllTouchpoint() : call"
            );

            // apply for each element & clear refs
            this.TouchpointList.ForEach(touchpoint => UnityEngine.GameObject.Destroy(touchpoint.Reference));
            this.TouchpointList.Clear();

        } /* ClearAllTouchpoint() */

        public void ActivateAllTouchpoint()
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileResponseAreaBehaviour.ActivateAllTouchpoint() : call"
            );

            // apply for each element
            this.TouchpointList.ForEach(touchpoint => touchpoint.Reference.GetComponentInChildren<Leap.Unity.Interaction.InteractionSlider>().controlEnabled = true);

        } /* ActivateAllTouchpoint() */

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var Idle = this.gameObject.AddComponent<IdleController>();
            var SpatialCondition = this.gameObject.AddComponent<SpatialConditionController>();
            var TemporalCondition = this.gameObject.AddComponent<TemporalConditionController>();
            var SpatioTemporalCondition = this.gameObject.AddComponent<SpatioTemporalConditionController>();

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : state controller added as gameObject's component");

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Close() : begin");
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.Close() : end");

        } /* Close() */

        #region Controllers section

        internal sealed class IdleController
            : UnityEngine.MonoBehaviour
        {

            private TactileResponseAreaBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // do idling

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */

        } /* class IdleController */

        internal sealed class SpatioTemporalConditionController
            : UnityEngine.MonoBehaviour
        {

            private TactileResponseAreaBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // bind 
                this._parent.ProjectionPlaneProximityDetector.OnProximity.AddListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // unbind
                this._parent.ProjectionPlaneProximityDetector.OnProximity.RemoveListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnDisable() : end"
                );

            } /* OnDisable() */

            public void OnFingerTipProximity(UnityEngine.GameObject obj)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatioTemporalConditionController.OnFingerTipProximity(UnityEngine.GameObject) : call"
                );

                // Returns a point on the given collider that is closest to the specified location.
                // Note that in case the specified location is inside the collider, or exactly on the boundary of it, the input location is returned instead.
                // The collider can only be BoxCollider, SphereCollider, CapsuleCollider or a convex MeshCollider.
                UnityEngine.Vector3 
                    closestPoint 
                        = UnityEngine.Physics.ClosestPoint(
                            obj.transform.position, 
                            this._parent.ProjectionPlaneCollider, 
                            this._parent.ProjectionPlaneCollider.transform.position, 
                            this._parent.ProjectionPlaneCollider.transform.rotation
                        ),
                    hitPoint 
                        = UnityEngine.Vector3.zero;
                

                // check
                bool bShouldAdd = false;
                if(closestPoint != obj.transform.position) {

                    // extract dir & skip any backward interaction
                    var dir = closestPoint - obj.transform.position;
                    if(dir.z < 0.0f)
                    {
                        return;
                    } 

                    // get a raycast hit position & notify to add the touchpoint
                    var ray = new UnityEngine.Ray(obj.transform.position, dir);
                    var hasHit = this._parent.ProjectionPlaneCollider.Raycast(ray, out var hitInfo, float.MaxValue);
                    if(hasHit && (this._parent.TouchpointList.Count < s_touchpointMaxCount))
                    {
                        bShouldAdd = true;
                        hitPoint = hitInfo.point;
                    }

                }
                else
                {

                    // already a hit
                    bShouldAdd = true;
                    hitPoint = closestPoint;
                    
                } /* if() */ 

                if(bShouldAdd)
                {

                    // finally clone tuned prefab at object origin
                    var touchpoint_obj
                        = UnityEngine.GameObject.Instantiate(
                            this._parent.TouchpointPrefab,
                            this._parent.transform.position + this._parent.transform.TransformDirection(UnityEngine.Vector3.back * 0.001f),
                            this._parent.transform.rotation,
                            this._parent.TouchpointGroup.transform
                        );

                    // update index UI
                    touchpoint_obj.GetComponentInChildren<TMPro.TMP_Text>().text = (this._parent.TouchpointList.Count + 1).ToString();

                    // slide prefab instance to normalized hit position in bounds X = (value-min)/(max-min)
                    var slider = touchpoint_obj.GetComponentInChildren<Leap.Unity.Interaction.InteractionSlider>();
                    slider.defaultHorizontalValue 
                        = (
                            ((hitPoint.x - this._parent.transform.position.x) / this._parent.transform.lossyScale.x)
                            - slider.horizontalSlideLimits.x
                        ) 
                        / (slider.horizontalSlideLimits.y - slider.horizontalSlideLimits.x);
                    slider.defaultVerticalValue
                        = (
                            ((hitPoint.y - this._parent.transform.position.y) / this._parent.transform.lossyScale.y)
                            - slider.verticalSlideLimits.x
                        ) 
                        / (slider.verticalSlideLimits.y - slider.verticalSlideLimits.x);

                    // make it non interactive
                    slider.controlEnabled = false;
                    
                    // active gameobject
                    touchpoint_obj.SetActive(true);

                    // instantiate our tactile touchpoint from current properties
                    ITactileTouchpoint touchpoint 
                        = new TactileTouchpoint(
                            slider.defaultHorizontalValue,
                            slider.defaultVerticalValue,
                            apollon.ApollonHighResolutionTime.Now.ToString(),
                            UnityEngine.Time.time,
                            touchpoint_obj
                        );
                    this._parent.AddTouchpoint(touchpoint);

                } /* if() */
                
                // check if we should now make all touchpoint active
                if(this._parent.TouchpointList.Count == s_touchpointMaxCount)
                {

                    this._parent.ActivateAllTouchpoint();

                } /* if() */
            
            } /* OnFingerTipProximity() */

        } /* class SpatioTemporalConditionController */

        internal sealed class TemporalConditionController
            : UnityEngine.MonoBehaviour
        {

            private TactileResponseAreaBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // bind 
                this._parent.ProjectionPlaneProximityDetector.OnProximity.AddListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // unbind
                this._parent.ProjectionPlaneProximityDetector.OnProximity.RemoveListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnDisable() : end"
                );

            } /* OnDisable() */

            public void OnFingerTipProximity(UnityEngine.GameObject obj)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.TemporalConditionController.OnFingerTipProximity(UnityEngine.GameObject) : call"
                );

            } /* OnFingerTipProximity() */

        } /* class TemporalConditionController */

        internal sealed class SpatialConditionController
            : UnityEngine.MonoBehaviour
        {

            private TactileResponseAreaBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // bind 
                this._parent.ProjectionPlaneProximityDetector.OnProximity.AddListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnEnable() : end"
                );

            } /* OnEnable()*/

            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<TactileResponseAreaBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // unbind
                this._parent.ProjectionPlaneProximityDetector.OnProximity.RemoveListener(this.OnFingerTipProximity);

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnDisable() : end"
                );

            } /* OnDisable() */

            public void OnFingerTipProximity(UnityEngine.GameObject obj)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> TactileResponseAreaBehaviour.SpatialConditionController.OnFingerTipProximity(UnityEngine.GameObject) : call"
                );

            } /* OnFingerTipProximity() */

        } /* class SpatialConditionController */

        #endregion

        #region MonoBehaviour Impl 
        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject TouchpointGroup = null;

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject TouchpointPrefab = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.Collider ProjectionPlaneCollider = null;
        
        [UnityEngine.SerializeField]
        private Leap.Unity.ProximityDetector ProjectionPlaneProximityDetector = null;

        private void Start()
        {

            this.ProjectionPlaneCollider.gameObject.layer = UnityEngine.LayerMask.NameToLayer("HandProjectorLayer");

        } /* Start() */

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.OnEnable() : initialize required");

                // call
                this.Initialize();

            } /* if() */

        } /* OnEnable() */

        private void OnDisable() 
        {

            // skip if it hasn't been initialized 
            if (this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */