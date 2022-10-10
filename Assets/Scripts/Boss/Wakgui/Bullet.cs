using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] float speed = 5.0f;
    int damage;
    void Start()
    {
        damage = GameMng.I.boss.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_CRISTAL);
    }

    private void OnEnable() {
        _rigidbody.velocity = transform.TransformDirection(Vector3.left * speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.stateMng.takeDamage(damage);
            ActiveFalse();
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
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.gameObject.SetActive(false);
    }
}
