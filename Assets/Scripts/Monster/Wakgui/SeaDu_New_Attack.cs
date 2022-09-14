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
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
    }
}
