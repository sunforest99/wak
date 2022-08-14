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
    void Start()
    {
        base.BossInitialize();
        GameMng.I.bossData = this.bossdata;
        StartCoroutine(Think());
    }
    
    IEnumerator Think()
    {
        yield return new WaitForSeconds(3.0f);

    }
}
