using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EventRoot.cs
//처음에 이벤트의 종류를 나타내는 class.

public class _Event
{ // 이벤트 종류.
    public enum TYPE
    {
         NONE = -1, //없음.
         ROCKET = 0, //우주선 수리.
         NUM, //이벤트가 몇 종류 있는지 나타낸다(=1).
    };
};


public class EventRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public _Event.TYPE getEventType(GameObject event_go)
    {
        _Event.TYPE type = _Event.TYPE.NONE;
        if (event_go != null) //인수의 GameOjbect가 비어있지 않으면.
        {
            if (event_go.tag == "Rocket")
            {
                type = _Event.TYPE.ROCKET;
            }
        }
        return (type);
    }

    //철광석이나 식물을 든 상태에서 우주선에 접촉했는지 확인
    public bool isEventIgnitable(Item.TYPE carried_item, GameObject event_go)
    {
        bool ret = false;
        _Event.TYPE type = _Event.TYPE.NONE;
        if (event_go != null)
        {
            type = this.getEventType(event_go); // 이벤트 타입을 구한다.
        }

        switch (type)
        {
            case _Event.TYPE.ROCKET:
                if (carried_item == Item.TYPE.IRON) //가지고 있는 것이 철광석이라면
                {                                  //'이벤트할 수 있어요!'라고 응답
                    ret = true;
                }
                if (carried_item == Item.TYPE.PLANT) //가지고 있는 것이 식물이라면,
                {                                    //'이벤트할 수 있어요!'라고 응답
                    ret = true;
                }
                break;
        }
        return (ret);
    }

    //지정된 게임오브젝트의 이벤트 타입 반환
    public string getIgnitableMessage(GameObject event_go)
    {
        string message = "";
        _Event.TYPE type = _Event.TYPE.NONE;
        if(event_go != null)
        {
            type = this.getEventType(event_go);
        }
        switch(type)
        {
            case _Event.TYPE.ROCKET:
                message = "수리한다";
                break;
        }
        return (message);
    }
}
