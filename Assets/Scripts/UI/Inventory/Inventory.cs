using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Slot> slotPool;
    [SerializeField] private GameObject slotPrefab;

    public GameObject[] inventoryBase = new GameObject[4];
    public GameObject[] equipBT = new GameObject[4];
    public Image[] inventoryBtnImg = new Image[4];
    [SerializeField] Image contentImg;
    [SerializeField] TMPro.TextMeshProUGUI contentText;

    [SerializeField] Sprite selectSp;
    [SerializeField] Sprite unselectSp;

    int itemtype = 0;
    public ITEM_INDEX getClickIndex = 0;

    void Awake()
    {
        for (int i = 0; i < 40; i++)
        {
            GameObject temp = Instantiate(slotPrefab, this.transform);

            slotPool.Add(temp.GetComponent<Slot>());
            temp.SetActive(false);
        }
    }

    void OnEnable()
    {
        createSlot(0);
        getSlots(0);
        if(Character.haveItem[0].Count == 0)
        {
            contentOnOff(false);
            for (int i = 0; i < equipBT.Length; i++)
                equipBT[i].SetActive(false);
        }
        else
        {
            getClickIndex = slotPool[0].itemData.itemIndex;
            contentOnOff(true);
            contentImg.sprite = getActiveObject().itemData.itemSp;
            contentText.text = getActiveObject().itemData.itemName + "\n" + getActiveObject().itemData.content;
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
    }

    public void btnKind(int kind)
    {
        deleteSlot(itemtype);
        getClickIndex = 0;
        itemtype = kind;
        createSlot(itemtype);
        getSlots(kind);
        if (Character.haveItem[kind].Count == 0)
        {
            Debug.Log("NO ITEM");
            contentOnOff(false);
        }
        else
        {
            Debug.Log(Character.haveItem[kind].Count);
            contentOnOff(true);
            contentSet(getActiveObject().itemData);
            getClickIndex = getActiveObject().itemData.itemIndex;
        }
    }

    public void slotBtn(int kind)                   // 배틀아이템 슬롯 장착 버튼 선택한 아이템이나 바꾸려는 슬롯에 있는 아이템이 쿨타임 중이면 교환 불가능
    {
        int idx = -1;
        for (int i = 0; i < Character.equipBattleItem.Length; i++)
        {
            if (Character.equipBattleItem[i] == null)
                continue;
            if (Character.equipBattleItem[i].itemData.itemIndex == getClickIndex)
            {
                idx = i;
                break;
            }
        }
        if (idx == -1)
            idx = kind - 1;

        if (!GameMng.I.character.usingBattleItem[kind - 1] && !GameMng.I.character.usingBattleItem[idx])
        {
            int index = -1;
            for (int i = 0; i < GameMng.I.BattleItemUI.ItemIdx.Length; i++)
            {
                if (GameMng.I.BattleItemUI.ItemIdx[i] == getClickIndex)
                    index = i;
            }
            for (int i = 0; i < Character.haveItem[0].Count; i++)
            {
                if (Character.haveItem[0][i].itemData.itemIndex == getClickIndex)
                {
                    Character.equipBattleItem[kind - 1] = Character.haveItem[0][i];
                    GameMng.I.BattleItemUI.ItemImg[kind - 1].gameObject.SetActive(true);
                    GameMng.I.BattleItemUI.ItemText[kind - 1].gameObject.SetActive(true);
                    GameMng.I.BattleItemUI.ItemImg[kind - 1].sprite = Character.haveItem[0][i].itemData.itemSp;
                    GameMng.I.BattleItemUI.ItemText[kind - 1].text = Character.haveItem[0][i].itemCount.ToString();
                    GameMng.I.BattleItemUI.ItemIdx[kind - 1] = Character.haveItem[0][i].itemData.itemIndex;
                    break;
                }
            }
            if (index == -1 || index == kind - 1)
                return;
            else
            {
                GameMng.I.BattleItemUI.ItemImg[index].gameObject.SetActive(false);
                GameMng.I.BattleItemUI.ItemText[index].gameObject.SetActive(false);
                GameMng.I.BattleItemUI.ItemIdx[index] = ITEM_INDEX.NONE;
                Character.equipBattleItem[index] = null;
            }
        }
    }

    public void equipBtn()
    {
        Debug.Log("아이템 장착");
    }

    public void useConsumableItemBtn()
    {
        Debug.Log("사용 아이템 사용");
    }

    public void createSlot(int kind)
    {
        for (int i = 0; i < Character.haveItem[kind].Count; i++)
        {
            Slot slotTemp = CheckSlotPool();
            slotTemp.gameObject.SetActive(true);
            slotTemp.itemData = Character.haveItem[kind][i].itemData;
            slotTemp.itemImg.sprite = slotTemp.itemData.itemSp;
            slotTemp.transform.parent = inventoryBase[kind].transform;
            slotTemp.transform.SetAsLastSibling();
            slotTemp.inventorySc = this;
            if (slotTemp.itemData.itemType != ITEM_TYPE.WEAPON_ITEM && slotTemp.itemData.itemType != ITEM_TYPE.SHIRTS_ITEM && slotTemp.itemData.itemType != ITEM_TYPE.PANTS_ITEM)
            {
                slotTemp.text_Count.gameObject.SetActive(true);
                slotTemp.text_Count.text = "x" + Character.haveItem[kind][i].itemCount.ToString();
            }
            else
                slotTemp.text_Count.gameObject.SetActive(false);
        }
        if (Character.haveItem[kind].Count > 0)
            equipBT[kind].SetActive(true);
    }

    Slot CheckSlotPool()
    {
        for (int i = 0; i < slotPool.Count; i++)
        {
            if (!slotPool[i].gameObject.activeSelf)
                return slotPool[i];
        }
        return null;
    }

    Slot getActiveObject()
    {
        for (int i = 0; i < slotPool.Count; i++)
        {
            if (slotPool[i].gameObject.activeSelf)
                return slotPool[i];
        }
        return null;
    }

    public void deleteSlot(int kind)        // 아이템 종류(베틀아이템, 소비아이템 등)
    {
        for (int i = 0; i < inventoryBase[kind].transform.childCount; i++)
            inventoryBase[kind].transform.GetChild(i).gameObject.SetActive(false);
        equipBT[kind].SetActive(false);
    }

    public void contentOnOff(bool onoff)
    {
        contentImg.gameObject.SetActive(onoff);
        contentText.gameObject.SetActive(onoff);
    }

    public void contentSet()
    {
        for (int i = 0; i < Character.haveItem[itemtype].Count; i++)
        {
            if (Character.haveItem[itemtype][i].itemData.itemIndex == getClickIndex)
            {
                contentImg.sprite = Character.haveItem[itemtype][i].itemData.itemSp;
                contentText.text = Character.haveItem[itemtype][i].itemData.itemName + "\n" + Character.haveItem[itemtype][i].itemData.content;
                break;
            }
        }
    }

    public void contentSet(ItemData item)
    {
        contentImg.sprite = item.itemSp;
        contentText.text = item.itemName + "\n" + item.content;
    }
}
