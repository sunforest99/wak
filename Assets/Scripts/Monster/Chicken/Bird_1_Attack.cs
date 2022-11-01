using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_1_Attack : MonoBehaviour
{
    int damage = 2000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, transform.position);
            GameMng.I.stateMng.takeDamage(damage);
        }
    }
}
