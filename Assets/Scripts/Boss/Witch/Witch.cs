using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WITCH_ACTION
{
    IDLE,
    PATTERN_SPEAR,
    PATTERN_DODGE_1,
    PATTERN_DODGE_2,
    PATTERN_DODGE_3,
    PATTERN_LAZER,
    PATTERN_NECKHANG,
    PATTERN_LINE_1,
    PATTERN_LINE_2,
    PATTERN_LINE_3,
    PATTERN_LINE_4,
    TELEPORT,
    MARBLE
}

public class Witch : Boss
{
    [Header("[ 보스 행동 ]")]
    [SerializeField] private WITCH_ACTION _action;

    [Header("패턴")]
    private int pattern_rand;       // <! 패턴 랜덤값

    // [Header("[ 기타 ]")]
    // [SerializeField] public bool isThink = false;

    void Start()
    {
        base.BossInitialize();
        GameMng.I.boss = this;

        base._minDistance = 2.0f;
        
        initObjectPool();

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

    // private void FixedUpdate()
    // {
    //     if (_currentHp >= 0 && _action == WITCH_ACTION.IDLE && _target != null)
    //         base.BossMove();
    // }

    bool checkMarble = false;       // Marble 전멸기를 실행해야 할때 true 가 됨
    bool doneMarble = false;        // Marble 전멸기를 한번 끝냈을때 true 가 됨

    void Update()
    {
        if (_currentHp >= 0)
        {
            // TODO : 보스 시작전에 target null 이여서 아래 자꾸 에러 로그 떠서 넣었음. 이렇게해서 일부 패턴 작동안하면 고려할 것 !!
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

    IEnumerator Think()
    {
        yield return new WaitForSeconds(3);
        // isThink = true;

        if (NetworkMng.I.roomOwner)
        {
            // Debug.Log(base._currentNesting)
            if (!checkMarble && !doneMarble && base._currentNesting < 125)
            {
                doneMarble = true;
                SendBossPattern((int)WITCH_ACTION.MARBLE);
            }
            else {
                pattern_rand = Random.Range((int)WITCH_ACTION.IDLE, (int)WITCH_ACTION.PATTERN_LINE_4 + 1);
                // pattern_rand = (int)WITCH_ACTION.PATTERN_SPEAR;
                switch (pattern_rand)
                {
                    case (int)WITCH_ACTION.IDLE:
                        SendBossPattern((int)WITCH_ACTION.IDLE, getTarget);
                        // SendBossPattern(WITCH_ACTION.IDLE,  /*타겟의 uniqueNumber*/));
                        break;
                    case (int)WITCH_ACTION.PATTERN_SPEAR:
                        SendBossPattern(pattern_rand);
                        break;
                    case (int)WITCH_ACTION.PATTERN_DODGE_1:
                    case (int)WITCH_ACTION.PATTERN_DODGE_2:
                    case (int)WITCH_ACTION.PATTERN_DODGE_3:
                        SendBossPattern(pattern_rand);
                        break;
                    case (int)WITCH_ACTION.PATTERN_LAZER:
                        SendBossPattern(pattern_rand);
                        break;
                    case (int)WITCH_ACTION.PATTERN_NECKHANG:
                        SendBossPattern(pattern_rand,
                            Random.Range(0, 3).ToString()
                        );
                        break;
                    case (int)WITCH_ACTION.TELEPORT:
                        SendBossPattern(pattern_rand, 
                            string.Format("{0}:{1}", Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x), Random.Range(GameMng.I.mapLeftBotton.z, GameMng.I.mapRightTop.z))
                        );
                        break;
                    case (int)WITCH_ACTION.PATTERN_LINE_1:
                    case (int)WITCH_ACTION.PATTERN_LINE_2:
                    case (int)WITCH_ACTION.PATTERN_LINE_3:
                    case (int)WITCH_ACTION.PATTERN_LINE_4:
                        SendBossPattern(pattern_rand);
                        break;
                }
            }
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
            SendBossPattern((int)WITCH_ACTION.IDLE, NetworkMng.I.uniqueNumber);
            // StartCoroutine(Think());
        }
    }

    public override void Action(string msg)
    {
        string[] txt = msg.Split(":");
        WITCH_ACTION cur_pattern = (WITCH_ACTION)System.Enum.Parse(typeof(WITCH_ACTION), txt[1]);
        
        // Debug.Log(msg);

        switch (cur_pattern)
        {
            case WITCH_ACTION.IDLE:
                _target = NetworkMng.I.v_users[txt[2]].transform.parent;
                if (NetworkMng.I.roomOwner)
                        StartCoroutine(Think());
                break;
            case WITCH_ACTION.PATTERN_SPEAR:
                Pattern_Spear();
                break;
            case WITCH_ACTION.PATTERN_DODGE_1:
                Pattern_Dodge_1();
                break;
            case WITCH_ACTION.PATTERN_DODGE_2:
                Pattern_Dodge_2();
                break;
            case WITCH_ACTION.PATTERN_DODGE_3:
                Pattern_Dodge_3();
                break;
            case WITCH_ACTION.PATTERN_LAZER:
                Pattern_Lazer();
                break;
            case WITCH_ACTION.PATTERN_NECKHANG:
                Pattern_Neckhang(txt[2]);
                break;
            case WITCH_ACTION.PATTERN_LINE_1:
            case WITCH_ACTION.PATTERN_LINE_2:
            case WITCH_ACTION.PATTERN_LINE_3:
            case WITCH_ACTION.PATTERN_LINE_4:
                Pattern_Line(cur_pattern);
                break;
            case WITCH_ACTION.TELEPORT:
                nextPos = new Vector2(float.Parse(txt[2]), float.Parse(txt[3]));
                Teleport();
                break;
            case WITCH_ACTION.MARBLE:
                checkMarble = true;
                nextPos = Vector2.zero;
                Teleport();
                break;
        }
    }

    public void endAct()
    {
        _action = WITCH_ACTION.IDLE;

        if (checkMarble)
            Marble();
        else if (NetworkMng.I.roomOwner)
            StartCoroutine(Think());
    }

    [SerializeField] GameObject[] spearObjs;
    void Pattern_Spear()
    {
        _action = WITCH_ACTION.PATTERN_SPEAR;
        animator.SetTrigger("Spear");

        try {
            Vector3 tempPos;
            for (int i = 0; i < spearObjs.Length && i < targetList.Count; i++) {
                tempPos = NetworkMng.I.v_users[ targetList[i] ].transform.position;
                tempPos.y = 0;
                spearObjs[i].transform.position = tempPos;
                spearObjs[i].SetActive(true);
            }
        } catch (KeyNotFoundException e) {
        }
    }

    void Pattern_Dodge_1()
    {
        _action = WITCH_ACTION.PATTERN_DODGE_1;
        animator.SetTrigger("Dodge");
        
        StartCoroutine(CreateDodge_1());
    }

    void Pattern_Dodge_2()
    {
        _action = WITCH_ACTION.PATTERN_DODGE_2;
        animator.SetTrigger("Dodge");
        
        StartCoroutine(CreateDodge_2());
    }

    void Pattern_Dodge_3()
    {
        _action = WITCH_ACTION.PATTERN_DODGE_3;
        animator.SetTrigger("Dodge");
        
        StartCoroutine(CreateDodge_3());
    }

    [SerializeField] GameObject lazerObj;
    void Pattern_Lazer()
    {
        _action = WITCH_ACTION.PATTERN_LAZER;
        animator.SetTrigger("Lazer");

        lazerObj.SetActive(true);
    }
    [SerializeField] GameObject neckHangObj;
    void Pattern_Neckhang(string pt)
    {
        _action = WITCH_ACTION.PATTERN_NECKHANG;
        animator.SetTrigger("NeckHang");
        
        switch (pt)
        {
            case "0":
                neckHangObj.transform.position = new Vector3(0, 0, -9.3f);
                break;
            case "1":
                neckHangObj.transform.position = Vector3.zero;
                break;
            case "2":
                neckHangObj.transform.position = new Vector3(0, 0, 9.3f);
                break;
        }
        neckHangObj.SetActive(true);
    }

    [SerializeField] Sprite[] warningSprs;
    [SerializeField] SpriteRenderer warningLineObj;
    [SerializeField] GameObject exploObj;
    void Pattern_Line(WITCH_ACTION pt)
    {
        _action = pt;
        animator.SetTrigger("Line");

        switch (pt)
        {
            // 좌측 빨간 장판
            case WITCH_ACTION.PATTERN_LINE_1:
                warningLineObj.sprite = warningSprs[0];
                warningLineObj.transform.position = new Vector3(-8.9f, 0, 15.1f);
                exploObj.transform.position = new Vector3(-9, 0, 0);
                break;
            // 우측 빨간 장판
            case WITCH_ACTION.PATTERN_LINE_2:
                warningLineObj.sprite = warningSprs[0];
                warningLineObj.transform.position = new Vector3(8.9f, 0, 15.1f);
                exploObj.transform.position = new Vector3(9, 0, 0);
                break;
            // 좌측 보라 장판
            case WITCH_ACTION.PATTERN_LINE_3:
                warningLineObj.sprite = warningSprs[1];
                warningLineObj.transform.position = new Vector3(-8.9f, 0, 15.1f);
                exploObj.transform.position = new Vector3(9, 0, 0);
                break;
            // 우측 보라 장판
            case WITCH_ACTION.PATTERN_LINE_4:
                warningLineObj.sprite = warningSprs[1];
                warningLineObj.transform.position = new Vector3(8.9f, 0, 15.1f);
                exploObj.transform.position = new Vector3(-9, 0, 0);
                break;
        }
        exploObj.SetActive(true);
        warningLineObj.gameObject.SetActive(true);
    }

    void Teleport()
    {
        _action = WITCH_ACTION.TELEPORT;
        animator.SetTrigger("Disappear");
    }

    Vector2 nextPos;
    public void moveTeleport()
    {
        this.transform.position = new Vector3(
            nextPos.x,
            this.transform.position.y,
            nextPos.y
        );
    }

    public Dictionary<int, string> checkMarbleDic = new Dictionary<int, string>();
    void Marble()
    {
        checkMarble = false;
        _action = WITCH_ACTION.MARBLE;
        animator.SetTrigger("Marble");
        
        StartCoroutine(CreateMarble());
    }

    [SerializeField] GameObject[] marbles;
    IEnumerator CreateMarble()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < 4; i++) {
            marbles[i].SetActive(true);
        }

        yield return new WaitForSeconds(2);


        for (int i = 4; i < 8; i++) {
            marbles[i].SetActive(true);
        }

        yield return new WaitForSeconds(4);

        for (int i = 0; i < 4; i++) {
            if (checkMarbleDic.ContainsKey(i) && checkMarbleDic.ContainsKey(i + 4)) {
                if (checkMarbleDic[i] == checkMarbleDic[i + 4]) {
                    // 문제없음

                } else {
                    // 실패
                    GameMng.I.stateMng.forcedDeath();
                    break;
                }
            } else {
                // 실패
                GameMng.I.stateMng.forcedDeath();
                break;
            }
        }
    }


