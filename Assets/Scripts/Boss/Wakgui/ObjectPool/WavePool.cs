using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WavePool : MonoBehaviour
{
    [SerializeField] Waves wave;
    private IObjectPool<Waves> wavePool;

    void Start()
    {
        wavePool = new ObjectPool<Waves>(Init, actionOnRelease : OnReleased, actionOnDestroy : OnDestroyWave, maxSize: 16);
    }

    private Waves Init()
    {
        Waves obj = Instantiate(wave);
        obj.transform.parent = this.transform;
        obj.setPool(wavePool);
        return obj;
    }

    private void OnReleased(Waves obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyWave(Waves obj)
    {
        Destroy(obj.gameObject);
    }

    public void Create(float x, float z, int rand)
    {
        var temp = wavePool.Get();
        temp.rand = rand;
        temp.transform.position = new Vector3(x, 0.4f, z);
        temp.gameObject.SetActive(true);
    }
}
