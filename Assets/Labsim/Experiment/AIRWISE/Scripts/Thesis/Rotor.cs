using UnityEngine;

public class Rotor : MonoBehaviour
{
    public Rigidbody m_rb;

    private void Awake()
    {
        this.m_rb = this.gameObject.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {

    }

}
