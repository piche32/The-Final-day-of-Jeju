using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    protected GameObject itemPrefab = null;
   

    //아이템 스폰!
    public void spawnItem()
    {
        GameObject go = GameObject.Instantiate(this.itemPrefab) as GameObject;
        Vector3 pos = this.gameObject.transform.position;

        //아이템 스폰 위치 조정
        pos.y = 0.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);

        //아이템 위치 이동
        go.transform.position = pos;
    }
}
