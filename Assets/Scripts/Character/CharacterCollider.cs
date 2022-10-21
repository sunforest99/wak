using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    int poolCount = 0;

    /*
     * @brief 아이템 획득시 인벤토리 새로고침
     * @param n 아이템 타입 (0:배틀아이템)(1:장비)(2:호감도)(3:사용아이템)
     * @param _item 아이템 정보
     */
    void itemSetting(int n, Item _item)
    {
        if (n == 0)
        {
            Character.haveItem[n].Add(_item);
            GameMng.I.userData.inventory[n].Add(new Item_Schema((int)_item.itemData.itemIndex, _item.itemCount));
        }
        else
        {
            int index = Character.haveItem[n].FindIndex(name => name.itemData.itemName == _item.itemData.itemName);
            if (index == -1)
            {
                Character.haveItem[n].Add(_item);
                GameMng.I.userData.inventory[n].Add(new Item_Schema((int)_item.itemData.itemIndex, _item.itemCount));
            }
            else
            {
                Character.haveItem[n][index].itemCount += _item.itemCount;
                GameMng.I.userData.inventory[n][index].mount += _item.itemCount;
                // if (_item.itemData.itemType == ITEM_TYPE.BATTLE_ITEM)
                // {
                //     for (int i = 0; i < Character.equipBattleItem.Length; i++)
                //     {
                //         if (Character.equipBattleItem[i] == null)
                //             continue;
                //         if (Character.equipBattleItem[i].itemData.itemIndex == Character.haveItem[n][index].itemData.itemIndex)
                //         {
                //             GameMng.I.BattleItemUI.ItemText[i].text = Character.equipBattleItem[i].itemCount.ToString();
                //             break;
                //         }
                //     }
                // }
            }
        }
    }

    void getItemEXP(Item item, int idx)
    {
        GameMng.I.getItemPool[idx].EXP_Img.sprite = item.itemData.itemSp;
        if (item.itemData.itemType != ITEM_TYPE.WEAPON_ITEM && item.itemData.itemType != ITEM_TYPE.SHIRTS_ITEM && item.itemData.itemType != ITEM_TYPE.PANTS_ITEM)
            GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName + " x" + item.itemCount.ToString();
        else
            GameMng.I.getItemPool[idx].EXP_Text.text = item.itemData.itemName;

        GameMng.I.getItemPool[idx].EXP_Game.SetActive(false);
        GameMng.I.getItemPool[idx].EXP_Game.SetActive(true);
        // for (int i = idx; i >= 0; i--) {
        //     GameMng.I.getItemPool[i].EXP_Game.SetActive(false);
        //     GameMng.I.getItemPool[i].EXP_Game.SetActive(true);
        // }
            // GameMng.I.getItemPool[i].EXP_Game.transform.localPosition = new Vector3(5.0f, 45.0f * (idx - i), 0.0f);
    }

    void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Boss_Weapon"))
        {
            GameMng.I.stateMng.takeDamage(GameMng.I.boss.bossData.getDamage());
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<ItemObj>().saveItem;
            switch (item.itemData.itemType)
            {
                // 배틀 아이템은 이제 획득 아이템이 아니게 되었음
                // case ITEM_TYPE.BATTLE_ITEM:
                //     itemSetting(0, item);
                //     break;
                case ITEM_TYPE.WEAPON_ITEM:
                case ITEM_TYPE.SHIRTS_ITEM:
                case ITEM_TYPE.PANTS_ITEM:
                    itemSetting(0, item);
                    break;
                case ITEM_TYPE.FAVORITE_ITEM:
                    itemSetting(1, item);
                    break;
                case ITEM_TYPE.CONSUMABLE_ITEM:
                    itemSetting(2, item);
                    break;
                default:
                    if (item.itemData.itemIndex.Equals(ITEM_INDEX.NONE))
                        GameMng.I.userData.coin += item.itemCount;
                    break;
            }
            
            if (!poolCount.Equals(0))
                StopCoroutine(itemPoolTimer());
            StartCoroutine(itemPoolTimer());
            
            Instantiate(GameMng.I.itemGetEff, transform.position, Quaternion.identity);

            getItemEXP(item, poolCount++);
            
            if (poolCount == 5)
                poolCount = 0;
            Destroy(other.gameObject);
        }
    }

    IEnumerator itemPoolTimer()
    {
        yield return new WaitForSeconds(4);
        poolCount = 0;
    }
}
