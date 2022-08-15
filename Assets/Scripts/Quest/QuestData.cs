using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QUEST_TYPE
{
    MAIN,
    SUB
}

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObject/QuestData")]
public class QuestData : ScriptableObject
{
    public QUEST_TYPE questType;
    public int questCode;              // 퀘스트 코드
    public string questName;           // 퀘스트 이름
    public int done_progress;          // 퀘스트 완료 진행률
    public ItemData[] rewardItem;      // 퀘스트 보상 - 아이템
    public float rewardExp;            // 퀘스트 보상 - 경험치

    public bool isDoingQuest()
    {
        return GameMng.I.userData.quest_ing.Find(q => (q.quest_code == questCode)) != null;
    }
    public bool isDoneQuest()
    {
        return GameMng.I.userData.quest_done.Contains(questCode);
    }
}




