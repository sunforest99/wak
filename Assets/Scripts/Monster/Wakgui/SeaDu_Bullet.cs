using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] float speed = 6.0f;

    int damage = 2000;

    void Start()
    {   
        if (DungeonMng._dungeon_Type.Equals(DUNGEON_TYPE.MONSTER_PURPLER))
        {
            damage *= Mathf.FloorToInt(damage * 1.5f);
        }
    }

    public void setVelocity() {
        _rigidbody.velocity = transform.TransformDirection(Vector3.right * speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.stateMng.takeDamage(damage);
            ActiveFalse();
        }
        // else if (other.CompareTag("Character"))
        // {
        //     ActiveFalse();
        // }
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
