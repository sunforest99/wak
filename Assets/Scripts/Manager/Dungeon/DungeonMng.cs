using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DUNGEON_TYPE
{
    NONE,       // 선택중일때
    MONSTER,
    MONSTER_PURPLER,
    NPC,
    REST,
    SHOP,
    RANDOM      // 랜덤은 들어가게 되면(맵 생성할때) 위 타입중 하나가 됨
}

public class DungeonMng : MonoBehaviour
{
    [Header("[  던전 전용 UI  ]")]  // ==========================================================================================================================
    [SerializeField] GameObject _mapUI;                                 // 맵 UI
    [SerializeField] GameObject curLocationUI;                          // 현재 위치 알려주는 UI


    [Space(20)][Header("[  던전 공용 데이터  ]")]  // ==========================================================================================================================
    public static DUNGEON_TYPE _dungeon_Type = DUNGEON_TYPE.MONSTER;    // 현재 들어가 있는 던전의 타입
    public static int _leftMonster = 0;                              // 현재 남아있는 잔여 몬스터
    protected static GameObject getNextWall;                            // _nextWall 이 Start때 들어감
    [SerializeField] GameObject _nextWall;                              // 다음 던전으로 넘어갈 수 있는 문을 막고 있는 벽 (몬스터 0마리 되면 false 해서 길 열어주기)
    [SerializeField] GameObject _nextPortal;                            // 던전 다음 포탈
    [SerializeField] GameObject _clearPortal;                           // 던전 클리어 포탈
    public Animator npcAnim;                                            // NPC 획득시 해당 NPC 가 들어감.

    [Space(20)][Header("[  던전 공용 프리팹  ]")]  // ==========================================================================================================================
    [SerializeField] protected GameObject purpleLight;                  // 몬스터 강화 오브젝트
    [SerializeField] protected GameObject[] npc = new GameObject[6];    // 던전에서 획득 가능한 NPC들, (enum.cs 의 NPC 와 순서가 같아야함)
    [SerializeField] protected GameObject campFire;                     // 휴식 오브젝트
    [SerializeField] protected GameObject npc_shop;                     // 상점 npc  (소피아(왁귀) | 캘리칼리(계륵))
    [SerializeField] protected GameObject prison;                       // NPC 갇혀있는 감옥
    [SerializeField] protected GameObject[] grasses;
    GameObject grass;
    int grassIdx = 0;


    protected virtual void Start()
    {
        GameMng.I._keyMode = KEY_MODE.UI_MODE;
        // _mapUI.SetActive(true);
        getNextWall = _nextWall;

        // StartCoroutine(showMapUI());
        grass = Instantiate(grasses[grassIdx++], Vector3.zero, Quaternion.identity);
    }

    IEnumerator showMapUI()
    {
        yield return new WaitForSeconds(2);

        _mapUI.SetActive(true);
    }

    DUNGEON_TYPE randomDungeon()
    {
        int rand = Random.Range(0, 100);
        if (rand < 30)
            return DUNGEON_TYPE.MONSTER;                // 30%
        else if (rand < 60)
            return DUNGEON_TYPE.MONSTER_PURPLER;        // 30%
        else if (rand < 80)
            return DUNGEON_TYPE.NPC;                    // 20%
        else if (rand < 95)
            return DUNGEON_TYPE.SHOP;                   // 15%
        return DUNGEON_TYPE.REST;                       // 5%
    }

