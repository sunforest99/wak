using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jing_Skill : DestroySelf
{
    bool isEntering = false;

    IEnumerator ShieldUp()
    {
        Character._stat.takenDamagePer = Character._stat.takenDamagePer * 0.8f;

        yield return new WaitForSeconds(5.0f);

        Character._stat.takenDamagePer = Character._stat.takenDamagePer / 0.8f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEntering)
        {
            isEntering = true;

            // 디버프 해제
            GameMng.I.stateMng.removeAllDebuff();
            
            // 데미지 감소 50% ON
            Character._stat.takenDamagePer = Character._stat.takenDamagePer * 0.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 디버프 해제
            GameMng.I.stateMng.removeAllDebuff();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isEntering)
        {
            isEntering = false;

            // 데미지 감소 50% OFF
            Character._stat.takenDamagePer = Character._stat.takenDamagePer / 0.5f;
        }
    }

    private void OnDisable() {
        if (isEntering) {
            isEntering = false;

            // 데미지 감소 버프 OFF
            Character._stat.takenDamagePer = Character._stat.takenDamagePer / 0.5f;
        }
    }
}
