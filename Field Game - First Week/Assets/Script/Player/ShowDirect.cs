using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDirect : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject target;

    private void Start()
    {
        target = GameObject.Find("Ship");
    }
    void Update()
    {
        this.transform.LookAt(target.transform.position);
    }
}
