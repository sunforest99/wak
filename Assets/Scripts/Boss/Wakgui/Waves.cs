using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum POS
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public class Waves : DestroySelf
{
    int rand;
    int damage;
    [SerializeField] POS pos;
    void Start()
    { 
        damage = GameMng.I.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_WAVE);
        rand = Random.Range(0, 4);
        switch (rand)
        {
            case (int)POS.DOWN:
                this.transform.position = new Vector3(Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x), GameMng.I.mapRightTop.y, 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, 90.0f);
                break;

            case (int)POS.UP:
                this.transform.position = new Vector3(Random.Range(GameMng.I.mapLeftBotton.x, GameMng.I.mapRightTop.x), GameMng.I.mapLeftBotton.y, 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, -90.0f);
                break;

            case (int)POS.RIGHT:
                this.transform.position = new Vector3(GameMng.I.mapLeftBotton.x, Random.Range(GameMng.I.mapLeftBotton.y, GameMng.I.mapRightTop.y), 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, 180.0f);
                break;

            case (int)POS.LEFT:
                this.transform.position = new Vector3(GameMng.I.mapRightTop.x, Random.Range(GameMng.I.mapLeftBotton.y, GameMng.I.mapRightTop.y), 0);
                this.transform.localRotation = Quaternion.identity;
                break;
        }
    }

    void Update()
    {
        this.transform.Translate(Vector3.left * 3.0f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.bossData.getBuffs[2]);
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
        }
    }
}
