using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

    private GameStatus game_status = null;
    private PlayerControl player_control = null;

    public enum STEP //게임 상태.
    {
        NONE = -1, //상태 정보 없음
        PLAY = 0, //플레이 중
        CLEAR, //클리어 상태
        GAMEOVER, //게임 오버 상태
        NUM, //상태가 몇 종류인지 나타낸다(=3)
    };

    public STEP step = STEP.NONE; //현대 단계
    public STEP next_step = STEP.NONE; //다음 단계
    public float step_timer = 0.0f; //타이머
    private float clear_time = 0.0f; //클리어 시간
    public static float GAME_OVER_TIME = 60.0f; //제한시간은 60초

    public GUIStyle guistyle;

    UIController uiCtrl = null;
    SoundController soundCtrl = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        this.game_status = this.gameObject.GetComponent<GameStatus>();
        this.player_control = GameObject.Find("Player").GetComponent<PlayerControl>();
        this.step = STEP.PLAY;
        this.next_step = STEP.PLAY;
        this.guistyle.fontSize = 64;
        LevelDesign.Instance.setLevelDate();

        uiCtrl = GameObject.Find("UI").GetComponent<UIController>();
        soundCtrl = GameObject.Find("Sound").GetComponent<SoundController>();
        uiCtrl.SetCapacityCount(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.step_timer += Time.deltaTime;
        uiCtrl.SetTime(this.step_timer);
        if (this.next_step == STEP.NONE)
        {
            switch (this.step)
            {
                case STEP.PLAY:
                    if (this.game_status.isGameClear())
                    {
                        //클리어 상태로 이동.
                        this.next_step = STEP.CLEAR;
                    }
                    if (this.game_status.isGameOver())
                    {
                        // 게임 오버 상태로 이동.
                        this.next_step = STEP.GAMEOVER;
                    }
                    if (this.step_timer >= GAME_OVER_TIME)
                    {
                        this.next_step = STEP.GAMEOVER;
                    }
                    break;

                //클리어 시 혹은 게임 오버 시의 처리
                //case STEP.CLEAR:

                //    if (Input.GetMouseButtonDown(0))
                //    {
                //        //마우스 버튼이 눌렸으면 GameScene을 다시 읽는다.
                //        SceneManager.LoadScene("GameScene");
                //        Time.timeScale = 1.0f;
                //    }
                //    break;

                //case STEP.GAMEOVER:
                //   // Time.timeScale = 0.0f;
                //    if (Input.GetMouseButtonDown(0))
                //    {
                //        //마우스 버튼이 눌렸으면 GameScene을 다시 읽는다.
                //        SceneManager.LoadScene("GameScene");
                //        Time.timeScale = 1.0f;
                //    }
                //    break;
            }
        }
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;
            switch (this.step)
            {
                case STEP.CLEAR:
                    //palyerControl을 제어 불가로.
                    this.player_control.enabled = false;
                    //현재의 경과 시간으로 클리어 시간을 갱신.
                    this.clear_time = this.step_timer;
                    LevelDesign.level++;
                    uiCtrl.SetSuccessUI(clear_time);
                    Time.timeScale = 0.0f;

                    //노래
                    soundCtrl.PlayBGM("Success");
                    break;

                case STEP.GAMEOVER:
                    this.player_control.enabled = false;
                    uiCtrl.SetGameoverUI();
                    Time.timeScale = 0.0f;

                    //노래
                    soundCtrl.PlayBGM("GameOver");
                    break;
            }
            this.step_timer = 0.0f;
        }
    }
}
