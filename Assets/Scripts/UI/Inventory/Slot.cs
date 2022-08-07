using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Inventory inventorySc;
    public ItemData itemData;
    public int itemCount;
    public Image itemImg;

    public TMPro.TextMeshProUGUI text_Count;

    public void HoverOn()
    {
        inventorySc.contentSet(itemData);
    }

    public void HoverOff()
    {
        inventorySc.contentSet();
    }

    public void AddItem(ItemData item, int count = 1)
    {
        itemData = item;
        itemCount = count;
        itemImg.sprite = item.itemSp;
        text_Count.gameObject.SetActive(true);

        if (itemData.itemType != ITEM_TYPE.WEAPON_ITEM && itemData.itemType != ITEM_TYPE.HEAD_ITEM)
            text_Count.text = itemCount.ToString();
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
