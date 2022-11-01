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
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, new Vector3(
                other.transform.position.x,
                other.transform.position.y + 1,
                other.transform.position.z
            ));
            if (DungeonMng._dungeon_Type.Equals(DUNGEON_TYPE.MONSTER_PURPLER))
                GameMng.I.stateMng.takeDamage(Random.Range(19000, 20000));
            else
                GameMng.I.stateMng.takeDamage(Random.Range(9000, 10000));
        }
    }
}
