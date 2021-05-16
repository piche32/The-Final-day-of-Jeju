using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EruptSiteMaterialControll : MonoBehaviour
{

    Material eruptSignMt = null;
    Material eruptedMt = null;
    Material diedMt = null;

    Transform eruptSiteModel = null;
    // Start is called before the first frame update
    void Awake()
    {
        eruptSignMt = Resources.Load("Material/EruptSign") as Material;
        eruptedMt = Resources.Load("Material/lava") as Material;
        diedMt = Resources.Load("Material/VolcanicRock") as Material;

    }

    public bool setMaterial(EruptSite.STATE state)
    {

        foreach (Transform t in this.transform)
        {
            if (t.gameObject.activeSelf) //활성화되어 있는 모델을 찾는다.
            {
                eruptSiteModel = t;
                break;
            }
        }

        Material m = null;
        if(state == EruptSite.STATE.NONE)
        {
            //아직 폭발하지 않아서 폭발할 거라는 표시인 메터리얼 적용해주기
            m = eruptSignMt;
        }
        if(state == EruptSite.STATE.ERUPTED)
        {
            m = eruptedMt;
        }
        if(state == EruptSite.STATE.DIE)
        {
            m = diedMt;
        }

        foreach(Transform t in eruptSiteModel)
        {
            t.gameObject.GetComponent<Renderer>().material = m;
        }

        return true;

    }
}
