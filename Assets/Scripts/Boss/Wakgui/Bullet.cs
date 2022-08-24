using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DestroySelf
{
    int damage;
    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_CRISTAL);
    }

    void Update()
    {
        if(this.transform.position.x < GameMng.I.mapLeftBotton.x ||
        this.transform.position.x > GameMng.I.mapRightTop.x ||
        this.transform.position.y < GameMng.I.mapLeftBotton.y || 
        this.transform.position.y > GameMng.I.mapRightTop.y)
        {
            destroySelf();
        }
        transform.Translate(Vector3.back * 3.0f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
            destroySelf();
        }
    }
}
