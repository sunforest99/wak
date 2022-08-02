using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "ScriptableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] int hp;
    [SerializeField] int minDmg;
    [SerializeField] int maxDmg;
    
    [SerializeField] int addDmg;
    [SerializeField] int heal;
    [SerializeField] float moveSpeed;
    [SerializeField] int critical;
    [SerializeField] int attackSpeed;
}
