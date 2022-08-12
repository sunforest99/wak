using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    private static GameMng _instance = null;


    [Header("[  유저 관리  ]")]  // ========================================================================================================================================
    public UserData userData;
    public StateMng stateMng;
    public Character character = null;
    public GameObject[] characterPrefab = new GameObject[3];


    [Space(20)][Header("[  기본 UI 관리  ]")]  // ==========================================================================================================================
    public DailogUI dailogUI;           // ( ? )
    public ItemSlotUI BattleItemUI;     // (체력바 위) 배틀아이템 UI
    public Transform skillUI;           // (좌측하단) 스킬 UI들 부모
    public Transform itemSlot;          // (체력바 위) 배틀아이템 
    [HideInInspector] public List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    [HideInInspector] public List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();
    [HideInInspector] public List<UnityEngine.UI.Image> battleItem_Img = new List<UnityEngine.UI.Image>();
    public GetItemEXP[] getItemPool = new GetItemEXP[5];
    [SerializeField] GameObject pingPrefab;


    [Space(20)][Header("[  NPC 관리  ]")]  // ==============================================================================================================================
    public RaycastHit2D hit;
    public Npcdata npcData;
    private float npcDistance = 3.0f;       // <! npc와의 최대 거리
    public GameObject dialogPrefab;


    [Space(20)][Header("[  맵 관리  ]")]  // ==============================================================================================================================
    public Vector2 mapRightTop;
    public Vector2 mapCenter;
    public Vector2 mapLeftBotton;


    [Space(20)][Header("[  보스 관리 (여기 있으면 안됨)  ]")]  // ===========================================================================================================
    public BossData bossData = null;        // 보스 정보  //!< 이거 여기 없이 사용할 방법 찾아야 함



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
        userData.job = 1;
        createMe();
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
    public int getCharacterDamage(bool isCrital, bool isBackAttack)        // <! 캐릭터 데미지 가져오기
    {
        if (Character.usingSkill)        // <! 스킬 대미지
            return Character.usingSkill.CalcSkillDamage(
                isCrital, isBackAttack,
                Character._stat.minDamage, Character._stat.maxDamage, Character._stat.incDamagePer, Character._stat.criticalPer, Character._stat.incBackattackPer
            );
        else        // <! 평타 데미지
            return 20000;
    }


    public void createPing(Vector2 pos)
    {
        Instantiate(pingPrefab, pos += new Vector2(0, 0.65f), Quaternion.identity);
    }

    public Character createPlayer(int job, string nickName)
    {
        GameObject temp = Instantiate(characterPrefab[job]);
        character = temp.GetComponent<Character>();
        character.nickName = nickName;

        return character;
    }

    public void createMe()
    {
        createPlayer(userData.job - 1, GameMng.I.userData.user_nickname);
        character._isPlayer = true;

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
}