using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : DestroySelf
{
    void Update()
    {
        transform.Translate(Vector3.down * 3.0f * Time.deltaTime);
    }

     private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            destroySelf();
        }
    }
}
