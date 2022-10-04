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
    [Header("패턴")]
    private int pattern_rand;
    [SerializeField] private GameObject rockTarget;
    private int baseAttackCount;
    public ChickenObjectPool objectPool = null;

    [Header("오레하 패턴")]
    [SerializeField] private Animator remember = null;

    [Header("납치 패턴")]
    [SerializeField] private GameObject eggGame = null;
    [SerializeField] private List<Egg> eggs = new List<Egg>();

    [Header("스핑크스")]
    [SerializeField] private GameObject sphinx = null;
    [SerializeField] private CHICKEN_ACTION action;
    public CHICKEN_ACTION getAction { get { return action; } }


    [Header("기타")]
    [SerializeField] public bool isThink = false;

    void Start()
    {
        base.BossInitialize();
        GameMng.I.boss = this;

        base._minDistance = 2.0f;

        if (NetworkMng.I.roomOwner)
        {
            StartCoroutine(SendRaidStartMsg());
        }
    }

    public void Setidle()
    {
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

            // if (!isThink && base._targetDistance < 3.0f && _target != null)
            // {
            //     isThink = true;
            //     Think();
            // }
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
    }

    public void Think()
    {
        int rand = 0;
        if (NetworkMng.I.roomOwner)
        {
            // if (baseAttackCount < 9)
            // {
            //     pattern_rand = Random.Range((int)CHICKEN_ACTION.IDLE + 1, (int)CHICKEN_ACTION.BASE_RETREAT + 1);
            //     // pattern_rand = (int)CHICKEN_ACTION.BASE_SPEAR;
            //     switch (pattern_rand)
            //     {
            //         case (int)CHICKEN_ACTION.IDLE:
            //             SendBossPattern(pattern_rand, getTarget);
            //             // isThink = false;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_SPEAR:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_OBA:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_ROAR:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_WING:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_JUMP_ATTACK:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_FOOT:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_FART:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //         case (int)CHICKEN_ACTION.BASE_RETREAT:
            //             SendBossPattern(pattern_rand);
            //             baseAttackCount++;
            //             break;
            //     }
            // }
            // else
            // {
            // pattern_rand = Random.Range((int)CHICKEN_ACTION.PATTERN_BIRDS, (int)CHICKEN_ACTION.PATTERN_REMEMBER + 1);
            // pattern_rand = (int)CHICKEN_ACTION.PATTERN_REMEMBER;
            // pattern_rand = (int)CHICKEN_ACTION.PATTERN_EGG;
            // switch (pattern_rand)
            // {
            //     case (int)CHICKEN_ACTION.PATTERN_BIRDS:
            //         rand = Random.Range(0, 4);
            //         SendBossPattern(pattern_rand, rand.ToString());
            //         baseAttackCount = 0;
            //         break;
            //     case (int)CHICKEN_ACTION.PATTERN_COUNTER_0:
            //         SendBossPattern(pattern_rand);
            //         baseAttackCount = 0;
            //         break;
            //     case (int)CHICKEN_ACTION.PATTERN_COUNTER_1:
            //         SendBossPattern(pattern_rand);
            //         baseAttackCount = 0;
            //         break;
            //     case (int)CHICKEN_ACTION.PATTERN_FALLING_ROCK:
            //         SendBossPattern(pattern_rand);
            //         baseAttackCount = 0;
            //         break;
            //     case (int)CHICKEN_ACTION.PATTERN_REMEMBER:
            //         action = CHICKEN_ACTION.PATTERN_REMEMBER;
            //         rand = Random.Range(0, 2);
            //         SendBossPattern(pattern_rand, rand.ToString());
            //         baseAttackCount = 0;
            //         break;
            //     case (int)CHICKEN_ACTION.PATTERN_EGG:
            //         baseAttackCount = 0;
            //         rand = Random.Range(0, 5);
            //         SendBossPattern(pattern_rand, rand.ToString() + ":" + getTarget);
            //         break;
            // }
            // }

            pattern_rand = (int)CHICKEN_ACTION.PATTERN_SPHINX;
            SendBossPattern(pattern_rand);
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
                Pattern_Sphinx();
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

    void Base_Spear()
    {
        action = CHICKEN_ACTION.BASE_SPEAR;
        animator.SetTrigger("Attack");
    }
    void Base_Oba()
    {
        action = CHICKEN_ACTION.BASE_OBA;
        animator.SetTrigger("EggBomb");
    }
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
    void Base_JumpAttack()
    {
        action = CHICKEN_ACTION.BASE_JUMP_ATTACK;
        animator.SetTrigger("JumpAttack");
    }
    void Base_Foot()
    {
        action = CHICKEN_ACTION.BASE_FOOT;
        animator.SetTrigger("FootAttack");
    }
    void Base_Fart()
    {
        action = CHICKEN_ACTION.BASE_FART;
        animator.SetTrigger("Fart");
    }
    void Base_RetReat()
    {
        action = CHICKEN_ACTION.BASE_RETREAT;
        animator.SetTrigger("Retreat");
    }

    IEnumerator Pattern_Bird(int rand)
    {
        action = CHICKEN_ACTION.PATTERN_BIRDS;
        animator.SetTrigger("CallBirds");

        yield return new WaitForSecondsRealtime(1.0f);

        objectPool.setBridActive(rand);
    }

    IEnumerator Pattern_Counter(int action)
    {
        int tempDmg = _currentHp;

        yield return new WaitForSecondsRealtime(1.0f);

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

    IEnumerator Pattern_FallingRock()
    {
        foreach (var trans in NetworkMng.I.v_users.Values)
        {
            GameObject temp = Instantiate(rockTarget, trans.transform.position, Quaternion.identity);
            temp.transform.SetParent(trans.transform);

            yield return new WaitForSecondsRealtime(1.0f);      // 에니메이션 
        }

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(2.0f);
            objectPool.setRockActive();
        }
    }

    IEnumerator Pattern_Remember(int rand)
    {
        action = CHICKEN_ACTION.PATTERN_REMEMBER;
        animator.SetTrigger("Fly");
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

    IEnumerator Pattern_Egg(int eggNum, string userNum)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(eggGame, new Vector3(-9.0f + (i * 6f), 1f, -6.59f), Quaternion.identity);
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

    void Egg_Break(int index)
    {
        eggs[index].transform.position = Vector3.zero;
        eggs[index].gameObject.SetActive(false);
    }

    void Pattern_Sphinx()
    {
        action = CHICKEN_ACTION.PATTERN_SPHINX;

        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(sphinx, new Vector3(-9.0f + (i * 6f), 1f, -6.59f), Quaternion.identity);
        }
    }
}
