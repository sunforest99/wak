using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================================
//                          보류
// =====================================================
[CreateAssetMenu(fileName = "New ItemData", menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public ITEM_TYPE itemType;                      // 아이템 타입
    public ITEM_INDEX itemIndex;                    // 아이템 인덱스 알아오기 (enum)

    public Sprite itemSp;                           // 아이템 스프라이트

    public string itemName;                         // 아이템 이름
    [TextArea] public string content;               // 아이템 설명

    public int count;                               // 아이템 갯수
    public int duration;                            // 아이템 쿨타임
}
