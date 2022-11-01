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
            damage = Mathf.FloorToInt(damage * 1.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(damage);
        if (other.CompareTag("Player"))
        {
            // TODO : 기절 추가할지 고민
            // GameMng.I.character.sleep();
            
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, new Vector3(
                other.transform.position.x,
                other.transform.position.y + 1,
                other.transform.position.z
            ));
            GameMng.I.stateMng.takeDamage(damage);
        }
    }

    public void ActiveOffSelf()
    {
        this.gameObject.SetActive(false);
    }
}
