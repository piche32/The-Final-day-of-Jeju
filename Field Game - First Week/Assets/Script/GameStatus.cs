using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    //음식, 과일을 사용했을 때 각각의 적재량
    public static float GAIN_CAPACITY_FOOD = 2f;
    public static float GAIN_CAPACITY_FRUIT = 1f;

    //음식, 과일을 운반했을 때 각각의 체력 소모 정도.
    public static float CONSUME_SATIETY_IRON = 0.20f;
    public static float CONSUME_SATIETY_APPLE = 0.1f;
    public static float CONSUME_SATIETY_PLANT = 0.1f;

    //용암의 데미지
    public static float DAMAGE_OF_LAVA = 0.1f;

    //디폴트 내구도 하락 지수 하락 정도
    public static float CONSUME_DURABILITY_ALWAYS = 0.03f;

    //오일을 먹었을 때 체력 회복 정도.
    public static float REGAIN_DURABILITY_OIL = 0.7f;

    public static float MAX_SHIP_CAPACITY = 30.0f; //배의 최대 적재 용량

    public float capacity = 0.0f; //배의 적재 정도(0.0f~1.0f).
    public float durability = 1.0f; //내구도, 체력(0.0f ~ 1.0f).
    public int itemCount = 0; //들고 있는 아이템 갯수
    

    public GUIStyle guistyle; //폰트 스타일

    UIController uiCtrl = null;
    // Start is called before the first frame update
    void Start()
    {
        
        this.guistyle.fontSize = 24; //폰트 크기를 24로.
        uiCtrl = GameObject.Find("UI").GetComponent<UIController>();
        uiCtrl.SetCapacityCount(this.capacity);

    }


    //배 적재를 진행
    public void addCapacity(float add)
    {
        this.capacity = this.capacity + add; // 0.0 ~ 1.0 강제 지정
        uiCtrl.SetCapacityCount(this.capacity);
    }

    //내구도을 늘리거나 줄임
    public void addDurability(float add)
    {
        this.durability = Mathf.Clamp01(this.durability + add);
        uiCtrl.SetDurabilityCount(this.durability);
        
    }

    //내구도 소모 메서드 추가
    public void alwaysDecreasedDurability()
    {
        this.durability = Mathf.Clamp01(this.durability - CONSUME_DURABILITY_ALWAYS * Time.deltaTime);
        uiCtrl.SetDurabilityCount(this.durability);
    }

    //게임 클리어 검사
    public bool isGameClear()
    {
        bool is_clear = false;
        if(this.capacity >= MAX_SHIP_CAPACITY) //수리 정도가 100% 이상이면
        {
            is_clear = true;        //클리어했다.
        }
        return (is_clear);
    }

    //게임 끝났는지 검사
    public bool isGameOver()
    {
        bool is_over = false;
        if(this.durability <= 0.1f) //체력 0이하면
        {                        //게임 오버.
            is_over = true;
        }
        return (is_over);
    }
}
