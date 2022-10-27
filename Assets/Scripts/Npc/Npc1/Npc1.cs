using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc1 : Npcdata
{
    void Start()
    {
        base.npcname = "npc1";

        if (true/* 이 NPC 를 클릭 했을 떄 */)
        {
            base.dialogs = NpcDialog();
        }

        // 이 NPC가 가지고 있을 퀘스트
        
        // 만약 이 (서브)퀘스트를 못끝냈다면 퀘스트 표시 띄움
        if (! GameMng.I.userData.quest_done.Contains( 0 ))
        {
            // 선행 (서브)퀘스트를 했는지?   // 선행퀘 조건 없는 퀘스트라면 지워도 상관 없음
            if (GameMng.I.userData.quest_done.Contains( 0 ))
            {

            }
            // 선행 (메인)퀘스트를 했는지?   // 선행퀘 조건 없는 퀘스트라면 지워도 상관 없음
            // if (GameMng.I.userData.main_quest.quest_code > 10)
            // {

            // }
            // 생성?
            // 자식?
        }

    }

    protected override IEnumerator NpcDialog()
    {
        yield return "안녕 npc1 이야";
        yield return "$ㅎㅇ player 1";

        // GameMng.I.dailogUI.selectBlock.SetActive(true);
        GameMng.I.npcUI.dialogUI.setSelectBlock("a", "b");

        yield return "$선택";

        if(GameMng.I.npcUI.dialogUI.flow)
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
        GameMng.I.npcUI.dialogUI.selectBlock.SetActive(true);
        yield return "$선택1";

        if(GameMng.I.npcUI.dialogUI.flow)
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
