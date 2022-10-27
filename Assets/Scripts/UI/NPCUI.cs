using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCUI : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    // public GameObject _startLoad;

    public UnityEngine.UI.Image npcFavoriteItemImg;

    public DialogUI dialogUI;
    public GameObject npcSelectUI;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _canvas.worldCamera = Camera.main;
    }

    public void showDialog()
    {
        npcSelectUI.SetActive(false);
        dialogUI.gameObject.SetActive(true);
        GameMng.I.npcData.NextDialog();
    }

    public void amIHaveFavoriteItem()
    {
        // TODO : haveItem 의 idx가 ITEM_INDEX 순서와 맞게
        int idx = Character.haveItem[2].FindIndex(name => name.itemData.itemName == GameMng.I.npcData.favoriteItem.itemIndex.ToString());
        
        // 못찾으면
        if (idx < 0)
        {
            // 아이템 주기 비활성화
        }
        else
        {
            // 아이템 주기 활성화
        }
    }

    public void giveFavoriteItem()
    {
        // TODO : haveItem 의 idx가 ITEM_INDEX 순서와 맞게
        int idx = Character.haveItem[2].FindIndex(name => name.itemData.itemName == GameMng.I.npcData.favoriteItem.itemIndex.ToString());

        // 아이템 개수가 안맞으면 이 함수에 들어올수 없지만 혹시 모르니
        if (idx >= 0)
        {
            GameMng.I.userData.love += Character.haveItem[2][idx].itemCount;
            
            Character.haveItem[2].RemoveAt(idx);
            GameMng.I.userData.inventory[2].RemoveAt(idx);

        }
    }

}
