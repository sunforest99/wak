using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcHaku : Npcdata
{
    void Start()
    {
        base.npcname = "미츠네 하쿠";
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
        if (!GameMng.I.userData.quest_done.Contains( (int)QUEST_CODE.PURPLE_LIGHT ))
        {
            // 퀘스트를 완료한건 아니지만 퀘스트 수령은 한 상태
            if (Character.sub_quest.ContainsKey(QUEST_CODE.PURPLE_LIGHT.ToString()))
            {
                if (Character.sub_quest_progress[QUEST_CODE.PURPLE_LIGHT.ToString()].Equals(0))
                {
                    base.dialogs = null;
                    setQuestIcon();
                }
                else
                {
                    base.dialogs = Quest_Purplelight_Done();
                    setQuestIcon(QUEST_TYPE.SUB);
                }
                setSpeech("퍼플라이트...");
            }
            // 퀘스트 수령도 안한 상태
            else
            {
                base.dialogs = Quest_Purplelight();
                setQuestIcon(QUEST_TYPE.SUB);
            }
        }
        else {
            base.dialogs = null;
            setQuestIcon();
            setSpeech("빛이 당신과 함께하길..");
            StartCoroutine(checkPlayerDistance());
        }
    }

    protected IEnumerator Quest_Purplelight()
    {
        yield return "혹시 " + GameMng.I.userData.user_nickname + "님, 퍼플라이트라고 들어 보셨나요?";
        yield return "던전내 가끔씩 나타나는 현상인데 몬스터들을 더욱 무섭게 만든다고 합니다!";
        yield return "퍼플라이트를 폐기해야합니다 !!";

        // 대화 시작과 동시에 서브 퀘스트 시작함을 알림
        setQuestIcon();
        GameMng.I.StartSubQuest(QUEST_CODE.PURPLE_LIGHT);
        // GameMng.I.nextSubQuest(QUEST_CODE.PURPLE_LIGHT);
    }

    protected IEnumerator Quest_Purplelight_Done()
    {
        yield return "세상에 퍼플라이트 던전에 정말 다녀온신겁니까?";
        yield return "용케 살아돌아오신 겁니다!!";

        GameMng.I.nextSubQuest(QUEST_CODE.PURPLE_LIGHT);
    }
}
