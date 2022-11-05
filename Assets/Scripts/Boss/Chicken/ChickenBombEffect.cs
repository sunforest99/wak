using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBombEffect : DestroySelf
{
    int damage = 2000;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, new Vector3(
                other.transform.position.x,
                other.transform.position.y + 1,
                other.transform.position.z
            ));
            GameMng.I.stateMng.takeDamage(damage);
        }
    }
}
