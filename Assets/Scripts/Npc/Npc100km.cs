    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc100km : Npcdata
{
    void Start()
    {
        base.npcname = "대도시까지 100km";
        checkQuest();
    }

    /**
     * @brief 이 NPC가 가지고 있을 퀘스트
     * @desc 대화 내용 및 아이콘을 출력하기 위해 Awake나 선행 퀘스트를 마쳤을때마다 호출해서 갱신해야함
     */
    void checkQuest()
    {
        if (Character.main_quest.questCode.Equals(0) && Character.main_quest_progress.Equals(0))
        {
            base.dialogs = Talk_MainQuest_0();
            setSpeech("표지판을 마우스 우클릭해주세요");
            StartCoroutine(checkPlayerDistance());
        }
        else {
            base.dialogs = Talk_MainQuest_0();
            // showMainQuestIcon();
            StopCoroutine(checkPlayerDistance());
        }
    }

    protected IEnumerator Talk_MainQuest_0()
    {
        yield return "(대도시까지 100km)";
        yield return "$후..";
        yield return "$이제 대도시까지 100km정도 남았구나";
        yield return "$꼭 자수성가하고 말겠어!!";

        GameMng.I.nextMainQuest();
        checkQuest();
    }
}
