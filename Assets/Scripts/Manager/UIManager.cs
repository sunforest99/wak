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
        
        itemLoad();
        QuestLoad();

        // StartCoroutine(waitingLoading());
    }

    IEnumerator waitingLoading()
    {
        yield return new WaitForSeconds(1);
        
        // 전직을 안했다는건 최초 숲으로 이동
        if (GameMng.I.userData.job.Equals(0))
        {
            SceneManager.LoadScene("TempleScene");
        }
        else
        {
            NetworkMng.I.changeRoom(ROOM_CODE.HOME);   // TODO <- 대도시로 변경
        }
    }

    private void Update() {

        if (Input.GetMouseButtonDown(1) && GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE))
        {
            // RaycastHit[] hits;
            // hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

            // for (int i = 0; i < hits.Length; i++)
            // {
            //     RaycastHit hit = hits[i];
            // }            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            string[] layers = { "Default", "Character", "Map" }; 
            int layerMask = LayerMask.GetMask(layers);
            // int layerMask = 1 << LayerMask.NameToLayer("Character");

            if (Physics.Raycast(ray, out hit, 100f, layerMask)) {
            // if (Physics.Raycast(ray.origin, ray.direction * 10000, out hit)) {
                if (hit.transform.CompareTag("Npc"))        // NPC 우선순위
                {
                    if (Vector3.Distance(hit.transform.position, GameMng.I.character.transform.position) < 2)
                    {
                        // 저장된 dialog 실행
                        // 근데 dialog 저장 방식이 맞는지 일단 확인
                        GameMng.I.npcData = hit.transform.GetComponent<Npcdata>();
                        
                        // 대화 거리가 없으면 그냥 닫기
                        if (GameMng.I.npcData.dialogs == null)
                        {
                            GameMng.I.npcData = null;
                            return;
                        }

                        // 이미 선택모드인지 체크
                        if (!GameMng.I.npcUI.npcSelectUI.activeSelf)
                        {
                            GameMng.I._keyMode = KEY_MODE.UI_MODE;

                            MCamera.I.setTargetChange(hit.transform);
                            MCamera.I.zoomIn();

                            // UI 레이어 제거
                            // MCamera.I._vCamera.
                            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("UI_Base"));

                            GameMng.I.npcUI.npcSelectUI.SetActive(true);
                        }
                    }
                    else
                    {
                        GameMng.I.showNotice("거리가 너무 멉니다.");
                    }
                    return;
                }
                else if (hit.transform.CompareTag("Character"))   // 나를 제외한 플레이어를 선택함
                {
                    Debug.Log("!!!!");
                    // txt[0] 닉네임
                    // txt[1] uniqueNumber
                    string[] txt = hit.transform.name.Split(':');
                    
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
                // 핑 생성
                else if (hit.transform.CompareTag("Floor") && Input.GetKey(KeyCode.LeftControl))
                {
                    GameMng.I.createPing(hit.point + new Vector3(0, 0.54f, 0));
                    NetworkMng.I.SendMsg(string.Format("PING:{0}:{1}:{2}", hit.point.x, hit.point.y, hit.point.z + 0.54f));
                }
                else if (hit.transform.CompareTag("Build"))
                {
                    if (Vector3.Distance(hit.transform.position, GameMng.I.character.transform.position) < 2)
                    {
                        hit.transform.GetComponent<Build>().activeBuild();
                        GameMng.I._keyMode = KEY_MODE.UI_MODE;
                    }
                    else
                    {
                        GameMng.I.showNotice("거리가 너무 멉니다.");
                    }
                    return;
                }
                selectPlayerName.transform.parent.gameObject.SetActive(false);
            }
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

    public void selectDialog()
    {
        GameMng.I.npcUI.showDialog();
        
        MCamera.I.zoomIn2();

        GameMng.I.npcData.isDialog = true;
    }

    public void selectGift()
    {

    }

    public void selectCancel()
    {
        GameMng.I.npcUI.npcSelectUI.SetActive(false);

        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;

        // UI 레이어 다시 ON
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("UI_Base");

        MCamera.I.setTargetChange(GameMng.I.character.transform);
        MCamera.I.zoomOut();
    }

    void itemSave()
    {
        for(int i = 0; i < Character.haveItem.Count; i++)
        {
            for(int j = 0; j < Character.haveItem[i].Count; j++)
            {
                GameMng.I.userData.inventory[i][j].item_code = (int)Character.haveItem[i][j].itemData.itemIndex;
                GameMng.I.userData.inventory[i][j].mount = Character.haveItem[i][j].itemCount;
            }
        }
    }

    void itemLoad()
    {
        for (int i = 0; i < GameMng.I.userData.inventory.Count; i++)
        {
            for (int j = 0; j < GameMng.I.userData.inventory[i].Count; j++)
            {
                Character.haveItem[i].Add(new Item(Resources.Load<ItemData>($"ItemData/{((ITEM_INDEX)GameMng.I.userData.inventory[i][j].item_code).ToString()}"), GameMng.I.userData.inventory[i][j].mount));
            }
        }
    }

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
            Character.sub_quest_progress[subQuestName] = 0;      // TODO : DB 데이터로 최신화 할 것

            // 퀘스트 UI에는 최대 5개 까지만 보여줌. TODO : 화면에 띄울 퀘스트를 선택해서 보여주게 하려면 바꿔야함.
            if (i+1 < 5)
            {
                GameMng.I.myQuestName[i+1].text = Character.sub_quest[subQuestName].questName;
                GameMng.I.myQuestContent[i+1].text = Character.sub_quest[subQuestName].progressContent[Character.sub_quest_progress[subQuestName]];
            }
        }
    }
}
