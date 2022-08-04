using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_portal : DestroySelf
{
    int damage;
    [SerializeField] Animator animator;

    void Start()
    {
        damage = GameMng.I.bossData.getPatternDmg((int)WAKGUI_ACTION.PATTERN_KNIFE);
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
            destroySelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("칼 데미지 : " + damage);
            GameMng.I.character.setTriggerSleep();
        }
    }
}
