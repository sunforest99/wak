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

public enum BUFF
{
    BUFF_GAL = 0,
    BUFF_THIEF,
    NOT_BUFF                    // 버프 아님(디버프)
}

public enum DEBUFF
{
    DEBUFF_BUPAE = 0,
    DEBUFF_CHIMSIK,
    DEBUFF_JAMSIK,
    DEBUFF_SHIELD,
    NOT_DEBUFF                  // 디버프 아님(버프)
}