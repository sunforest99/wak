using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckHang : MonoBehaviour
{
    int damage;
    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WITCH_ACTION.PATTERN_NECKHANG);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, this.transform.position);
            GameMng.I.stateMng.takeDamage(damage);
        }
    }
}