    IEnumerator CreateDodge_1()
    {
        yield return new WaitForSeconds(2);

        int count = 0;
        while (count++ < 10)
        {
            // 좌측
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, 0)
            );
            // 위
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, 90)
            );
            // 우측
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, 180)
            );
            // 아래
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, 270)
            );
            
            
            Vector3 moveTo = _target.position;
            moveTo -= transform.position;
            moveTo.y = _target.position.y;

            float lookAngle = Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;

            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 180)
            );
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 190)
            );
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 170)
            );

            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator CreateDodge_2()
    {
        yield return new WaitForSeconds(2);

        int count = 0;
        while (count++ < 10)
        {
            Vector3 moveTo = _target.position;
            moveTo -= transform.position;
            moveTo.y = _target.position.y;

            float lookAngle = Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;

            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 185)
            );
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 195)
            );
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 175)
            );
            shootDodge(
                new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                Quaternion.Euler(90, 0, lookAngle + 165)
            );

            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator CreateDodge_3()
    {
        yield return new WaitForSeconds(2);

        int count = 0;
        while (count++ < 10)
        {
            for (int i = 0; i < 8; i++) {
                shootDodge(
                    new Vector3(this.transform.position.x, 0.31f, this.transform.position.z),
                    Quaternion.Euler(90, 0, 45 * i + count * 10)
                );
            }

            yield return new WaitForSeconds(0.4f);
        }
    }

    [SerializeField] GameObject dodgeBulletObj;
    public Queue<GameObject> dodgePool = new Queue<GameObject>();
    void initObjectPool()
    {
        dodgePool.Clear();
        Dodge d;
        for (int i = 0; i < 50; i++) {
            d = Instantiate(dodgeBulletObj, Vector3.zero, Quaternion.Euler(20, 0, 0)).GetComponent<Dodge>();
            d._witch = this;
            dodgePool.Enqueue(d.gameObject);
        }
    }
    void shootDodge(Vector3 pos, Quaternion rot)
    {
        GameObject obj;
        if (dodgePool.Count > 0)
            obj = dodgePool.Dequeue();
        else {
            obj = Instantiate(dodgeBulletObj, Vector3.zero, Quaternion.Euler(20, 0, 0));
            obj.GetComponent<Dodge>()._witch = this;
        }
        obj.transform.position = pos;
        obj.transform.localRotation = rot;
        obj.SetActive(true);
    }
}
