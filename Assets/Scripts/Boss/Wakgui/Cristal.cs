using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawn;

    public WakguiObjectPool objectPool;

    int hp;
    int count;

    void Start()
    {
        count = 0;
        hp = 5;
        spawn.transform.localRotation = Quaternion.Euler(70, 0, Random.Range(-360, 361));

        StartCoroutine(CreateBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            this.transform.localPosition = Vector3.zero;
            this.gameObject.SetActive(false);
        }
        spawn.transform.Rotate(new Vector3(0.0f, 0.0f, 50.0f) * Time.deltaTime);
    }

    IEnumerator CreateBullet()
    {
        if (count < 1)
        {
            objectPool.setBulletActive(this.transform.position, spawn.transform.rotation.eulerAngles);
            Debug.Log(spawn.transform.rotation.eulerAngles);
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
