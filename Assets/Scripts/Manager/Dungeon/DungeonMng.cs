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
    protected static int _leftMonster = 0;                              // 현재 남아있는 잔여 몬스터
    protected static GameObject getNextWall;                            // _nextWall 이 Start때 들어감
    [SerializeField] GameObject _nextWall;                              // 다음 던전으로 넘어갈 수 있는 문을 막고 있는 벽 (몬스터 0마리 되면 false 해서 길 열어주기)


    [Space(20)][Header("[  던전 공용 프리팹  ]")]  // ==========================================================================================================================
    [SerializeField] protected GameObject purpleLight;                  // 몬스터 강화 오브젝트


    void Start()
    {
        GameMng.I._keyMode = KEY_MODE.UI_MODE;
        _mapUI.SetActive(true);
        getNextWall = _nextWall;
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
        getNextWall.SetActive(false);
        
        if (_dungeon_Type.Equals(DUNGEON_TYPE.RANDOM))
            _dungeon_Type = randomDungeon();
        
        switch (_dungeon_Type)
        {
            case DUNGEON_TYPE.MONSTER:
                dungeonMonster();
                break;
            case DUNGEON_TYPE.MONSTER_PURPLER:
                dungeonMonsterPurple();
                break;
            case DUNGEON_TYPE.NPC:
                dungeonNPC();
                break;
            case DUNGEON_TYPE.REST:
                dungeonReset();
                break;
            case DUNGEON_TYPE.SHOP:
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
        resetDungeon();
        initDungeon();
    }
    public void selectRandomDungeon(Transform clickedBT) {
        curLocationUI.transform.position = clickedBT.position;
        
        _dungeon_Type = DUNGEON_TYPE.RANDOM;
        resetDungeon();
        initDungeon();
    }

    protected virtual void dungeonMonster() {}
    protected virtual void dungeonMonsterPurple() {}
    protected virtual void dungeonNPC() {}
    protected virtual void dungeonReset() {}
    protected virtual void dungeonShop() {}
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