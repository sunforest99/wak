// =====================================================
//                          보류
// =====================================================
public enum ITEM_TYPE
{
    BATTLE_ITEM,                // 배틀 아이템

    WEAPON_ITEM,                // 장비 아이템 - 무기
    SHIRTS_ITEM,                // 장비 아이템 - 머리
    PANTS_ITEM,                 // 장비 아이템 - 하의

    FAVORITE_ITEM,              // NPC 호감도 아이템

    UNUSEFUL_ITEM,              // 무쓸모 아이템 - 잡템
    CONSUMABLE_ITEM,            // 사용 아이템
}

/*
    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    추가할때 Resources/ItemData 에도 파일 만들어 주어야함
*/
public enum ITEM_INDEX
{
    _BATTLE_ITEM_INDEX_ = 0,
    POTION,
    POTION_2,
    CLEANSER,
    SHIELD,
    DMGUP,
    SHIELDUP,
    
    _WEAPON_ITEM_INDEX_ = 100,
    STICK,          // 나뭇가지
    STAFF,          // 일반 완드
    SWORD,
    SANGHYUN,
    WAKCHORI,
    BAT,
    WAND,           // 완드 (뱀파이어 서바이벌)
    BROOM,          // 빗자루
    CANDY,
    ROSE,
    JA,
    MIC,
    _WEAPON_ITEM_INDEX_END_,

    _SHIRTS_ITEM_INDEX = 150,
    SHIRTS_0,
    SHIRTS_1,
    SHIRTS_2,
    SHIRTS_3,
    SHIRTS_4,
    SHIRTS_5,
    SHIRTS_6,
    _SHIRTS_ITEM_INDEX_END_,

    _PANTS_ITEM_INDEX = 170,
    PANTS_0,
    PANTS_1,
    PANTS_2,
    PANTS_3,
    PANTS_4,
    PANTS_5,
    PANTS_6,
    _PANTS_ITEM_INDEX_END_,

    _FAVORITE_ITEM_INDEX_ = 200,
    SCIENCE_AWARD,
    LANOVEL,
    AIR,
    FISH,
    MAGIC_WAND,
    WINE,
    NINJA,
    HAIR_ROLE,
    KANOLAYU,
    YANG_GANG,
    STORYBOOK,
    DRAGON_BALL,
    MONEY,
    MOVEY_DVD,
    ARMY,
    TOOTH,
    DONGGASSE,
    SWIP_SHIRT,
    ROBOT,
    _FAVORITE_ITEM_INDEX_END_,

    _CONSUMABLE_ITEM_INDEX_ = 300,
    REDMUSHROOM,

    NONE
}

public enum QUEST_CODE
{
    /* 서브 퀘스트 이름 */
    TEMP_QUEST_0,
    PURPLE_LIGHT,
    R_U_HUMAN
}

// 새로 추가되면 무조건 종류상관없이 뒤에 
public enum BUFF
{
    BUFF_GAL = 0,
    BUFF_THIEF,
    DEBUFF_BUPAE,
    DEBUFF_CHIMSIK,
    DEBUFF_JAMSIK,
    DEBUFF_SHIELD,
    BUFF_NESIGYUNG,
    BUFF_SANPELLE,
    BUFF_GOSEGU,
    BUFF_COTTON
    
}

public enum JOB
{
    NONE,           // -
    WARRIER,        // 전사
    MAGICIAN,       // 법사
    HEALER          // 힐러
}

public enum NPC
{
    BUJUNG,         // 부정형
    CHUNSIK,        // 곽춘식
    GWONMIN,        // 권민
    MANDOO,         // 김치만두
    BUG,            // 단답벌레
    WAKPAGO,        // 왁파고
    __NONE__
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

    HOME = 0,
    // TEMPLE,         // 직업 선택 (초기 회원가입때 소켓 연결전에 들어오게 변경됨)
    DUNGEON_0,      // 던전 0
    DUNGEON_1,      // 던전 1

    _WORLD_MAP_,    /////////////// 이 아래 부터 월드 전용 맵 (네트워크 들어가며 모든 유저들 있을수 있음)

    SQUARE,         // 광장(대도시)
    
    _PARTY_MAP_,    /////////////// 이 아래 부터 파티 전용 맵

    RAID_0,         // 레이드 0
    RAID_0_REPAIR,  // 레이드 0 정비소
    RAID_1,         // 레이드 1
    RAID_1_REPAIR   // 레이드 1 정비소
}
