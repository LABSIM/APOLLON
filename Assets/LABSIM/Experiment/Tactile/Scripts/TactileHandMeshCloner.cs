using System.Collections;
using System.Collections.Generic;

// avoid compilation during player building
#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(TactileHandMeshCloner))]
public class TactileHandMeshClonerEditor 
    : UnityEditor.Editor 
{
   
   public override void OnInspectorGUI() 
   {

        // draw default 
        this.DrawDefaultInspector();

        // then add a button 
        var current_target = this.target as TactileHandMeshCloner;
        if(UnityEngine.GUILayout.Button("Snapshot"))
        {
            current_target.CloneHandMesh();
        }

    } /* OnInspectorGUI() */

} /* class TestEditor*/

#endif

public class TactileHandMeshCloner 
    : UnityEngine.MonoBehaviour
{

    [UnityEngine.SerializeField]
    private UnityEngine.GameObject HandReference = null;
    
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject WristAnchor = null;

    [UnityEngine.SerializeField]
    private UnityEngine.Material MaterialReference = null;
    
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject SnapshotAnchor = null;

    public UnityEngine.GameObject SkinnedMeshSnapshot { private set; get; } = null;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // if(this.SkinnedMeshSnapshot != null && !this.SkinnedMeshSnapshot.activeSelf)
        // {
        //     this.SkinnedMeshSnapshot.SetActive(true);
        // }

    }

} /* class TactileHandMeshCloner */
