using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    // 아이템 여러개 소환할때 왠만해서는 같은 아이템들끼리 안붙게 Instantiate 할때 위치를 조금씩 다르게 하기
    
    public Item saveItem;
    [SerializeField] SpriteRenderer itemSprRender;
    [SerializeField] Rigidbody _rigidbody;
    bool activeReady = false;

    void Start()
    {
        itemSprRender.sprite = saveItem.itemData.itemSp;
        if (saveItem.itemData.itemType.Equals(ITEM_TYPE.FAVORITE_ITEM))
            transform.localScale = Vector3.one;
        else if (saveItem.itemData.itemType.Equals(ITEM_TYPE.SHIRTS_ITEM))
            transform.localScale = new Vector3(0.7f, 0.7f, 1);
        else if (saveItem.itemData.itemType.Equals(ITEM_TYPE.PANTS_ITEM))
            transform.localScale = new Vector3(0.9f, 0.9f, 1);
        else if (saveItem.itemData.itemType.Equals(ITEM_TYPE.UNUSEFUL_ITEM))
            transform.localScale = new Vector3(2, 2, 1);
        _rigidbody.AddForce(new Vector3(Random.Range(-1.5f, 1.5f), 10, Random.Range(-1.5f, 1.5f)), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Floor") && !activeReady)
        {
            activeReady = true;
            StartCoroutine(activeOn());
        }
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Floor") && !activeReady)
        {
            activeReady = true;
            StartCoroutine(activeOn());
        }
    }

    IEnumerator activeOn()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.tag = "Item";
    }
}