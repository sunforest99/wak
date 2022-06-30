using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;

public class Npcdata : MonoBehaviour
{
    [HideInInspector] public bool isDailog;
    [SerializeField] protected string npcname;

    public GameObject tempDialog;     // <! 이름 바꾸기

    protected IEnumerator dialogs;

    void Update()
    {
        if (isDailog)
        {
            if (Input.GetMouseButtonDown(1) && !GameMng.I.dailogUI.SelectBtn.activeSelf)
            {
                NextDialog();
            }
        }
    }

    public void NextDialog()
    {
        if (dialogs.MoveNext() == true)
        {
            GameMng.I.dailogUI.setNpcText = npcname;
            GameMng.I.dailogUI.setPlayerName = "Player";        // <! 나중에 닉네임 넣기
            if (dialogs.Current.ToString().FirstOrDefault() == '$')
            {
                GameMng.I.dailogUI.setPlayerText = dialogs.Current.ToString().Remove(0, 1);
                GameMng.I.dailogUI.ui[0].SetAsFirstSibling();
                GameMng.I.dailogUI.ui[1].SetAsLastSibling();
            }
            else
            {
                GameMng.I.dailogUI.setNpcText = dialogs.Current.ToString();
                GameMng.I.dailogUI.ui[0].SetAsLastSibling();
                GameMng.I.dailogUI.ui[1].SetAsFirstSibling();
            }
        }
        else
        {
            ExitDialog();
        }

    }

    void ExitDialog()
    {
        Destroy(tempDialog);
        isDailog = false;
        GameMng.I.dailogUI = null;
    }

    protected virtual IEnumerator NpcDialog()
    {
        yield return null;
    }

}
