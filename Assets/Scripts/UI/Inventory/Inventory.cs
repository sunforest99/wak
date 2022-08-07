using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventoryBase = new GameObject[4];
    public GameObject[] equipBT = new GameObject[4];
    public Image[] inventoryBtnImg = new Image[4];
    [SerializeField] Image contentImg;
    [SerializeField] TMPro.TextMeshProUGUI contentText;

    [SerializeField] Sprite selectSp;
    [SerializeField] Sprite unselectSp;

    [SerializeField] Slot[] slots;

    int itemtype = 0;
    public ITEM_INDEX getClickIndex = 0;

    void OnEnable()
    {
        createSlot(0);
        getSlots(0);
        if (inventoryBase[0].transform.childCount == 0)
        {
            contentOnOff(false);
            for (int i = 0; i < equipBT.Length; i++)
                equipBT[i].SetActive(false);
        }
        else
        {
            contentOnOff(true);
            contentImg.sprite = slots[0].itemData.itemSp;
            contentText.text = slots[0].itemData.itemName + "\n" + slots[0].itemData.content;
            equipBT[0].SetActive(true);
        }
    }
    void OnDisable()
    {
        deleteSlot(itemtype);
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
        deleteSlot(itemtype);
        getClickIndex = 0;
        itemtype = kind;
        createSlot(itemtype);
        slots = null;
        getSlots(kind);
        if (GameMng.I.character.haveItem[kind].Count == 0)
            contentOnOff(false);
        else
        {
            contentOnOff(true);
            contentSet(slots[0].itemData);
        }
    }

    public void slotBtn(int kind)
    {
        Debug.Log(kind + "번 슬롯 장착");
    }

    public void equipBtn()
    {
        Debug.Log("아이템 장착");
    }

    public void createSlot(int kind)
    {
        for (int i = 0; i < GameMng.I.character.haveItem[kind].Count; i++)
        {
            GameObject temp = Instantiate(GameMng.I.character.haveItem[kind][i].itemData.itemSlotPre, inventoryBase[kind].transform);
            Slot slotTemp = temp.GetComponent<Slot>();
            slotTemp.inventorySc = this;
            slotTemp.text_Count.text = "x" + GameMng.I.character.haveItem[kind][i].itemCount.ToString();
        }
        if (GameMng.I.character.haveItem[kind].Count > 0)
            equipBT[kind].SetActive(true);
    }

    public void deleteSlot(int kind)
    {
        for (int i = 0; i < inventoryBase[kind].transform.childCount; i++)
            Destroy(inventoryBase[kind].transform.GetChild(i).gameObject);
        equipBT[kind].SetActive(false);
    }

    // public void AcquireItem(ItemData item, int count = 1)                                       // 같은 종류 아이템 합쳐주는 함수
    // {
    //     if (item.itemType != ITEM_TYPE.WEAPON_ITEM && item.itemType != ITEM_TYPE.HEAD_ITEM)
    //     {
    //         for (int i = 0; i < slots.Length; i++)
    //         {
    //             if (slots[i].itemData != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 나서
    //             {
    //                 if (slots[i].itemData.itemName == item.itemName)
    //                 {
    //                     slots[i].SetSlotCount(count);
    //                     return;
    //                 }
    //             }
    //         }
    //     }

    //     for (int i = 0; i < slots.Length; i++)
    //     {
    //         if (slots[i].itemData == null)
    //         {
    //             slots[i].AddItem(item, count);
    //             return;
    //         }
    //     }
    // }

    public void contentOnOff(bool onoff)
    {
        contentImg.gameObject.SetActive(onoff);
        contentText.gameObject.SetActive(onoff);
    }

    public void contentSet()
    {
        // contentImg.sprite = slots[getClickIndex].itemData.itemSp;
        // contentText.text = slots[getClickIndex].itemData.itemName + "\n" + slots[getClickIndex].itemData.content;
        for (int i = 0; i < GameMng.I.character.haveItem[itemtype].Count; i++)
        {
            if (GameMng.I.character.haveItem[itemtype][i].itemData.itemIndex == getClickIndex)
            {
                contentImg.sprite = GameMng.I.character.haveItem[itemtype][i].itemData.itemSp;
                contentText.text = GameMng.I.character.haveItem[itemtype][i].itemData.itemName + "\n" + GameMng.I.character.haveItem[itemtype][i].itemData.content;
            }
        }
    }

    public void contentSet(ItemData item)
    {
        contentImg.sprite = item.itemSp;
        contentText.text = item.itemName + "\n" + item.content;
    }
}
