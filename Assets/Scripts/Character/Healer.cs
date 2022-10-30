using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character
{
    GameObject fanservice;

    public override void init()
    {
        _job = JOB.HEALER;
        DASH_SPEED = 12;
        MOVE_SPEED = 9;
        
        fanservice = Instantiate(GameMng.I.healerSkillPrefab[3], Vector3.zero, Quaternion.Euler(90, 0, 0));
        fanservice.GetComponent<FanService>().parent = this.transform.parent;
        if (_isPlayer)
        {
            fanservice.tag = "Skill";
            fanservice.name = "4";
        }

        // if 이게 내꺼라면
        if (_isPlayer)
            settingStat();        
    }
    public override void input_attack()
    {
        Vector3 point = getMouseHitPoint();

        if (!float.IsNegativeInfinity(point.x))
        {
            NetworkMng.I.UseSkill(SKILL_CODE.ATTACK, point.x, point.z);
            attack(new Vector2(point.x, point.z), true);
        }
    }
    public override void input_skill_1()
    {
        Vector3 point = getMouseHitPoint();
        if (!float.IsNegativeInfinity(point.x))
        {
            if (Vector3.Distance(point, transform.position) < 10)
            {
                // 사전 거리 안이면 스킬 시전 가능
                StartCoroutine(SkillCoolDown(0));
                NetworkMng.I.UseSkill(SKILL_CODE.SKILL_1, point.x, point.z);
                skill_1(new Vector2(point.x, point.z), true);
            }
            else
            {
                // 거리 밖이면 사용 불가능 말함
            }
        }
    }
    public override void input_skill_4()
    {
        Vector3 point = getMouseHitPoint();
        if (!float.IsNegativeInfinity(point.x))
        {
            if (Vector3.Distance(point, transform.position) < 10)
            {
                // 사전 거리 안이면 스킬 시전 가능
                StartCoroutine(SkillCoolDown(3));
                NetworkMng.I.UseSkill(SKILL_CODE.SKILL_4, point.x, point.z);
                skill_4(new Vector2(point.x, point.z), true);
            }
            else
            {
                // 거리 밖이면 사용 불가능 말함
            }
        }
    }

    public override void attack(Vector2 attackDir, bool isMe = false)
    {
        Vector3 moveTo = new Vector3(attackDir.x, 0.3f, attackDir.y);
        moveTo -= transform.position;
        float lookAngle =  Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.3f;

        GameObject attObj = Instantiate(GameMng.I.healerSkillPrefab[0], spawnPos, Quaternion.Euler(90, 0, lookAngle)) as GameObject;
        attObj.GetComponent<Rigidbody>().velocity = attObj.transform.TransformDirection(Vector3.right * 5);
        if (isMe)
            attObj.tag = "Weapon_disposable_me";

        if (transform.position.x > attackDir.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));

        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Attack");
    }

    public override void skill_1(Vector2 skillPos, bool isMe = false)
    {
        if (transform.position.x > skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));

        Instantiate(GameMng.I.healerSkillPrefab[1], new Vector3(skillPos.x, 0, skillPos.y), Quaternion.Euler(20, 0, 0));
        
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Tree");
    }
    public override void skill_2(Vector2 skillPos, bool isMe = false)
    {
        // TODO : 나한테 쉴드 줌
        GameMng.I.stateMng.user_Shield_Numerical.Add(
            new ShieldBuff(5, Mathf.FloorToInt(GameMng.I.stateMng.user_HP_Numerical.fullHp * 0.3f))
        );
        GameMng.I.stateMng.ShieldPos();
        
        GameMng.I.stateMng.ActiveBuff(
            skilldatas[1].getBuffData
        );

        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Thief");
    }
    public override void skill_3(Vector2 skillPos, bool isMe = false)
    {
        if (transform.position.x < skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));

        // tag = "Weapon"

        _action = CHARACTER_ACTION.CAN_MOVE;
        _anim.SetTrigger("Skill_Winterspring");
    }
    public override void skill_4(Vector2 skillPos, bool isMe = false)
    {
        if (transform.position.x > skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));
            
        GameObject attObj = Instantiate(GameMng.I.healerSkillPrefab[2], new Vector3(skillPos.x, -0.2f, skillPos.y), Quaternion.identity) as GameObject;
        if (isMe)
        {
            attObj.tag = "Skill_disposable_me";
            attObj.name = "3";
        }

        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Location");
    }
    public override void skill_5(Vector2 skillPos, bool isMe = false)
    {
        fanservice.transform.position = new Vector3(transform.parent.position.x, -0.199f, transform.parent.position.z);
        fanservice.SetActive(true);

        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Music");
    }
    
    void settingStat()
    {
        _stat = new Stat(
            1       /* 받는 피해량 퍼센트 */,
            5       /* 대쉬 쿨타임 */,
            10      /* 기상기 쿨타임 */,
            9       /* 이동 속도 */
        );
    }
}
