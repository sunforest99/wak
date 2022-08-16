using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    int poolCount = 0;
    void itemSetting(int n, Item _item)
    {
        if (n == 1)
            Character.haveItem[n].Add(_item);
        else
        {
            int index = Character.haveItem[n].FindIndex(name => name.itemData.itemName == _item.itemData.itemName);
            if (index == -1)
                Character.haveItem[n].Add(_item);
            else
            {
                Character.haveItem[n][index].itemCount += _item.itemCount;
                if (_item.itemData.itemType == ITEM_TYPE.BATTLE_ITEM)
                {
                    for (int i = 0; i < Character.equipBattleItem.Length; i++)
                    {
                        if (Character.equipBattleItem[i] == null)
                            continue;
                        if (Character.equipBattleItem[i].itemData.itemIndex == Character.haveItem[n][index].itemData.itemIndex)
                        {
                            GameMng.I.BattleItemUI.ItemText[i].text = Character.equipBattleItem[i].itemCount.ToString();
                            break;
                        }
                    }
                }
            }
        }
    }

    // void getItemEXP(Item item, int idx)
    // {
    //     GameMng.I.getItemPool[idx].EXP_Game.SetActive(true);
    //     GameMng.I.getItemPool[idx].EXP_Game.transform.SetAsLastSibling();
    //     GameMng.I.getItemPool[idx].EXP_Img.sprite = item.itemData.itemSp;
    //     if (item.itemData.itemType != ITEM_TYPE.WEAPON_ITEM && item.itemData.itemType != ITEM_TYPE.HEAD_ITEM)
    //         GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName + " x" + item.itemCount.ToString();
    //     else
    //         GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName;
    // }
    void getItemEXP(Item item, int idx)
    {
        GameMng.I.getItemPool[idx].EXP_Img.sprite = item.itemData.itemSp;
        if (item.itemData.itemType != ITEM_TYPE.WEAPON_ITEM && item.itemData.itemType != ITEM_TYPE.HEAD_ITEM)
            GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName + " x" + item.itemCount.ToString();
        else
            GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName;

        for (int i = idx; i >= 0; i--)
            GameMng.I.getItemPool[i].EXP_Game.transform.localPosition = new Vector3(5.0f, 45.0f * (idx - i), 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Weapon"))
        {
            GameMng.I.stateMng.user_HP_Numerical.Hp -= GameMng.I.bossData.getDamage();
        }

        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<ItemObj>().saveItem;
            switch (item.itemData.itemType)
            {
                case ITEM_TYPE.BATTLE_ITEM:
                    itemSetting(0, item);
                    break;
                case ITEM_TYPE.HEAD_ITEM:
                case ITEM_TYPE.WEAPON_ITEM:
                    itemSetting(1, item);
                    break;
                case ITEM_TYPE.CONSUMABLE_ITEM:
                    itemSetting(2, item);
                    break;
                case ITEM_TYPE.UNUSEFUL_ITEM:
                    itemSetting(3, item);
                    break;
            }
            getItemEXP(item, poolCount++);
            if (poolCount == 5)
                poolCount = 0;
            Destroy(other.gameObject);
        }
    }
}
