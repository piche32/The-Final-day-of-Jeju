using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    Animator animator = null;
    PlayerControl selfCtrl = null;
    Rigidbody rigidbody = null;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        selfCtrl = this.GetComponent<PlayerControl>();
        rigidbody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    
}
