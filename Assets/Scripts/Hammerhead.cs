using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HAMMERHEAD_ACTION
{
    NONE,
    BASE_ATTACK_1,
    BASE_ATTACK_2,
    BASE_ATTACK_3,
    BASE_ATTACK_4,
    PATTERN_1,
    PATTERN_2,
    PATTERN_3,
    PATTERN_4,
    ANNIHILATION
}

public class Hammerhead : Boss
{
    public HAMMERHEAD_ACTION action;

    private int rand;

    void Start()
    {
        radetime = 720.0f;
        bossName = "귀상어두";
        moveSpeed = 0.3f;
        startHp = 14242442;
        bossnameText.text = bossName;
        currentHp = startHp;
        StartCoroutine(Think());
    }

    void Update()
    {
        if (currentHp >= 0)
        {
            base.RaidTimer();
            base.ChangeHpText();
            base.ChangeHpbar();
            base.BossMove();
        }
        else
        {
            base.SetHpText();
        }
    }

    IEnumerator Think()
    {
        action = HAMMERHEAD_ACTION.NONE;
        yield return new WaitForSeconds(1.0f);

        rand = Random.Range((int)HAMMERHEAD_ACTION.NONE, (int)HAMMERHEAD_ACTION.BASE_ATTACK_4);

        switch (rand)
        {
            case (int)HAMMERHEAD_ACTION.NONE:
                StartCoroutine(Think());
                break;
            case (int)HAMMERHEAD_ACTION.BASE_ATTACK_1:
                StartCoroutine(BaseAttack1());
                break;
            case (int)HAMMERHEAD_ACTION.BASE_ATTACK_2:
                StartCoroutine(BaseAttack2());
                break;
            case (int)HAMMERHEAD_ACTION.BASE_ATTACK_3:
                StartCoroutine(BaseAttack3());
                break;
            case (int)HAMMERHEAD_ACTION.BASE_ATTACK_4:
                StartCoroutine(BaseAttack4());
                break;
        }
    }

    IEnumerator BaseAttack1()
    {
        action = HAMMERHEAD_ACTION.BASE_ATTACK_1;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }
    IEnumerator BaseAttack2()
    {
        action = HAMMERHEAD_ACTION.BASE_ATTACK_2;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }
    IEnumerator BaseAttack3()
    {
        action = HAMMERHEAD_ACTION.BASE_ATTACK_3;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }
    IEnumerator BaseAttack4()
    {
        action = HAMMERHEAD_ACTION.BASE_ATTACK_4;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }
}
