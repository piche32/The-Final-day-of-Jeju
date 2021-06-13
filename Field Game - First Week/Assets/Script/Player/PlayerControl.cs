using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static float MOVE_AREA_RADIUS = 50.0f; //섬의 반지름.
    public static float MAX_MOVE_SPEED = 13.0f; //이동 속도.
    private float moveSpeed = MAX_MOVE_SPEED;
    public float MoveSpeed { get { return moveSpeed; } }

 private float rotSpeed = 2f;
    public float RotSpeed { get { return rotSpeed; } }

    [SerializeField] GameObject healing = null;
    private struct Key
    { //키 조작 정보 구조체.
        public bool up;
        public bool down;
        public bool right;
        public bool left;
        public bool pick; //줍는다
        public bool drop; //버린다
        public bool action; //먹는다 / 수리한다
        public bool hit; //때린다
    };
    private Key key; // 키 조작 정보를 보관하는 변수

    public enum STEP
    { // 플레이어의 상태를 나타내는 열거체
        NONE = -1, //상태 정보 없음
        MOVE = 0, //이동 중
        LOADING, //수리 중
        EATING, //식사 중
        HIT, //때리는 중
        NUM, // 상태가 몇 종류 있는지 나타낸다 (4)
    };

    private STEP step = STEP.NONE; //현재 상태.
    public STEP Step { get { return step; } }
    private STEP next_step = STEP.NONE; //다음 상태.
    public STEP NextStep { get { return next_step; } }
    public float step_timer = 0.0f; //타이머.

    private Animator animator;

    private GameObject closest_item = null; //플레이어의 정면에 있는 GameObject;
                                            // private GameObject carried_item = null; //플레이어가 들어올린 GameOjbect;
    private ItemRoot item_root = null; //ItemRoot 스크립트를 가짐
    public GUIStyle guistyle; //폰트 스타일

    private GameObject closest_event = null; //주목하고 있는 이벤트를 저장.
    private EventRoot event_root = null; //EventRoot 클래스를 사용하기 위한 변수.
    private GameObject ship_model = null; //배의 모델을 사용하기 위한 변수

    private GameStatus game_status = null;

    private List<GameObject> items; //들고 있는 아이템
    [SerializeField] int maxItemsCount = 10; //아이템 최대 소지 갯수

    UIController uiCtrl = null;
    SoundController soundCtrl = null;
    void Start()
    {
        this.step = STEP.NONE; //현 단계 상태를 초기화
        this.next_step = STEP.MOVE; //다음 단계 상태를 초기화

        this.animator = this.GetComponent<Animator>();

        this.item_root = GameObject.Find("GameRoot").GetComponent<ItemRoot>();
        this.guistyle.fontSize = 32;

        this.event_root =
            GameObject.Find("GameRoot").GetComponent<EventRoot>();

        this.game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();

        this.ship_model = GameObject.Find("Ship").transform.Find("ship_model").gameObject;

        //items
        //메모리 영역 확보
        this.items = new List<GameObject>();

        uiCtrl = GameObject.Find("UI").GetComponent<UIController>();
        soundCtrl = GameObject.Find("Sound").GetComponent<SoundController>();
    }

    // 입력 정보를 가져오고 상태에 변화가 있을 때의 처리를 거쳐 각 상태별로 실행
    void Update()
    {
        if (UIController.isPause) return;
        this.get_input(); //입력 정보 취득

        this.step_timer += Time.deltaTime;
        float eat_time = 1.0f; //연료는 0.5초에 걸쳐 먹는다.
        float load_time = 0.5f; //적재 시 걸리는 시간도 0.5초
        float hit_time = 1.0f; //때리는 데 걸리는 시간은 1초

        //상태를 변화시킨다-------------------------------------
        if (this.next_step == STEP.NONE)
        { //다음 예정이 없으면
            switch (this.step)
            {
                case STEP.MOVE:
                    do
                    {
                        if (!this.key.action && !this.key.hit && !this.key.pick) //액션 키가 눌려 있지 않다.
                            break; //루프 탈출

                        if (this.key.action)
                        {//주목하는 이벤트가 있을 때,
                            if (this.closest_event != null)
                            {
                                if (closest_event.CompareTag("Ship"))
                                {
                                    if (!this.is_event_ignitable()) //이벤트를 시작할 수 없으면.
                                    {
                                        break; //아무 것도 하지 않는다.
                                    }
                                    //이벤트 종류를 가져온다.
                                    _Event.TYPE ignitable_event = this.event_root.getEventType(this.closest_event);
                                    switch (ignitable_event)
                                    {
                                        case _Event.TYPE.SHIP: //이벤트의 종류가 SHIP 이면.
                                                               //LOADING(적재) 상태로 이행.
                                            this.next_step = STEP.LOADING;
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        if (this.closest_item != null && this.key.pick)
                        {
                            //가까운 아이템 판별
                            Item.TYPE closest_item_type =
                                this.item_root.getItemType(this.closest_item);

                            switch (closest_item_type)
                            {
                                case Item.TYPE.OIL: //오일이라면.
                                                    //case Item.TYPE.FRUIT: //과일이라면
                                                    //'식사 중' 상태로 이행.
                                    this.next_step = STEP.EATING;
                                    break;
                            }
                        }

                        //때리기 키가 눌려있을 때
                        if (this.key.hit)
                        {
                            if (closest_event != null && closest_event.CompareTag("AppleRespawn"))
                            {
                                soundCtrl.PlaySFX("Hit");
                                closest_event.GetComponentInParent<Tree>().hit();
                            }

                            if (closest_event != null && closest_event.CompareTag("Box"))
                            {
                                soundCtrl.PlaySFX("Hit");
                                closest_event.GetComponentInParent<Box>().hit();
                            }
                            this.next_step = STEP.HIT;
                            break;
                        }

                    } while (false);
                    break;

                case STEP.EATING: //'식사 중' 상태의 처리
                    if (this.step_timer > eat_time)
                    { //2초 대기
                        this.next_step = STEP.MOVE; //'이동' 상태로 이행
                        healing.SetActive(false);
                    }
                    break;

                case STEP.LOADING: // '적재 중' 상태의 처리.
                    if (this.step_timer > load_time) //2초 대기.
                    {
                        this.next_step = STEP.MOVE; //'이동' 상태로 이행.  
                    }
                    break;

                case STEP.HIT: //'때리기' 상태의 처리
                    if (this.step_timer > hit_time) //2초 대기
                        this.next_step = STEP.MOVE; //'이동' 상태로 이행
                    break;
            }
        }

        //상태가 변화했을 때---------------
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;

            switch (this.step)
            {
                case STEP.MOVE:
                    break;

                case STEP.EATING: //'식사 중' 상태의 처리
                    if (this.closest_item != null)
                    {
                        //들고 있는 아이템의 '내구도 회복 정도'를 가져와서 설정.
                        this.game_status.addDurability(this.item_root.getRegainSatiety(this.closest_item));
                        //가지고 있던 아이템을 폐기
                        GameObject.Destroy(this.closest_item);
                        this.closest_item = null;
                        animator.SetTrigger("Eat");
                        healing.SetActive(true);

                    }
                    break;

                case STEP.LOADING: // '적재 중'이 되면.
                    if (items.Count > 0)
                    {
                        GameObject item = items[0];
                        this.game_status.addCapacity(this.item_root.getLoadCapacity(item));
                        items.RemoveAt(0);
                        GameObject.Destroy(item);
                        game_status.itemCount--;
                        uiCtrl.SetItemCount(game_status.itemCount);
                        addSpeed(game_status.getWeight(item_root.getItemType(item))); //3개 이상 들고 나서부터 속도가 조금씩 떨어진다.
                        //foreach(GameObject item in items)
                        //{
                        //    //들고 있는 아이템의 '적재 상태'를 가져와서 설정.
                        //    this.game_status.addCapacity(this.item_root.getLoadCapacity(item));
                        //    //가지고 있는 아이템 삭제.

                        //    items.Remove(item);
                        //    GameObject.Destroy(item);
                        //    game_status.itemCount--;
                        //  //  this.closest_item = null;
                        //}
                        ship_model.transform.parent.GetComponent<Animator>().SetTrigger("Load");

                        //효과음
                        soundCtrl.PlaySFX("LoadItem");
                    }
                    break;

                case STEP.HIT:
                    animator.SetTrigger("Hit");

                    break;
            }
            this.step_timer = 0;
        }

        //각 상황에서 반복할 것------------------
        switch (this.step)
        {
            case STEP.MOVE:
                this.move_control();
                this.pick_or_drop_control();

                //이동 가능한 경우는  항상 내구도가 소모된다.
                //this.game_status.alwaysDecreasedDurability();
                break;

                //case STEP.LOADING:
                //    //배를 위 아래로 움직인다.
                //    Vector3 pos = this.ship_model.transform.localPosition;
                //    if (pos.y < 2) pos.y += 0.1f;
                //    else pos.y -= 0.1f;
                //    this.ship_model.transform.localPosition = pos;
                //    break;
        }


    }


    private void get_input()
    {
        this.key.up = false;
        this.key.down = false;
        this.key.right = false;
        this.key.left = false;

        //상
        this.key.up |= Input.GetKey(KeyCode.UpArrow);
        this.key.up |= Input.GetKey(KeyCode.Keypad8);

        //하
        this.key.down |= Input.GetKey(KeyCode.DownArrow);
        this.key.down |= Input.GetKey(KeyCode.Keypad2);

        //우
        this.key.right |= Input.GetKey(KeyCode.RightArrow);
        this.key.right |= Input.GetKey(KeyCode.Keypad6);

        //좌
        this.key.left |= Input.GetKey(KeyCode.LeftArrow);
        this.key.left |= Input.GetKey(KeyCode.Keypad4);

        //Z키
        this.key.pick = Input.GetKeyDown(KeyCode.Z);
        this.key.action = Input.GetKeyDown(KeyCode.Z) | Input.GetKeyDown(KeyCode.X);

        //X키
        this.key.drop = Input.GetKeyDown(KeyCode.X);

        //스페이스 바
        this.key.hit = Input.GetKeyDown(KeyCode.Space);
    }

    private void move_control()
    {
        Vector3 move_vector = Vector3.zero; //이동용 벡터
        Vector3 position = this.transform.position; //현재 위치를 보관
        bool is_moved = false;

       /* if (this.key.right)
        { //우
            move_vector += transform.right; //이동용 벡터를 오른쪽으로 향한다.
            is_moved = true; //'이동 중' 플래그
        }

        if (this.key.left)
        { //좌
            move_vector += -transform.right; //이동용 벡터를 왼쪽으로 향한다.
            is_moved = true; //'이동 중' 플래그
        }*/

        if (this.key.up)
        {
            move_vector += transform.forward;
            is_moved = true;
            
        }

        if (this.key.down)
        {
            move_vector += -transform.forward;
            is_moved = true;
        }

        move_vector.Normalize(); //길이를 1로
        move_vector *= moveSpeed * Time.deltaTime; //속도 X 시간 = 거리
        position += move_vector; //위치를 이동
        position.y = 0.0f; //높이를 0

        //세계의 중앙에서 갱신한 위치까지의 거리가 섬의 반지름보다 크면
        if (position.magnitude > MOVE_AREA_RADIUS)
        {
            position.Normalize();
            position *= MOVE_AREA_RADIUS; //위치를 섬의 끝자락에 머물게 한다.
        }

        //새로 구한 위치의 높이를 현재 높이로 되돌린다.
        position.y = this.transform.position.y;
        //실제 위치를 새로 구한 위치로 변경한다.
        this.transform.position = position;

        Vector3 rot = this.transform.rotation.eulerAngles;
        //Rotation
        if (this.key.down)
        {
            if (this.key.left)
            {
                rot.y += rotSpeed;
            }
            if (this.key.right)
            {
                rot.y += -rotSpeed;

            }
        }
        else
        {
            if (this.key.left)
            {
                rot.y += -rotSpeed;
            }
            if (this.key.right)
            {
                rot.y += rotSpeed;

            }
        }

        if (rot.y >= 360.0f) rot.y -= 360.0f;
        else if (rot.y <= 360.0f) rot.y += 360.0f;
        this.transform.rotation = Quaternion.Euler(rot);
        //이동 벡터의 길이가 0.01보다 큰 경우
        //=어느 정도 이상 이동한 경우
        /*if (move_vector.magnitude > 0.01f)
        {
            if (this.key.down)
            {
                //캐릭터의 방향을 천천히 바꾼다.
                Quaternion q = Quaternion.LookRotation(-move_vector, Vector3.up);
                this.transform.rotation =
                    Quaternion.Lerp(this.transform.rotation, q, 0.01f);
            }
            else
            {
                //캐릭터의 방향을 천천히 바꾼다.
                Quaternion q = Quaternion.LookRotation(move_vector, Vector3.up);
                this.transform.rotation =
                    Quaternion.Lerp(this.transform.rotation, q, 0.01f);
            }
        }*/

        /*if (is_moved)
        {
            foreach (GameObject item in items)
            {
                //들고 있는 아이템에 따라 '체력 소모 정도'를 조사한다.
                float consume = this.item_root.getConsumeDurability(item);
                //가져온 '소모 정도'를 내구도에서 뺀다.
                this.game_status.addDurability(-consume * Time.deltaTime);
            }
        }*/

        if (is_moved)
        {
            animator.SetFloat("Speed", moveSpeed / MAX_MOVE_SPEED);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }

    }

    private bool is_event_ignitable()
    {
        bool ret = false;
        do
        {
            if (this.closest_event == null) //주목 이벤트가 없으면.
            {
                break; //false를 반환한다. 
            }

            if (this.items.Count == 0) break;
            ////들고 있는 아이템 종류를 가져온다.
            //Item.TYPE carried_item_type =
            //    this.item_root.getItemType(this.carried_item);

            ////들고 있는 아이템 종류와 주목하는 이벤트의 종류에서,
            ////이벤트가 가능한지 판정하고, 이벤트 불가라면 false를 반환한다.
            //if (!this.event_root.isEventIgnitable(carried_item_type, this.closest_event))
            //{
            //    break;
            //}
            ret = true; // 여기까지 오면 이벤트를 시작할 수 있다고 판정!
        } while (false);
        return (ret);
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject other_go = other.gameObject;

        //트리거의 GameObject 레이어 설정이 Item이라면
        if (other_go.layer == LayerMask.NameToLayer("Item"))
        {
            //이미 플레이어가 들고 있다면
            if (other.transform.parent != null && other.transform.parent.tag == "Player") return;
            //아무것도 주목하고 있지 않으면
            if (this.closest_item == null)
            {
                if (this.is_other_in_view(other_go)) //정면에 있으면
                {
                    this.closest_item = other_go; //주목한다
                }
                //뭔가 주목하고 있으면
                else if (this.closest_item == other_go)
                {
                    if (!this.is_other_in_view(other_go)) //정면에 없으면
                    {
                        this.closest_item = null; //주목을 그만둔다.
                    }
                }
            }
        }

        //트리거의 GameObject의 레이어 설정이 Event라면.
        else if (other_go.layer == LayerMask.NameToLayer("Event"))
        {
            //아무것도 주목하고 있지 않으면.
            if (this.closest_event == null)
            {
                if (this.is_other_in_view(other_go)) //정면에 있으면.
                {
                    this.closest_event = other_go; //주목한다.
                }
            }
            //뭔가에 주목하고 있으면.
            else if (this.closest_event == other_go)
            {
                if (!this.is_other_in_view(other_go)) //정면에 없으면.
                {
                    this.closest_event = null; //주목을 그만둔다.
                }
            }
        }
    }

    public void hitLava(float Damage)
    {
        this.game_status.addDurability(-Damage); //내구성 소모
        soundCtrl.PlaySFX("Alert");
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.closest_item == other.gameObject)
            this.closest_item = null; //주목을 그만둔다.

        if (this.closest_event == other.gameObject)
            this.closest_event = null; //주목을 그만둔다.
    }


    private void pick_or_drop_control()
    {
        do
        {
            if (!this.key.pick && !this.key.drop) //'줍기/버리기'키가 눌리지 않았으면.
            {
                break; // 종료
            }

            if (this.key.drop) //버리기 키가 눌리면
            {
                if (items.Count > 0) //들고 있는 아이템이 있다면
                {                   //내려놓기
                    GameObject carried_item = items[0];
                    carried_item.transform.localPosition = Vector3.forward * 1.0f + Vector3.up * 5.0f;
                    carried_item.transform.parent = null; //자식 설정을 해제

                    items.RemoveAt(0);
                    this.game_status.itemCount--;
                    uiCtrl.SetItemCount(game_status.itemCount);
                    addSpeed(game_status.getWeight(item_root.getItemType(carried_item)));

                    //효과음
                    soundCtrl.PlaySFX("DropItem");
                    return;
                }
                break;
            }

            if (this.key.pick)
            {
                if (this.closest_item == null) break;
                if (this.closest_item.tag == "Oil")
                {
                    break;
                } //오일은 바로 먹는다.

                if (items.Count < maxItemsCount) //아이템을 들 수 있다면
                {
                    this.closest_item.transform.parent = this.transform;
                    this.closest_item.transform.localPosition = Vector3.up * 2.0f * (items.Count + 1);
                    items.Add(this.closest_item);
                    //주목 중인 아이템을 들어올린다.

                    soundCtrl.PlaySFX("GetItem"); //효과음

                    //Rigidbody 때문에 Player가 물체에 닿으면 튕기므로 Field에 있을 땐 isKinematic을 꺼두고
                    //Player가 들었을 때만 isKinematic을 켜준다.
                    Rigidbody rigidbody = this.closest_item.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = false;
                    }

                    addSpeed(-1 * game_status.getWeight(item_root.getItemType(this.closest_item)));

                    this.closest_item = null;
                    this.game_status.itemCount++;
                    uiCtrl.SetItemCount(game_status.itemCount);

                }
            }
        } while (false);
    }

    //접촉한 물건이 자신의 정면에 있는지 판단한다.
    private bool is_other_in_view(GameObject other)
    {
        bool ret = false;
        float sightAngle = 120.0f;
        do
        {
            Vector3 heading = //자신이 현재 향하고 있는 방향을 보관
                this.transform.TransformDirection(Vector3.forward);
            Vector3 to_other = //자신 쪽에서 본 아이템의 방향을 보관
                other.transform.position - this.transform.position;
            heading.y = 0.0f;
            to_other.y = 0.0f;
            heading.Normalize(); //길이를 1
            to_other.Normalize(); //길이 1

            float angle = Vector3.Angle(heading, to_other);

            float dist = Vector3.Distance(other.transform.position, this.transform.position);


            if (dist < 5) //거리가 일정 이하일 때, 시야각 조정
            {
                sightAngle = 240.0f;
            }

            if (angle < sightAngle / 2)
            {
                ret = true;
            }
            /*float dp = Vector3.Dot(heading, to_other); //양쪽 벡터의 내적을 취득.
            if (dp < Mathf.Cos(45.0f)) // 내적이 45도인 코사인 값 미만이면
            {
                break;
            }
            ret = true; //내적이 45도인 코사인 값 이상이면 정면에 있음*/
        } while (false);
        return (ret);
    }

    //사과를 먹는 메시지 추가
   /* private void OnGUI()
    {
        float x = 20.0f;
        float y = Screen.height - 40.0f;

        if (this.closest_item != null) //앞에 오일이 있으면
        {
            if (this.closest_item.tag == "Oil")
                GUI.Label(new Rect(x, y, 200.0f, 80.0f), "Z: 마신다", guistyle);
            else
                GUI.Label(new Rect(x, y, 200.0f, 80.0f), "Z: 줍는다", guistyle);

        }
        else if (items.Count > 0) //들고 있는 게 있으면
        {
            GUI.Label(new Rect(x, y, 200.0f, 80.0f), "X: 버린다", guistyle);
            do
            {
                if (this.is_event_ignitable())
                {
                    break;
                }
            } while (false);
        }

        switch (this.step)
        {
            case STEP.EATING:
                GUI.Label(new Rect(Screen.width - 300.0f, y, 300.0f, 80.0f), "위잉위잉위잉잉잉잉......", guistyle);
                break;
            case STEP.LOADING:
                GUI.Label(new Rect(Screen.width - 300.0f, y, 300.0f, 80.0f), "음식 적재중", guistyle);
                break;
        }

        if (this.is_event_ignitable()) //이벤트가 시작 가능한 경우
        { //이벤트용 메시지를 취득
            string message = this.event_root.getIgnitableMessage(this.closest_event);
            GUI.Label(new Rect(x + 300.0f, y, 300.0f, 80.0f), message, guistyle);
        }

        //GUI.Label(new Rect(x, y - 40.0f, 200.0f, 20.0f), "속도: " + moveSpeed.ToString(), guistyle);
    }
   */
    void addSpeed(float value)
    {
        moveSpeed += value;
        float percentSpeed = moveSpeed / MAX_MOVE_SPEED * 100.0f;
        uiCtrl.SetSpeed(percentSpeed);
    }
}
