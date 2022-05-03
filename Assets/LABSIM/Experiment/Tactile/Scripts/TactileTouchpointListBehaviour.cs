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

        [UnityEngine.SerializeField]
        private UnityEngine.GameObject TouchpointPrefab = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.BoxCollider ProjectionPlaneCollider = null;

        #region MonoBehaviour Impl 

        private void Start()
        {
            
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

        public void AddTouchpoint(UnityEngine.Vector3 hit_pos)
        {

            // finally clone tuned prefab at object origin
            var touchpoint 
                = UnityEngine.GameObject.Instantiate(
                    this.TouchpointPrefab,
                    this.transform.position,
                    this.transform.rotation,
                    this.transform
                );
        
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
            
            // active it 
            touchpoint.SetActive(true);
            
        } /* AddTouchpoint() */

    } /* class TactileResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */
