using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    public Witch _witch;

    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] float speed = 5.0f;
    int damage;
    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WITCH_ACTION.PATTERN_DODGE_1);
    }

    private void OnEnable() {
        _rigidbody.velocity = transform.TransformDirection(Vector3.left * speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, this.transform.position);
            GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.boss.bossData.getBuffs[0]);
            GameMng.I.stateMng.takeDamage(damage);
            ActiveFalse();
            // 디버프!
        }
        else if (other.CompareTag("Character"))
        {
            ActiveFalse();
        }
        else if (other.CompareTag("Map_Wall"))
        {
            GameMng.I.showEff(EFF_TYPE.REMOVE_EFF, this.transform.position);
            ActiveFalse();
        }
    }

    void ActiveFalse()
    {
        _witch.dodgePool.Enqueue(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
