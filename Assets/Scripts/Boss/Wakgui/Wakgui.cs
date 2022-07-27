using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WAKGUI_ACTION
{
    IDLE,
    BASE_STAP,
    BASE_SLASH,
    BASE_ROAR,
    BASE_RUSH,
    PATTERN_POO,
    PATTERN_KNIFE,
    PATTERN_JUMP,
    PATTERN_CRISTAL,
    PATTERN_WAVE,
    PATTERN_COUNTER,
    PATTERN_CIRCLE
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
        base.BossInitialize();
        StartCoroutine(Think());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            bossdata.moveSpeed = 5.0f;
        }

        if (_currentHp >= 0)
        {
            base.ChangeHpbar();
            base.RaidTimer();
            base.ChangeHpText();
            if(action == WAKGUI_ACTION.IDLE)
                base.BossMove();
        }
        else
        {
            base.SetZeroHp();
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(3.0f);

        if (baseAttackCount < 4 && action == WAKGUI_ACTION.IDLE)
        {
            pattern_rand = Random.Range((int)WAKGUI_ACTION.IDLE, (int)WAKGUI_ACTION.BASE_ROAR + 1);
            // pattern_rand = (int)WAKGUI_ACTION.BASE_RUSH;
            switch (pattern_rand)
            {
                case (int)WAKGUI_ACTION.IDLE:
                    _target = GameMng.I.targetList[GameMng.I.targetCount];
                    StartCoroutine(Think());
                    break;
                case (int)WAKGUI_ACTION.BASE_STAP:      // <! 찌르기
                    baseAttackCount++;
                    Base_Stap();
                    break;
                case (int)WAKGUI_ACTION.BASE_SLASH:      // <! 내려찍기
                    baseAttackCount++;
                    Base_Slash();
                    break;
                case (int)WAKGUI_ACTION.BASE_ROAR:      // <! 포효
                    baseAttackCount++;
                    Base_Roar();
                    break;
                case (int)WAKGUI_ACTION.BASE_RUSH:      // <! 돌진
                    baseAttackCount++;
                    Base_Rush();
                    break;
            }
        }

        else
        {
            // pattern_rand = Random.Range((int)WAKGUI_ACTION.PATTERN_POO, (int)WAKGUI_ACTION.PATTERN_COUNTER + 1);
            pattern_rand = (int)WAKGUI_ACTION.PATTERN_JUMP;

            switch (pattern_rand)
            {
                case (int)WAKGUI_ACTION.PATTERN_POO:      // <! 똥 생성
                    Pattern_Poo();
                    break;
                case (int)WAKGUI_ACTION.PATTERN_KNIFE:      // <! 칼날 찌르기
                    StartCoroutine(Pattern_Knife());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_JUMP:      // <! 점프 공격
                    StartCoroutine(Pattern_Jump());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_CRISTAL:      // <! 수정 생성
                    Pattern_Cristal();
                    break;
                case (int)WAKGUI_ACTION.PATTERN_WAVE:      // <! 파도
                    StartCoroutine(Pattern_Wave());
                    break;
                case (int)WAKGUI_ACTION.PATTERN_COUNTER:      // <! 반격기
                    StartCoroutine(Think());
                    break;
            }
        }
    }

    /**
     * @brief 기본공격 찌르기
     */
    void Base_Stap()
    {
        animator.SetTrigger("Stap");
        StartCoroutine(Think());
    }

    /**
     * @brief 기본공격 내려찍기
     */
    void Base_Slash()
    {
        animator.SetTrigger("Slash");
        StartCoroutine(Think());
    }

    /**
     * @brief 기본공격 포효
     */
    void Base_Roar()
    {
        animator.SetTrigger("Roar");
        StartCoroutine(Think());
    }

    /**
     * @brief 기본공격 돌진
     */
    void Base_Rush()
    {
        animator.SetTrigger("Rush");
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 똥 생성
     */
    void Pattern_Poo()
    {
        animator.SetTrigger("Poo");

        Instantiate(patten.poo, _target.localPosition, Quaternion.identity);

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 칼날 생성
     * @TODO 칼날 생성 위치조정
     */
    IEnumerator Pattern_Knife()
    {
        animator.SetTrigger("Knife");

        for (int i = 0; i < bossdata.maxKnife; i++)
        {
            yield return new WaitForSeconds(1.0f);
            Instantiate(patten.knife, new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 점프
     */
    IEnumerator Pattern_Jump()
    {
        animator.SetTrigger("Jump");

        baseAttackCount = 0;
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Think());

    }

    /**
     * @brief 패턴 수정 생성
     * @TODO 수정 생성 위치조정
     */
    void Pattern_Cristal()
    {
        animator.SetTrigger("Cristal");
        for (int i = 0; i < bossdata.maxCristal; i++)
        {
            Instantiate(patten.cristal, new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0), Quaternion.identity);
        }

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 파도
     * @TODO 파도 생성 위치조정
     */
    IEnumerator Pattern_Wave()
    {
        animator.SetTrigger("Wave");
        for (int i = 0; i < bossdata.maxWave; i++)
        {
            yield return new WaitForSeconds(2.0f);
            Instantiate(patten.waves, Vector3.zero, Quaternion.identity);
        }

        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 반격
     */
    IEnumerator Patteron_Counter()
    {
        animator.SetTrigger("Counter");
        while(action == WAKGUI_ACTION.PATTERN_COUNTER)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Think());
    }
}
