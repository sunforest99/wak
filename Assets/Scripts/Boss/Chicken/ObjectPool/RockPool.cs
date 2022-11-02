using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RockPool : MonoBehaviour
{
     [SerializeField] Rock rock;
    private IObjectPool<Rock> rockPool;

    void Start()
    {
        rockPool = new ObjectPool<Rock>(Init, OnObject, OnReleased, OnDestroyRock, maxSize: 10);
    }
    private Rock Init()
    {
        Rock obj = Instantiate(rock);
        obj.transform.parent = this.transform;
        obj.setPool(rockPool);
        return obj;
    }

    private void OnObject(Rock obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReleased(Rock obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyRock(Rock obj)
    {
        Destroy(obj.gameObject);
    }

    public void Create(Transform trans)
    {
        var temp = rockPool.Get();
        temp.transform.position = new Vector3(trans.position.x, 5f, trans.position.z);
        temp.gameObject.SetActive(true);
    }
}
