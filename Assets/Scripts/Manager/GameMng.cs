using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KEY_MODE
{
    PLAYER_MODE,    // 평상시, 캐릭터 이동할때
    TYPING_MODE,    // 엔터눌러서 채팅칠때
    MINIGAME_MODE,  // 미니게임 할때
    UI_MODE      // NPC와 대화, 혹은 퀘스트 등에 대한 상황
}

public enum EFF_TYPE
{
    EFF,
    BACK_EFF,
    TAKEN_EFF,
    REMOVE_EFF
}

public class GameMng : MonoBehaviour
{
    private static GameMng _instance = null;

    [HideInInspector] public KEY_MODE _keyMode = KEY_MODE.PLAYER_MODE;

    [Header("[  유저 관리  ]")]  // ========================================================================================================================================
    public UserData userData;
    public StateMng stateMng;
    public Character character = null;
    public GameObject[] characterPrefab = new GameObject[3];
    public GameObject[] magicianSkillPrefab = new GameObject[4];     // 기본공격, 바람, 용둔, 침스터콜
    public GameObject[] healerSkillPrefab = new GameObject[3];  // 기본공격, 나무, 레이저
    // public bool isFocusing = true;      // 캐릭터에게 포커싱 맞출지 (카메라가 따라올지 유무)

    [Space(20)][Header("[  기본 UI 관리  ]")]  // ==========================================================================================================================
    public NPCUI npcUI;                 // NPC 클릭시 선택 UI (대화UI, 선물UI)
    public ItemSlotUI BattleItemUI;     // (체력바 위) 배틀아이템 UI
    public Transform skillUI;           // (좌측하단) 스킬 UI들 부모
    public Transform itemSlot;          // (체력바 위) 배틀아이템 
    [SerializeField] GameObject pingPrefab;
    public ChatMng chatMng;
    public TMPro.TextMeshProUGUI[] myQuestName;       // 내가 진행중인 퀘스트 이름 text
    public TMPro.TextMeshProUGUI[] myQuestContent;    // 내가 진행중인 퀘스트 내용 text
    
    public Sprite[] questTypeSpr;         // 메인퀘스트, 서브퀘스트 Sprite 파일

    /* 스킬 */
    [HideInInspector] public List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    [HideInInspector] public List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();
    /* 배틀아이템 */
    [HideInInspector] public List<UnityEngine.UI.Image> battleItem_Img = new List<UnityEngine.UI.Image>();
    public GetItemEXP[] getItemPool = new GetItemEXP[5];
    /* 알림창 */
    public TMPro.TextMeshProUGUI alertMessage;  // 알림창 메세지
    public UnityEngine.UI.Button agreeBT;       // 알림창-수락 버튼
    public UnityEngine.UI.Button refuseBT;      // 알림창-거절 버튼
    public TMPro.TextMeshProUGUI noticeMessage; // 화면 중앙 하단에 글자만 띄우는 정보 메세지


    [Space(20)][Header("[  NPC 관리  ]")]  // ==============================================================================================================================
    // public RaycastHit2D hit;
    public Npcdata npcData;
    // private float npcDistance = 3.0f;       // <! npc와의 최대 거리
    // public GameObject dialogPrefab;
    

    [Space(20)][Header("[  맵 관리  ]")]  // ==============================================================================================================================
    public Vector3 mapRightTop;
    public Vector3 mapCenter;
    public Vector3 mapLeftBotton;
    public GameObject grass_destroy_eff;


    [Space(20)][Header("[  이펙트 관리  ]")]  // ==============================================================================================================================
    public Transform damageEff;         // 데미지 량 띄워주는 이펙트
    public GameObject eff;              // 일반 이펙트
    public GameObject backEff;          // 백어텍 이펙트
    public GameObject takenEff;         // 받는 피해 이펙트
    public GameObject removeEff;        // 사라지는 이펙트
    public Queue<GameObject> effPool = new Queue<GameObject>();         // 일반 이펙트 풀
    public Queue<GameObject> backEffPool = new Queue<GameObject>();     // 백어택 이펙트 풀
    public Queue<GameObject> takenEffPool = new Queue<GameObject>();    // 내가 데미지 입는 이펙트 풀
    public Queue<GameObject> removeEffPool = new Queue<GameObject>();   // 사라지는 이펙트 풀
    public Material[] materials = new Material[2];
    public GameObject itemGetEff;       // 아이템 획득 이펙트

