using UnityEngine;

public class InhibitCollider : MonoBehaviour
{
    void Awake()
    {

        foreach(var collider in this.gameObject.GetComponentsInChildren<UnityEngine.MeshCollider>())
        {

            collider.enabled = false;
        
        }
        
    }

}
