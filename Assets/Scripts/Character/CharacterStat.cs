using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARACTER_ACTION
{
    IDLE,                   // 일반 상태, 움직이거나 스킬 사용이 가능한 상태
    CANT_ANYTHING,          // 스킬 쓰는 상태, 아무것도 못함
    SLEEP_CANT_ANYTHING,    // 기절 상태, 기상기 외에는 아무것도 못함
    ATTACK_CANT_ANYTHING,   // 기본 공격, 연속 기본공격 입력외에는 아무것도 못함
    CAN_MOVE                // 스킬 쓰는 상태, 캔슬이 가능한 상태
}

public struct Stat
{
    public int minDamage, maxDamage;    // 실 최소 ~ 최대 데미지
    public float incBackattackPer;      // 백어택 증가량 퍼센트  ex) 1.2 라면  데미지 120%
    public float criticalPer;           // 치명타 확률  ex) 값이 10이라면 10%
    // public float incDamagePer;          // 공격력 증가 퍼센트
    
    public float takenDamagePer;        // 받는 피해량 퍼센트
    public float incHPPer;              // 체력 증가량 퍼센트
    public float takenHealPer;          // 받는 회복량 퍼센트
    
    public float moveSpeedPer;          // 이동 속도 퍼센트
    public int dashCool;              // 대쉬 쿨타임
    public int wakeUpCool;            // 기상기 쿨타임

    public int skill_0_cool, skill_1_cool, skill_2_cool, skill_3_cool, skill_4_cool;
    public float skill_0_dmg, skill_1_dmg, skill_2_dmg, skill_3_dmg, skill_4_dmg;

