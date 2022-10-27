using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcRusuk : Npcdata
{
    void Start()
    {
        base.npcname = "해루석";
        checkQuest();
    }

    /**
     * @brief 이 NPC가 가지고 있을 퀘스트
     * @desc 대화 내용 및 아이콘을 출력하기 위해 Awake나 선행 퀘스트를 마쳤을때마다 호출해서 갱신해야함
     */
    void checkQuest()
    {
        StopCoroutine(checkPlayerDistance());
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
