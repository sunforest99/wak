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
    [SerializeField] protected GameObject purpleLight;            // 몬스터 강화 오브젝트

    DUNGEON_TYPE _dungeon_Type;                         // 현재 들어가 있는 던전의 타입
    public GameObject nextWall;                         // 다음 던전으로 넘어갈 수 있는 문을 막고 있는 벽 (몬스터 0마리 되면 false 해서 길 열어주기)
    public int _leftMonster = 0;                        // 현재 남아있는 잔여 몬스터

    void Start()
    {
        initDungeon();
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
        
        // _monsters.Clear();

        _dungeon_Type = DUNGEON_TYPE.NONE;

        if (_dungeon_Type.Equals(DUNGEON_TYPE.RANDOM))
            _dungeon_Type = randomDungeon();
        
        switch (_dungeon_Type)
        {
            case DUNGEON_TYPE.MONSTER:
                break;
            case DUNGEON_TYPE.MONSTER_PURPLER:
                break;
            case DUNGEON_TYPE.NPC:
                break;
            case DUNGEON_TYPE.REST:
                break;
            case DUNGEON_TYPE.SHOP:
                break;
        }
    }

    protected virtual void dungeonMonster() {}
    protected virtual void dungeonMonsterPurple() {}
    protected virtual void dungeonNPC() {}
    protected virtual void dungeonRest() {}
    protected virtual void dungeonShop() {}
    protected virtual void dungeonRandom() {}

    protected virtual void monsterDie()
    {
        _leftMonster--;

        if (_leftMonster.Equals(0))
        {
            // 다음 던전으로 들어갈 수 있는 문 열림
        }
    }
}