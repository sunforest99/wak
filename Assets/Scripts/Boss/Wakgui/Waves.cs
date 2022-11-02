using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

enum POS
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public class Waves : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    private IObjectPool<Waves> wavePool;

    public int rand;
    int damage;

    void Start()
    {
        damage = 100;

        Invoke("HoxyDestory", 10.0f);
    }

    public void setPool(IObjectPool<Waves> pool)
    {
        wavePool = pool;
    }

    private void OnEnable()
    {
        switch (rand)
        {
            case (int)POS.UP:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, -90.0f);
                break;

            case (int)POS.DOWN:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 90.0f);
                break;

            case (int)POS.RIGHT:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 180.0f);
                break;

            case (int)POS.LEFT:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 0);
                break;
        }

        _rigidbody.velocity = transform.TransformDirection(Vector3.left * 3f);
    }

    void HoxyDestory()
    {
        wavePool.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.I.showEff(EFF_TYPE.TAKEN_EFF, this.transform.position);
            GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.boss.bossData.getBuffs[2]);
            GameMng.I.stateMng.takeDamage(damage);
        }
        else if (other.CompareTag("Map_Wall"))
        {
            GameMng.I.showEff(EFF_TYPE.REMOVE_EFF, this.transform.position);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }
    }
}
