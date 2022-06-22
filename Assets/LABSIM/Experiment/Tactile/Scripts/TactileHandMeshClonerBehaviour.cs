
// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileHandMeshClonerBehaviour 
        : UnityEngine.MonoBehaviour
    {

        // bridge
        private bool m_bHasInitialized = false;
        
        // bridge
        public TactileHandMeshClonerBridge Bridge { get; set; }

        // property
        private UnityEngine.GameObject SkinnedMeshSnapshot { set; get; } = null;
        
        #region Methods 

        // CloneHandMesh 
        public void CloneHandMesh()
        {
            
            if(UnityEngine.Application.isPlaying)
            {

                if(this.SkinnedMeshSnapshot == null) 
                {  

                    // intantiate a game object
                    this.SkinnedMeshSnapshot = new UnityEngine.GameObject("LH_Snapshot");
                    this.SkinnedMeshSnapshot.transform.SetParent(this.SnapshotAnchor.transform, false);
                    this.SkinnedMeshSnapshot.layer = UnityEngine.LayerMask.NameToLayer("HandProjectorLayer");
                    this.SkinnedMeshSnapshot.transform.SetPositionAndRotation(
                        this.SnapshotAnchor.transform.TransformPoint(UnityEngine.Vector3.zero),
                        this.SnapshotAnchor.transform.localRotation
                    );

                    // add a mesh filter, move the wrist anchor once (will be updated by the ultraleap script right after) & clone the skinned hand mesh
                    var filter = this.SkinnedMeshSnapshot.AddComponent<UnityEngine.MeshFilter>();
                    this.WristAnchor.transform.SetPositionAndRotation(
                        UnityEngine.Vector3.zero,
                        UnityEngine.Quaternion.identity
                    );
                    this.HandReference.GetComponentInChildren<UnityEngine.SkinnedMeshRenderer>().BakeMesh(filter.mesh, false);

                    // add a renderer & assign the outlining shader material
                    var renderer = this.SkinnedMeshSnapshot.AddComponent<UnityEngine.MeshRenderer>();
                    renderer.material = this.MaterialReference;
                    renderer.enabled = true;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    renderer.receiveShadows = true;
                    renderer.allowOcclusionWhenDynamic = false;
                    renderer.forceRenderingOff = false;

                    // this.SkinnedMeshSnapshot.SetActive(false);
                    // this.SkinnedMeshSnapshot.isStatic = false;

                } /* if() */

            } /* if() */

        } /* CloneHandMesh() */

        // Initialize
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.Initialize() : begin");
            
            // do

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.Initialize() : end");

        } /* Initialize() */

        // Close
        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.Close() : begin");

            // do
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.Close() : end");

        } /* Close() */

        #endregion 

        #region MonoBehaviour Impl 
        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject HandReference = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject WristAnchor = null;

        [UnityEngine.SerializeField]
        private UnityEngine.Material MaterialReference = null;
        
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject SnapshotAnchor = null;

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileHandMeshClonerBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

    } /* class TactileHandMeshClonerBehaviour */
    
} /* } Labsim.experiment.tactile */
