using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
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
                Character.haveItem[n][index].itemCount += _item.itemCount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Weapon"))
        {
            Debug.Log("기본 패턴 데미지 " + GameMng.I.bossData.getDamage());
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
            Destroy(other.gameObject);
        }
    }
}
