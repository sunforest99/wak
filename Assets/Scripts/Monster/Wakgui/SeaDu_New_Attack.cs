using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_New_Attack : MonoBehaviour
{
    int damage = 1000;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (DungeonMng._dungeon_Type.Equals(DUNGEON_TYPE.MONSTER_PURPLER))
                GameMng.I.stateMng.takeDamage(Random.Range(19000, 20000));
            else
                GameMng.I.stateMng.takeDamage(Random.Range(9000, 10000));
        }
    }
}
