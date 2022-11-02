using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Knife_portal : MonoBehaviour
{
    int damage;
    [SerializeField] Animator animator;
    [SerializeField] IObjectPool<Knife_portal> knifePool;

    void Start()
    {
        damage = 1000;
        Invoke("HoxyDestory", 10.0f);
    }

    public void setPool(IObjectPool<Knife_portal> pool)
    {
        knifePool = pool;
    }
    
    void animationDone()
    {
        this.transform.localPosition = Vector3.zero;
        knifePool.Release(this);
    }

    void HoxyDestory()
    {
        knifePool.Release(this);
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
