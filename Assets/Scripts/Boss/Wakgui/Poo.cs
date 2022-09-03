using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poo : DestroySelf
{
    [SerializeField] float waitingTime;
    [SerializeField] float maintainTime;
    [SerializeField] Animator _anim;
    int damage;
    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_POO);
        StartCoroutine(Remove());
    }
    
    IEnumerator Remove()
    {
        yield return new WaitForSecondsRealtime(waitingTime);
        
        transform.SetParent(transform.root.parent);
        transform.position = new Vector3(transform.position.x, -0.19f, transform.position.z);
        _anim.SetBool("Active", true);

        yield return new WaitForSecondsRealtime(maintainTime);
        
        destroySelf();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.boss.bossData.getBuffs[1]);
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
    }
}
