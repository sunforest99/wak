using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempleCanvas : MonoBehaviour
{
    // [SerializeField] GameObject normalPerson;

    public void selectJob(int job)
    {
        GameMng.I.userData.job = job;
    }

    public void deicdeJob()
    {
        // 직업을 결정했다면 기존 사람을 지움
        Destroy(GameMng.I.character.gameObject);
        // Character character = GameMng.I.createPlayer("", GameMng.I.userData.job, GameMng.I.userData.user_nickname);
        // character.isMe();
        GameMng.I.createMe();

        if (Character.main_quest.questCode.Equals(4))
        {
            GameMng.I.nextMainQuest();
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
