using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrier : Character
{
    public override void init()
    {
        _job = JOB.WARRIER;
        DASH_SPEED = 12;
        MOVE_SPEED = 8;
        DASH_COOLTIME = 6;
        WAKEUP_COOLTIME = 10;

        // if 이게 내꺼라면
        settingStat();        
    }
    public override void attack(Vector2 attackDir, bool isMe = false)
    {
        if (continuousAttack >= 3)
            return;
        
        continuousAttack++;
        if (continuousAttack.Equals(1))
            _anim.SetTrigger("Attack");
        _anim.SetInteger("C_Attack", continuousAttack);

        // 공격시 좌우 반전되는거 막기 위함
        if (_action != CHARACTER_ACTION.ATTACK_CANT_ANYTHING)
        {
            // 좌우 반전
            if (attackDir.x < Screen.width / 2)
                transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
            else
                transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));
        }

        _action = CHARACTER_ACTION.ATTACK_CANT_ANYTHING;
    }
    
    public override void skill_1(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Gal");
    }
    public override void skill_2(Vector2 skillPos, bool isMe = false)
    {
        StartCoroutine(agdzForce());
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_AGDZ");
    }
    public override void skill_3(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CAN_MOVE;
        _anim.SetTrigger("Skill_Bigrr");
    }
    public override void skill_4(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_SG");
    }
    public override void skill_5(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_JH");
    }

    IEnumerator agdzForce()
    {
        yield return new WaitForSeconds(0.5f);

        // 왼쪽 보고 있으면 왼쪽으로 -10 힘 주기 (20 혹은 340인데 소수점 오차 위해서 100 이하라는 숫자로 확인함)
        addForceImpulse(new Vector3(transform.eulerAngles.x <= 100 ? -10 : 10, 0, 0));
    }

    void settingStat()
    {
        switch(Mathf.FloorToInt(GameMng.I.userData.level)) {
            case 1:
                _stat = new Stat(10000, 20000, 1, 1, 1, 1, 6, 1.02f);
                break;
            case 2:
                _stat = new Stat(30000, 60000, 1, 1, 1, 1, 7, 1.04f);
                break;
            case 3:
                _stat = new Stat(50000, 100000, 1, 1, 1, 1, 7.6f, 1.06f);
                break;
            case 4:
                _stat = new Stat(100000, 200000, 1, 1, 1, 1, 8.3f, 1.08f);
                break;
            case 5:
                _stat = new Stat(200000, 400000, 1, 1, 1, 1, 8.8f, 1.1f);
                break;
            case 6:
                _stat = new Stat(300000, 600000, 1, 1, 1, 1, 9.2f, 1.12f);
                break;
            case 7:
                _stat = new Stat(475000, 950000, 1, 1, 1, 1, 9.5f, 1.14f);
                break;
            case 8:
                _stat = new Stat(650000, 1300000, 1, 1, 1, 1, 9.7f, 1.16f);
                break;
            case 9:
                _stat = new Stat(800000, 1600000, 1, 1, 1, 1, 9.9f, 1.18f);
                break;
            case 10:
                _stat = new Stat(1000000, 2000000, 1, 1, 1, 1, 10, 1.2f);
                break;
        }
    }
}
