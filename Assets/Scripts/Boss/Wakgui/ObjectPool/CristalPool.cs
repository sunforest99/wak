using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CristalPool : MonoBehaviour
{
    public Cristal cristal;
    private IObjectPool<Cristal> cristalPool;
    [SerializeField] BulletPool bulletPool;
    void Start()
    {
        cristalPool = new ObjectPool<Cristal>(Init, OnObject, OnReleased, maxSize: 30);
    }

    private Cristal Init()
    {
        Cristal obj = Instantiate(cristal);
        obj.transform.parent = this.transform;
        obj.setPool(cristalPool);
        return obj;
    }

    private void OnObject(Cristal obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReleased(Cristal obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void Create(float x, float z, int num)
    {
        var temp = cristalPool.Get();
        temp.bulletPool = bulletPool;
        temp.transform.position = new Vector3(x, 0.4f, z);
        temp.spawnNum = num;
    }
}
