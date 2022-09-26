using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInkCollider : DestroySelf
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
    }

    IEnumerator keep()
    {
        yield return new WaitForSecondsRealtime(3);
        destroySelf();
    }
}
