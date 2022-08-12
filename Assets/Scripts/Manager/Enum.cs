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