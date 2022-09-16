using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_New_Skill0 : ActiveSelf
{
    int damage = 2000;

    void Start()
    {   
        if (DungeonMng._dungeon_Type.Equals(DUNGEON_TYPE.MONSTER_PURPLER))
        {
            damage *= Mathf.FloorToInt(damage * 1.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
        else if (other.CompareTag("Map_Wall"))
        {
            ActiveOffSelf();
        }
    }
}
