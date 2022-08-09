using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] private string _name;
    public string getSkillName { get { return _name; } }

    [SerializeField] private float _skillDamge;

    [SerializeField] private int _collDown;
    public int getColldown { get { return _collDown; } }

    [SerializeField] bool isCanBack;

    [SerializeField] private int critical;

    [SerializeField] private Sprite _skillImg;
    public Sprite getSkllImg { get { return _skillImg; } }

    public float skillLevel;

    [SerializeField] private BuffData _buffdata;
    public BuffData getBuffData { get { return _buffdata; } }

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
