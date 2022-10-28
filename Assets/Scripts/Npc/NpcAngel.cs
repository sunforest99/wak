using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcAngel : Npcdata
{
    [SerializeField] GameObject questBook;
    public Transform soul;       // 캐릭터, 플레이어

    int questProgress = 0;

    void Start()
    {
        Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("UI_Base"));
        base.npcname = "???";
        checkQuest();
    }

    /**
     * @brief 이 NPC가 가지고 있을 퀘스트
     * @desc 대화 내용 및 아이콘을 출력하기 위해 Awake나 선행 퀘스트를 마쳤을때마다 호출해서 갱신해야함
     */
    void checkQuest()
    {
        questBook.SetActive(false);
        StopCoroutine(checkPlayerDistance());

        // 선행 (메인)퀘스트를 했는지
        if (questProgress.Equals(0))
        {
            base.dialogs = Talk_MainQuest_1();
            setQuestIcon(QUEST_TYPE.MAIN);
            setSpeech("(우클릭으로 말걸기..)");
        }
        else if (questProgress.Equals(1))
        {
            base.dialogs = Talk_MainQuest_2();
            setQuestIcon(QUEST_TYPE.MAIN);
            questBook.SetActive(true);
        }
        else if (questProgress.Equals(2))
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
    }

    protected IEnumerator Talk_MainQuest_1()
    {
        yield return "...";
        yield return ".....";
        yield return "아무튼 많은 텍스트";
        yield return "아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트아무튼 많은 텍스트";

        // //yield return "";    // 선택 메세지 띄우기 전에 틈을 주려면 빈 메세지 넣기
        // GameMng.I.npcUI.dialogUI.setSelectBlock("선택 1", "선택 2");
        // yield return "";    // <- 얘는 선택메세지 후에는 꼭 있어야함

        // if (getFlow())
        // {
        //     yield return "선택1에 따른 대사";
        // }
        // else
        // {
        //     yield return "선택2에 따른 대사 - 1";
        //     yield return "선택2에 따른 대사 - 2";
        // }

        yield return "앞에 있는 책을 통해 힘을 드리겠습니다";

        questProgress++;
        checkQuest();
    }

    protected IEnumerator Talk_MainQuest_2()
    {
        if (GameMng.I.userData.job > 0)
        {
            yield return "앞의 책을 통해 전직해주세요..";
            yield return ";;";
            base.dialogs = Talk_MainQuest_2();          // <- 만약 이후 퀘가 있어서 다시 말걸어야 한다면 꼭 마지막에 다시 선언
        }
        else
        {
            yield return "그런 힘을 선택하다니...";
            yield return "와!";

            questProgress++;
            checkQuest();
        }
    }

    protected IEnumerator Talk_MainQuest_3()
    {
        yield return "준비가 되셨나요?";
        yield return "행운을 빕니다.";

        // TODO : 튜토리얼 던전
        SceneManager.LoadScene("TutorialDungeon");
    }

    protected override IEnumerator NpcDialog()
    {
        yield return "...";
        yield return ".....";
    }

    public override void ExitDialog()
    {
        // Destroy(tempDialog);
        isDialog = false;
        GameMng.I.npcUI.dialogUI.gameObject.SetActive(false);
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;

        MCamera.I.setTargetChange(soul);
        MCamera.I.zoomOut2();
    }
}
