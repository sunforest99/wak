using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Character
{
    public override void init()
    {
        _job = JOB.MAGICIAN;
        DASH_SPEED = 22;
        MOVE_SPEED = 6;
        DASH_COOLTIME = 7;
        WAKEUP_COOLTIME = 11;

        // if 이게 내꺼라면
        settingStat();        
    }

    public override void skill_1()
    {
        StartCoroutine(SkillCoolDown(0));
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Window");
        // NetworkMng.I.UseSkill(SKILL_CODE.SKILL_1);
    }
    public override void skill_2()
    {
        StartCoroutine(SkillCoolDown(1));
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Chimstercall");
        // NetworkMng.I.UseSkill(SKILL_CODE.SKILL_2);
    }
    public override void skill_3()
    {
        StartCoroutine(SkillCoolDown(2));
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Wakpasun");
        // NetworkMng.I.UseSkill(SKILL_CODE.SKILL_3);
    }
    public override void skill_4()
    {
        StartCoroutine(SkillCoolDown(3));
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("Skill_Fire");
        // NetworkMng.I.UseSkill(SKILL_CODE.SKILL_4);
    }
    public override void skill_5()
    {
        StartCoroutine(SkillCoolDown(4));
        _action = CHARACTER_ACTION.CANT_ANYTHING;
        _anim.SetTrigger("");
        // NetworkMng.I.UseSkill(SKILL_CODE.SKILL_5);
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
