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
        DASH_COOLTIME = 7;
        WAKEUP_COOLTIME = 11;

        // if 이게 내꺼라면
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
        Vector3 moveTo = new Vector3(skillPos.x, 0.3f, skillPos.y);
        moveTo -= transform.position;
        float lookAngle =  Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.5f;

        attObj = Instantiate(GameMng.I.magicianSkillPrefab[1], spawnPos, Quaternion.Euler(90, 0, lookAngle)) as GameObject;
        attObj.GetComponent<Rigidbody>().velocity = attObj.transform.TransformDirection(Vector3.right * 5);
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