    void initDungeon()
    {
        _leftMonster = 0;
        campFire.SetActive(false);
        npc_shop.SetActive(false);
        
        Destroy(grass);
        grassIdx++;
        if (grassIdx >= grasses.Length) grassIdx = 0;
        grass = Instantiate(grasses[grassIdx], Vector3.zero, Quaternion.identity);

        if (_dungeon_Type.Equals(DUNGEON_TYPE.RANDOM))
            _dungeon_Type = randomDungeon();
        
        switch (_dungeon_Type)
        {
            case DUNGEON_TYPE.MONSTER:
                getNextWall.SetActive(true);
                dungeonMonster();
                break;
            case DUNGEON_TYPE.MONSTER_PURPLER:
                getNextWall.SetActive(true);
                dungeonMonsterPurple();
                break;
            case DUNGEON_TYPE.NPC:
                getNextWall.SetActive(false);
                dungeonNPC();
                break;
            case DUNGEON_TYPE.REST:
                getNextWall.SetActive(false);
                dungeonRest();
                break;
            case DUNGEON_TYPE.SHOP:
                getNextWall.SetActive(false);
                dungeonShop();
                break;
        }
    }

    void resetDungeon()
    {
        // 맵 변경되니까 기존 데이터 제거
        _mapUI.SetActive(false);
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
    }

    public void selectMonsterDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;

        _dungeon_Type = DUNGEON_TYPE.MONSTER;
        resetDungeon();
        initDungeon();
    }
    public void selectMonsterPurpleDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.MONSTER_PURPLER;
        resetDungeon();
        initDungeon();

        // [퍼플라이트]퀘스트 0단계 (퍼플라이트 던전방문하기)를 진행하고 있었다면 클리어
        if (Character.sub_quest.ContainsKey(QUEST_CODE.PURPLE_LIGHT.ToString())) {
            if (Character.sub_quest_progress[QUEST_CODE.PURPLE_LIGHT.ToString()].Equals(0))
                GameMng.I.nextSubQuest(QUEST_CODE.PURPLE_LIGHT);
        }
    }
    public void selectNPCDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.NPC;
        resetDungeon();
        initDungeon();
    }
    public void selectRestDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.REST;

        // 배틀아이템 개수 리셋 (충전)
        Character.equipBattleItem[0].itemCount  = Character.equipBattleItem[0].itemData.count;
        Character.equipBattleItem[1].itemCount  = Character.equipBattleItem[1].itemData.count;
        Character.equipBattleItem[2].itemCount  = Character.equipBattleItem[2].itemData.count;

        resetDungeon();
        initDungeon();
    }
    public void selectShopDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.SHOP;
        resetDungeon();
        initDungeon();
    }
    public void selectRandomDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.RANDOM;
        resetDungeon();
        initDungeon();
    }

    /**
     * @brief 던전의 마지막 스테이지 눌렀을때
     */
    public void lastDungeon() {
        _nextPortal.SetActive(false);
        _clearPortal.SetActive(true);
    }

    protected virtual void dungeonMonster() {}
    protected virtual void dungeonMonsterPurple() {}
    protected virtual void dungeonNPC() {}
    protected virtual void dungeonRest() {
        campFire.SetActive(true);

        // 배틀 아이템 초기화
        for (int i = 0; i < Character.equipBattleItem.Length; i++)
        {
            Character.equipBattleItem[i].itemCount = Character.equipBattleItem[i].itemData.count;
            GameMng.I.BattleItemUI.ItemText[i].text = Character.equipBattleItem[i].itemCount.ToString();
            
            // 기존에 쿨타임 중이던 것도 초기화 시키려면 아래 주석 해제. (But, 기존에 진행중이던 코루틴이 있다면 꼬이는 부분이 있음)
            //GameMng.I.battleItem_Img[i].color = Color.white;
            //GameMng.I.battleItem_Img[i].fillAmount = 1;
            //Character.usingBattleItem[i] = false;
            //GameMng.I.character.StopAllCoroutines();
        }
    }
    protected virtual void dungeonShop() {
        npc_shop.SetActive(true);
    }
    protected virtual void dungeonRandom() {}

    public static void monsterDie()
    {
        _leftMonster--;
        
        if (_leftMonster.Equals(0))
        {
            // 다음 던전으로 들어갈 수 있는 문 열림
            getNextWall.SetActive(false);
        }
    }
}