using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DestroySelf
{
    int damage;
    void Start()
    {
        damage = GameMng.I.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_CRISTAL);
    }

    void Update()
    {
        transform.Translate(Vector3.down * 3.0f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
            destroySelf();
        }
    }
}
