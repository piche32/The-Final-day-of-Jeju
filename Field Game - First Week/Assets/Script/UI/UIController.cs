using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject successUI = null;
    [SerializeField] GameObject gameoverUI = null;
    [SerializeField] Text itemCount = null;
    [SerializeField] Text durabilityCount = null;
    [SerializeField] Text capacityCount = null;
    [SerializeField] Text time = null;
    [SerializeField] Text speed = null;
    [SerializeField] GameObject pauseUI = null;

    public static bool isPause = false;

    void SetUITextsUnactive()
    {
        itemCount.gameObject.SetActive(false);
        durabilityCount.gameObject.SetActive(false);
        capacityCount.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
        speed.gameObject.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1.0f;

    }

    public void SetSuccessUI(float clearTime)
    {
        successUI.SetActive(true);
        Text leftTime = successUI.transform.Find("LeftTime").GetComponent<Text>();
        float left = SceneControl.GAME_OVER_TIME - clearTime;

        int min = (int)(left / 60.0f);
        int sec = (int)(left - min * 60.0f);
        leftTime.text = (min.ToString("00") + ":" + sec.ToString("00"));
        SetUITextsUnactive();
    }
    
    public void SetGameoverUI()
    {
        gameoverUI.SetActive(true);
        SetUITextsUnactive();
    }

    public void SetItemCount(float value)
    {
        itemCount.text = "아이템 " + value.ToString() + " / 10";
    }

    public void SetDurabilityCount(float value)
    {
        durabilityCount.text = "내구도: " + (value*100.0f).ToString("000");
    }

    public void SetTime(float value)
    {
        if(DataController.Instance._gameData.level != 4)
        {
            value = SceneControl.GAME_OVER_TIME - value;
        }
        int min = (int)(value / 60.0f);
        int sec = (int)(value - min * 60.0f);
        time.text = (min.ToString("00") + ":" + sec.ToString("00"));
    }

    public void SetCapacityCount(float value)
    {
        if (DataController.Instance._gameData.level == 4)
        {
            capacityCount.text = "적재량 " + value.ToString() + "/ - ";
        }
        else
            capacityCount.text = "적재량 " + value.ToString() + " / " + GameStatus.MAX_SHIP_CAPACITY.ToString(); 
    }

    public void SetSpeed(float value)
    {
        speed.text = "현재 속도: " + value.ToString("000") + " %";
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) //일시정지 상태 -> 일시정지 해제
            {
                Time.timeScale = 1.0f;
                pauseUI.SetActive(false);
            }
            else
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            isPause = !isPause;
        }
    }

}
