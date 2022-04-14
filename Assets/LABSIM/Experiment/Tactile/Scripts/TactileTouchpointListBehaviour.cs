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

        public void AddTouchpoint()
        {
            
            var pos 
                = Leap.Unity.UnityVectorExtension.ToVector3(
                    Leap.Unity.Hands.Get(Leap.Unity.Chirality.Right).Finger(1).TipPosition
                );

            var touchpoint 
                = UnityEngine.GameObject.Instantiate(
                    this.TouchpointPrefab,
                    pos,
                    UnityEngine.Quaternion.identity,
                    this.transform
                );

        } 

    } /* class TactileResponseAreaBehaviour */

} /* } Labsim.experiment.tactile */
