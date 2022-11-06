using System.Collections;
using UnityEngine;

public class NpcDuksu : Npcdata
{    
    void Start()
    {
        base.npcname = "이덕수 할아바이";
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
        if (!GameMng.I.userData.quest_done.Contains( (int)QUEST_CODE.GODOK_MISIK ))
        {
            // 퀘스트를 완료한건 아니지만 퀘스트 수령은 한 상태
            if (Character.sub_quest.ContainsKey(QUEST_CODE.GODOK_MISIK.ToString()))
            {
                // 진행도 0  =>  퀘스트 수령만 하고 진행 하나도 안함
                if (Character.sub_quest_progress[QUEST_CODE.GODOK_MISIK.ToString()].Equals(0))
                {
                    base.dialogs = Quest_Godok_Check();
                    setQuestIcon(QUEST_TYPE.SUB);
                }

                setSpeech("이..");
            }
            // 퀘스트 수령도 안한 상태
            else
            {
                base.dialogs = Quest_Godok();
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

    protected IEnumerator Quest_Godok()
    {
        yield return "고독한 미식가";
        yield return "던전내 가끔씩 나타나는 현상인데 몬스터들을 더욱 무섭게 만든다고 합니다!";
        yield return "퍼플라이트를 폐기해야합니다 !!";

        // 대화 시작과 동시에 서브 퀘스트 시작함을 알림
        setQuestIcon();
        GameMng.I.StartSubQuest(QUEST_CODE.GODOK_MISIK);
        
    }

    protected IEnumerator Quest_Godok_Check()
    {
        bool check = false;

        try {
            int idx = Character.haveItem[1].FindIndex(name => name.itemData.itemIndex == ITEM_INDEX.BAZIRAK_REHOI);

            // 아이템 개수가 안맞으면 이 함수에 들어올수 없지만 혹시 모르니
            if (idx >= 0)
            {
                if (Character.haveItem[2][idx].itemCount > 1) {
                    Character.haveItem[2][idx].itemCount--;
                } else if (Character.haveItem[2][idx].itemCount == 1) {
                    Character.haveItem[2].RemoveAt(idx);
                }
                check = true;
            }
        } catch(System.IndexOutOfRangeException e) {
        }

        if (check) {
            yield return "아니 이것만 있으면 성공할수 있단겐가!";
            yield return "정말 고 맙 네. 내 보답이 서운하지 않았음 하군";

            GameMng.I.nextSubQuest(QUEST_CODE.GODOK_MISIK);
        } else {
            yield return "...";
            yield return "아직 멀은건가?";
        }
    }
}
