using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public Bullet bullet;
    private IObjectPool<Bullet> bulletPool;

    void Start()
    {
        bulletPool = new ObjectPool<Bullet>(Init, actionOnRelease : OnReleased, actionOnDestroy : OnDestroyBullet, maxSize : 5 );
    }

    private Bullet Init()
    {
        Bullet obj = Instantiate(bullet);
        obj.transform.parent = this.transform;
        obj.setPool(bulletPool);
        return obj;
    }

    private void OnReleased(Bullet obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);
    }

    public void Create(Vector3 vec, Quaternion rotate)
    {
         var temp = bulletPool.Get();
        temp.transform.position = vec;
        temp.transform.rotation = rotate;
        temp.gameObject.SetActive(true);
    }
}
