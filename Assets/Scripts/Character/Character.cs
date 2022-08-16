using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CHARACTER_ACTION
{
    IDLE,                   // 일반 상태, 움직이거나 스킬 사용이 가능한 상태
    CANT_ANYTHING,          // 스킬 쓰는 상태, 아무것도 못함
    SLEEP_CANT_ANYTHING,    // 기절 상태, 기상기 외에는 아무것도 못함
    CAN_MOVE                // 스킬 쓰는 상태, 캔슬이 가능한 상태
}

public struct Stat
{
    public Stat(int minDamage, int maxDamage,
    float incDamagePer, float takenDamagePer, float moveSpeedPer, float takenHealPer, float criticalPer, float incBackattackPer)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.incDamagePer = incDamagePer;
        this.takenDamagePer = takenDamagePer;
        this.moveSpeedPer = moveSpeedPer;
        this.takenHealPer = takenHealPer;
        this.criticalPer = criticalPer;
        this.incBackattackPer = incBackattackPer;
    }
    public int minDamage, maxDamage;    // 실 최소 ~ 최대 데미지
    public float incDamagePer;          // 공격력 증가 퍼센트
    public float takenDamagePer;        // 받는 피해량 퍼센트
    public float moveSpeedPer;          // 이동 속도 퍼센트
    public float takenHealPer;          // 받는 회복량 퍼센트
    public float criticalPer;           // 치명타 확률  ex) 값이 10이라면 10%
    public float incBackattackPer;      // 백어택 증가량 퍼센트  ex) 1.2 라면  데미지 120%
}

public enum SKILL_CODE
{
    SKILL_1,
    SKILL_2,
    SKILL_3,
    SKILL_4,
    SKILL_5,
    DASH,
    WAKEUP,
    ATTACK
}