    // 데미지 증가량 (기본 1~1.2), 받는 피해량 (기본 1), 대쉬 쿨타임, 기상 쿨타임, 이동 속도
    public Stat(
        float takenDamagePer, 
        int dashCool, int wakeUpCool, float moveSpeedPer        
    )
    {
        /**********************************************************************
         *                              데미지
         *********************************************************************/
        this.minDamage = 628318;        // 628,318      // 314159 * 2
        this.maxDamage = 942477;        // 942,477      // 314159 * 3

        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DAMAGE_3))
        {
            this.minDamage = Mathf.FloorToInt(this.minDamage * 1.3f);
            this.maxDamage = Mathf.FloorToInt(this.minDamage * 1.3f);
        }
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DAMAGE_2))
        {
            this.minDamage = Mathf.FloorToInt(this.minDamage * 1.2f);
            this.maxDamage = Mathf.FloorToInt(this.minDamage * 1.2f);
        }
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DAMAGE_1))
        {
            this.minDamage = Mathf.FloorToInt(this.minDamage * 1.1f);
            this.maxDamage = Mathf.FloorToInt(this.minDamage * 1.1f);
        }
        
        this.incBackattackPer = 1.2f;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.INC_BACK_3))
            this.incBackattackPer = 1.5f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.INC_BACK_2))
            this.incBackattackPer = 1.4f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.INC_BACK_1))
            this.incBackattackPer = 1.3f;
        
        this.criticalPer = 9;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.CRITICAL_PER_3))
            this.criticalPer = 15;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.CRITICAL_PER_2))
            this.criticalPer = 13;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.CRITICAL_PER_1))
            this.criticalPer = 11;
        
        /**********************************************************************
         *                              강인함
         *********************************************************************/
        this.takenDamagePer = takenDamagePer;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.TAKEN_3))
            this.takenDamagePer *= 0.85f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.TAKEN_2))
            this.takenDamagePer *= 0.9f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.TAKEN_1))
            this.takenDamagePer *= 0.95f;

        this.takenHealPer = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HEAL_3))
            this.takenHealPer = 1.15f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HEAL_2))
            this.takenHealPer = 1.1f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HEAL_1))
            this.takenHealPer = 1.05f;
        
        this.incHPPer = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HP_3))
            this.incHPPer = 1.3f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HP_2))
            this.incHPPer = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.HP_1))
            this.incHPPer = 1.1f;


        /**********************************************************************
         *                              기민함
         *********************************************************************/
        this.dashCool = dashCool;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DASH_3))
            this.dashCool -= 3;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DASH_2))
            this.dashCool -= 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.DASH_1))
            this.dashCool -= 1;

        this.wakeUpCool = wakeUpCool;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.WAKEUP_3))
            this.wakeUpCool -= 3;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.WAKEUP_2))
            this.wakeUpCool -= 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.WAKEUP_1))
            this.wakeUpCool -= 1;

        this.moveSpeedPer = moveSpeedPer;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SPEED_3))
            this.moveSpeedPer *= 1.3f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SPEED_2))
            this.moveSpeedPer *= 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SPEED_1))
            this.moveSpeedPer *= 1.1f;
        

        /**********************************************************************
         *                              능력(스킬)
         *********************************************************************/
        this.skill_0_cool = 0; this.skill_1_cool = 0; this.skill_2_cool = 0; this.skill_3_cool = 0; this.skill_4_cool = 0;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_0_COOL_1))         this.skill_0_cool = 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_0_COOL_0))    this.skill_0_cool = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_1_COOL_1))         this.skill_1_cool = 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_1_COOL_0))    this.skill_1_cool = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_2_COOL_1))         this.skill_2_cool = 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_2_COOL_0))    this.skill_2_cool = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_3_COOL_1))         this.skill_3_cool = 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_3_COOL_0))    this.skill_3_cool = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_4_COOL_1))         this.skill_4_cool = 2;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_4_COOL_0))    this.skill_4_cool = 1;

        this.skill_0_dmg = 1; this.skill_1_dmg = 1; this.skill_2_dmg = 1; this.skill_3_dmg = 1; this.skill_4_dmg = 1;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_0_DMG_1))          this.skill_0_dmg = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_0_DMG_0))     this.skill_0_dmg = 1.1f;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_1_DMG_1))          this.skill_1_dmg = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_1_DMG_0))     this.skill_1_dmg = 1.1f;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_2_DMG_1))          this.skill_2_dmg = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_2_DMG_0))     this.skill_2_dmg = 1.1f;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_3_DMG_1))          this.skill_3_dmg = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_3_DMG_0))     this.skill_3_dmg = 1.1f;
        if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_4_DMG_1))          this.skill_4_dmg = 1.2f;
        else if (GameMng.I.userData.upgrade.Contains(Skill_TREE.SKILL_4_DMG_0))     this.skill_4_dmg = 1.1f;

        // 장착중인 무기, 장비에 따라 추가 스탯 변동 일어나게
        // if (GameMng.I.userData.character.weapon)
    }
}

public enum SKILL_CODE
{
    SKILL_1,
    SKILL_2,
    SKILL_3,
    SKILL_4,
    SKILL_5,
    DASH,
    WAKEUP,
    ATTACK
}

public enum Skill_TREE
{
    // 데미지
    DAMAGE_1, DAMAGE_2, DAMAGE_3,
    CRITICAL_PER_1, CRITICAL_PER_2, CRITICAL_PER_3,
    INC_BACK_1, INC_BACK_2, INC_BACK_3,
    // 강인함
    TAKEN_1, TAKEN_2, TAKEN_3,
    HP_1, HP_2, HP_3,
    HEAL_1, HEAL_2, HEAL_3,
    // 기민함
    SPEED_1, SPEED_2, SPEED_3,
    DASH_1, DASH_2, DASH_3,
    WAKEUP_1, WAKEUP_2, WAKEUP_3,
    // 능력
    SKILL_0_COOL_0, SKILL_0_COOL_1, SKILL_0_DMG_0, SKILL_0_DMG_1,
    SKILL_1_COOL_0, SKILL_1_COOL_1, SKILL_1_DMG_0, SKILL_1_DMG_1,
    SKILL_2_COOL_0, SKILL_2_COOL_1, SKILL_2_DMG_0, SKILL_2_DMG_1,
    SKILL_3_COOL_0, SKILL_3_COOL_1, SKILL_3_DMG_0, SKILL_3_DMG_1,
    SKILL_4_COOL_0, SKILL_4_COOL_1, SKILL_4_DMG_0, SKILL_4_DMG_1
}