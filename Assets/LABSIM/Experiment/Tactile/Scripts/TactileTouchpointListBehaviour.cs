using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

// avoid compilation during player build
#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(TactileTouchpointListBehaviour))]
    public class TactileTouchpointListBehaviourEditor 
        : UnityEditor.Editor 
    {
    
        public override void OnInspectorGUI() 
        {

            // draw default 
            this.DrawDefaultInspector();

        } /* OnInspectorGUI() */

    } /* class TactileTouchpointListBehaviourEditor */

#endif

    public class TactileTouchpointListBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        public static readonly uint s_touchpointMaxCount = 5;

        // properties 
        
        // public uint TouchpointCount { get; private set; }

        #region MonoBehaviour Impl 

        [UnityEngine.SerializeField, UnityEngine.HideInInspector]
        public System.Collections.Generic.List<UnityEngine.GameObject> Touchpoints { get; private set; } = new System.Collections.Generic.List<UnityEngine.GameObject>();

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject TouchpointPrefab = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.Collider ProjectionPlaneCollider = null;

        private void Start()
        {

            this.ProjectionPlaneCollider.gameObject.layer = UnityEngine.LayerMask.NameToLayer("HandProjectorLayer");

        } /* Start() */

        private void Update()
        {
            
        } /* Update() */

        #endregion

        #region IMGUI Impl

        private void OnGUI() 
        {
            
        } /* OnGUI() */

        #endregion

        #region UltraLeap Events

        public void OnFingerTipProximity(UnityEngine.GameObject obj)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileTouchpointListBehaviour.OnFingerTipProximity(UnityEngine.GameObject) : call"
            );

            // Returns a point on the given collider that is closest to the specified location.
            // Note that in case the specified location is inside the collider, or exactly on the boundary of it, the input location is returned instead.
            // The collider can only be BoxCollider, SphereCollider, CapsuleCollider or a convex MeshCollider.
            var closestPoint 
                = UnityEngine.Physics.ClosestPoint(
                    obj.transform.position, 
                    this.ProjectionPlaneCollider, 
                    this.ProjectionPlaneCollider.transform.position, 
                    this.ProjectionPlaneCollider.transform.rotation
                );

            // check
            if(closestPoint != obj.transform.position) {

                // extract dir & skip any backward interaction
                var dir = closestPoint - obj.transform.position;
                if(dir.z < 0.0f)
                {
                    return;
                } 

                // get a raycast hit position & add touchpoint
                var ray = new UnityEngine.Ray(obj.transform.position, dir);
                var hasHit = this.ProjectionPlaneCollider.Raycast(ray, out var hitInfo, float.MaxValue);
                if(hasHit && (this.Touchpoints.Count < s_touchpointMaxCount))
                {
                    this.AddTouchpoint(hitInfo.point, this.Touchpoints.Count);
                }

            }
            else
            {
                // already a hit
                this.AddTouchpoint(closestPoint, this.Touchpoints.Count);
            }
            
            
            // check if we should now make all touchpoint active
            if(this.Touchpoints.Count == s_touchpointMaxCount)
            {
                this.ActivateAllTouchpoint();
            }
        
        } /* OnFingerTipProximity() */

        #endregion

        private void AddTouchpoint(UnityEngine.Vector3 hit_pos, int index)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileTouchpointListBehaviour.AddTouchpoint(UnityEngine.Vector3) : call"
            );

            // finally clone tuned prefab at object origin
            var touchpoint 
                = UnityEngine.GameObject.Instantiate(
                    this.TouchpointPrefab,
                    this.transform.position + this.transform.TransformDirection(UnityEngine.Vector3.back * 0.001f),
                    this.transform.rotation,
                    this.transform
                );

            // update index UI
            touchpoint.GetComponentInChildren<TMPro.TMP_Text>().text = (index++).ToString();

            // slide prefab instance to normalized hit position in bounds X = (value-min)/(max-min)
            var slider = touchpoint.GetComponentInChildren<Leap.Unity.Interaction.InteractionSlider>();
            slider.defaultHorizontalValue 
                = (
                    ((hit_pos.x - this.transform.position.x) / this.transform.lossyScale.x)
                    - slider.horizontalSlideLimits.x
                ) 
                / (slider.horizontalSlideLimits.y - slider.horizontalSlideLimits.x);
            slider.defaultVerticalValue
                = (
                    ((hit_pos.y - this.transform.position.y) / this.transform.lossyScale.y)
                    - slider.verticalSlideLimits.x
                ) 
                / (slider.verticalSlideLimits.y - slider.verticalSlideLimits.x);

            // make it non interactive
            slider.controlEnabled = false;
            
            // active gameobject
            touchpoint.SetActive(true);

            // finally, track current into list
            this.Touchpoints.Add(touchpoint);
            
        } /* AddTouchpoint() */

        public void ClearAllTouchpoint()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileTouchpointListBehaviour.ClearAllTouchpoint() : call"
            );

            // apply for each element & clear refs
            this.Touchpoints.ForEach(touchpoint => UnityEngine.GameObject.Destroy(touchpoint));
            this.Touchpoints.Clear();

        } /* ClearAllTouchpoint() */

        public void ActivateAllTouchpoint()
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileTouchpointListBehaviour.ActivateAllTouchpoint() : call"
            );

            // apply for each element 
            this.Touchpoints.ForEach(touchpoint => touchpoint.GetComponentInChildren<Leap.Unity.Interaction.InteractionSlider>().controlEnabled = true);

        } /* ActivateAllTouchpoint() */

    } /* class TactileResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */
