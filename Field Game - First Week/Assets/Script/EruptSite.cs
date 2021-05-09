using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    };

};

public class EruptSite : MonoBehaviour
{
    private eruptSiteShape.TYPE shapeType = eruptSiteShape.TYPE.NONE; //용암 피해 범위 유형



    float timer; //내장 타이머
    float destroyTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        shapeType = (eruptSiteShape.TYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(eruptSiteShape.TYPE)).Length - 1);

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

    }

    public void init(float destroyTime)
    {
        this.destroyTime = destroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava")
            Destroy(this.gameObject);
    }
}
