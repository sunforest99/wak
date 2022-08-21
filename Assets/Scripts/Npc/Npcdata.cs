using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;

public class Npcdata : MonoBehaviour
{
    [HideInInspector] public bool isDialog;
    [SerializeField] protected string npcname;
    [SerializeField] TMPro.TextMeshPro speechTxt;
    [SerializeField] GameObject speechBox;
    // public GameObject tempDialog;     // <! 이름 바꾸기

    protected IEnumerator dialogs;

    [SerializeField]
    protected SpriteRenderer questIcon;

    void Update()
    {
        if (isDialog)
        {
            if (Input.GetMouseButtonDown(1) && !GameMng.I.dailogUI.selectBlock.activeSelf)
            {
                NextDialog();
            }
        }
    }

    public bool NextDialog()
    {
        if (dialogs == null)
            return false;
        
        if (dialogs.MoveNext() == true)
        {
            GameMng.I.dailogUI.setNpcText = npcname;
            // GameMng.I.dailogUI.setPlayerName = "Player";        // <! 나중에 닉네임 넣기
            if (dialogs.Current.ToString().FirstOrDefault() == '$')
            {
                GameMng.I.dailogUI.setPlayerText = dialogs.Current.ToString().Remove(0, 1);
                // GameMng.I.dailogUI.ui[0].SetAsFirstSibling();
                // GameMng.I.dailogUI.ui[1].SetAsLastSibling();
            }
            else
            {
                GameMng.I.dailogUI.setNpcText = dialogs.Current.ToString();
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

    void ExitDialog()
    {
        // Destroy(tempDialog);
        isDialog = false;
        GameMng.I.dailogUI.gameObject.SetActive(false);

        // UI 레이어 다시 ON
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("UI");
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

        if (Vector2.Distance(GameMng.I.character.transform.position, transform.position) < 10)
        {
            speechTxt.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            speechTxt.gameObject.SetActive(false);
        }

        StartCoroutine(checkPlayerDistance());
    }


    protected void setSpeech(string msg)
    {
        speechTxt.text = msg;

        float x = speechTxt.preferredWidth;
        x = (x > 3) ? 3.5f : x + 0.5f;
        speechBox.transform.localScale = new Vector2(x, speechTxt.preferredHeight + 0.5f);
    }
}
