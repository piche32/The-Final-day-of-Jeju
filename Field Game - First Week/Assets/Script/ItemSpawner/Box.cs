using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab = null; //음식 오브젝트
    public void hit() //맞으면 음식 나오게 하기
    {
        GameObject go = GameObject.Instantiate(this.foodPrefab) as GameObject;
        Vector3 pos = this.transform.position;
        go.transform.position = pos;
        Destroy(this.gameObject);
    }
}
