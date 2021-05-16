using System.Collections;
using System.Collections.Generic; //List를 사용하기 위해
using UnityEngine;

public class Item
{
    public enum TYPE
    { //아이템 종류
        NONE = -1,
        BOX,
        FRUIT,
        FOOD,
        OIL,
        NUM, //아이템이 몇 종류인가 나타낸다.
    };
};

public class ItemRoot : MonoBehaviour
{
    public GameObject boxPrefab = null; //Prefab 'box'

    protected List<GameObject> respawnPointsBox; //box 출현 지점 List

    public static float RESPAWN_TIME_BOX = 12.0f; //박스 출현 시간 상수

    private float respawn_timer_box = 0.0f; //박스 출현 시간

    public static int RESPAWN_NUM_BOX = 15; //박스 출현 갯수
    private int respawn_num_box = 0;

    //초기화 작업을 시행한다.
    void Start()
    {
        //box
        //메모리 영역 확보
        this.respawnPointsBox = new List<GameObject>();
        //"BoxRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장
        GameObject[] boxRespawns =
            GameObject.FindGameObjectsWithTag("BoxRespawn");

        //배열 respawns 내의 개개의 GameObject를 순서대로 처리한다.
        foreach(GameObject go in boxRespawns)
        {
            //랜더러 획득
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if(renderer != null)
            {
                renderer.enabled = false;
            }

            this.respawnPointsBox.Add(go);
        }

        this.respawnBox(); //박스를 두 개 생성
        this.respawnBox(); //박스를 두 개 생성

    }

    //각 아이템의 타이머 값이 출현 시간을 초과하면 해당 아이템을 출현
    void Update()
    {
        respawn_timer_box += Time.deltaTime;
        if(respawn_timer_box > RESPAWN_TIME_BOX && respawn_num_box < RESPAWN_NUM_BOX)
        {
            respawn_timer_box = 0.0f;
            this.respawnBox(); //박스를 출현시킨다.
        }
    }

    //아이템의 종류를 Item.TYPE형으로 반환하는 메소드
    public Item.TYPE getItemType(GameObject item_go)
    {
        Item.TYPE type = Item.TYPE.NONE;
        if (item_go != null) //인수로 받은 GameObject가 비어있지 않으면
        {
            switch (item_go.tag)
            {
                case "Box": type = Item.TYPE.BOX; break;
                case "Food": type = Item.TYPE.FOOD; break;
                case "Fruit": type = Item.TYPE.FRUIT; break;
                case "Oil": type = Item.TYPE.OIL; break;
            }
        }
        return (type);
    }

    //박스를 출현시킨다.
    public void respawnBox()
    {
        //박스 프리팹을 인스턴스화.
        GameObject go = GameObject.Instantiate(this.boxPrefab) as GameObject;
        //박스의 출현 포인트를 랜덤하게 취득.
        int n = Random.Range(0, this.respawnPointsBox.Count);
        while(respawnPointsBox[n].transform.childCount != 0){
            n = Random.Range(0, this.respawnPointsBox.Count);
        }

        Vector3 pos = this.respawnPointsBox[n].transform.position;
        //출현 위치를 조정
        pos.y = 1.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);
        //박스의 위치를 이동
        go.transform.position = pos;
        go.transform.parent = this.respawnPointsBox[n].transform;

        respawn_num_box++;

    }

    //들고 있는 아이템에 따른 '적재량 진척 상태'를 반환
    public float getLoadCapacity(GameObject item_go)
    {
        float gain = 0.0f;
        if (item_go == null)
        {
            gain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type) //들고 있는 아이템의 종류로 갈라진다.
            {
                case Item.TYPE.FOOD:
                    gain = GameStatus.GAIN_CAPACITY_FOOD; break;
                case Item.TYPE.FRUIT:
                    gain = GameStatus.GAIN_CAPACITY_FRUIT; break;
            }
        }
        return (gain);
    }

    //들고 있는 아이템에 따른 '체력 감소 상태'를 반환
    public float getConsumeDurability(GameObject item_go)
    {
        float consume = 0.0f;
        if (item_go == null)
        {
            consume = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type) //들고 있는 아이템의 종류로 갈라진다.
            {
                default: break;
            //    case Item.TYPE.IRON:
          //          consume = GameStatus.CONSUME_SATIETY_IRON; break;
          //      case Item.TYPE.APPLE:
           //         consume = GameStatus.CONSUME_SATIETY_APPLE; break;
           //     case Item.TYPE.PLANT:
            //        consume = GameStatus.CONSUME_SATIETY_PLANT; break;
            }
        }
        return (consume);
    }

    //들고 있는 아이템에 따른 '체력 회복 상태'를 반환
    public float getRegainSatiety(GameObject item_go)
    {
        float regain = 0.0f;
        if (item_go == null)
        {
            regain = 0.0f;
        }
        else
        {
            Item.TYPE type = this.getItemType(item_go);
            switch (type) //들고 있는 아이템의 종류로 갈라진다.
            {
                case Item.TYPE.OIL:
                    regain = GameStatus.REGAIN_DURABILITY_OIL; break;
            }
        }   
        return (regain);
    }


}


