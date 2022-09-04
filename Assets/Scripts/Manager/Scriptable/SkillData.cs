using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    // 스킬 이름
    [SerializeField] string _name;
    public string getSkillName { get { return _name; } }

    // 스킬 데미지
    [SerializeField] float _skillDamge;

    // 쿨 타임
    [SerializeField] int _coolTime;
    public int getCoolTime { get { return _coolTime; } }

    // 백어택 가능 스킬인지
    [SerializeField] bool isCanBack;
    public bool isBackAttackSkill { get { return isCanBack; } }

    // 스킬 이미지
    [SerializeField] Sprite _skillImg;
    public Sprite getSkllImg { get { return _skillImg; } }

    // 스킬 레벨
    public float skillLevel;

    // 스킬에 붙은 버프 데이터
    [SerializeField] BuffData _buffdata;
    public BuffData getBuffData { get { return _buffdata; } }

    // 타격시 카메라 흔들림 정도 (보통 5f)
    [SerializeField] float _intensity;
    public float getIntensity { get { return _intensity; } }

    // 타격시 카메라 흔들림 시간 (보통 .1f)
    [SerializeField] float _shakeTime;
    public float getShakeTime { get { return _shakeTime; } }


    public int CalcSkillDamage(bool isCritcal, bool isBackAttack, int minDamage, int maxDamage, float incDamagePer, float criticalPer, float incBackattackPer)
    {
        int BackAttackDmg(bool isBackAttack, int minDamage, int maxDamage, float incDamagePer, float incBackattackPer)
        {
            if (isBackAttack)
            {
                return (int)((float)Random.Range(minDamage, maxDamage + 1) * _skillDamge * skillLevel * incBackattackPer);
            }
            else
            {
                // 스킬 데미지 + (100,000 ~ 200,000 중 렌덤) * 공격력 증가 * (보스 방어력)
                return (int)((float)Random.Range(minDamage, maxDamage + 1) * _skillDamge * skillLevel);
            }
        }

        if (isCritcal)
        {
            return (int)((float)Random.Range(minDamage, maxDamage + 1) * _skillDamge * skillLevel * 1.5f);
        }
        else
        {
            return (int)((float)Random.Range(minDamage, maxDamage + 1) * _skillDamge * skillLevel);
        }
    }

}
