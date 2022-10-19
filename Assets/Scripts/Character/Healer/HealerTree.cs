using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerTree : DestroySelf
{
    public int skillLevel = 1;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO : 
            // 스킬레벨에 따른
            // 나에게 힐함
            // 변한 내 hp 네트워크로 다른이들에게 보내기
            
            GameMng.I.stateMng.user_HP_Numerical.Hp += Mathf.FloorToInt(104719 * Character._stat.takenHealPer);
            if (GameMng.I.stateMng.user_HP_Numerical.Hp > GameMng.I.stateMng.user_HP_Numerical.fullHp)
                GameMng.I.stateMng.user_HP_Numerical.Hp = GameMng.I.stateMng.user_HP_Numerical.fullHp;
        }
    }
}
