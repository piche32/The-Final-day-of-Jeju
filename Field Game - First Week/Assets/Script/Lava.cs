using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Lava : MonoBehaviour
{
    Rigidbody rb;
    private float damage = 10.0f;
    public float Damage { get { return damage; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
    }

    public void init(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision) { 
    
        Destroy(this.gameObject);

    }

}
