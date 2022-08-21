using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [System.Serializable]
// public class SkillIcons
// {
//     public Sprite[] icons;
// }

public class UIManager : MonoBehaviour
{
    // ===== 스킬 =====================================================================================================
    // 0:전사, 1:법사, 2:힐러   (사용할땐 GameMng.I.userData.job 에서 항상 -1을 빼서 사용)
    // [SerializeField] SkillIcons[] job_skill_icons;
    // [SerializeField] UnityEngine.UI.Image[] skill_icons;
    // [SerializeField] GameObject skillIconWindow;
    [SerializeField] Canvas _canvas;
    [SerializeField] TMPro.TextMeshProUGUI selectPlayerName;
    [SerializeField] UnityEngine.UI.Button selectPlayerInviteBT;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GameMng.I.userData.main_quest.quest_code = 1;
        QuestLoad();
    }

    private void Update() {

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit)
            if (!hit.collider.Equals(null))
            {
                if (hit.collider.tag.Equals("Character"))   // 나를 제외한 플레이어를 선택함
                {
                    // txt[0] 닉네임
                    // txt[1] uniqueNumber
                    string[] txt = hit.collider.name.Split(':');
                    
                    // UI 캔버스 상에서 내가 마우스 클릭한 위치로 찾기
                    Vector2 canvasMousePos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvas.transform as RectTransform, 
                        Input.mousePosition, 
                        _canvas.worldCamera, 
                        out canvasMousePos
                    );
                    
                    selectPlayerName.text = txt[0];
                    selectPlayerName.transform.parent.localPosition = canvasMousePos;
                    selectPlayerName.transform.parent.gameObject.SetActive(true);
                    selectPlayerInviteBT.onClick.RemoveAllListeners();
                    selectPlayerInviteBT.onClick.AddListener(() => {
                        selectPlayerName.transform.parent.gameObject.SetActive(false);
                        NetworkMng.I.SendMsg(string.Format("INVITE_PARTY:{0}", txt[1]));
                    });
                    return;
                }
                else if (hit.collider.tag.Equals("Npc"))
                {
                    if (Vector2.Distance(hit.collider.transform.position, GameMng.I.character.transform.position) < 1.4f)
                    {
                        // 저장된 dialog 실행
                        // 근데 dialog 저장 방식이 맞는지 일단 확인
                        GameMng.I.npcData = hit.collider.GetComponent<Npcdata>();
                        GameMng.I.npcData.NextDialog();
                        GameMng.I.dailogUI.gameObject.SetActive(true);
                        GameMng.I.npcData.isDialog = true;

                        // UI 레이어 제거
                        Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("UI"));
                    }
                    else
                    {
                        GameMng.I.showNotice("거리가 너무 멉니다.");
                    }
                    return;
                }
            }
            selectPlayerName.transform.parent.gameObject.SetActive(false);
        }  
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _canvas.worldCamera = Camera.main;
    }

    // void setSkillIcons()
    // {
    //     if (GameMng.I.userData.job == 0)
    //     {
    //         skillIconWindow.SetActive(false);
    //         return;
    //     }

    //     for (int i = 0; i < 5; i++)
    //     {
    //         skill_icons[i].sprite = job_skill_icons[GameMng.I.userData.job - 1].icons[i];
    //     }
    // }


    void QuestLoad()
    {
        // 메인 퀘스트 so 파일명은 진행 순서에 따라서 MAIN_${} 으로 결정됨.
        Character.main_quest = Resources.Load<QuestData>($"QuestData/Main/MAIN_{GameMng.I.userData.main_quest.quest_code}");
        Character.main_quest_progress = 0;      // TODO : DB 데이터로 최신화 할 것
        GameMng.I.myQuestName[0].text = Character.main_quest.questName;
        GameMng.I.myQuestContent[0].text = Character.main_quest.progressContent[Character.main_quest_progress];
        
        string subQuestName = "";

        // 서브 퀘스트 so 파일명은 ENUM에 모두 저장함.
        for(int i = 0; i < GameMng.I.userData.sub_quest.Count; i++)
        {
            subQuestName = ((QUEST_CODE)GameMng.I.userData.sub_quest[i].quest_code).ToString();
            Character.sub_quest.Add(
                subQuestName,
                Resources.Load<QuestData>($"QuestData/Sub/{subQuestName}") 
            );
            Character.sub_quest_progress[subQuestName] = 0;

            // 퀘스트 UI에는 최대 5개 까지만 보여줌. TODO : 화면에 띄울 퀘스트를 선택해서 보여주게 하려면 바꿔야함.
            if (i+1 < 5)
            {
                GameMng.I.myQuestName[i+1].text = Character.main_quest.questName;
                GameMng.I.myQuestContent[i+1].text = Character.main_quest.progressContent[Character.main_quest_progress];
            }
        }
    }
}
