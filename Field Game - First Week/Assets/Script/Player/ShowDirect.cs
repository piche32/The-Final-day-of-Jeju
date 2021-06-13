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
        Vector3 dest = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        this.transform.LookAt(dest);
    }
}
