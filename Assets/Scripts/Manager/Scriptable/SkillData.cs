using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] private string _name;
    public string getSkillName { get { return _name; } }
    [SerializeField] private int _minSkillDamage;

    [SerializeField] private int _maxSkillDamage;

    [SerializeField] private int _collDown;
    public int getColldown { get { return _collDown; } }

    [SerializeField] bool isCanBack;

    [SerializeField] private int critical;

    [SerializeField] private Sprite _skillImg;
    public Sprite getSkllImg { get { return _skillImg; } }

    public int skillLevel;

    public int CalcSkillDamage(bool isBackAttack)
    {
        if (isBackAttack)
        {
            return (int)((float)Random.Range(_minSkillDamage, _maxSkillDamage + 1) * 1.5f) * skillLevel;
        }
        else
        {
            return Random.Range(_minSkillDamage, _maxSkillDamage + 1) * skillLevel;
        }
    }

}