    [Space(20)][Header("[  아이템 관리  ]")]  // ==============================================================================================================================
    public GameObject itemObj;

    [Space(20)][Header("[  보스 관리 (여기 있으면 안됨)  ]")]  // ===========================================================================================================
    public Boss boss = null;                    // 보스 정보 //!< 이거 여기 없이 사용할 방법이 있다면 좋음
    public EstherManager estherManager = null;  // 에스더 정보  //!< bossData와 같이 보스맵에서 나갈때마다 초기화 해주어야 함
    // public Dictionary<string, Monster> _monsters = new Dictionary<string, Monster>();


    public static GameMng I
    {
        get
        {
            if (_instance.Equals(null))
            {
                Debug.LogError("Instance is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);        // <! 필요하면 쓰장
    }

    private void Start()
    {
        userData.user_nickname = "임시 닉네임 (" +Random.Range(0, 1000) + ")";
    }

    // public void mouseRaycast(Vector2 charPos)      // <! 이름바꾸기
    // {
    //     Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //     Ray2D ray = new Ray2D(pos, Vector2.zero);

    //     hit = Physics2D.Raycast(ray.origin, ray.direction);

    //     if (hit.collider != null && hit.collider.CompareTag("Npc") && Vector2.Distance(hit.collider.transform.localPosition, charPos) <= npcDistance)
    //     {
    //         npcData = hit.collider.gameObject.GetComponent<Npcdata>();

    //         if (!npcData.isDailog && npcData != null)
    //         {
    //             npcData.isDailog = true;

    //             GameObject temp = Instantiate(GameMng.I.dialogPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    //             npcData.tempDialog = temp;
    //             temp.transform.parent = uiCanvas;
    //             temp.transform.localPosition = Vector3.zero;
    //             temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    //         }
    //     }
    // }
    public int getCharacterDamage(SkillData usingSkill, bool isCrital, bool isBackAttack)        // <! 캐릭터 데미지 가져오기
    {        
        if (usingSkill)        // <! 스킬 대미지
            return usingSkill.CalcSkillDamage(
                isCrital, isBackAttack,
                Character._stat.minDamage, Character._stat.maxDamage, Character._stat.incBackattackPer
            );
        else        // <! 평타 데미지
            return Random.Range(Character._stat.minDamage / 20, Character._stat.maxDamage / 20);
    }


    public void createPing(Vector3 pos)
    {
        Instantiate(pingPrefab, pos, Quaternion.identity);
    }

    public Character createPlayer(string uniqueNumber, int job, string nickName, float posX = 0, float posY = 0)
    {
        GameObject temp = Instantiate(characterPrefab[job], new Vector3(posX, 0, posY), Quaternion.identity);
        Character cha = temp.transform.GetChild(0).GetComponent<Character>();
        cha.nickName = nickName;
        temp.name = nickName + ":" + uniqueNumber;

        return cha;
    }

    public void createMe()
    {
        character = createPlayer(NetworkMng.I.uniqueNumber, userData.job, userData.user_nickname);
        character.isMe();

        if (userData.job.Equals(0))     // 무직(초반 캐릭터)는 스킬과 아이템이 없음
            return;
        
        for (int i = 0; i < skillUI.transform.childCount; i++)
        {
            skill_Img.Add(skillUI.GetChild(i).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>());
            cooltime_UI.Add(skillUI.GetChild(i).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>());
        }
        for (int i = 0; i < itemSlot.transform.childCount; i++)
            battleItem_Img.Add(itemSlot.GetChild(i).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>());

        for (int i = 0; i < character.skilldatas.Length; i++)
            skill_Img[i].sprite = character.skilldatas[i].getSkllImg;

        // for (int i = 0; i < Character.equipBattleItem.Length; i++)
        //     battleItem_Img[i].sprite = Character.equipBattleItem[i].itemData.itemSp;
    }

    /**
     * @brief 맵 이동 투표창 띄우기
     * @param changeRoomCode 이동하고자 하는 맵의 코드
     */
    public void alertMapChange(string changeRoomCode)
    {
        alertMessage.name = changeRoomCode;       // 변경할 방 코드 임시 저장
        alertMessage.text = "파티원이 맵 이동을 권유합니다. \n 수락 : 1  거절 : 0";
        alertMessage.transform.parent.gameObject.SetActive(true);
        agreeBT.onClick.RemoveAllListeners();
        agreeBT.onClick.AddListener(() => {
            NetworkMng.I.SendMsg("VOTE_ROOM_CHANGE:1");
            agreeBT.interactable = false;
            refuseBT.interactable = false;
        });
        refuseBT.onClick.RemoveAllListeners();
        refuseBT.onClick.AddListener(() => {
            NetworkMng.I.SendMsg("VOTE_ROOM_CHANGE:0");
            agreeBT.interactable = false;
            refuseBT.interactable = false;
        });
    }

    /**
     * @brief 화면 중앙 알림 띄울때 사용
     * @param msg 화면에 띄울 알림 메세지 내용
     */
    public void showNotice(string msg)
    {
        noticeMessage.gameObject.SetActive(false);
        noticeMessage.text = msg;
        noticeMessage.gameObject.SetActive(true);
    }

    /**
     * @brief 서브 퀘스트를 처음 시작할때 알림
     * @param qcode 서브 퀘스트 코드
     */
    public void StartSubQuest(QUEST_CODE qcode)
    {
        string subQuestName = qcode.ToString();
        Character.sub_quest.Add(
            subQuestName,
            Resources.Load<QuestData>($"QuestData/Sub/{subQuestName}") 
        );
        Character.sub_quest_progress[subQuestName] = 0;
    }
    /*
     * @brief 메인 퀘스트 진행률을 높일때 사용. 퀘스트 완료까지 체크함
     */
    public void nextMainQuest()
    {
        // 대화를 모두 진행했다면 해당 퀘스트의 진행률을 올림
        Character.main_quest_progress++;
        
        // 퀘스트마다 있는 진행률을 완료했다면 다음 퀘스트로 이동
        if (Character.main_quest_progress >= Character.main_quest.progressContent.Length)
        {
            // 경험치 지급
            // rewardExp(Character.main_quest.rewardExp);

            // 보상 아이템 지급
            rewardItem(Character.main_quest.rewardItem);

            Character.main_quest = Resources.Load<QuestData>($"QuestData/Main/MAIN_{Character.main_quest.questCode + 1}");
            Character.main_quest_progress = 0;
        }
        else
        {
            // 퀘스트 자체가 완료된것이 아니기 때문에 퀘스트 내용 UI만 변경함
            myQuestContent[0].text = Character.main_quest.progressContent[Character.main_quest_progress];
        }
    }

    /*
     * @brief 서브 퀘스트 진행률을 높일때 사용. 퀘스트 완료까지 체크함
     * @param questCode 체크할 서브 퀘스트 코드
     */
    public void nextSubQuest(QUEST_CODE questCode)
    {
        string questName = questCode.ToString();
        
        Character.sub_quest_progress[questName]++;

        if (Character.sub_quest_progress[questName] >= Character.sub_quest[questName].progressContent.Length)
        {
            // 경험치 지급
            // rewardExp(Character.sub_quest[questName].rewardExp);

            // 보상 아이템 지급
            rewardItem(Character.sub_quest[questName].rewardItem);

            Character.sub_quest.Remove(questName);
            Character.sub_quest_progress.Remove(questName);

            userData.quest_done.Add(
                Character.sub_quest[questName].questCode
            );
        }
        else
        {
            // TODO : 지금은 퀘스트 제목이 같은걸 찾아서 검색함. 크게 상관없으나 좋은 방법이 있다면 변경해보기
            // 퀘스트 자체가 완료된것이 아니기 때문에 퀘스트 내용 UI만 변경함
            // 서브 퀘스트 아직 표시 안해서 주석처리함
            // for (int i = 1; i < 5; i++)
            // {
            //     if (myQuestName[i].text.Equals(Character.sub_quest[questName].questName))
            //     {
            //         myQuestContent[i].text = Character.sub_quest[questName].progressContent[Character.sub_quest_progress[questName]];
            //         break;
            //     }
            // }
        }
    }

    // void rewardExp(float reward)
    // {
    //     // if (Mathf.FloorToInt(userData.level) <
    //     //     Mathf.FloorToInt(userData.level + reward))
    //     // {
    //     //     // 레벨업 애니메이션 작동
    //     // }
    //     // userData.level += reward;
    // }

    void rewardItem(ItemData[] reward)
    {
        // 아이템 지급
    }

    
    /*
     * @brief 데미지 이펙트 표시
     * @param pos 데미지 이펙트 좌표
     * @param damage 수치
     * @param isCritical 크리티컬 여부
     */
    public void createDamage(Vector3 pos, int damage, bool isCritical)
    {
        Transform damageObj = Instantiate(GameMng.I.damageEff, pos, Quaternion.Euler(20, 0, 0));
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set(damage, isCritical);
    }

    /*
     * @brief 데미지 이펙트 표시
     * @param pos 데미지 이펙트 좌표
     */
    public void createDamage(Vector3 pos)
    {
        Transform damageObj = Instantiate(GameMng.I.damageEff, pos, Quaternion.Euler(20, 0, 0));
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set("immune");
    }

    /*
     * @brief 이펙트 표시
     * @param isBackAttack 백어택 여부
     * @param pos 데미지 이펙트 좌표
     */
    public void createEffect(bool isBackAttack, Vector3 pos)
    {
        showEff(isBackAttack ? EFF_TYPE.EFF : EFF_TYPE.BACK_EFF, pos);
    }

    /*
     * @brief 히트 이펙트, 백어택 이펙트, 사라지는 이펙트 풀 초기화 (씬 변경됬을때만 사용할것)
     */
    public void initAllEff()
    {
        effPool.Clear();
        removeEffPool.Clear();
        takenEffPool.Clear();
        backEffPool.Clear();
        // 마을에서는 생성 안함
        if (!NetworkMng.I.myRoom.Equals(ROOM_CODE.HOME))
        {
            for (int i = 0; i < 5; i++)
            {
                takenEffPool.Enqueue(
                    Instantiate(takenEff, Vector3.zero, Quaternion.Euler(20, 0, 0))
                );
                effPool.Enqueue(
                    Instantiate(eff, Vector3.zero, Quaternion.Euler(20, 0, 0))
                );
                if (userData.job.Equals((int)JOB.WARRIER))  // 백어택 이펙트는 전사만 가짐
                    backEffPool.Enqueue(
                        Instantiate(backEff, Vector3.zero, Quaternion.Euler(20, 0, 0))
                    );
            }
        }
        for (int i = 0; i < 10; i++)
        {
            removeEffPool.Enqueue(
                Instantiate(removeEff, Vector3.zero, Quaternion.Euler(20, 0, 0))
            );
        }
    }
    public void showEff(EFF_TYPE type, Vector3 pos)
    {
        GameObject obj;
        switch (type)
        {
            case EFF_TYPE.EFF:
                if (effPool.Count > 0)
                    obj = effPool.Dequeue();
                else
                    obj = Instantiate(eff, Vector3.zero, Quaternion.Euler(20, 0, 0));
                break;
            case EFF_TYPE.BACK_EFF:
                if (backEffPool.Count > 0)
                    obj = backEffPool.Dequeue();
                else
                    obj = Instantiate(backEff, Vector3.zero, Quaternion.Euler(20, 0, 0));
                break;
            case EFF_TYPE.TAKEN_EFF:
                if (takenEffPool.Count > 0)
                    obj = takenEffPool.Dequeue();
                else
                    obj = Instantiate(takenEff, Vector3.zero, Quaternion.Euler(20, 0, 0));
                break;
            default:
                if (removeEffPool.Count > 0)
                    obj = removeEffPool.Dequeue();
                else
                    obj = Instantiate(removeEff, Vector3.zero, Quaternion.Euler(20, 0, 0));
                break;
        }
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
    }

    public void endEff(EFF_TYPE type, GameObject obj)
    {
        obj.SetActive(false);
        switch (type)
        {
            case EFF_TYPE.EFF:
                effPool.Enqueue(obj);
                break;
            case EFF_TYPE.BACK_EFF:
                backEffPool.Enqueue(obj);
                break;
            case EFF_TYPE.TAKEN_EFF:
                takenEffPool.Enqueue(obj);
                break;
            default:
                removeEffPool.Enqueue(obj);
                break;
        }
    }
}