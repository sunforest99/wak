using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WAKGUI_ACTION
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
    PATTERN_5,
    PATTERN_6,
    ANNIHILATION
}

public class Wakgui : Boss
{
    [System.Serializable]
    struct pattenObj
    {
        public GameObject knife;        // <! 칼날 공격
        public GameObject waves;        // <! 파도 공격
        public GameObject cristal;      // <! 닷지류 수정 생성
        public GameObject poo;
    }

    public WAKGUI_ACTION action;

    [SerializeField] private int baseAttackCount;       // <! 기본패턴 몇번 후 패턴 사용할 것인지 (나중에 바꿀듯)

    private int pattern_rand;       // <! 패턴 랜덤값

    [SerializeField] pattenObj patten;      // <! 패턴 프리팹 담는 구조체

    float pooSpawnTime;

    [SerializeField] private Animator animator = null;

    void Start()
    {
        base.BossInitialize(100, 10.0f, "귀상어두", 0.3f, 14242442);
        StartCoroutine(Think());

        StartCoroutine(t());
    }

    void Update()
    {
        if (_currentHp >= 0)
        {
            // base.ChangeHpbar();
            base.RaidTimer();
            base.ChangeHpText();
            base.BossMove();
        }
        else
        {
            base.SetZeroHp();
        }
    }

    IEnumerator t()
    {
        yield return new WaitForSeconds(.2f);
        _nestingHp -= 569696;
        StartCoroutine(t());
    }

    IEnumerator Think()
    {
        action = WAKGUI_ACTION.NONE;
        yield return new WaitForSeconds(1.0f);

        if (baseAttackCount < 4)
        {
            pattern_rand = Random.Range((int)WAKGUI_ACTION.NONE, (int)WAKGUI_ACTION.BASE_ATTACK_3 + 1);

            switch (pattern_rand)
            {
                case (int)WAKGUI_ACTION.NONE:
                    baseAttackCount++;
                    StartCoroutine(Think());
                    break;
                case (int)WAKGUI_ACTION.BASE_ATTACK_1:      // <! 찌르기
                    baseAttackCount++;
                    StartCoroutine(BaseAttack1());
                    break;
                case (int)WAKGUI_ACTION.BASE_ATTACK_2:      // <! 내려찍기
                    baseAttackCount++;
                    StartCoroutine(BaseAttack2());
                    break;
                case (int)WAKGUI_ACTION.BASE_ATTACK_3:      // <! 포효
                    baseAttackCount++;
                    StartCoroutine(BaseAttack3());
                    break;
                case (int)WAKGUI_ACTION.BASE_ATTACK_4:      // <! 돌진
                    baseAttackCount++;
                    StartCoroutine(BaseAttack4());
                    break;
            }
        }
        else
        {
            pattern_rand = Random.Range((int)WAKGUI_ACTION.PATTERN_1, (int)WAKGUI_ACTION.PATTERN_6 + 1);
            // pattern_rand = (int)WAKGUI_ACTION.PATTERN_1;

            switch (pattern_rand)
            {
                case (int)WAKGUI_ACTION.PATTERN_1:      // <! 똥 생성
                    StartCoroutine(Patten_1());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_2:      // <! 칼날 찌르기
                    StartCoroutine(Patten_2());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_3:      // <! 땅속 등장 공격
                    baseAttackCount = 0;
                    StartCoroutine(Think());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_4:      // <! 수정 생성
                    StartCoroutine(Patten_4());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_5:      // <! 파도
                    StartCoroutine(Patten_5());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_6:      // <! 반격기
                    baseAttackCount = 0;
                    StartCoroutine(Think());
                    break;
            }
        }

    }

    /**
     * @brief 기본공격 찌르기
     */
    IEnumerator BaseAttack1()
    {
        action = WAKGUI_ACTION.BASE_ATTACK_1;
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isStap", true);
        while (false == animator.IsInTransition(0))
        {
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("isStap", false);
        StartCoroutine(Think());
    }
    
    /**
     * @brief 기본공격 내려찍기
     */
    IEnumerator BaseAttack2()
    {
        action = WAKGUI_ACTION.BASE_ATTACK_2;
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isSlash", true);
        while (false == animator.IsInTransition(0))
        {
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("isSlash", false);
        StartCoroutine(Think());
    }

    /**
     * @brief 기본공격 포효
     */
    IEnumerator BaseAttack3()
    {
        action = WAKGUI_ACTION.BASE_ATTACK_3;
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isRoar", true);
        while (false == animator.IsInTransition(0))
        {
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("isRoar", false);
        StartCoroutine(Think());
    }
    
    /**
     * @brief 기본공격 돌진
     */
    IEnumerator BaseAttack4()
    {
        action = WAKGUI_ACTION.BASE_ATTACK_4;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 똥 생성
     */
    IEnumerator Patten_1()
    {
        action = WAKGUI_ACTION.PATTERN_1;
        yield return new WaitForSeconds(2.0f);
        Instantiate(patten.poo, _target.localPosition, Quaternion.identity);

        yield return new WaitForSeconds(1.0f);
        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 칼날 찌르기
     */
    IEnumerator Patten_2()
    {
        // int randcount = Random.Range(1, 8);

        action = WAKGUI_ACTION.PATTERN_2;
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(2.0f);
            Instantiate(patten.knife, new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
        yield return new WaitForSeconds(1.0f);
        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 수정 생성
     */
    IEnumerator Patten_4()
    {
        action = WAKGUI_ACTION.PATTERN_4;
        for (int i = 0; i < 4; i++)
        {
            Instantiate(patten.cristal, new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(1.0f);
        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 파도
     */
    IEnumerator Patten_5()
    {
        action = WAKGUI_ACTION.PATTERN_5;
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(2.0f);
            Instantiate(patten.waves, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(1.0f);
        baseAttackCount = 0;
        StartCoroutine(Think());
    }
}
