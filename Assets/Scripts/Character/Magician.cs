using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Character
{
    GameObject attObj;

    public override void init()
    {
        _job = JOB.MAGICIAN;
        DASH_SPEED = 12;
        MOVE_SPEED = 9;

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
            StartCoroutine(SkillCoolDown(0));
            NetworkMng.I.UseSkill(SKILL_CODE.SKILL_1, point.x, point.z);
            skill_1(new Vector2(point.x, point.z), true);
        }
    }
    public override void input_skill_3()
    {
        Vector3 point = getMouseHitPoint();

        if (!float.IsNegativeInfinity(point.x))
        {
            StartCoroutine(SkillCoolDown(2));
            NetworkMng.I.UseSkill(SKILL_CODE.SKILL_3, point.x, point.z);
            skill_3(new Vector2(point.x, point.z), true);
        }
    }
    public override void input_skill_5()
    {
        Vector3 point = getMouseHitPoint();
        if (!float.IsNegativeInfinity(point.x))
        {
            if (Vector3.Distance(point, transform.position) < 10)
            {
                // 사전 거리 안이면 스킬 시전 가능
                StartCoroutine(SkillCoolDown(4));
                NetworkMng.I.UseSkill(SKILL_CODE.SKILL_5, point.x, point.z);
                skill_5(new Vector2(point.x, point.z), true);
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

        attObj = Instantiate(GameMng.I.healerSkillPrefab[0], spawnPos, Quaternion.Euler(90, 0, lookAngle)) as GameObject;
        // TODO : 기본 공격 스피드 조절 (스탯에 따라서?)
        attObj.GetComponent<Rigidbody>().velocity = attObj.transform.TransformDirection(Vector3.right * 10);


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
        Vector3 moveTo = new Vector3(skillPos.x, 0.3f, skillPos.y);
        moveTo -= transform.position;
        float lookAngle =  Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.5f;

        attObj = Instantiate(GameMng.I.magicianSkillPrefab[1], spawnPos, Quaternion.Euler(90, 0, lookAngle)) as GameObject;
        // TODO : 기본 공격 스피드 조절 (스탯에 따라서?)
        attObj.GetComponent<Rigidbody>().velocity = attObj.transform.TransformDirection(Vector3.right * 7);

        attObj.transform.GetChild(0).rotation = Quaternion.Euler(20, 0, 0);
        if (isMe)
        {
            attObj.tag = "Skill_disposable_me";
            attObj.name = "0";
        }

        if (transform.position.x > skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));
        
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Wind");
    }
    public override void skill_2(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Wakpasun");
    }
    public override void skill_3(Vector2 skillPos, bool isMe = false)
    {
        Vector3 moveTo = new Vector3(skillPos.x, 0.3f, skillPos.y);
        moveTo -= transform.position;
        float lookAngle =  Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.3f;

        attObj = Instantiate(GameMng.I.magicianSkillPrefab[2], spawnPos, Quaternion.Euler(90, 0, lookAngle)) as GameObject;
        attObj.GetComponent<Rigidbody>().velocity = attObj.transform.TransformDirection(Vector3.right * 5);
        if (isMe)
        {
            attObj.tag = "Skill_disposable_me";
            attObj.name = "2";
        }

        if (transform.position.x > skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));
        
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Fire");
    }
    public override void skill_4(Vector2 skillPos, bool isMe = false)
    {
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("");
    }
    public override void skill_5(Vector2 skillPos, bool isMe = false)
    {
        if (transform.position.x > skillPos.x)
            transform.rotation = Quaternion.Euler(new Vector3(20f, 0, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(-20f, -180f, 0f));

        attObj =  Instantiate(GameMng.I.magicianSkillPrefab[3], new Vector3(skillPos.x, 0, skillPos.y), Quaternion.Euler(20, 0, 0)) as GameObject;
        if (isMe) {
            attObj.tag = "Skill";
            attObj.name = "4";
        }
        
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Chimstercall");
    }
    
    public void attObjActiveOn()
    {
        if (attObj)
            attObj.SetActive(true);
    }

    public void fireSkillForce()
    {
        addForceImpulse(new Vector3(transform.rotation.x < 100 ? 7 : -7, 0, 0));
    }

    void settingStat()
    {
        _stat = new Stat(
            1       /* 받는 피해량 퍼센트 */,
            7       /* 대쉬 쿨타임 */,
            11      /* 기상기 쿨타임 */,
            9       /* 이동 속도 */
        );
    }
}
