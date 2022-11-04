using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCUI : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    // public GameObject _startLoad;

    public GameObject dialogBT;
    public UnityEngine.UI.Image npcFavoriteItemImg;
    public UnityEngine.UI.Button favoriteBT;

    public DialogUI dialogUI;
    public GameObject npcSelectUI;
    
    public bool isDialog = false;

    

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (isDialog)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    && !GameMng.I.npcUI.dialogUI.selectBlock.activeSelf
                    && GameMng.I.npcData)
            {
                GameMng.I.npcData.NextDialog();
            }
        }
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
        int idx = Character.haveItem[1].FindIndex(name => name.itemData.itemIndex == GameMng.I.npcData.favoriteItem.itemIndex);
        
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
        int idx = Character.haveItem[1].FindIndex(name => name.itemData.itemIndex == GameMng.I.npcData.favoriteItem.itemIndex);

        // 아이템 개수가 안맞으면 이 함수에 들어올수 없지만 혹시 모르니
        if (idx >= 0)
        {
            GameMng.I.userData.love += Character.haveItem[1][idx].itemCount;
            
            Character.haveItem[1].RemoveAt(idx);
            // GameMng.I.userData.inventory[2].RemoveAt(idx);
        }
    }

    public void activeSelectUI(bool dg = true, bool fv = true)
    {
        if (GameMng.I.npcData.dialogs == null) {
            dg = false;
        }

        dialogBT.SetActive(dg);
        npcFavoriteItemImg.transform.parent.gameObject.SetActive(fv);

        if (fv) {
            try {
                int idx = Character.haveItem[1].FindIndex(name => name.itemData.itemIndex == GameMng.I.npcData.favoriteItem.itemIndex);

                // 아이템 개수가 안맞으면 비활성화
                if (idx < 0) {
                    favoriteBT.interactable = false;
                } else {
                    favoriteBT.interactable = true;
                }
            } catch(System.IndexOutOfRangeException e) {
            }
        }

        npcSelectUI.SetActive(true);
    }

}
