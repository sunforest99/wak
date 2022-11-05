using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BirdPool : MonoBehaviour
{
    [SerializeField] Bird bird;
    private IObjectPool<Bird> birdPool;

    void Start()
    {
        birdPool = new ObjectPool<Bird>(Init, OnObject, OnReleased, OnDestroyBird, maxSize: 10);
    }
    private Bird Init()
    {
        Bird obj = Instantiate(bird);
        obj.transform.parent = this.transform;
        obj.setPool(birdPool);
        return obj;
    }

    private void OnObject(Bird obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReleased(Bird obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyBird(Bird obj)
    {
        Destroy(obj.gameObject);
    }

    public void Create(Transform bossPos, int pos)
    {
        var temp = birdPool.Get();
        temp.transform.position = bossPos.position;
        // temp.rand = pos;
        temp.gameObject.SetActive(true);
        temp.Init(pos);
        // StartCoroutine(BridsetActive(temp));
    }
}
