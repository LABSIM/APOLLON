using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public MeshCollider[] m_walls;
    // Start is called before the first frame update
    private void Awake()
    {
        this.m_walls = this.transform.GetComponentsInChildren<MeshCollider>();
    }
}
