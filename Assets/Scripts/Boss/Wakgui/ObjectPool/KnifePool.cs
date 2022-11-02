using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class KnifePool : MonoBehaviour
{
    [SerializeField] private Knife_portal knife;

    private IObjectPool<Knife_portal> knifePool;

    void Start()
    {
        knifePool = new ObjectPool<Knife_portal>(Init, OnObject, OnReleased, maxSize: 10);
    }

    private Knife_portal Init()
    {
        Knife_portal obj = Instantiate(knife);
        obj.transform.parent = this.transform;
        obj.setPool(knifePool);
        return obj;
    }

    private void OnObject(Knife_portal obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReleased(Knife_portal obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyKnife(Knife_portal obj)
    {
        Destroy(obj.gameObject);
    }

    public void Create(float x, float z)
    {
        var temp = knifePool.Get();
        temp.transform.position = new Vector3(x, 0.4f, z);
    }
}
