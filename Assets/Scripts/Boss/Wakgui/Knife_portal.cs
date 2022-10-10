using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_portal : MonoBehaviour
{
    int damage;
    [SerializeField] Animator animator;

    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_KNIFE);
    }

    // void Update()
    // {
    //     if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
    //     {
    //         this.transform.localPosition = Vector3.zero;
    //         this.gameObject.SetActive(false);
    //     }
    // }
    
    void animationDone()
    {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("칼 데미지 : " + damage);
            GameMng.I.character.sleep();
            GameMng.I.stateMng.takeDamage(damage);
        }
    }
}
