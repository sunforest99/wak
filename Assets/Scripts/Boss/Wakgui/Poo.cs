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
        damage = GameMng.I.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_POO);
        StartCoroutine(Remove());
    }
    
    IEnumerator Remove()
    {
        yield return new WaitForSeconds(waitingTime);
        transform.SetParent(transform.root.parent);
        _anim.SetBool("Active", true);
        yield return new WaitForSeconds(maintainTime);
        destroySelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.bossData.getBuffs[1]);
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
    }
}
