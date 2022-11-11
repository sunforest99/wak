using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHICKEN_ACTION
{
    IDLE,
    BASE_SPEAR,             // 기본 내려찍기 (기본 평타라 애니메이션은 Attack)
    BASE_OBA,               // 기본 오뱅알
    BASE_ROAR,              // 기본 포효
    BASE_WING,              // 기본 윙 (날개 펼쳐서 날개 공격)
    BASE_JUMP_ATTACK,       // 기본 점프 공격
    BASE_FOOT,              // 기본 발로차기
    BASE_FART,              // 기본 방구
    BASE_RETREAT,           // 기본 후퇴
    PATTERN_BIRDS,          // 패턴 새떼
    PATTERN_COUNTER_0,      // 패턴 카운터 (반격기)
    PATTERN_COUNTER_1,      // 패턴 카운터 (비반격기)
    PATTERN_FALLING_ROCK,   // 패턴 낙석
    PATTERN_REMEMBER,       // 패턴 오레하(기억해서 피하기)
    PATTERN_EGG,
    PATTERN_SPHINX,
    EGG_BROKEN
}

public class Chicken : Boss
{
    [Header("[ 패턴 ]")]
    private int pattern_rand;
    [SerializeField] private GameObject rockTarget;
    private int baseAttackCount;
    [SerializeField] BirdPool birdPool;
    [SerializeField] RockPool rockPool;

    [Header("[ 오레하 패턴 ]")]
    [SerializeField] private GameObject Oreha = null;
    [SerializeField] private Animator remember = null;

    [Header("[ 납치 패턴 ]")]
    [SerializeField] private GameObject eggGame = null;
    [SerializeField] private List<Egg> eggs = new List<Egg>();

    [Header("[ 스핑크스 ]")]
    [SerializeField] private Sphinx[] sphinx = new Sphinx[2];       // 귀찮으니 
    public TMPro.TextMeshPro question;

    [Header("[ 보스 행동 ]")]
    [SerializeField] private CHICKEN_ACTION action;
    public CHICKEN_ACTION getAction { get { return action; } set { action = value; } }


    [Header("[ 기타 ]")]
    [SerializeField] public bool isThink = false;

    void Start()
    {
        base.BossInitialize();
        GameMng.I.boss = this;

        base._minDistance = 1.5f;

        if (NetworkMng.I.roomOwner)
        {
            StartCoroutine(SendRaidStartMsg());
        }
    }

