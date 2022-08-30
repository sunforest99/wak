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
    // 패턴 ======================================================================================================
    [Header("패턴")]
    private int pattern_rand;       // <! 패턴 랜덤값
    public WAKGUI_ACTION action;    // 현재 패턴
    [SerializeField] private int baseAttackCount;       // <! 기본패턴 몇번 후 패턴 사용할 것인지 (나중에 바꿀듯)
    [SerializeField] pattenObj patten;      // <! 패턴 프리팹 담는 구조체

    // 패턴-똥 시간 ==
    float pooSpawnTime;

    // 전멸기-왕따게임 ==
    [Header("전멸기 왕따게임")]
    public List<OutCast> outCasts;
    public int outcastRand;
    public GameObject getOutcast { get { return patten.outcast; } }
    public GameObject[] getTotem { get { return patten.totem; } }

    // 전멸기-구슬먹기 ==

    [Header("전멸기 구슬먹기")]
    public List<Marble> marblelist;
    bool checkCircle = false;           // !< 구슬부시기 패턴 채크
    bool checkOutcast = false;          // !< 왕따 패턴 체크
    bool checkPattern = false;
    public GameObject[] getCircle { get { return patten.circle; } }

    public List<int> visit;

    // ??
    [Header("Other")]
    public int circle_answer;
    public List<string> targetList;      // 보스 타겟 설정하는거
    public string getTarget => targetList[Random.Range(0, targetList.Count)];        // 타겟 렌덤

    [SerializeField] private WakguiObjectPool objectPool;

    void Start()
    {
        base.BossInitialize();
        GameMng.I.boss = this;

    NetworkMng.I.roomOwner = true;
        if (NetworkMng.I.roomOwner)
        {
            StartCoroutine(RaidStart());
        }

        // StartCoroutine(Teleport("Pattern_Circle"));
    }

    IEnumerator RaidStart()
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
            if (action == WAKGUI_ACTION.IDLE && _target != null)
                base.BossMove();

            if (action == WAKGUI_ACTION.IDLE && !checkPattern && !checkCircle && base._currentNesting < 90)
            {
                Teleport(true);
                checkPattern = true;
                checkCircle = true;
                _isAnnihilation = true;
                int rand = Random.Range(0, 4);
                for (int j = 0; j < 4;)
                {
                    if (visit.Contains(rand))
                    {
                        rand = Random.Range(0, 4);
                    }
                    else
                    {
                        visit.Add(rand);
                        j++;
                    }
                }
                if (NetworkMng.I.roomOwner)
                {
                    SendBossPattern(WAKGUI_ACTION.PATTERN_CIRCLE, Random.Range(0, 6).ToString() + ":-1:" +
                    visit[0] + ":" + visit[1] + ":" + visit[2] + ":" + visit[3]);
                }
                // StopCoroutine(Action());
            }
            else if (action == WAKGUI_ACTION.IDLE && checkCircle && !checkOutcast && base._currentNesting < 50)
            {
                Teleport(false);
                checkPattern = true;
                checkOutcast = true;
                _isAnnihilation = true;
                if (NetworkMng.I.roomOwner)
                    SendBossPattern(WAKGUI_ACTION.PATTERN_OUTCAST, Random.Range(0, 4).ToString());
                // StopCoroutine(Action());
            }
        }
        else
        {
            base.SetZeroHp();
        }
    }

    void SendBossPattern(WAKGUI_ACTION action, string msg = "")
    {
        // 뒤에 추가로 데이터가 필요로 하지 않은 패턴은 msg를 공백으로 보내서 split을 줄임
        // 만약 이거때문에 버그가 잦다면 붙여도 무관
        NetworkMng.I.SendMsg(string.Format("BOSS_PATTERN:{0}{1}", (int)action, msg != "" ? ":" + msg : msg));
    }

    public void Think()
    {
        // yield return new WaitForSeconds(3.0f);

        if (NetworkMng.I.roomOwner)
        {
            if (baseAttackCount < 4)
            {
                pattern_rand = Random.Range((int)WAKGUI_ACTION.BASE_STAP, (int)WAKGUI_ACTION.BASE_ROAR + 1);
                switch (pattern_rand)
                {
                    case (int)WAKGUI_ACTION.IDLE:
                        SendBossPattern(WAKGUI_ACTION.IDLE, getTarget);
                        // SendBossPattern(WAKGUI_ACTION.IDLE,  /*타겟의 uniqueNumber*/));
                        break;
                    case (int)WAKGUI_ACTION.BASE_STAP:      // <! 찌르기
                        SendBossPattern(WAKGUI_ACTION.BASE_STAP);
                        baseAttackCount++;
                        break;
                    case (int)WAKGUI_ACTION.BASE_SLASH:      // <! 내려찍기
                        SendBossPattern(WAKGUI_ACTION.BASE_SLASH);
                        baseAttackCount++;
                        break;
                    case (int)WAKGUI_ACTION.BASE_ROAR:      // <! 포효
                        SendBossPattern(WAKGUI_ACTION.BASE_ROAR);
                        baseAttackCount++;
                        break;
                    case (int)WAKGUI_ACTION.BASE_RUSH:      // <! 돌진
                        SendBossPattern(WAKGUI_ACTION.BASE_RUSH);
                        baseAttackCount++;
                        break;
                }
            }

            else
            {
                // pattern_rand = Random.Range((int)WAKGUI_ACTION.PATTERN_POO, (int)WAKGUI_ACTION.PATTERN_COUNTER + 1);
                pattern_rand = (int)WAKGUI_ACTION.PATTERN_CRISTAL;
                switch (pattern_rand)
                {
                    case (int)WAKGUI_ACTION.PATTERN_POO:      // <! 똥 생성
                        SendBossPattern(WAKGUI_ACTION.PATTERN_POO);
                        baseAttackCount = 0;
                        break;
                    case (int)WAKGUI_ACTION.PATTERN_KNIFE:      // <! 칼날 찌르기
                        for (int i = 0; i < bossdata.maxKnife; i++)
                        {
                            SendBossPattern(WAKGUI_ACTION.PATTERN_KNIFE,
                            Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x).ToString() + ":" +
                            Random.Range(GameMng.I.mapLeftBotton.z, GameMng.I.mapRightTop.z).ToString());               // TODO, 뒤에 칼날 생성될 위치,각도를 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                        }
                        baseAttackCount = 0;
                        break;
                    case (int)WAKGUI_ACTION.PATTERN_JUMP:      // <! 점프 공격
                        SendBossPattern(WAKGUI_ACTION.PATTERN_JUMP);                     // TODO, 뒤에 점프 공격할 대상의 uniqueNumber 보내줘야함.
                        baseAttackCount = 0;
                        break;
                    case (int)WAKGUI_ACTION.PATTERN_CRISTAL:      // <! 수정 생성
                        // for (int i = 0; i < bossdata.maxCristal; i++)
                        // {
                            SendBossPattern(WAKGUI_ACTION.PATTERN_CRISTAL, Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x).ToString() + ":" +
                            Random.Range(GameMng.I.mapLeftBotton.z, GameMng.I.mapRightTop.z).ToString());         // TODO, 뒤에 수정 생성될 위치 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                        // }

                        baseAttackCount = 0;
                        break;
                    case (int)WAKGUI_ACTION.PATTERN_WAVE:      // <! 파도
                        for (int i = 0; i < bossData.maxWave; i++)
                        {
                            SendBossPattern(WAKGUI_ACTION.PATTERN_WAVE,
                            Pattern_Wave_Think());            // TODO, 뒤에 파도 생성될 위치,방향 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                        }
                        baseAttackCount = 0;
                        break;
                    case (int)WAKGUI_ACTION.PATTERN_COUNTER:      // <! 반격기
                        SendBossPattern(WAKGUI_ACTION.PATTERN_COUNTER);
                        baseAttackCount = 0;
                        break;
                }
            }
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
            SendBossPattern(WAKGUI_ACTION.IDLE, NetworkMng.I.uniqueNumber);
        }
        Think();
    }

    public override void Action(string msg)
    {
        string[] txt = msg.Split(":");
        switch (int.Parse(txt[1]))
        {
            case (int)WAKGUI_ACTION.IDLE:
                // SendBossPattern(WAKGUI_ACTION.IDLE,  /*타겟의 uniqueNumber*/));
                // Action();
                _target = NetworkMng.I.v_users[txt[2]].transform.parent;
                Think();
                break;
            case (int)WAKGUI_ACTION.BASE_STAP:      // <! 찌르기
                Base_Stap();
                break;
            case (int)WAKGUI_ACTION.BASE_SLASH:      // <! 내려찍기
                Base_Slash();
                break;
            case (int)WAKGUI_ACTION.BASE_ROAR:      // <! 포효
                Base_Roar();
                break;
            case (int)WAKGUI_ACTION.BASE_RUSH:      // <! 돌진
                Base_Rush();
                break;
            case (int)WAKGUI_ACTION.PATTERN_POO:      // <! 똥 생성
                Pattern_Poo();
                break;
            case (int)WAKGUI_ACTION.PATTERN_KNIFE:      // <! 칼날 찌르기             // TODO, 뒤에 칼날 생성될 위치,각도를 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                Pattern_Knife(float.Parse(txt[2]), float.Parse(txt[3]));
                break;
            case (int)WAKGUI_ACTION.PATTERN_JUMP:      // <! 점프 공격
                StartCoroutine(Pattern_Jump());                             // TODO, 뒤에 점프 공격할 대상의 uniqueNumber 보내줘야함.
                break;
            case (int)WAKGUI_ACTION.PATTERN_CRISTAL:      // <! 수정 생성
                Pattern_Cristal(float.Parse(txt[2]), float.Parse(txt[3]));      // TODO, 뒤에 수정 생성될 위치 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                break;
            case (int)WAKGUI_ACTION.PATTERN_WAVE:      // <! 파도
                Pattern_Wave(float.Parse(txt[2]), float.Parse(txt[3]), int.Parse(txt[4]));                             // TODO, 뒤에 파도 생성될 위치,방향 한번에 보내주는것이 가장 좋음. 아니라면 일일이 데이터 새로 보내야함
                break;
            case (int)WAKGUI_ACTION.PATTERN_COUNTER:      // <! 반격기
                StartCoroutine(Pattern_Counter());
                break;
            case (int)WAKGUI_ACTION.PATTERN_CIRCLE:      // <! 전멸기 색 패턴
                circle_answer = int.Parse(txt[2]);
                if (int.Parse(txt[3]) >= 0)
                {
                    Destroy(marblelist[int.Parse(txt[3])]);
                }
                visit[0] = int.Parse(txt[4]);
                visit[1] = int.Parse(txt[5]);
                visit[2] = int.Parse(txt[6]);
                visit[3] = int.Parse(txt[7]);
                break;
            case (int)WAKGUI_ACTION.PATTERN_OUTCAST:      // <! 전멸기 토템 페턴
                outcastRand = int.Parse(txt[2]);
                break;
        }
    }

    /**
     * @brief 기본공격 찌르기
     */
    void Base_Stap()
    {
        animator.SetTrigger("Stap");
    }

    /**
     * @brief 기본공격 내려찍기
     */
    void Base_Slash()
    {
        animator.SetTrigger("Slash");
    }

    /**
     * @brief 기본공격 포효
     */
    void Base_Roar()
    {
        animator.SetTrigger("Roar");
    }

    /**
     * @brief 기본공격 돌진
     */
    void Base_Rush()
    {
        animator.SetTrigger("Rush");
    }

    /**
     * @brief 패턴 똥 생성
     */
    void Pattern_Poo()
    {
        animator.SetTrigger("Poo");

        // 나중에 살아 있는 플레이어 들에게 모두 쏘기
        GameObject poo = Instantiate(patten.poo, Vector3.zero, Quaternion.identity) as GameObject;
        poo.transform.SetParent(_target);
        poo.transform.localPosition = new Vector3(0, -2.5f, 0);
    }

    /**
     * @brief 패턴 칼날 생성
     */
    void Pattern_Knife(float posX, float posZ)
    {
        animator.SetTrigger("Knife");
        objectPool.setKnifeActive(posX, posZ);
    }

    /**
     * @brief 패턴 점프
     */
    IEnumerator Pattern_Jump()
    {
        animator.SetTrigger("Jump");

        yield return new WaitForSeconds(2.0f);
    }

    /**
     * @brief 패턴 수정 생성
     */
    void Pattern_Cristal(float posX, float posZ)
    {
        animator.SetTrigger("Cristal");

        objectPool.setCristalActive(posX, posZ);
    }

    string Pattern_Wave_Think()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case (int)POS.DOWN:
                return Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x).ToString() + ":" + GameMng.I.mapRightTop.z.ToString() + ":" + rand.ToString();
            case (int)POS.UP:
                return Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x).ToString() + ":" + GameMng.I.mapLeftBotton.z.ToString() + ":" + rand.ToString();
            case (int)POS.RIGHT:
                return GameMng.I.mapLeftBotton.x.ToString() + ":" + Random.Range(GameMng.I.mapLeftBotton.z, GameMng.I.mapRightTop.z).ToString() + ":" + rand.ToString();
            case (int)POS.LEFT:
                return GameMng.I.mapRightTop.x.ToString() + ":" + Random.Range(GameMng.I.mapLeftBotton.z, GameMng.I.mapRightTop.z).ToString() + ":" + rand.ToString();
        }
        return null;
    }
    /**
     * @brief 패턴 파도
     */
    void Pattern_Wave(float posX, float posZ, int rand)
    {
        animator.SetTrigger("Wave");

        objectPool.WaveObject(posX, posZ, rand);
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
    }

    void Teleport(bool pettern_check)
    {
        animator.SetTrigger("Teleporting");

        if (pettern_check)
        {
            animator.SetBool("CheckCircle", true);
            StartCoroutine(Pattern_Circle());
        }
        else
        {
            animator.SetBool("CheckCircle", false);
            StartCoroutine(Pattern_Outcast());
        }
    }

    public IEnumerator Pattern_Circle()
    {
        marblelist.Clear();
        animator.SetInteger("RandCircle", circle_answer);

        yield return new WaitForSeconds(15.0f);
        action = WAKGUI_ACTION.IDLE;
        _isAnnihilation = false;
        checkPattern = false;

        yield return null;
    }

    public IEnumerator Pattern_Outcast()
    {
        yield return new WaitForSeconds(15.0f);
        action = WAKGUI_ACTION.IDLE;
        _isAnnihilation = false;

        yield return null;
    }
}