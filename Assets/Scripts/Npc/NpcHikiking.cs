using System.Collections;
using UnityEngine;

public class NpcHikiking : Npcdata
{
    [SerializeField] GameObject questObjectsPrefab;
    GameObject questObjects = null;
    
    void Start()
    {
        base.npcname = "히키킹";
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
        if (!GameMng.I.userData.quest_done.Contains( (int)QUEST_CODE.R_U_HUMAN ))
        {
            // 퀘스트를 완료한건 아니지만 퀘스트 수령은 한 상태
            if (Character.sub_quest.ContainsKey(QUEST_CODE.R_U_HUMAN.ToString()))
            {
                // 진행도 0  =>  퀘스트 수령만 하고 진행 하나도 안함
                if (Character.sub_quest_progress[QUEST_CODE.R_U_HUMAN.ToString()].Equals(0))
                {
                    base.dialogs = null;
                    setQuestIcon();
                }
                else
                {
                    base.dialogs = Quest_RUPeople_Done();
                    setQuestIcon(QUEST_TYPE.SUB);
                }
                // 마을로 재진입 했을때 스스로 생성되게
                if (questObjects == null)
                    questObjects = Instantiate(questObjectsPrefab, Vector3.zero, Quaternion.identity);

                setSpeech("이 히키킹구..");
            }
            // 퀘스트 수령도 안한 상태
            else
            {
                base.dialogs = Quest_RUPeople();
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

    public override void actSomething()
    {
        if (questObjects == null)
            return;

        questObjects.GetComponent<RUHuman>().found();
    }

    protected IEnumerator Quest_RUPeople()
    {
        yield return "너 사람이지";
        yield return "던전내 가끔씩 나타나는 현상인데 몬스터들을 더욱 무섭게 만든다고 합니다!";
        yield return "퍼플라이트를 폐기해야합니다 !!";

        // 대화 시작과 동시에 서브 퀘스트 시작함을 알림
        setQuestIcon();
        GameMng.I.StartSubQuest(QUEST_CODE.R_U_HUMAN);
        if (questObjects == null)
            questObjects = Instantiate(questObjectsPrefab, Vector3.zero, Quaternion.identity);
    }

    protected IEnumerator Quest_RUPeople_Done()
    {
        yield return "세상에 퍼플라이트 던전에 정말 다녀온신겁니까?";
        yield return "용케 살아돌아오신 겁니다!!";

        GameMng.I.nextSubQuest(QUEST_CODE.R_U_HUMAN);
        if (questObjects != null)
            Destroy(questObjects);
    }
}
