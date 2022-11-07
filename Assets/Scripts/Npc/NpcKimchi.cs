using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcKimchi : Npcdata
{
    [SerializeField] NpcPreeter _preeter;

    void Start()
    {
        base.npcname = "김치만두번영택사스가";
        checkQuest();
    }

    /**
     * @brief 이 NPC가 가지고 있을 퀘스트
     * @desc 대화 내용 및 아이콘을 출력하기 위해 Awake나 선행 퀘스트를 마쳤을때마다 호출해서 갱신해야함
     */
    public override void checkQuest()
    {
        StopCoroutine(checkPlayerDistance());

        // 만약 (서브)퀘스트의 경우는 아래처럼 contains 확인과 선행 퀘스트도 확인해야함
        // (메인)퀘스트는 순서가 있는 단일 퀘스트지만 (서브)퀘스트는 선행 퀘스트가 순서대로가 아니기 때문
        if (!GameMng.I.userData.quest_done.Contains( (int)QUEST_CODE.BAEDAL ))
        {
            // 퀘스트를 완료한건 아니지만 퀘스트 수령은 한 상태
            if (Character.sub_quest.ContainsKey(QUEST_CODE.BAEDAL.ToString()))
            {
                // 진행도 0  =>  퀘스트 수령만 하고 진행 하나도 안함
                if (Character.sub_quest_progress[QUEST_CODE.BAEDAL.ToString()].Equals(1))
                {
                    setQuestIcon(QUEST_TYPE.SUB);
                    base.dialogs = Quest_Baedal_Done();
                    setSpeech("....");
                }
                else
                {
                    base.dialogs = null;
                    setQuestIcon();
                }
            }
            // 퀘스트 수령도 안한 상태
            else
            {
                base.dialogs = null;
                setQuestIcon();
            }
        }
        else {
            base.dialogs = null;
            setQuestIcon();
            setSpeech("빛이 당신과 함께하길..");
            StartCoroutine(checkPlayerDistance());
        }
    }

    protected IEnumerator Quest_Baedal_Done()
    {
        yield return "배달 감사합니다.";
        yield return "그거 아세요? 돈까스는 스시집에서 시키는게 제일입니다. 흐흐";

        GameMng.I.nextSubQuest(QUEST_CODE.BAEDAL);
        if (_preeter.questObjects != null)
            Destroy(_preeter.questObjects);
    }
}
