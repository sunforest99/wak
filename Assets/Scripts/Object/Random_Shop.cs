using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Shop : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button[] randItemBt;
    [SerializeField] UnityEngine.UI.Image[] randItemImg;
    [SerializeField] TMPro.TextMeshProUGUI[] randItemTxt;
    [SerializeField] TMPro.TextMeshProUGUI curCoinTxt;
    [SerializeField] Transform itemSpawnPos;

    private void OnEnable() {

        curCoinTxt.text = "보유 코인 : " + GameMng.I.userData.coin;

        List<int> rList = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int rand;
            int per = Random.Range(0, 100);

            do
            {
                // rand = Random.Range((int)ITEM_INDEX._FAVORITE_ITEM_INDEX_ + 1, (int)ITEM_INDEX._FAVORITE_ITEM_INDEX_END_);
                if (per < 20)           // 20% 확률로 호감도 아이템
                    rand = Random.Range((int)ITEM_INDEX._FAVORITE_ITEM_INDEX_ + 1, (int)ITEM_INDEX._FAVORITE_ITEM_INDEX_END_);
                else if (per < 50)      // 30% 확률로 장비 아이템
                    rand = Random.Range((int)ITEM_INDEX._EQUIP_ITEM_INDEX + 1, (int)ITEM_INDEX._EQUIP_ITEM_INDEX_END_);
                else                    // 50% 확률로 무기 아이템
                    rand = Random.Range((int)ITEM_INDEX._WEAPON_ITEM_INDEX_ + 1, (int)ITEM_INDEX._WEAPON_ITEM_INDEX_END_);
                // Debug.Log("@" + rand);
            } while (rList.Contains(rand));
            
            rList.Add(rand);

            ItemData item = Resources.Load<ItemData>($"ItemData/{((ITEM_INDEX)rand).ToString()}");
            
            // 내가 가진 코인이 비용보다 많아야 활성화
            if (GameMng.I.userData.coin > item.itemCost) {
                randItemBt[i].interactable = true;
            }
            randItemImg[i].sprite = item.itemSp;
            randItemBt[i].name = item.itemCost + "";
            randItemTxt[i].text = item.itemCost + " 코인";
            randItemBt[i].onClick.RemoveAllListeners();
            randItemBt[i].onClick.AddListener(() => {
                // 코인 지불
                GameMng.I.userData.coin -= item.itemCost;
                curCoinTxt.text = "보유 코인 : " + GameMng.I.userData.coin;

                // 아이템 획득
                GameObject obj = Instantiate(
                    GameMng.I.itemObj,
                    itemSpawnPos.position - new Vector3(3, 0, 3),
                    Quaternion.Euler(20, 0, 0)
                );
                obj.GetComponent<ItemObj>().saveItem = new Item(item, 1);
                obj.SetActive(true);

                // 버튼 disabled
                randItemBt[i].interactable = false;

                // 내가 가진 코인이 변경되었기 때문에 아이템들 구매할 수 있는지 재확인
                for (int j = 0; j < 3; j++)
                {
                    if (GameMng.I.userData.coin < int.Parse(randItemBt[i].name))
                    {
                        randItemBt[i].interactable = false;
                    }
                }
            });
        }
    }

    public void exitShop()
    {
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
    }
}
