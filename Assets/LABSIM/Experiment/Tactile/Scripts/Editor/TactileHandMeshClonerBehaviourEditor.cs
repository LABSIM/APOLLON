
// avoid namespace pollution
namespace Labsim.experiment.tactile
{

// avoid compilation during player building
#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(TactileHandMeshClonerBehaviour))]
    public sealed class TactileHandMeshClonerBehaviourEditor 
        : UnityEditor.Editor 
    {
    
        public override void OnInspectorGUI() 
        {

            // draw default 
            this.DrawDefaultInspector();

            // then add a button 
            var current_target = this.target as TactileHandMeshClonerBehaviour;
            if(UnityEngine.GUILayout.Button("Snapshot"))
            {
                (
                    TactileManager.Instance.getBridge(TactileManager.IDType.TactileHandMeshCloner) 
                    as TactileHandMeshClonerBridge
                ).Dispatcher.RaiseButtonClonePressed();
            }

        } /* OnInspectorGUI() */

    } /* class TactileHandMeshClonerBehaviourEditor */

#endif
    
} /* } Labsim.experiment.tactile */
