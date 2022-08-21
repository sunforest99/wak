// =====================================================
//                          보류
// =====================================================
public enum ITEM_TYPE
{
    CONSUMABLE_ITEM,            // 사용 아이템
    WEAPON_ITEM,                // 장비 아이템 - 무기
    HEAD_ITEM,                  // 장비 아이템 - 머리
    BATTLE_ITEM,                // 배틀 아이템
    UNUSEFUL_ITEM               // 무쓸모 아이템 - 잡템
}

public enum ITEM_INDEX
{
    POTION = 0,
    CLEANSER,
    SPEEDUP,
    WEAPON_ITEM_INDEX,
    STICK,
    BROOM,
    BAT,
    CANDY,
    SANGHYUN,
    WAKCHORI,
    WAND,
    STAFF,
    ROSE,
    SWORD,
    CONSUMABLE_ITEM_INDEX,
    REDMUSHROOM,
    NONE
}

public enum QUEST_CODE
{
    /* 서브 퀘스트 이름 */
    TEMP_QUEST_0
}

public enum BUFF
{
    BUFF_GAL = 0,
    BUFF_THIEF,
    DEBUFF_BUPAE,
    DEBUFF_CHIMSIK,
    DEBUFF_JAMSIK,
    DEBUFF_SHIELD
}

public enum JOB
{
    NONE,           // -
    WARRIER,        // 전사
    MAGICIAN,       // 법사
    HEALER          // 힐러
}

[System.Serializable]
public struct ItemSlotUI
{
    public UnityEngine.UI.Image[] ItemImg;
    public TMPro.TextMeshProUGUI[] ItemText;
    public ITEM_INDEX[] ItemIdx;
}

[System.Serializable]
public struct GetItemEXP
{
    public UnityEngine.GameObject EXP_Game;
    public UnityEngine.UI.Image EXP_Img;
    public TMPro.TextMeshProUGUI EXP_Text;
}

public enum ROOM_CODE
{
    /*********************************************************************
     *
     *    이 데이터가 바뀐다면 꼭! 바로 서버내의 데이터도 바꿀 것
     *
     *********************************************************************/

    // _LOCAL_MAP_,  ////////////// 이 아래 부터 로컬 전용 맵 (네트워크 통신 안함)

    FOREST = 0,
    TEMPLE,

    _WORLD_MAP_,    /////////////// 이 아래 부터 월드 전용 맵 (네트워크 들어가며 모든 유저들 있을수 있음)

    SQUARE,         // 광장(대도시)
    
    _PARTY_MAP_,    /////////////// 이 아래 부터 파티 전용 맵

    FIELD_0,        // 필드 0
    RAID_0,         // 레이드 0
    RAID_0_REPAIR,  // 레이드 0 정비소
    RAID_1,         // 레이드 1
    RAID_1_REPAIR   // 레이드 1 정비소
}