    public void SetIdle()
    {
        StartCoroutine(setIdle());

    }
    IEnumerator setIdle()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        this.isThink = false;
        action = CHICKEN_ACTION.IDLE;
    }

    IEnumerator SendRaidStartMsg()
    {
        yield return new WaitForSeconds(3.0f);
        NetworkMng.I.SendMsg("RAID_START");
    }

    void Update()
    {
        if (_currentHp >= 0)
        {
            if (_target != null)
                _targetDistance = Vector3.Distance(_target.position, this.transform.position);
            base.ChangeHpbar();
            base.RaidTimer();
            base.ChangeHpText();
        }
        else
        {
            base.SetZeroHp();
        }
    }

    private void FixedUpdate()
    {
        if (_currentHp >= 0 && action == CHICKEN_ACTION.IDLE && _target != null)
            base.BossMove();
        else if (_currentHp >= 0 && action == CHICKEN_ACTION.BASE_OBA && _target != null)
            BossMoveHorizon();
    }

    /**
     * @brief 보스 이동 (좌우만)
     */
    protected void BossMoveHorizon()
    {
        Vector3 dir = Vector3.right * (isRFlip ? 3 : -3);

        rigid.MovePosition(Vector3.MoveTowards(
            this.transform.position,
            this.transform.position + dir,
            bossdata.getMoveSpeed * Time.deltaTime));
    }


    public void Think()
    {
        int rand = 0;
        if (NetworkMng.I.roomOwner)
        {
            if (baseAttackCount < 9)
            {
            pattern_rand = Random.Range((int)CHICKEN_ACTION.IDLE + 1, (int)CHICKEN_ACTION.BASE_RETREAT + 1);
            // pattern_rand = (int)CHICKEN_ACTION.BASE_RETREAT;
            switch (pattern_rand)
            {
                case (int)CHICKEN_ACTION.IDLE:
                    SendBossPattern(pattern_rand, getTarget);
                    // isThink = false;
                    break;
                case (int)CHICKEN_ACTION.BASE_SPEAR:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_OBA:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_ROAR:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_WING:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_JUMP_ATTACK:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FOOT:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FART:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_RETREAT:
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
            }
            }
            else
            {
                pattern_rand = Random.Range((int)CHICKEN_ACTION.PATTERN_BIRDS, (int)CHICKEN_ACTION.PATTERN_SPHINX + 1);
                // pattern_rand = (int)CHICKEN_ACTION.PATTERN_BIRDS;
                switch (pattern_rand)
                {
                    case (int)CHICKEN_ACTION.PATTERN_BIRDS:
                        rand = Random.Range(0, 4);
                        SendBossPattern(pattern_rand, rand.ToString());
                        baseAttackCount = 0;
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_COUNTER_0:
                        SendBossPattern(pattern_rand);
                        baseAttackCount = 0;
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_COUNTER_1:
                        SendBossPattern(pattern_rand);
                        baseAttackCount = 0;
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_FALLING_ROCK:
                        SendBossPattern(pattern_rand);
                        baseAttackCount = 0;
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_REMEMBER:
                        action = CHICKEN_ACTION.PATTERN_REMEMBER;
                        rand = Random.Range(0, 2);
                        SendBossPattern(pattern_rand, rand.ToString());
                        baseAttackCount = 0;
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_EGG:
                        baseAttackCount = 0;
                        rand = Random.Range(0, 5);
                        SendBossPattern(pattern_rand, rand.ToString() + ":" + getTarget);
                        break;
                    case (int)CHICKEN_ACTION.PATTERN_SPHINX:
                        baseAttackCount = 0;
                        rand = Random.Range(0, 4);
                        SendBossPattern(pattern_rand, rand.ToString());
                        break;
                }
            }
        }
    }

    public override void Action(string msg)
    {
        string[] txt = msg.Split(":");
        switch (int.Parse(txt[1]))
        {
            case (int)CHICKEN_ACTION.IDLE:
                _target = NetworkMng.I.v_users[txt[2]].transform.parent;
                isThink = false;
                break;
            case (int)CHICKEN_ACTION.BASE_SPEAR:
                Base_Spear();
                break;
            case (int)CHICKEN_ACTION.BASE_OBA:
                Base_Oba();
                break;
            case (int)CHICKEN_ACTION.BASE_ROAR:
                Base_Roar();
                break;
            case (int)CHICKEN_ACTION.BASE_WING:
                Base_Wing();
                break;
            case (int)CHICKEN_ACTION.BASE_JUMP_ATTACK:
                Base_JumpAttack();
                break;
            case (int)CHICKEN_ACTION.BASE_FOOT:
                Base_Foot();
                break;
            case (int)CHICKEN_ACTION.BASE_FART:
                Base_Fart();
                break;
            case (int)CHICKEN_ACTION.BASE_RETREAT:
                Base_RetReat();
                break;
            case (int)CHICKEN_ACTION.PATTERN_BIRDS:     // ---- 이 아래부터 패턴 9 ~ 13
                StartCoroutine(Pattern_Bird(int.Parse(txt[2])));
                break;
            case (int)CHICKEN_ACTION.PATTERN_COUNTER_0:
                StartCoroutine(Pattern_Counter(int.Parse(txt[2])));
                break;
            case (int)CHICKEN_ACTION.PATTERN_COUNTER_1:
                StartCoroutine(Pattern_Counter(int.Parse(txt[2])));
                break;
            case (int)CHICKEN_ACTION.PATTERN_FALLING_ROCK:
                StartCoroutine(Pattern_FallingRock());
                break;
            case (int)CHICKEN_ACTION.PATTERN_REMEMBER:
                StartCoroutine(Pattern_Remember(int.Parse(txt[2])));
                break;
            case (int)CHICKEN_ACTION.PATTERN_EGG:
                StartCoroutine(Pattern_Egg(int.Parse(txt[2]), txt[3]));
                break;
            case (int)CHICKEN_ACTION.EGG_BROKEN:
                Egg_Break(int.Parse(txt[2]));
                break;
            case (int)CHICKEN_ACTION.PATTERN_SPHINX:
                Pattern_Sphinx(int.Parse(txt[2]));
                break;
        }
    }

    public override void Raid_Start()
    {
        NetworkMng.I.v_users.Add(NetworkMng.I.uniqueNumber, GameMng.I.character);

        foreach (var trans in NetworkMng.I.v_users)
        {
            targetList.Add(trans.Key);
        }

        animator.SetTrigger("idle");

        if (NetworkMng.I.roomOwner)
        {
            SendBossPattern((int)CHICKEN_ACTION.IDLE, NetworkMng.I.uniqueNumber);
        }
    }

    // 내려찍기
    void Base_Spear()
    {
        action = CHICKEN_ACTION.BASE_SPEAR;
        animator.SetTrigger("Attack");
    }

    // 오뱅알 EggBomb
    void Base_Oba()
    {
        action = CHICKEN_ACTION.BASE_OBA;
        animator.SetTrigger("EggBomb");
        
        if (this.transform.position.x > 0 && !isRFlip)
        {
            isRFlip = true;
            isLFlip = false;
            this.bossO.localRotation = Quaternion.Euler(-20, 180, 0);
        }
        else if (this.transform.position.x < 0 && !isLFlip)
        {
            isLFlip = true;
            isRFlip = false;
            this.bossO.localRotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        }
    }

    // 표효
    void Base_Roar()
    {
        action = CHICKEN_ACTION.BASE_ROAR;
        animator.SetTrigger("Roar");
    }
    void Base_Wing()
    {
        action = CHICKEN_ACTION.BASE_WING;
        animator.SetTrigger("Wing");
    }

    // 내려찍기
    void Base_JumpAttack()
    {
        action = CHICKEN_ACTION.BASE_JUMP_ATTACK;
        animator.SetTrigger("JumpAttack");
    }

    // 발준때 페턴
    void Base_Foot()
    {
        action = CHICKEN_ACTION.BASE_FOOT;
        animator.SetTrigger("FootAttack");
    }

    // 패턴 방귀
    void Base_Fart()
    {
        action = CHICKEN_ACTION.BASE_FART;
        animator.SetTrigger("Fart");
    }

    // 후퇴 패턴
    void Base_RetReat()
    {
        action = CHICKEN_ACTION.BASE_RETREAT;
        animator.SetTrigger("Retreat");
    }

    // 새떼 패턴
    IEnumerator Pattern_Bird(int rand)
    {
        action = CHICKEN_ACTION.PATTERN_BIRDS;
        animator.SetTrigger("CallBirds");

        yield return new WaitForSecondsRealtime(1.0f);

        // 여기
        birdPool.Create(this.transform, rand);
    }

    // 반격 패턴
    IEnumerator Pattern_Counter(int action)
    {
        int tempDmg = _currentHp;

        yield return new WaitForSecondsRealtime(0.8f);

        if (action == (int)CHICKEN_ACTION.PATTERN_COUNTER_0)        // 반격
        {
            this.action = CHICKEN_ACTION.PATTERN_COUNTER_0;
            animator.SetTrigger("Counter_0");
            while (this.action == CHICKEN_ACTION.PATTERN_COUNTER_0)
            {
                if (tempDmg - _currentHp > 1000000)
                {
                    animator.SetTrigger("Annihilate");
                    break;
                }

                yield return null;
            }
        }

        else if (action == (int)CHICKEN_ACTION.PATTERN_COUNTER_1)       // 딜 넣어야함
        {
            this.action = CHICKEN_ACTION.PATTERN_COUNTER_1;
            animator.SetTrigger("Counter_1");
            while (true)
            {
                if (this.action != CHICKEN_ACTION.PATTERN_COUNTER_1 && tempDmg - _currentHp < 1000000)
                {
                    animator.SetTrigger("Annihilate");
                    break;
                }
                else if (this.action != CHICKEN_ACTION.PATTERN_COUNTER_1)
                {
                    break;
                }

                yield return null;
            }
        }
    }

    // 네트워크 테스트 해보기
    IEnumerator Pattern_FallingRock()
    {
        action = CHICKEN_ACTION.PATTERN_FALLING_ROCK;

        animator.SetTrigger("FallingRock");

        for (int i = 0; i < 3; i++)
        {
            foreach (var trans in NetworkMng.I.v_users.Values)
            {
                GameObject temp = Instantiate(rockTarget, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
                temp.transform.SetParent(trans.transform.parent);
                temp.transform.localPosition = new Vector3(0, 0.0f, -0.5f);
                yield return new WaitForSecondsRealtime(1f);      // 1초 뒤에 꺼지게하고
                Destroy(temp);
            }
            yield return new WaitForSecondsRealtime(1.0f);   // 돌 생성 2초간격으로
            foreach (var trans in NetworkMng.I.v_users.Values)
            {
                rockPool.Create(trans.transform);
            }
        }
    }

    // 오레하 패턴 대미지 모션? 필요
    IEnumerator Pattern_Remember(int rand)
    {
        action = CHICKEN_ACTION.PATTERN_REMEMBER;
        animator.SetTrigger("Fly");
        Oreha.SetActive(true);
        yield return new WaitForSecondsRealtime(5.0f);
        switch (rand)
        {
            case 0:
                remember.SetTrigger("remember_0");
                break;
            case 1:
                remember.SetTrigger("remember_1");
                break;
        }
    }

    // 확정 사살기
    IEnumerator Pattern_Egg(int eggNum, string userNum)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(eggGame, new Vector3(-11.0f + (i * 7f), 1f, -6.59f), Quaternion.identity);
            temp.transform.localScale = new Vector3(2f, 2f, 2f);
            eggs.Add(temp.GetComponent<Egg>());
            eggs[i].uniqueNum = i;
            if (i == eggNum)
            {
                eggs[i].character = NetworkMng.I.v_users[userNum];  // 타깃 설정 
                eggs[i].character._action = CHARACTER_ACTION.CANT_ANYTHING;     // 타깃 못움직이게 설정
                eggs[i].character.gameObject.transform.position = eggs[i].transform.position;   // 타깃 위치 설정 
                eggs[i].character.tag = "Untagged";
            }
        }

        yield return new WaitForSecondsRealtime(10.0f);

        for (int i = 0; i < eggs.Count; i++)
        {
            if (eggs[i].gameObject.activeSelf)
            {
                if (eggs[i].character != null)
                {
                    // 대충 유저 죽음
                    NetworkMng.I.SendMsg(string.Format("USER_DIE:{0}", eggs[i].characterId));
                }
            }
        }
    }

    // 알 파괴
    void Egg_Break(int index)
    {
        eggs[index].transform.position = Vector3.zero;
        eggs[index].gameObject.SetActive(false);
    }

    void Pattern_Sphinx(int rand)
    {
        action = CHICKEN_ACTION.PATTERN_SPHINX;

        question.gameObject.SetActive(true);
        for (int i = 0; i < sphinx.Length; i++)
        {
            sphinx[i].gameObject.SetActive(true);
        }

        switch (rand)
        {
            case 0:
                question.text = "문제 1";
                sphinx[0]._isAnswer = true;
                break;
            case 1:
                question.text = "문제 2";
                sphinx[1]._isAnswer = true;
                break;
            case 2:
                question.text = "문제 3";
                sphinx[0]._isAnswer = true;
                break;
            case 3:
                question.text = "문제 4";
                sphinx[1]._isAnswer = true;
                break;
        }
    }

    [SerializeField] GameObject eggBomb;
    void EggBombSpawn()
    {
        Instantiate(eggBomb, new Vector3(transform.position.x, 0.324f, transform.position.z), Quaternion.Euler(20, 0, 0));
    }
}
