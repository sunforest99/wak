using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] float speed = 5.0f;

    public int damage = 1;

    public void setVelocity() {
        _rigidbody.velocity = transform.TransformDirection(Vector3.right * speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Character"))
        {
            GameMng.I.stateMng.user_HP_Numerical.Hp -= damage;
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
