using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 임시 스크립트임
// 쓸모없어지면 삭제할 것

public class GoToTemple : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {        
        if (other.gameObject.CompareTag("Player"))
        {
            // 1.대도시로, 2.생존하기 퀘스트 넘기고 바로 전직하기로 넘어감
            Character.main_quest.questCode = 2;
            Character.main_quest = Resources.Load<QuestData>($"QuestData/Main/MAIN_{Character.main_quest.questCode + 1}");
            Character.main_quest_progress = 0;
            GameMng.I.myQuestName[0].text = Character.main_quest.questName;
            GameMng.I.myQuestContent[0].text = Character.main_quest.progressContent[Character.main_quest_progress];
            SceneManager.LoadScene("TempleScene");
        }
    }
}
