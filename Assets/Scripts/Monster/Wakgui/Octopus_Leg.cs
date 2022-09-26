using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus_Leg : MonoBehaviour
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
            // TODO : 기절 추가할지 고민
            // GameMng.I.character.sleep();
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
    }

    public void ActiveOffSelf()
    {
        this.gameObject.SetActive(false);
    }
}
