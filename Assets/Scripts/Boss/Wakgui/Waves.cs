using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum POS
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public class Waves : DestroySelf
{
    int rand;

    [SerializeField] POS pos;
    void Start()
    {
        rand = Random.Range(0, 4);
        switch (rand)
        {
            case (int)POS.DOWN:
                this.transform.localPosition = new Vector3(Random.Range(-13, 13), 8, 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, 90.0f);
                break;

            case (int)POS.UP:
                this.transform.localPosition = new Vector3(Random.Range(-13, 13), -8, 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, -90.0f);
                break;

            case (int)POS.RIGHT:
                this.transform.localPosition = new Vector3(-13, Random.Range(-7, 8), 0);
                this.transform.localRotation = Quaternion.Euler(0, 0, 180.0f);
                break;

            case (int)POS.LEFT:
                this.transform.localPosition = new Vector3(13, Random.Range(-7, 8), 0);
                this.transform.localRotation = Quaternion.identity;
                break;
        }
    }

    void Update()
    {
        this.transform.Translate(Vector3.down * 3.0f * Time.deltaTime);
    }
}
