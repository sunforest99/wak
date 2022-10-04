using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public int uniqueNum;
    int hp = 2;
    public string characterId;
    public Character character = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (hp > 0)
            {
                hp--;
            }
            else
            {
                NetworkMng.I.SendMsg(string.Format("BOSS_PATTERN:{0}{1}", (int)CHICKEN_ACTION.EGG_BROKEN, uniqueNum));
            }
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            // Debug.Log(NetworkMng.I.v_users.key)
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (character == null)
        {
            // 터지는 액션 취기  (대충))
            GameMng.I.stateMng.user_HP_Numerical.Hp -= 1000;
        }
        else
        {
            character._action = CHARACTER_ACTION.IDLE;
            Debug.Log(character._action);
            character.tag = "Player";
        }
    }
}
