using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cristal : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    [SerializeField] IObjectPool<Cristal> cristalPool;

    // 총알 
    public BulletPool bulletPool;

    public int uniqueNum;       // 고유 번호 Instantiate 때 초기화해줌
    public int spawnNum;        // 소환됬을때 번호

    int hp;
    [SerializeField] int count;

    void Start()
    {
        spawnNum = 0;
        count = 0;
        hp = 5;
        spawn.transform.localRotation = Quaternion.Euler(90, 0, 0);

        StartCoroutine(CreateBullet());
    }

    public void setPool(IObjectPool<Cristal> pool)
    {
        cristalPool = pool;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            NetworkMng.I.SendMsg(string.Format("BOSS_PATTERN:{0}:{1}", (int)WAKGUI_ACTION.CRISTAL_BROKEN, uniqueNum.ToString()));
            this.transform.localPosition = Vector3.zero;
            DestoryCristal();
        }

        spawn.transform.Rotate(new Vector3(0.0f, 0.0f, 20.0f) * Time.deltaTime);
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(.5f);
            bulletPool.Create(this.transform.position, spawn.transform.rotation);
        }
        
        yield return new WaitForSeconds(5.0f);

        StartCoroutine(CreateBullet());
    }

    void DestoryCristal()
    {
        cristalPool.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            hp--;
        }
    }
}
