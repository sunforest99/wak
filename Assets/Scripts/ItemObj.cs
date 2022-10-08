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
        _rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 5, Random.Range(-1f, 1f)), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Floor") && !activeReady)
        {
            StartCoroutine(activeOn());
        }
    }

    IEnumerator activeOn()
    {
        activeReady = true;
        // this.gameObject.tag = "Item";
        yield return new WaitForSeconds(0.5f);
        this.gameObject.tag = "Item";
    }
}