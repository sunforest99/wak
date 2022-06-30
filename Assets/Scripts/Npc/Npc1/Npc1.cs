using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc1 : Npcdata
{
    void Awake()
    {
        base.npcname = "npc1";
        base.dialogs = NpcDialog();
    }

    protected override IEnumerator NpcDialog()
    {
        yield return "안녕 npc1 이야";
        yield return "$ㅎㅇ player 1";

        GameMng.I.dailogUI.SelectBtn.SetActive(true);
        yield return "$선택";

        if(GameMng.I.dailogUI.flow)
        {
            yield return "대화의 흐름 1";
            yield return "$대화의 흐름 2";
            yield return "대화의 흐름 3";
        }
        else
        {
            yield return "대화의 흐름 -1";
            yield return "대화의 흐름 -2";
            yield return "대화의 흐름 -3";
        }
        
        yield return "asdf";
        GameMng.I.dailogUI.SelectBtn.SetActive(true);
        yield return "$선택1";

        if(GameMng.I.dailogUI.flow)
        {
            yield return "대화의 흐름 4";
            yield return "대화의 흐름 5";
            yield return "대화의 흐름 6";
        }
        else
        {
            yield return "대화의 흐름 -3";
            yield return "대화의 흐름 -4";
        }
    }
}
