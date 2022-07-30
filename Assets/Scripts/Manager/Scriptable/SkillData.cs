using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New SkillData", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] private int _code;
    [SerializeField] private int _minSkillDamage;

    [SerializeField] private int _maxSkillDamage;

    [SerializeField] private int _collDown;
    public int getColldown { get { return _collDown; } }

    [SerializeField] bool isCanBack;

    [SerializeField] private int critical;


    public int CalcSkillDamage()
    {
        return Random.Range(_minSkillDamage, _maxSkillDamage + 1);
    }
    
}
