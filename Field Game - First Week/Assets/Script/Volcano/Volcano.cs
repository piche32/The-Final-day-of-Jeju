using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{

    [SerializeField] GameObject lavaPrefab = null; //용암 프리팹
    [SerializeField] GameObject eruptSitePrefab = null; // 용암 피해 범위 프리팹

    private Transform lavas, eruptSites;


    [SerializeField] float ERUPT_PERIOD = 30.0f; //화산 폭발하는 주기 상수
    [SerializeField] float ERUPT_TIME = 10.0f; //화산 폭발하는 시간 상수
    private float eruptPeriod = 0.0f; //화산 폭발하는 주기
    private float eruptTime = 0.0f; //화산 폭발하는 시간

    [SerializeField] float FALL_TIME = 20.0f; //용암 떨어지는 시간 상수
    private float fallTime = 0.0f; //용암 떨어지는 시간

    [SerializeField] float fallingHeight = 50.0f; //용암 떨어지는 높이

    public static int NumOfLava = 5; //한 번에 만들어지는 용암의 갯수

    bool isErupting; //화산 폭발 하는 중인지

    [SerializeField] float DAMAGE_LAVA = 0.1f; //용암 데미지 상수

    float eruptSiteScale = 1.0f; //생성되는 용암이 퍼지는 범위의 크기
    float stepTime = 0.0f;
    float eruptSiteScaleTime = 10.0f; //생성되는 용암이 퍼지는 범위의 크기가 바뀌는 시간

    SoundController soundCtrl;

    // Start is called before the first frame update
    void Start()
    {
        isErupting = false;
        lavas = gameObject.transform.Find("lavas");
        eruptSites = gameObject.transform.Find("eruptSites");
        eruptPeriod = ERUPT_PERIOD - 5.0f;

        soundCtrl = GameObject.Find("Sound").GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isErupting)
        {
            eruptTime += Time.deltaTime;
            for(int i = 0; i < NumOfLava; i++)
            {
                eruptLava();
            }
            if (eruptTime > ERUPT_TIME)
            {
                eruptTime = 0.0f;
                isErupting = false;
                soundCtrl.SetBgmSpeed(false);
            }
        }
        else
        {
            eruptPeriod += Time.deltaTime;
            if (eruptPeriod > ERUPT_PERIOD)
            {
                eruptPeriod = 0.0f;
                soundCtrl.SetBgmSpeed(true);
                soundCtrl.PlaySFX("Warning");
                isErupting = true;
            }
        }

        //용암이 퍼지는 범위가 커지는 코드
        stepTime += Time.deltaTime;
        if(stepTime > eruptSiteScaleTime)
        {
            eruptSiteScale += 0.2f;
            stepTime = 0.0f;
            eruptSiteScaleTime -= 10.0f;
            if(eruptSiteScaleTime <= 10.0f)
            {
                eruptSiteScaleTime = 30.0f;
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
        pos.x = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS), PlayerControl.MOVE_AREA_RADIUS);
        pos.z = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS), PlayerControl.MOVE_AREA_RADIUS);

        int layerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit ray;
        while (Physics.Raycast(pos, -Vector3.up, out ray, fallingHeight, layerMask)) {
            pos.x = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS), PlayerControl.MOVE_AREA_RADIUS);
            pos.z = Random.Range(-(PlayerControl.MOVE_AREA_RADIUS), PlayerControl.MOVE_AREA_RADIUS);
        }

        //용암 프리팹 인스턴스화
        GameObject go = GameObject.Instantiate(this.lavaPrefab, lavas) as GameObject;
        go.transform.position = pos;
        go.GetComponent<Lava>().init(DAMAGE_LAVA);

        GameObject siteGo = GameObject.Instantiate(this.eruptSitePrefab, eruptSites) as GameObject;
        pos.y = 0.0f;
        siteGo.transform.position = pos;
        siteGo.GetComponent<EruptSite>().SetScale(eruptSiteScale);

        go.GetComponent<Lava>().eruptSite = siteGo.GetComponent<EruptSite>();
    }
}
