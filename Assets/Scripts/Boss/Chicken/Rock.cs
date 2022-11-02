using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Rock : MonoBehaviour
{
    private IObjectPool<Rock> rockPool;

    Rigidbody rigd = null;
    void Start()
    {
        rigd = GetComponent<Rigidbody>();
        rigd.velocity = transform.TransformDirection(Vector3.down * 3f);

        Invoke("DestroyRock", 6f);
    }

    public void setPool(IObjectPool<Rock> pool)
    {
        rockPool = pool;
    }

    private void DestroyRock()
    {
        rockPool.Release(this);
    }
}
