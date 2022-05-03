using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileSpatialResponseAreaBehaviour 
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        private TactileTouchpointListBehaviour TouchpointListBehaviour = null;

        [UnityEngine.SerializeField]
        private UnityEngine.BoxCollider ProjectionPlaneCollider = null;

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

        public void OnFingerTipProximity(UnityEngine.GameObject obj)
        {

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
            
            // extract dir & skip any backward interaction
            var dir = (closestPoint - obj.transform.position).normalized;
            if(dir.z < 0.0f)
            {
                return;
            } 

            // get a raycast hit position & add touchpoint
            var ray = new UnityEngine.Ray(obj.transform.position, dir);
            var hasHit = this.ProjectionPlaneCollider.Raycast(ray, out var hitInfo, float.MaxValue);
            if(hasHit)
            {
                this.TouchpointListBehaviour.AddTouchpoint(hitInfo.point);
            }
        
        } /* OnFingerTipProximity */

        #endregion

    } /* class TactileSpatialResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */
