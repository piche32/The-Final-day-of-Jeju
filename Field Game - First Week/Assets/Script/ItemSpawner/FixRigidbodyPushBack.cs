using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Rigidbody 때문에 Player랑 충돌하면 Player 혼자 날아가버려서
/// Field에 있을 때는 isKinematic 켜두고
/// Player가 들었을 때는 isKinematic 끄기
/// </summary>
public class FixRigidbodyPushBack : MonoBehaviour
{
    Rigidbody rb = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Field"))
        {
            if(rb != null)
                rb.isKinematic = true;
        }
    }
}
