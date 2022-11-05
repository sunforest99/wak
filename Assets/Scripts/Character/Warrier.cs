using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrier : Character
{
    public override void init()
    {
        _job = JOB.WARRIER;
        DASH_SPEED = 12;

        // if 이게 내꺼라면
        if (_isPlayer)
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
            if (attackDir.x < 0)
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

    public override void settingStat()
    {
        _stat = new Stat(
            1       /* 받는 피해량 퍼센트 */,
            8       /* 대쉬 쿨타임 */,
            12      /* 기상기 쿨타임 */,
            8       /* 이동 속도 */
        );
    }
}
