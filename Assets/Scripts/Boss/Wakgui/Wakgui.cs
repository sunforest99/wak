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
    PATTERN_CIRCLE,
    PATTERN_OUTCAST,
    TELEPORT,
    TELEPORT_SPAWN
}

// TODO : 보스 action 바꿔주기 ( OutCast 끝나면 )
public class Wakgui : Boss
{
    [System.Serializable]
    struct pattenObj
    {
        public GameObject knife;        // <! 칼날 공격
        public GameObject waves;        // <! 파도 공격
        public GameObject cristal;      // <! 닷지류 수정 생성
        public GameObject poo;
        public GameObject[] circle;
        public GameObject outcast;
        public GameObject[] totem;
    }
    public WAKGUI_ACTION action;

    [SerializeField] private int baseAttackCount;       // <! 기본패턴 몇번 후 패턴 사용할 것인지 (나중에 바꿀듯)

    private int pattern_rand;       // <! 패턴 랜덤값

    [SerializeField] pattenObj patten;      // <! 패턴 프리팹 담는 구조체

    public List<OutCast> outCasts;
    public GameObject getOutcast { get { return patten.outcast; } }
    public GameObject[] getTotem { get { return patten.totem; } }

    public List<Marble> marblelist;
    bool Checkcircle = false;           // !< 구슬부시기 패턴 채크
    bool Checkoutcast = false;          // !< 왕따 패턴 체크
    bool Checkpatern = false;
    public GameObject[] getCircle { get { return patten.circle; } }

    float pooSpawnTime;

    [SerializeField] private Animator animator = null;

    public int accumulateDmg;
    public int circle_answer;

    void Start()
    {
        base.BossInitialize();
        GameMng.I.bossData = this.bossdata;
        StartCoroutine(Think());

        // StartCoroutine(Teleport("Petern_Circle"));
    }

    void Update()
    {
        if (_currentHp >= 0)
        {
            base.ChangeHpbar();
            base.RaidTimer();
            base.ChangeHpText();
            if (action == WAKGUI_ACTION.IDLE)
                base.BossMove();

            if (!Checkpatern && !Checkcircle && base._currentNesting < 90 && action == WAKGUI_ACTION.IDLE)
            {
                Teleport(true);
                Checkpatern = true;
                Checkcircle = true;
                _isAnnihilation = true;
                StopCoroutine(Think());
            }
            else if (Checkcircle && !Checkoutcast && base._currentNesting < 50 && action == WAKGUI_ACTION.IDLE)
            {
                Teleport(false);
                Checkpatern = true;
                Checkoutcast = true;
                StopCoroutine(Think());
            }
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
            bossdata.setBossAction = pattern_rand;
            // pattern_rand = (int)WAKGUI_ACTION.BASE_RUSH;
            switch (pattern_rand)
            {
                case (int)WAKGUI_ACTION.IDLE:
                    //_target = GameMng.I.stateMng.getTarget;
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

        else if (action == WAKGUI_ACTION.IDLE)
        {
            pattern_rand = Random.Range((int)WAKGUI_ACTION.PATTERN_POO, (int)WAKGUI_ACTION.PATTERN_COUNTER + 1);
            bossdata.setBossAction = pattern_rand;
            // pattern_rand = (int)WAKGUI_ACTION.PATTERN_KNIFE;

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
                    StartCoroutine(Pattern_Counter());
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

        // 나중에 살아 있는 플레이어 들에게 모두 쏘기
        GameObject poo = Instantiate(patten.poo, Vector3.zero, Quaternion.identity) as GameObject;
        //poo.transform.SetParent(_target);
        poo.transform.localPosition = new Vector3(0, -2.5f, 0);

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 칼날 생성
     */
    IEnumerator Pattern_Knife()
    {
        animator.SetTrigger("Knife");

        for (int i = 0; i < bossdata.maxKnife; i++)
        {
            yield return new WaitForSeconds(1.0f);
            Instantiate(patten.knife, new Vector3(Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x), Random.Range(GameMng.I.mapLeftBotton.y, GameMng.I.mapRightTop.y), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
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
     */
    void Pattern_Cristal()
    {
        animator.SetTrigger("Cristal");
        for (int i = 0; i < bossdata.maxCristal; i++)
        {
            Instantiate(patten.cristal, new Vector3(Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x), Random.Range(GameMng.I.mapLeftBotton.y, GameMng.I.mapRightTop.y), 0), Quaternion.identity);
        }

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 파도
     */
    IEnumerator Pattern_Wave()
    {
        animator.SetTrigger("Wave");
        for (int i = 0; i < bossdata.maxWave; i++)
        {
            yield return new WaitForSeconds(2.0f);
            Instantiate(patten.waves, Vector3.zero, Quaternion.identity);
        }

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    /**
     * @brief 패턴 반격
     */
    IEnumerator Pattern_Counter()
    {
        int tempDmg = _currentHp;
        animator.SetTrigger("Counter");
        yield return new WaitForSeconds(1.0f);
        while (action == WAKGUI_ACTION.PATTERN_COUNTER)
        {
            if (tempDmg - _currentHp > 1000000)
            {
                animator.SetTrigger("Annihilate");
                break;
            }

            yield return null;
        }

        baseAttackCount = 0;
        StartCoroutine(Think());
    }

    void Teleport(bool pettern_check)
    {
        animator.SetTrigger("Teleporting");

        if (pettern_check)
        {
            animator.SetBool("CheckCircle", true);
            StartCoroutine(Petern_Circle());
        }
        else
        {
            animator.SetBool("CheckCircle", false);
            StartCoroutine(Petern_Outcast());
        }
    }

    public IEnumerator Petern_Circle()
    {
        marblelist.Clear();
        circle_answer = Random.Range(0, 6);
        animator.SetInteger("randCircle", circle_answer);

        yield return new WaitForSeconds(15.0f);
        action = WAKGUI_ACTION.IDLE;
        _isAnnihilation = false;
        StartCoroutine(Think());
        Checkpatern = false;
        yield return null;
    }

    public IEnumerator Petern_Outcast()
    {
        Debug.Log("ASDfasdfasd");
        yield return null;
    }
}
