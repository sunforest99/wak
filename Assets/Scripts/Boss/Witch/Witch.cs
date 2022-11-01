using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WITCH_ACTION
{
    IDLE,
    PATTERN_SPEAR,
    PATTERN_DODGE,
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

    [Header("[ 기타 ]")]
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

    public void Think()
    {
        isThink = true;
        // yield return new WaitForSeconds(1.0f);

        if (_isAnnihilation)
            return;

        if (NetworkMng.I.roomOwner)
        {
            
            pattern_rand = Random.Range((int)WITCH_ACTION.IDLE, (int)WITCH_ACTION.PATTERN_LINE_4 + 1);
            switch (pattern_rand)
            {
                case (int)WITCH_ACTION.IDLE:
                    SendBossPattern((int)WITCH_ACTION.IDLE);
                    // SendBossPattern(WITCH_ACTION.IDLE,  /*타겟의 uniqueNumber*/));
                    break;
                case (int)WITCH_ACTION.PATTERN_SPEAR:
                    SendBossPattern(pattern_rand);
                    break;
                case (int)WITCH_ACTION.PATTERN_DODGE:
                    SendBossPattern(pattern_rand);
                    break;
                case (int)WITCH_ACTION.PATTERN_LAZER:
                    SendBossPattern(pattern_rand);
                    break;
                case (int)WITCH_ACTION.PATTERN_NECKHANG:
                    SendBossPattern(pattern_rand);
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
        }
        // StartCoroutine(Think());
    }

    public override void Action(string msg)
    {
        string[] txt = msg.Split(":");
        WITCH_ACTION cur_pattern = (WITCH_ACTION)System.Enum.Parse(typeof(WITCH_ACTION), txt[1]);
        switch (cur_pattern)
        {
            case WITCH_ACTION.IDLE:
                // SendBossPattern(WITCH_ACTION.IDLE,  /*타겟의 uniqueNumber*/));
                // Action();
                _target = NetworkMng.I.v_users[txt[2]].transform.parent;
                isThink = false;
                // StartCoroutine(Think());
                break;
            case WITCH_ACTION.PATTERN_SPEAR:
                Pattern_Spear();
                break;
            case WITCH_ACTION.PATTERN_DODGE:
                Pattern_Dodge();
                break;
            case WITCH_ACTION.PATTERN_LAZER:
                Pattern_Lazer();
                break;
            case WITCH_ACTION.PATTERN_NECKHANG:
                Pattern_Neckhang();
                break;
            case WITCH_ACTION.PATTERN_LINE_1:
            case WITCH_ACTION.PATTERN_LINE_2:
            case WITCH_ACTION.PATTERN_LINE_3:
            case WITCH_ACTION.PATTERN_LINE_4:
                Pattern_Line(cur_pattern);
                break;
            case WITCH_ACTION.TELEPORT:
                _target = NetworkMng.I.v_users[txt[2]].transform.parent;
                nextPos = new Vector2(float.Parse(txt[2]), float.Parse(txt[3]));
                break;
            case WITCH_ACTION.MARBLE:
                Marble();
                break;
        }
    }

    void Pattern_Spear()
    {
        _action = WITCH_ACTION.PATTERN_SPEAR;
        animator.SetTrigger("Spear");
    }

    void Pattern_Dodge()
    {
        _action = WITCH_ACTION.PATTERN_DODGE;
        animator.SetTrigger("Dodge");
        
    }

    void Pattern_Lazer()
    {
        _action = WITCH_ACTION.PATTERN_LAZER;
        animator.SetTrigger("Lazer");
        
    }
    void Pattern_Neckhang()
    {
        _action = WITCH_ACTION.PATTERN_NECKHANG;
        animator.SetTrigger("NeckHang");
        
    }

    void Pattern_Line(WITCH_ACTION pt)
    {
        _action = pt;
        animator.SetTrigger("Line");
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

    void Marble()
    {
        _action = WITCH_ACTION.MARBLE;
        animator.SetTrigger("Marble");
        
    }
}
