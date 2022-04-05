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
    public UnityEngine.SkinnedMeshRenderer SkinnedMeshReference = null;

    // [UnityEngine.SerializeField]
    // public UnityEngine.GameObject Parent = null;

    public UnityEngine.GameObject SkinnedMeshSnapshot { private set; get; } = null;

    public void CloneHandMesh()
    {
        
        if(UnityEngine.Application.isPlaying)
        {

            if(this.SkinnedMeshSnapshot == null) 
            {
                

                // intantiate
                this.SkinnedMeshSnapshot = new UnityEngine.GameObject("LH_Snapshot");
                var filter = this.SkinnedMeshSnapshot.AddComponent<UnityEngine.MeshFilter>();
                var renderer = this.SkinnedMeshSnapshot.AddComponent<UnityEngine.MeshRenderer>();

                // configure
                this.SkinnedMeshSnapshot.transform.SetParent(this.transform);
                this.SkinnedMeshSnapshot.layer = UnityEngine.LayerMask.NameToLayer("HandProjectorLayer");
                this.SkinnedMeshReference.BakeMesh(filter.mesh, false);
                renderer.enabled = true;
                renderer.material = this.SkinnedMeshReference.material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = true;
                renderer.allowOcclusionWhenDynamic = false;
                renderer.forceRenderingOff = false;

                this.SkinnedMeshSnapshot.SetActive(false);
                this.SkinnedMeshSnapshot.isStatic = false;

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

        if(this.SkinnedMeshSnapshot != null && !this.SkinnedMeshSnapshot.activeSelf)
        {
            this.SkinnedMeshSnapshot.SetActive(true);
        }

    }

} /* class TactileHandMeshCloner */
