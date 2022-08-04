using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrecterCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster_Weapon"))
        {
            Debug.Log("기본 패턴 데미지 " + GameMng.I.bossData.getPatternDmg());
        }
    }
}
