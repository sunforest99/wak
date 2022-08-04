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
        damage = GameMng.I.bossData.getPatternDmg((int)WAKGUI_ACTION.PATTERN_POO);
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
}
