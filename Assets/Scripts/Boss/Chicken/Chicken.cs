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
}

public class Chicken : Boss
{
    [Header("패턴")]
    private int pattern_rand;
    private int baseAttackCount;

    [SerializeField] private CHICKEN_ACTION action;

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

            if (!isThink && base._targetDistance < 3.0f && _target != null)
            {
                Think();
            }
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

    void Think()
    {
        isThink = true;
        if (NetworkMng.I.roomOwner)
        {
            // pattern_rand = Random.Range((int)CHICKEN_ACTION.IDLE + 1, (int)CHICKEN_ACTION.BASE_RETREAT + 1);
            pattern_rand = (int)CHICKEN_ACTION.BASE_SPEAR;
            switch (pattern_rand)
            {
                case (int)CHICKEN_ACTION.IDLE:
                    SendBossPattern(pattern_rand, getTarget);
                    // isThink = false;
                    break;
                case (int)CHICKEN_ACTION.BASE_SPEAR:
                    action = CHICKEN_ACTION.BASE_SPEAR;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_OBA:
                    action = CHICKEN_ACTION.BASE_OBA;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_ROAR:
                    action = CHICKEN_ACTION.BASE_ROAR;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_WING:
                    action = CHICKEN_ACTION.BASE_WING;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_JUMP_ATTACK:
                    action = CHICKEN_ACTION.BASE_JUMP_ATTACK;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FOOT:
                    action = CHICKEN_ACTION.BASE_FOOT;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FART:
                    action = CHICKEN_ACTION.BASE_FART;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_RETREAT:
                    action = CHICKEN_ACTION.BASE_RETREAT;
                    SendBossPattern(pattern_rand);
                    baseAttackCount++;
                    break;
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
        }
    }

    public override void Raid_Start()
    {
        foreach (var trans in NetworkMng.I.v_users)
        {
            targetList.Add(trans.Key);
        }
        NetworkMng.I.v_users.Add(NetworkMng.I.uniqueNumber, GameMng.I.character);

        animator.SetTrigger("idle");

        if (NetworkMng.I.roomOwner)
        {
            SendBossPattern((int)CHICKEN_ACTION.IDLE, NetworkMng.I.uniqueNumber);
        }

        // StartCoroutine(Think());
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
}
