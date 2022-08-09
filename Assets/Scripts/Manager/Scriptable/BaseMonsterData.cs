using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterData : ScriptableObject
{
    [SerializeField] protected int _startHp;          // <! 보스 총 체력
    public int getStartHp { get { return _startHp; } }

    [SerializeField] protected string _name;      // <! 보스이름
    public string getName { get { return _name; } }

    [SerializeField] protected float _moveSpeed;      // <! 보스 이동
    public float getMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField] protected int[] _damage;
    public virtual int getDamage() => _damage[0];
}
