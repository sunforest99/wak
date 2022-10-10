using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInkCollider : DestroySelf
{
    int damage = 2000;
    float contactTime = 0;


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
            contactTime = 0;
            GameMng.I.stateMng.takeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            contactTime += Time.deltaTime;
            if (contactTime >= 0.1f) {
                GameMng.I.stateMng.takeDamage(damage);
                // 디버프를 줄 지 고민중
                contactTime -= 0.1f;
            }
        }
    }

    IEnumerator keep()
    {
        yield return new WaitForSecondsRealtime(3);
        destroySelf();
    }
}
