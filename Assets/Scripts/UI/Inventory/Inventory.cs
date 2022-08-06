using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventoryBase = new GameObject[4];
    public Image[] inventoryBtnImg = new Image[4];
    [SerializeField] Image contentImg;
    [SerializeField] TMPro.TextMeshProUGUI contentText;

    [SerializeField] Sprite selectSp;
    [SerializeField] Sprite unselectSp;

    [SerializeField] Slot[] slots;

    int itemtype = 0;
    public int getClickIndex = 0;

    void OnEnable()
    {
        getSlots(0);
        contentImg.sprite = slots[0].itemData.itemSp;
        contentText.text = slots[0].itemData.itemName + "\n" + slots[0].itemData.content;
    }

    void getSlots(int kind)
    {
        for (int i = 0; i < inventoryBase.Length; i++)
        {
            inventoryBtnImg[i].sprite = unselectSp;
            inventoryBase[i].SetActive(false);
        }
        inventoryBtnImg[kind].sprite = selectSp;
        inventoryBase[kind].SetActive(true);
        slots = inventoryBase[kind].GetComponentsInChildren<Slot>();
    }

    public void btnKind(int kind)
    {
        getClickIndex = 0;
        itemtype = kind;
        slots = null;
        getSlots(kind);
    }

    public void AcquireItem(ItemData item, int count = 1)                                       // 같은 종류 아이템 합쳐주는 함수
    {
        if (item.itemType != ITEM_TYPE.WEAPON_ITEM && item.itemType != ITEM_TYPE.HEAD_ITEM)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemData != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 나서
                {
                    if (slots[i].itemData.itemName == item.itemName)
                    {
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
    }

    public void contentSet()
    {
        contentImg.sprite = slots[getClickIndex].itemData.itemSp;
        contentText.text = slots[getClickIndex].itemData.itemName + "\n" + slots[getClickIndex].itemData.content;
    }

    public void contentSet(ItemData item)
    {
        contentImg.sprite = item.itemSp;
        contentText.text = item.itemName + "\n" + item.content;
    }
}
