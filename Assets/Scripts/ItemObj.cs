using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public Item saveItem;
    [SerializeField] SpriteRenderer itemSprRender;

    void Start()
    {
        Spawn();
    }

    IEnumerator bouncingSpawn()
    {
        float tick = 0;
        float startPosY = transform.position.y + Random.Range(-0.6f, 0.6f);
        float moveForceX = Random.Range(1f, 1.8f);

        while (tick < 0.1f) {
            transform.position += new Vector3(moveForceX, 1f, 0) * Time.deltaTime;
            tick += Time.deltaTime;
            yield return null;
        }
        while (startPosY < transform.position.y) {
            transform.position += new Vector3(moveForceX, -1.8f, 0) * Time.deltaTime;
            yield return null;
        }
    }

    public void Spawn()
    {
        itemSprRender.sprite = saveItem.itemData.itemSp;
        StartCoroutine(bouncingSpawn());
    }
}