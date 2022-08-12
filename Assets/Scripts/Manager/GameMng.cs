using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public RaycastHit2D hit;

    public GameObject dialogPrefab;
    [SerializeField]
    GameObject pingPrefab;

    public DailogUI dailogUI;
    public Npcdata npcData;
    private float npcDistance = 3.0f;       // <! npc와의 최대 거리

    private static GameMng _instance = null;

    public GameObject[] characterPrefab = new GameObject[3];

    public UserData userData;

    public Character character = null;

    public ItemSlotUI BattleItemUI;

    public Transform skillUI;
    public Transform itemSlot;

    public List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    public List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();
    public List<UnityEngine.UI.Image> battleItem_Img = new List<UnityEngine.UI.Image>();
    public GetItemEXP[] getItemPool = new GetItemEXP[5];

    public float level = 10;

    public int getCharecterDamage(bool isCrital, bool isBackAttack)        // <! 캐릭터 데미지 가져오기
    {
        if (Character.usingSkill)        // <! 스킬 대미지
            return Character.usingSkill.CalcSkillDamage(
                isCrital, isBackAttack,
                character._stat.minDamage, character._stat.maxDamage, character._stat.incDamagePer, character._stat.criticalPer, character._stat.incBackattackPer
            );
        else        // <! 평타 데미지
            return 20000;
    }

    public StateMng stateMng;

    public BossData bossData = null;        // 보스 정보

    public Vector2 mapRightTop;
    public Vector2 mapCenter;
    public Vector2 mapLeftBotton;

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
        createPlayer();
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

    public void createPing(Vector2 pos)
    {
        Instantiate(pingPrefab, pos += new Vector2(0, 0.65f), Quaternion.identity);
    }

    public void createPlayer()
    {
        GameObject temp = Instantiate(characterPrefab[userData.job - 1]);
        character = temp.GetComponent<Character>();

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