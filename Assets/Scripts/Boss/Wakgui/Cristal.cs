using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : MonoBehaviour
{
    [SerializeField] GameObject spawn;

    public WakguiObjectPool objectPool;

    public int uniqueNum;       // 고유 번호 Instantiate 때 초기화해줌
    public int spawnNum;        // 소환됬을때 번호

    int hp;
    [SerializeField]int count;

    void Start()
    {
        spawnNum = 0;
        count = 0;
        hp = 5;
        spawn.transform.localRotation = Quaternion.Euler(90, 0, objectPool.rand[spawnNum]);

        StartCoroutine(CreateBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            NetworkMng.I.SendMsg(string.Format("BOSS_PATTERN:{0}:{1}", (int)WAKGUI_ACTION.CRISTAL_BROKEN, uniqueNum.ToString()));
            this.transform.localPosition = Vector3.zero;
            this.gameObject.SetActive(false);
        }
        spawn.transform.Rotate(new Vector3(0.0f, 0.0f, 20.0f) * Time.deltaTime);
    }

    IEnumerator CreateBullet()
    {
        if (count < 100)
        {
            objectPool.setBulletActive(this.transform.position, spawn.transform.rotation);
            count++;
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
            count = 0;
        }
        StartCoroutine(CreateBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            hp--;
        }
    }
}
