using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SKILL_INDEX 
{
    SKILL_0,
    SKILL_1,
    SKILL_2,
    SKILL_3,
    SKILL_4
}

[CreateAssetMenu(fileName = "New SkillData", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] SKILL_INDEX skillIdx;

    // 스킬 이름
    [SerializeField] string _name;
    public string getSkillName { get { return _name; } }

    // 스킬 데미지
    [SerializeField] float _skillDamge;

    // 쿨 타임
    [SerializeField] int _coolTime;
    public int getCoolTime { get {
        switch (skillIdx)
        {
            case SKILL_INDEX.SKILL_0:
                return _coolTime - Character._stat.skill_0_cool;
            case SKILL_INDEX.SKILL_1:
                return _coolTime - Character._stat.skill_1_cool;
            case SKILL_INDEX.SKILL_2:
                return _coolTime - Character._stat.skill_2_cool;
            case SKILL_INDEX.SKILL_3:
                return _coolTime - Character._stat.skill_3_cool;
            case SKILL_INDEX.SKILL_4:
                return _coolTime - Character._stat.skill_4_cool;
        }
        return _coolTime;
    } }

    // 백어택 가능 스킬인지
    [SerializeField] bool isCanBack;
    public bool isBackAttackSkill { get { return isCanBack; } }

    // 스킬 이미지
    [SerializeField] Sprite _skillImg;
    public Sprite getSkllImg { get { return _skillImg; } }

    // 스킬 레벨
    // public float skillLevel;

    // 스킬에 붙은 버프 데이터
    [SerializeField] BuffData _buffdata;
    public BuffData getBuffData { get { return _buffdata; } }

    // 타격시 카메라 흔들림 정도 (보통 5f)
    [SerializeField] float _intensity;
    public float getIntensity { get { return _intensity; } }

    // 타격시 카메라 흔들림 시간 (보통 .1f)
    [SerializeField] float _shakeTime;
    public float getShakeTime { get { return _shakeTime; } }


    public int CalcSkillDamage(
        bool isCritcal, bool isBackAttack,
        int minDamage, int maxDamage,
        float incBackattackPer
    )
    {
        // 캐릭터 자체의 데미지 범위 내에서 스킬 자체 데미지 보정 반영
        float dmg = Random.Range(minDamage, maxDamage) * _skillDamge;
        
        // 스킬 강화 내용 반영
        switch (skillIdx)
        {
            case SKILL_INDEX.SKILL_0:
                dmg *= Character._stat.skill_0_dmg;
                break;
            case SKILL_INDEX.SKILL_1:
                dmg *= Character._stat.skill_1_dmg;
                break;
            case SKILL_INDEX.SKILL_2:
                dmg *= Character._stat.skill_2_dmg;
                break;
            case SKILL_INDEX.SKILL_3:
                dmg *= Character._stat.skill_3_dmg;
                break;
            case SKILL_INDEX.SKILL_4:
                dmg *= Character._stat.skill_4_dmg;
                break;
        }

        // 크리티컬시 데미지 150%
        if (isCritcal)
            dmg *= 1.5f;

        // 백어택시 백어택 데미지 증가량 반영
        if (isBackAttack)
            dmg *= Character._stat.incBackattackPer;

        return (int)dmg;
    }

}
