using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory inventorySc;
    private Button button;
    public ItemData itemData;
    public int itemCount;
    public Image itemImg;

    public TMPro.TextMeshProUGUI text_Count;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(clickSlot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventorySc.contentSet(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventorySc.contentSet();
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            Destroy(gameObject);
    }
    public void clickSlot()
    {
        inventorySc.getClickIndex = itemData;
    }
}
