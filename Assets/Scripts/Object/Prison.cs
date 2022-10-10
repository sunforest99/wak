using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour
{
    [SerializeField] Animator _anim;

    int count = 5;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me") || other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me"))
        {
            count--;

            MCamera.I.shake(5, .15f);

            if (count <= 0)
            {
                _anim.SetTrigger("Destroy");
            }
            else
            {
                _anim.SetTrigger("Hit");
            }
        }
    }
}
