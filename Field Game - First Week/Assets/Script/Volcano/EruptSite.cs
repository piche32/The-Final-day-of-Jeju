using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//자식 오브젝트의 콜라이더 사용하기 위해 반드시 rigidbody 필요
[RequireComponent(typeof(Rigidbody))]
public class eruptSiteShape
{
    public enum TYPE //용암 피해 범위 유형
    {
        NONE = -1,
        A,
        B,
        C,
        D,
        E,
        NUM
    };

};



public class EruptSite : MonoBehaviour
{

    public enum STATE
    {
        NONE = -1,
        ERUPTED, //폭발했는지
        DIE, //폭발끝나고 죽은 건지
        NUM,
    }
    STATE state = STATE.NONE;
    public STATE State { get { return state; } }

    private eruptSiteShape.TYPE shapeType = eruptSiteShape.TYPE.NONE; //용암 피해 범위 유형
    public eruptSiteShape.TYPE ShapeType { get { return shapeType; } }
    
    float timer; //내장 타이머
    //float eruptTime = 1.0f;

    [SerializeField]float destroyedTime = 10.0f;
    [SerializeField] float defaultScale = 1.5f;

    float damage = 0.0f;


  //  용암이 퍼지는 범위가 커지는 간격, 크기
  //    간격: scaleUpTerm(60s)
  //    크기: scaleUpRatio(0.2f)


    // Start is called before the first frame update
    void Start()
    {
        shapeType = (eruptSiteShape.TYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(eruptSiteShape.TYPE)).Length - 2);

        switch (shapeType)
        {
            case eruptSiteShape.TYPE.A:
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case eruptSiteShape.TYPE.B:
                this.gameObject.transform.GetChild(1);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case eruptSiteShape.TYPE.C:
                this.gameObject.transform.GetChild(2);
                this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case eruptSiteShape.TYPE.D:
                this.gameObject.transform.GetChild(3);
                this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case eruptSiteShape.TYPE.E:
                this.gameObject.transform.GetChild(4);
                this.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                break;
        }

        Vector3 rot;
        rot.y = UnityEngine.Random.Range(0, 360);
        rot.x = 0;
        rot.z = 0;

        transform.Rotate(rot);

        timer = 0.0f;

        this.GetComponent<EruptSiteMaterialControll>().setMaterial(state);

        if(this.transform.position.magnitude > PlayerControl.MOVE_AREA_RADIUS)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (UIController.isPause) 
            return;

        timer += Time.deltaTime;
        if (timer > destroyedTime)
            Destroy(this.gameObject);
    }

    public void Erupt(float damage)
    {
        state = STATE.ERUPTED;
        this.damage = damage;
        this.GetComponent<EruptSiteMaterialControll>().setMaterial(state);
        timer = destroyedTime - 1.0f;
    }

    
    private void OnCollisionStay(Collision collision)
    {
        if (state == STATE.ERUPTED) //폭발 신호가 들어오면
        {
            if (collision.gameObject.tag == "Player") //플레이어가 밟고 있으면 
            {
                collision.gameObject.GetComponent<PlayerControl>().hitLava(damage);
                state = STATE.DIE;
                this.GetComponent<EruptSiteMaterialControll>().setMaterial(state);
            }
        }
    }

    public void SetScale(float scale)
    {
        this.transform.localScale = Vector3.one * defaultScale * scale;
    }
}
