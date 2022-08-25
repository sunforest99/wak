using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : DestroySelf
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawn;

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
        if(hp <= 0)
        {
            destroySelf();
        }
        spawn.transform.Rotate(new Vector3(0.0f, 0.0f, 50.0f) * Time.deltaTime);
    }

    IEnumerator CreateBullet()
    {
        Debug.Log("!!!!!!!!");
        if (count < 7)
        {
            Instantiate(bullet, this.transform.localPosition, spawn.transform.rotation);
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
        if(other.CompareTag("Weapon"))
        {
            hp--;
        }
    }
}
