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
        inventorySc.getClickIndex = itemData.itemIndex;
        // switch (itemData.itemType)
        // {
        //     case ITEM_TYPE.BATTLE_ITEM:                                                                         // 배틀 아이템
        //         inventorySc.getClickIndex = (int)itemData.itemIndex;
        //         break;
        //     case ITEM_TYPE.HEAD_ITEM:                                                                           // 장비 아이템
        //     case ITEM_TYPE.WEAPON_ITEM:
        //         inventorySc.getClickIndex = (int)itemData.itemIndex - ((int)ITEM_INDEX.WEAPON_ITEM_INDEX + 1);
        //         break;
        //     case ITEM_TYPE.CONSUMABLE_ITEM:                                                                     // 사용 아이템
        //         inventorySc.getClickIndex = (int)itemData.itemIndex - ((int)ITEM_INDEX.CONSUMABLE_ITEM_INDEX + 1);
        //         break;
        //         // case ITEM_TYPE.UNUSEFUL_ITEM:                                                                       // 잡템
        //         //     inventorySc.getClickIndex = (int)itemData.itemIndex;
        //         //     break;
        // }
    }
}