public class Character : MonoBehaviour
{
    // 캐릭터 ====================================================================================================
    public Animator _anim;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] BoxCollider2D _collider;
    public void setTriggerSleep() => _anim.SetTrigger("Sleep");

    // 데이터 ====================================================================================================
    public bool _isPlayer = false;
    public string nickName = "";
    public JOB _job = JOB.NONE;
    const float MAX_DASH_TIME = 0.1f;
    public float curDashTime = 0.1f;
    /*========= 직업에 따라서 아래 수치가 다름 =========*/
    public static Stat _stat;
    protected static float DASH_SPEED = 20;
    protected static float MOVE_SPEED = 5;
    protected static float DASH_COOLTIME = 6;
    protected static float WAKEUP_COOLTIME = 10;

    // 행동 ======================================================================================================
    Vector3 _moveDir;       // 캐릭터 움직이는 방향
    bool isMoving = false;
    public CHARACTER_ACTION _action;

    // 스킬 ======================================================================================================
    public SkillData[] skilldatas = new SkillData[5];
    public static SkillData usingSkill;
    private bool[] checkSkill = new bool[7];    // 스킬5개 + 대쉬 + 기상기
    public bool[] usingBattleItem = new bool[3];

    // 발자국 =====================================================================================================
    int footprintIdx = 0;
    [SerializeField] GameObject[] footprints;

    // 아이템 ====================================================================================================
    public static Item[] equipBattleItem = new Item[3];
    public static List<List<Item>> haveItem = new List<List<Item>>();

    private void Awake()
    {
        GameMng.I.stateMng.targetList.Add(this);        // 파티를 들어갔을떄
    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            footprints[i] = Instantiate(footprints[3], Vector3.zero, Quaternion.identity) as GameObject;
        }
        //GameMng.I.character = this;
        // GameMng.I.stateMng.targetList.Add(this);        // 파티를 들어갔을떄
        _action = CHARACTER_ACTION.IDLE;
        init();
        for (int i = 0; i < 4; i++)
            haveItem.Add(new List<Item>());
    }

    void Update()
    {
        if (_isPlayer)
            inputKey();
    }

    void FixedUpdate()
    {
        inputMove();
    }

    protected IEnumerator SkillCoolDown(int skillnum)        // <! 나중에 바꾸기
    {
        float cooltime = 0;

        // 대쉬
        if (skillnum == 5)
        {
            GameMng.I.skill_Img[skillnum].transform.parent.gameObject.SetActive(true);
            cooltime = DASH_COOLTIME;
        }
        // 기상기
        else if (skillnum == 6)
        {
            GameMng.I.skill_Img[skillnum].transform.parent.gameObject.SetActive(true);
            cooltime = WAKEUP_COOLTIME;
        }
        // 스킬
        else
        {
            usingSkill = skilldatas[skillnum];
            if (usingSkill.getBuffData && usingSkill.getBuffData.isBuff)
            {
                GameMng.I.stateMng.ActiveBuff(usingSkill.getBuffData);
            }
            cooltime = usingSkill.getColldown;
        }

        checkSkill[skillnum] = true;
        float time = 0.0f;
        GameMng.I.skill_Img[skillnum].color = new Color32(175, 175, 175, 255);
        GameMng.I.cooltime_UI[skillnum].gameObject.SetActive(true);

        while (time < cooltime)
        {
            time += Time.deltaTime;
            GameMng.I.skill_Img[skillnum].fillAmount = time / cooltime;
            GameMng.I.cooltime_UI[skillnum].text = Mathf.Floor(cooltime - time).ToString();
            yield return new WaitForEndOfFrame();
        }

        GameMng.I.cooltime_UI[skillnum].gameObject.SetActive(false);
        GameMng.I.skill_Img[skillnum].color = Color.white;
        checkSkill[skillnum] = false;

        if (skillnum == 5 || skillnum == 6)
        {
            GameMng.I.skill_Img[skillnum].transform.parent.gameObject.SetActive(false);
        }

    }

    protected IEnumerator BattleItemCoolDown(int itemnum)
    {
        usingBattleItem[itemnum] = true;
        equipBattleItem[itemnum].itemCount--;
        GameMng.I.BattleItemUI.ItemText[itemnum].text = equipBattleItem[itemnum].itemCount.ToString();
        float cooltime = equipBattleItem[itemnum].itemData.duration;

        float time = 0.0f;
        GameMng.I.battleItem_Img[itemnum].color = new Color32(175, 175, 175, 255);

        while (time < cooltime)
        {
            time += Time.deltaTime;
            GameMng.I.battleItem_Img[itemnum].fillAmount = time / cooltime;
            yield return new WaitForEndOfFrame();
        }

        GameMng.I.battleItem_Img[itemnum].color = Color.white;
        usingBattleItem[itemnum] = false;
    }

    void startMoving()
    {
        if (!isMoving)
        {
            _anim.SetBool("Move", true);
            isMoving = true;
            StartCoroutine(showFootprint());
        }
    }

    IEnumerator showFootprint()
    {
        footprints[footprintIdx].SetActive(true);
        footprints[footprintIdx].transform.position = transform.position - new Vector3(0, 0.65f, 0);
        footprintIdx = footprintIdx >= 2 ? 0 : footprintIdx + 1;

        yield return new WaitForSeconds(0.4f);

        if (_action == CHARACTER_ACTION.CANT_ANYTHING)
            isMoving = false;

        if (isMoving)
            StartCoroutine(showFootprint());

    }

    void inputMove()
    {
        Vector3 moveDist = _moveDir.normalized * Time.deltaTime;
        _rigidBody.MovePosition(transform.position + moveDist * MOVE_SPEED);

        // 대시 이동
        if (curDashTime < MAX_DASH_TIME)
        {
            curDashTime += Time.deltaTime;
            _rigidBody.MovePosition(transform.position + moveDist * DASH_SPEED);
        }

        // 이동 애니메이션 관리
        if (_moveDir.x != 0 || _moveDir.y != 0)
        {
            startMoving();
        }
        else
        {
            isMoving = false;
            _anim.SetBool("Move", false);
        }
    }

    void inputKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !checkSkill[6] && _action == CHARACTER_ACTION.SLEEP_CANT_ANYTHING)
        {
            wakeup();
            // NetworkMng.I.UseSkill(SKILL_CODE.WAKEUP);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !checkSkill[5] && _action != CHARACTER_ACTION.SLEEP_CANT_ANYTHING)
        {
            dash();
            // NetworkMng.I.UseSkill(SKILL_CODE.DASH);
        }

        // 아무것도 아닌 상태가 아닌 경우는 이동이 가능한 상태
        if (_action == CHARACTER_ACTION.CANT_ANYTHING || _action == CHARACTER_ACTION.SLEEP_CANT_ANYTHING)
        {
            _moveDir.x = 0;
            _moveDir.y = 0;
            return;
        }

        // 이동
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        // 방향
        if (_moveDir.x < 0)
            transform.rotation = Quaternion.Euler(Vector3.zero);
        else if (_moveDir.x > 0)
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));

        // 모든 스킬은 IDLE 상태에서만 가능하기 때문에 체크함
        if (_action != CHARACTER_ACTION.IDLE)
            return;

        // 스킬
        if (Input.GetKeyDown(KeyCode.Q) && !checkSkill[0])
            skill_1();
        else if (Input.GetKeyDown(KeyCode.E) && !checkSkill[1])
            skill_2();
        else if (Input.GetKeyDown(KeyCode.R) && !checkSkill[2])
            skill_3();
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !checkSkill[3])
            skill_4();
        else if (Input.GetKeyDown(KeyCode.F) && !checkSkill[4])
            skill_5();

        // 임시 기절 키
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            sleep();
        }

        // 마우스 좌클릭 - 일반 공격
        if (Input.GetMouseButtonDown(0) && GameMng.I.dailogUI == null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // NetworkMng.I.UseSkill(SKILL_CODE.ATTACK, Input.mousePosition);
            attack(Input.mousePosition);
        }
        // 핑
        else if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
        {
            GameMng.I.createPing(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        // 마우스 우클릭 - 상호작용
        else if (Input.GetMouseButtonDown(1))
        {
            //GameMng.I.mouseRaycast(this.transform.localPosition);
        }

        // 배틀 아이템 사용
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (equipBattleItem[0] != null && equipBattleItem[0].itemCount > 0 && !usingBattleItem[0])
            {
                useItem(equipBattleItem[0].itemData.itemIndex);
                StartCoroutine(BattleItemCoolDown(0));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (equipBattleItem[1] != null && equipBattleItem[1].itemCount > 0 && !usingBattleItem[1])
            {
                useItem(equipBattleItem[1].itemData.itemIndex);
                StartCoroutine(BattleItemCoolDown(1));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (equipBattleItem[2] != null && equipBattleItem[2].itemCount > 0 && !usingBattleItem[2])
            {
                useItem(equipBattleItem[2].itemData.itemIndex);
                StartCoroutine(BattleItemCoolDown(2));
            }
        }
    }

    void dash()
    {
        StartCoroutine(SkillCoolDown(5));
        _action = CHARACTER_ACTION.CAN_MOVE;
        _anim.SetTrigger("Dash");
        curDashTime = 0.0f;
    }

    void wakeup()
    {
        StartCoroutine(SkillCoolDown(6));
        _action = CHARACTER_ACTION.CAN_MOVE;
        _anim.SetTrigger("Wakeup");
    }

    public virtual void init() { }
    public virtual void skill_1() { }
    public virtual void skill_2() { }
    public virtual void skill_3() { }
    public virtual void skill_4() { }
    public virtual void skill_5() { }

    public void attack(Vector2 attackDir)
    {
        // 좌우 반전
        if (attackDir.x < Screen.width / 2)
            transform.rotation = Quaternion.Euler(Vector3.zero);
        else
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));


        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Attack");
    }

    void endAct()
    {
        _action = CHARACTER_ACTION.IDLE;
        usingSkill = null;
    }

    public void sleep()
    {
        _action = CHARACTER_ACTION.SLEEP_CANT_ANYTHING;
        _anim.SetTrigger("Sleep");
    }

    IEnumerator SpeedUp(int apply_count, float saveSpeed)
    {
        yield return new WaitForSeconds(1.0f);
        if (apply_count >= 1)
            StartCoroutine(SpeedUp(apply_count - 1, saveSpeed));
        else if (apply_count == 0)
            MOVE_SPEED = saveSpeed;
    }

    IEnumerator itemCool(Item item)
    {
        yield return new WaitForSeconds(1.0f);
        if (item.apply_count >= 1)
        {
            item.apply_count--;
            StartCoroutine(itemCool(item));
        }
        else
            item.apply_count = 0;
    }

    void useItem(ITEM_INDEX kind)
    {
        switch (kind)
        {
            case ITEM_INDEX.POTION:
                if (GameMng.I.stateMng.user_HP_Numerical.Hp < GameMng.I.stateMng.user_HP_Numerical.fullHp)
                    GameMng.I.stateMng.user_HP_Numerical.Hp += (int)(GameMng.I.stateMng.user_HP_Numerical.fullHp * 30 / 100);
                if (GameMng.I.stateMng.user_HP_Numerical.Hp > GameMng.I.stateMng.user_HP_Numerical.fullHp)
                    GameMng.I.stateMng.user_HP_Numerical.Hp = GameMng.I.stateMng.user_HP_Numerical.fullHp;
                break;

            case ITEM_INDEX.CLEANSER:
                if (GameMng.I.stateMng.nPlayerDeBuffCount > 0)
                {
                    int rand = Random.Range(1, GameMng.I.stateMng.nPlayerDeBuffCount + 1);
                }
                break;

            case ITEM_INDEX.SPEEDUP:

                float save = MOVE_SPEED;
                MOVE_SPEED = MOVE_SPEED * 200 / 100;
                StartCoroutine(SpeedUp(5, save));
                break;
        }
    }

    public void isMe()
    {
        _isPlayer = true;
        _collider.enabled = true;
    }

    void OnDestroy()
    {
        GameMng.I.stateMng.targetList.Remove(this);
    }
}