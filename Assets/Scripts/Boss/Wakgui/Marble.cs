using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : DestroySelf
{
    int hp = 2;
    public bool answer;
    void Start()
    {
        StartCoroutine(timeEnd());
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime);
        if( hp <= 0)
        {
            destroySelf();
        }
    }

    IEnumerator timeEnd()
    {
        yield return new WaitForSeconds(20);
        if (!answer)
        {
            Debug.Log("전멸");
            GameMng.I.stateMng.user_HP_Numerical.Hp -=  GameMng.I.stateMng.user_HP_Numerical.fullHp;
        }
        destroySelf();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Weapon"))
        {
            hp --;
        }
    }
}
