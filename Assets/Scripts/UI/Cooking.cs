using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    ITEM_INDEX[] ingList_0 = {
        ITEM_INDEX.GAMJA,
        ITEM_INDEX.GOSU,
        ITEM_INDEX.BAZIRAK,
        ITEM_INDEX.GOBONGBAP,
    };
    ITEM_INDEX[] ingList_1 = {
        ITEM_INDEX.GOLDEN_POO,
        ITEM_INDEX.JANDI,
        ITEM_INDEX.GOOL,
        ITEM_INDEX.GOOL,
    };
    [SerializeField] ItemData[] resultCooks;
    //     ITEM_INDEX.GAMJATOK,
    //     ITEM_INDEX.SSALGUKSU,
    //     ITEM_INDEX.BAZIRAK_REHOI,

    [SerializeField] UnityEngine.UI.Image resultImg;
    [SerializeField] GameObject resultUI;

    public void cookRecipe(int recipeIdx)
    {
        try {
            int ing_0 = Character.haveItem[2].FindIndex(name => name.itemData.itemIndex == ingList_0[recipeIdx]);
            int ing_1 = Character.haveItem[2].FindIndex(name => name.itemData.itemIndex == ingList_1[recipeIdx]);

            // 아이템 개수가 안맞으면 이 함수에 들어올수 없지만 혹시 모르니
            if (ing_0 >= 0 && ing_1 >= 0)
            {
                // 재료 사용
                if (Character.haveItem[2][ing_0].itemCount >= 1 && Character.haveItem[2][ing_1].itemCount >= 1) {
                    Character.haveItem[2][ing_0].itemCount--;
                    Character.haveItem[2][ing_1].itemCount--;
                } else {
                    GameMng.I.showNotice("재료가 부족합니다!");
                    return;
                }
                
                // 재료가 0개가 되었다면 삭제
                if (Character.haveItem[2][ing_0].itemCount == 0)
                    Character.haveItem[2].RemoveAt(ing_0);
                if (Character.haveItem[2][ing_1].itemCount == 0)
                    Character.haveItem[2].RemoveAt(ing_1);
                
                Item item = new Item(resultCooks[recipeIdx], 1);

                // 인벤토리에 아이템 넣기
                CharacterCollider.itemSetting(2, item);
                // 아이템 획득 UI 표시                
                CharacterCollider.getItemEXP(item, 0);

                resultImg.sprite = item.itemData.itemSp;
                resultUI.SetActive(true);
            }
            else
            {
                GameMng.I.showNotice("재료가 부족합니다!");
            }


        } catch(System.IndexOutOfRangeException e) {
        }
    }

    public void exitWindow()
    {
        this.gameObject.SetActive(false);
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
    }
}
