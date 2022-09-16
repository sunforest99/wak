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

    [Header("기타")]
    protected bool isThink = false;

    void Start()
    {
        base.BossInitialize();

        GameMng.I.boss = this;

        if (NetworkMng.I.roomOwner)
        {
            StartCoroutine(SendRaidStartMsg());
        }
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
            base.ChangeHpbar();
            base.RaidTimer();
            base.ChangeHpText();

            if (base._targetDistance < 6f && !isThink)
            {
                Think();
            }
        }
        else
        {
            base.SetZeroHp();
        }
    }

    public void Think()
    {
        isThink = true;

        if (NetworkMng.I.roomOwner)
        {
            pattern_rand = Random.Range((int)CHICKEN_ACTION.IDLE, (int)CHICKEN_ACTION.BASE_RETREAT + 1);
            switch (pattern_rand)
            {
                case (int)CHICKEN_ACTION.IDLE:
                    SendBossPattern(WAKGUI_ACTION.IDLE, getTarget);
                    break;
                case (int)CHICKEN_ACTION.BASE_SPEAR:
                    SendBossPattern(WAKGUI_ACTION.BASE_STAP);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_OBA:
                    SendBossPattern(WAKGUI_ACTION.BASE_SLASH);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_ROAR:
                    SendBossPattern(WAKGUI_ACTION.BASE_ROAR);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_WING:
                    SendBossPattern(WAKGUI_ACTION.BASE_RUSH);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_JUMP_ATTACK:
                    SendBossPattern(WAKGUI_ACTION.BASE_RUSH);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FOOT:
                    SendBossPattern(WAKGUI_ACTION.BASE_RUSH);
                    baseAttackCount++;
                    break;
                case (int)CHICKEN_ACTION.BASE_FART:
                    SendBossPattern(WAKGUI_ACTION.BASE_RUSH);
                    baseAttackCount++;
                    break;
            }
        }
    }

    public override void Raid_Start()
    {
        foreach (var trans in NetworkMng.I.v_users)
        {
            // targetList.Add(trans.Key);
        }
        NetworkMng.I.v_users.Add(NetworkMng.I.uniqueNumber, GameMng.I.character);

        animator.SetTrigger("idle");

        if (NetworkMng.I.roomOwner)
        {
            SendBossPattern(WAKGUI_ACTION.IDLE, NetworkMng.I.uniqueNumber);
        }
        // StartCoroutine(Think());
    }

    public override void Action(string msg)
    {

    }
}
