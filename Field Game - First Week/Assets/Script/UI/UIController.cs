using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject successUI = null;
    [SerializeField] GameObject gameoverUI = null;
    [SerializeField] GameObject resultUI = null;
    [SerializeField] Text itemCount = null;
    [SerializeField] Text durabilityCount = null;
    [SerializeField] Slider durabilitySlider = null;
    [SerializeField] Text capacityCount = null;
    [SerializeField] Slider capacitySlider = null;
    [SerializeField] Text time = null;
    [SerializeField] Text level = null;
    [SerializeField] Transform levels = null;
    [SerializeField] Text speed = null;
    [SerializeField] Text esc = null;
    [SerializeField] GameObject pauseUI = null;
    [SerializeField] GameObject UITexts = null;

    public static bool isPause = false;

    void SetUITextsUnactive()
    {
        /* itemCount.gameObject.SetActive(false);
         durabilityCount.gameObject.SetActive(false);
         capacityCount.gameObject.SetActive(false);
         time.gameObject.SetActive(false);
         level.gameObject.SetActive(false);
         speed.gameObject.SetActive(false);
         esc.gameObject.SetActive(false);*/

        UITexts.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1.0f;

    }

    public void SetSuccessUI(float clearTime)
    {
        successUI.SetActive(true);
        Text leftTime = successUI.transform.GetChild(2).Find("LeftTime").GetComponent<Text>();
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

    public void SetResultUI(float clearTime, float capacity)
    {
        resultUI.SetActive(true);
        SetUITextsUnactive();

        Text time = resultUI.transform.GetChild(2).Find("Time").GetComponent<Text>();
        Text capacityText = resultUI.transform.GetChild(2).transform.Find("Capacity").GetComponent<Text>();
        
        int min = (int)(clearTime / 60.0f);
        int sec = (int)(clearTime - min * 60.0f);
        time.text = (min.ToString("00") + ":" + sec.ToString("00"));
        capacityText.text = capacity.ToString();
    }

    public void SetItemCount(float value)
    {
        itemCount.text = value.ToString() + " / 10";
    }

    public void SetDurabilityCount(float value)
    {
        durabilityCount.text = "내구도: " + (value*100.0f).ToString("000");
    }

    public void SetDurability(float value)
    {
        durabilitySlider.value = value;
    }
    public void SetTime(float value)
    {
        if(DataController.Instance.gameData.level != 4)
        {
            value = SceneControl.GAME_OVER_TIME - value;
        }
        int min = (int)(value / 60.0f);
        int sec = (int)(value - min * 60.0f);
        time.text = (min.ToString("00") + ":" + sec.ToString("00"));
    }

    public void setLevel(float value)
    {
        level.text = "Stage " + value.ToString();
        for(int i = 0; i < levels.childCount; i++)
        {
            if (i + 1 == value)
            {
                levels.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                levels.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void SetCapacityCount(float value)
    {
        if (DataController.Instance.gameData.level == 4)
        {
            capacityCount.text = "적재량 " + value.ToString() + "/ - ";
        }
        else
            capacityCount.text = "적재량 " + value.ToString() + " / " + GameStatus.MAX_SHIP_CAPACITY.ToString();

        capacitySlider.value = value;
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
                offPauseUI();
                isPause = false;
            }
            else
            {
                if (Time.timeScale == 0.0f) return; //게임 오버 혹은 성공 시 pause 켜지지 않게 하기
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f;
                isPause = true;
            }
        }
    }

    public void OnDisable()
    {
        isPause = false;
        Time.timeScale = 1.0f;

    }

    public void initCapacity()
    {
        capacitySlider.maxValue = GameStatus.MAX_SHIP_CAPACITY;
        capacitySlider.value = 0.0f;
    }

    public void offPauseUI()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        isPause = false;
    }
}
