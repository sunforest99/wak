using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================================
//                          보류
// =====================================================
[CreateAssetMenu(fileName ="New ItemData", menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ITEM_TYPE itemType;

    [SerializeField] private int cost;

}
