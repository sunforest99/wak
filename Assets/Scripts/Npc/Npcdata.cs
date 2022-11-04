using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;

public class Npcdata : MonoBehaviour
{
    [HideInInspector] public bool isDialog;
    protected string npcname;
    [SerializeField] TMPro.TextMeshPro speechTxt;
    [SerializeField] GameObject speechBG;
    public Animator _anim;
    // public GameObject tempDialog;     // <! 이름 바꾸기
    
    // public ITEM_INDEX favoriteItem;         // 호감도 아이템
    public ItemData favoriteItem;

    public IEnumerator dialogs = null;

    [SerializeField]
    protected SpriteRenderer questIcon;

    // void Update()
    // {
    //     if (isDialog)
    //     {
    //         if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
    //                 && !GameMng.I.npcUI.dialogUI.selectBlock.activeSelf)
    //         {
    //             NextDialog();
    //         }
    //     }
    // }

    public bool NextDialog()
    {
        if (dialogs == null)
            return false;
        
        if (dialogs.MoveNext() == true)
        {
            // 빈 메세지라면 그냥 넘기기 (선택 메세지 전 틈줄때 의미있음)
            if (dialogs.Current.ToString() == "")
                return true;
            
            // GameMng.I.npcUI.dialogUI.setNpcName = npcname;
            
            GameMng.I.npcUI.dialogUI.setNpcName = npcname;
            GameMng.I.npcUI.dialogUI.setNpcText = dialogs.Current.ToString();

            // GameMng.I.dailogUI.setPlayerName = "Player";        // <! 나중에 닉네임 넣기
            if (dialogs.Current.ToString().FirstOrDefault() == '$')
            {
                GameMng.I.npcUI.dialogUI.setPlayerText = dialogs.Current.ToString().Remove(0, 1);
                // GameMng.I.dailogUI.ui[0].SetAsFirstSibling();
                // GameMng.I.dailogUI.ui[1].SetAsLastSibling();
            }
            else
            {
                GameMng.I.npcUI.dialogUI.setNpcText = dialogs.Current.ToString();
                // GameMng.I.dailogUI.ui[0].SetAsLastSibling();
                // GameMng.I.dailogUI.ui[1].SetAsFirstSibling();
            }
        }
        else
        {
            ExitDialog();
        }
        
        return true;
    }

    /**
     * @brief 퀘스트 체크
     */
    public virtual void checkQuest()
    {
    }

    /**
     * @brief 어떤 행동을 명령할때 사용
     */
    public virtual void actSomething()
    {
    }

    public virtual void ExitDialog()
    {
        // checkQuest();
        GameMng.I.npcData = null;
        GameMng.I.npcUI.isDialog = false;
        GameMng.I.npcUI.dialogUI.gameObject.SetActive(false);
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;

        // UI 레이어 다시 ON
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("UI_Base");

        MCamera.I.setTargetChange(GameMng.I.character.transform);
        MCamera.I.zoomOut2();
    }

    protected virtual IEnumerator NpcDialog()
    {
        yield return null;
    }
    
    protected void setQuestIcon()
    {
        questIcon.gameObject.SetActive(false);
    }
    protected void setQuestIcon(QUEST_TYPE qType)
    {
        questIcon.sprite = GameMng.I.questTypeSpr[qType == QUEST_TYPE.MAIN ? 0 : 1];
        questIcon.gameObject.SetActive(true);
    }

    /*
     * @brief NPC 고유 대사(독백) 말하기
     */
    protected IEnumerator checkPlayerDistance()
    {
        yield return new WaitForSeconds(5);

        if (Vector2.Distance(GameMng.I.character.transform.parent.position, transform.position) < 10)
        {
            speechTxt.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            speechTxt.gameObject.SetActive(false);
        }

        StartCoroutine(checkPlayerDistance());
    }

    protected bool getFlow()
    {
        MCamera.I.setTargetChange(this.transform);
        return GameMng.I.npcUI.dialogUI.flow;
    }

    protected void setSpeech(string msg)
    {
        speechTxt.text = msg;

        float x = speechTxt.preferredWidth;
        x = (x > 3) ? 3.5f : x + 0.5f;
        speechBG.transform.localScale = new Vector2(x, speechTxt.preferredHeight + 0.5f);
    }
}
