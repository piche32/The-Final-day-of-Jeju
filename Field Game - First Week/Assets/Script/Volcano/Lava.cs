using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Lava : MonoBehaviour
{
    Rigidbody rb;
    private float damage = 0.0f;
    public float Damage { get { return damage; } }

    public EruptSite eruptSite = null;
    LavaSound sound = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        damage = GameStatus.DAMAGE_OF_LAVA;
        sound = GetComponent<LavaSound>();
    }

    public void init(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision) {
        if(eruptSite != null)
            eruptSite.Erupt(damage);
        sound.PlayBoomSound();
        if(this.GetComponent<Renderer>() != null)
            this.GetComponent<Renderer>().enabled = false;
        if(this.GetComponent<Collider>() != null)
            this.GetComponent<Collider>().enabled = false;
    }

}
