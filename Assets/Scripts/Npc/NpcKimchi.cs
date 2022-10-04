using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcKimchi : Npcdata
{
    void Start()
    {
        base.npcname = "김치만두";
        checkQuest();
    }

    /**
     * @brief 이 NPC가 가지고 있을 퀘스트
     * @desc 대화 내용 및 아이콘을 출력하기 위해 Awake나 선행 퀘스트를 마쳤을때마다 호출해서 갱신해야함
     */
    void checkQuest()
    {
        StopCoroutine(checkPlayerDistance());

        // 선행 (메인)퀘스트를 했는지
        if (Character.main_quest.questCode.Equals(1))
        {
            base.dialogs = Talk_MainQuest_3();
            setQuestIcon(QUEST_TYPE.MAIN);
        }
        else {
            base.dialogs = null;
            setQuestIcon();
            setSpeech("빛이 당신과 함께하길..");
            StartCoroutine(checkPlayerDistance());
        }
        // 한 퀘스트에 여러 대화가 있다면 그때는 .quest_progress로 분리
        
        
        // 만약 (서브)퀘스트의 경우는 아래처럼 contains 확인과 선행 퀘스트도 확인해야함
        // (메인)퀘스트는 순서가 있는 단일 퀘스트지만 (서브)퀘스트는 선행 퀘스트가 순서대로가 아니기 때문
        // else if (! GameMng.I.userData.quest_done.Contains( 0 ))
        // {
        //     // 선행 (서브)퀘스트를 했는지?   // 선행퀘 조건 없는 퀘스트라면 지워도 상관 없음
        //     if (GameMng.I.userData.quest_done.Contains( 0 ))
        //     {

        //     }
        //     // 선행 (메인)퀘스트를 했는지?   // 선행퀘 조건 없는 퀘스트라면 지워도 상관 없음
        //     if (Character.main_quest.questCode.quest_code > 10)
        //     {

        //     }
        //     // 생성?
        //     // 자식?
        // }
    }

    protected IEnumerator Talk_MainQuest_3()
    {
        yield return "...";
        yield return ".....";
        yield return "아무튼 많은 텍스트";
        yield return "아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트";

        //yield return "";    // 선택 메세지 띄우기 전에 틈을 주려면 빈 메세지 넣기
        GameMng.I.npcUI.dialogUI.setSelectBlock("선택 1", "선택 2");
        yield return "";    // <- 얘는 선택메세지 후에는 꼭 있어야함

        if (getFlow())
        {
            yield return "선택1에 따른 대사";
        }
        else
        {
            yield return "선택2에 따른 대사 - 1";
            yield return "선택2에 따른 대사 - 2";
        }

        yield return "앞에 있는 책을 통해 힘을 드리겠습니다";

        GameMng.I.nextMainQuest();
        checkQuest();
    }


    protected IEnumerator Talk_SubQuest_0()
    {
        yield return "...";
        yield return ".....";

        // 대화 시작과 동시에 서브 퀘스트 시작함을 알림
        GameMng.I.StartSubQuest(QUEST_CODE.TEMP_QUEST_0);
        // GameMng.I.nextSubQuest(QUEST_CODE.TEMP_QUEST_0);
    }

    protected override IEnumerator NpcDialog()
    {
        yield return "...";
        yield return ".....";
    }
}
