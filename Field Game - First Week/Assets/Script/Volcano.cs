using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{

    [SerializeField] GameObject lavaPrefab = null; //용암 프리팹
    [SerializeField] GameObject eruptSitePrefab = null; // 용암 피해 범위 프리팹

    private Transform lavas, eruptSites;


    [SerializeField] float ERUPT_PERIOD = 20.0f; //화산 폭발하는 주기 상수
    [SerializeField] float ERUPT_TIME = 20.0f; //화산 폭발하는 시간 상수
    private float eruptPeriod = 0.0f; //화산 폭발하는 주기
    private float eruptTime = 0.0f; //화산 폭발하는 시간

    [SerializeField] float FALL_TIME = 100.0f; //용암 떨어지는 시간 상수
    private float fallTime = 0.0f; //용암 떨어지는 시간

    [SerializeField] float fallingHeight = 100.0f; //용암 떨어지는 높이

    bool isErupting; //화산 폭발 하는 중인지

    [SerializeField] float DESTROY_TIME_ERUPTSITE = 10.0f; //용암 피해 범위 사라지는 시간 상수
    [SerializeField] float DAMAGE_LAVA = 0.1f; //용암 데미지 상수

    float stepTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        isErupting = true;
        lavas = gameObject.transform.Find("lavas");
        eruptSites = gameObject.transform.Find("eruptSites");
    }

    // Update is called once per frame
    void Update()
    {
        if (isErupting)
        {
            eruptTime += Time.deltaTime;
            eruptLava();
            if (eruptTime > ERUPT_TIME)
            {
                eruptTime = 0.0f;
                // isErupting = false;
            }
        }
        else
        {
            eruptPeriod += Time.deltaTime;
            if (eruptPeriod > ERUPT_PERIOD)
            {
                eruptPeriod = 0.0f;
                isErupting = true;
            }
        }
    }

    void eruptLava()
    {
        fallTime += Time.deltaTime;
        if (fallTime <= FALL_TIME)
        {
            return;
        }
        fallTime = 0.0f;

        Vector3 pos;
        //출현 위치 정하기
        pos.y = fallingHeight;
        pos.x = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS / 2), PlayerControl.MOVE_AREA_RADIUS / 2);
        pos.z = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS / 2), PlayerControl.MOVE_AREA_RADIUS / 2);

        int layerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit ray;
        while (Physics.Raycast(pos, -Vector3.up, out ray, fallingHeight, layerMask)) {
            pos.x = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS / 2), PlayerControl.MOVE_AREA_RADIUS / 2);
            pos.z = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS / 2), PlayerControl.MOVE_AREA_RADIUS / 2);
        }

        //용암 프리팹 인스턴스화
        GameObject go = GameObject.Instantiate(this.lavaPrefab, lavas) as GameObject;
        go.transform.position = pos;
        go.GetComponent<Lava>().init(DAMAGE_LAVA);

        GameObject siteGo = GameObject.Instantiate(this.eruptSitePrefab, eruptSites) as GameObject;
        pos.y = 0.0f;
        siteGo.transform.position = pos;
        siteGo.GetComponent<EruptSite>().init(DESTROY_TIME_ERUPTSITE);
    }
}
