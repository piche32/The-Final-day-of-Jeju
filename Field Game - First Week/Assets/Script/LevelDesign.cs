using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
레벨 디자인
아이템 무게로 인한 플레이어 속도 감소
  과일: 1%
  음식: 1%

배의 적재 용량
1: 30개
2: 40개
3: 40개
4: -

제한 시간
1: 3분
2: 3분
3: 2분
4: -

내구도(용암 데미지를 2배로 바꿔줌)
1: 100
2: 50 (내구도 하락 지수를 두 배로 바꿔주기)
3: 50
4: 50
*/

public class LevelDesign : Singleton<LevelDesign>
{
    protected LevelDesign () { }

    //public static int level = 1;

    private float UNLIMITED = float.MaxValue;
    Transform stages = null;


    public void init()
    {
        DataController.Instance.LoadGameData();
        stages = GameObject.Find("Stage").transform;
        setLevelData();
    }


    private void setStage(int level)
    {
        for(int i = 0; i < stages.childCount; i++)
        {
            if(i + 1 == level)
            {
                stages.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                stages.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void setLevelData()
    {
        setStage(DataController.Instance.gameData.level);
        switch (DataController.Instance.gameData.level)
        {
            case 1:
                GameStatus.DAMAGE_OF_LAVA = 0.1f;
                GameStatus.MAX_SHIP_CAPACITY = 30.0f;
                SceneControl.GAME_OVER_TIME = 180.0f;
                Volcano.NumOfLava = 20;
                break;
            case 2:
                GameStatus.DAMAGE_OF_LAVA = 0.2f;
                GameStatus.MAX_SHIP_CAPACITY = 35.0f;
                SceneControl.GAME_OVER_TIME = 170.0f;
                Volcano.NumOfLava = 30;
                break;
            case 3:
                GameStatus.DAMAGE_OF_LAVA = 0.25f;
                GameStatus.MAX_SHIP_CAPACITY = 35.0f;
                SceneControl.GAME_OVER_TIME = 150.0f;
                Volcano.NumOfLava = 40;
                break;
            case 4:
            default:
                GameStatus.DAMAGE_OF_LAVA = 0.25f;
                GameStatus.MAX_SHIP_CAPACITY = 200.0f;
                SceneControl.GAME_OVER_TIME = UNLIMITED;
                Volcano.NumOfLava = 50;
                break;

        }
    }
}
